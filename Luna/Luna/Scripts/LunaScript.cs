using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Luna {
	public abstract class LunaScript {
		protected LunaConnectionBase luna;

		protected virtual int period {
			get { return 16; }
		}
		public abstract void Run();
        public virtual void Exit() { }

        private Thread thread;

		public void Start(LunaConnectionBase luna) {
			if (thread == null) {
				this.luna = luna;
				thread = new Thread(ThreadFunction);
				thread.Start();
			}
		}

		private bool shouldRun = true;

		public void Stop() {
			if (thread != null) {
				shouldRun = false;
				thread.Join();
				thread = null;
			}
		}

		private void ThreadFunction() {
			System.Diagnostics.Stopwatch st = new System.Diagnostics.Stopwatch();
			st.Start();
			while (shouldRun) {
				double t0 = st.Elapsed.TotalMilliseconds;
				//System.Console.WriteLine(1 / (t - time));
				Run();
				luna.Send();
                double t1 = st.Elapsed.TotalMilliseconds;
                double sleepTime = period - (t1 - t0);
                if (sleepTime > 0) Thread.Sleep((int) Math.Round(sleepTime));
			}
            Exit();
		}

		public static LunaScript CompileScript(string fileName) {
			string code;
			try {
				code = File.ReadAllText(fileName);
			} catch {
				MessageBox.Show("Błąd wczytywania pliku",
					"Błąd wczytywania pliku",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return null;
			}

			Microsoft.CSharp.CSharpCodeProvider csProvider = new Microsoft.CSharp.CSharpCodeProvider();

			CompilerParameters options = new CompilerParameters() {
				GenerateExecutable = false,
				GenerateInMemory = true,
			};

			options.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
			options.ReferencedAssemblies.Add("System.dll");


			CompilerResults result;
			result = csProvider.CompileAssemblyFromSource(options, code);

			if (result.Errors.HasErrors) {
				StringBuilder builder = new StringBuilder();
				foreach (var err in result.Errors) {
					builder.AppendLine(err.ToString());
				}

				MessageBox.Show(builder.ToString(),
					"Błędy kompilacji",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
				return null;
			}

			if (result.Errors.HasWarnings) {
				StringBuilder builder = new StringBuilder();
				foreach (var err in result.Errors) {
					builder.AppendLine(err.ToString());
				}

				MessageBox.Show(builder.ToString(),
					"Ostrzeżenia kompilacji",
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);
			}

			foreach (Type type in result.CompiledAssembly.GetExportedTypes()) {
				if (typeof(LunaScript).IsAssignableFrom(type.BaseType)) {
					ConstructorInfo constructor = type.GetConstructor(System.Type.EmptyTypes);
					if (constructor != null && constructor.IsPublic) {
						LunaScript scriptObject = constructor.Invoke(null) as LunaScript;
						if (scriptObject != null) {
							return scriptObject;
						} else {
							MessageBox.Show("Wystąpił błąd.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
						}
					} else {
						MessageBox.Show("Nie znaleziono konstruktora.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}

				}
			}

			MessageBox.Show("Nie znaleziono klasy implementującej interfejs LunaScript.\n" +
				@"namespace Luna {
public interface LunaScript {
		void Run(double[] leftPixels, double[] rightPixels, ref double leftWhite, ref double rightWhite);
	}
}",
				"Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);

			return null;
		}
	}
}

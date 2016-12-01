
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Luna {
	internal class Program : ApplicationContext
    {
        private int formCount;
        public static Program mainContext;

        private Program()
        {
            this.AddForm(new ConnectionAwaitForm());
            mainContext = this;
        }

        public void AddForm(Form form)
        {
            this.formCount++;
            form.FormClosed += new FormClosedEventHandler(this.OnFormClosed);
            form.Show();
        }

        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Program());
        }

        private void OnFormClosed(object sender, EventArgs e)
        {
            this.formCount--;
            if (this.formCount <= 0)
            {
                base.ExitThread();
            }
        }
    }
}

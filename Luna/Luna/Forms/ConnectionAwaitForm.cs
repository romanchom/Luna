using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Luna
{
    public partial class ConnectionAwaitForm : Form
    {
        private Thread connectionThread;
        private bool shouldExit;

        public ConnectionAwaitForm()
        {
            this.InitializeComponent();
            this.connectionThread = new Thread(new ThreadStart(this.TryConnect));
            this.connectionThread.Start();
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            this.shouldExit = true;
            this.connectionThread.Join();
            base.OnFormClosing(e);
        }

        private void TryConnect()
        {
            while (!this.shouldExit)
            {
                try
                {
                    LunaConnectionBase luna = new LunaUdp();
                    base.BeginInvoke((Action) (() => {
                        Program.mainContext.AddForm(new SessionForm(luna));
                        Close();
                    })
                    );
                    this.shouldExit = true;
                }
                catch
                {
                }
            }
        }
    }
}

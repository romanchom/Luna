using System;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;

namespace Luna {
	public partial class SerialPortSelectionForm : Form {
		public SerialPortSelectionForm() {
			InitializeComponent();
		}

		private void SerialPortsComboBoxDropDown(object sender, EventArgs e) {
			SerialPortsComboBox.Items.Clear();
			SerialPortsComboBox.Items.AddRange(SerialPort.GetPortNames());
        }

		private void ConnectButton_Click(object sender, EventArgs e) {
			string name = (string)SerialPortsComboBox.SelectedItem;
			
			try {
				new SessionForm(name).Show();
				Close();
			} catch (Exception ex) {
				MessageBox.Show("Połączenie z Łuną nie powiodło się. Szczegóły:\n" + ex.ToString(),
					"Porażka",
					MessageBoxButtons.OK,
					MessageBoxIcon.Error);
			}
		}
	}
}

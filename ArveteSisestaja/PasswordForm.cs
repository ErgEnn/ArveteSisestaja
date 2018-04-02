using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArveteSisestaja {
	public partial class PasswordForm : Form {
		public PasswordForm() {
			InitializeComponent();
		}

		private void saveButton_Click(object sender, EventArgs e) {
			SettingsHandler.UpdateValue(SettingsHandler.SETTING.ANC_USERNAME,ancUsername.Text);
			SettingsHandler.UpdateValue(SettingsHandler.SETTING.ANC_PASSWORD, ancPassword.Text);
			SettingsHandler.UpdateValue(SettingsHandler.SETTING.OMNIVA_USERNAME, omnivaUsername.Text);
			SettingsHandler.UpdateValue(SettingsHandler.SETTING.OMNIVA_PASSWORD, omnivaPassword.Text);
			SettingsHandler.UpdateValue(SettingsHandler.SETTING.ENCRYPTION_TEST, "TEST");
			Hide();
		}
	}
}

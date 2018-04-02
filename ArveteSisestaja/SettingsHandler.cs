using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArveteSisestaja {
	public class SettingsHandler {

		private static Dictionary<SETTING, string> _settings;
		public enum SETTING {
			ANC_USERNAME,
			ANC_PASSWORD,
			OMNIVA_USERNAME,
			OMNIVA_PASSWORD,
			ENCRYPTION_TEST
		};//All settings must be registered here in order for them to be loaded

		public static event SettingsChangedEvent SettingsChanged;
		public delegate void SettingsChangedEvent(SettingsChangedEventArgs scea);



		/*
		 * Loads all settings into _settings dictionary
		 */
		public static void LoadSettings() {

			bool loadingFailed = false;
			bool needsReentering = false;
			do {
				_settings = new Dictionary<SETTING, string>();
				try {
					var appSettings = ConfigurationManager.AppSettings;
					foreach(SETTING s in Enum.GetValues(typeof(SETTING))) {
						if(appSettings[s.ToString()] == null) {
							needsReentering = true;
							AddUpdateAppSetting(s.ToString(), "");
							_settings.Add(s, "");
						} else {
							_settings.Add(s, appSettings[s.ToString()]);
						}
					}
					loadingFailed = false;
				} catch(Exception e) {
					loadingFailed = true;
					if(MessageBox.Show("Seadete laadimise viga\n" + e.Message, "Kriitiline viga", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel) { Environment.Exit(-1); }
				}
				if (needsReentering || !GetSetting(SETTING.ENCRYPTION_TEST).Equals("TEST")) {
					new PasswordForm().Show();
				}

			} while(loadingFailed);
		}

		/*
		 * Gets current setting from _settings or default value if setting is empty
		 */
		public static string GetSetting(SETTING s, string defaultValue) {
			string r = GetSetting(s);
			return r == "" ? defaultValue : r;
		}

		/*
		 * Gets current setting from _settings
		 */
		public static string GetSetting(SETTING s) {
			return EncryptionHandler.Unprotect(_settings[s]);
		}



		/*
		 * Updates setting in _settings and also in config
		 */
		public static void UpdateValue(SETTING s, string value) {
			string old = GetSetting(s);
			value = EncryptionHandler.Protect(value);
			_settings[s] = value;
			AddUpdateAppSetting(s.ToString(), _settings[s]);
			if(SettingsChanged != null) {
				SettingsChangedEventArgs scea = new SettingsChangedEventArgs {
					Setting = s,
					OldValue = old,
					NewValue = value
				};
				SettingsChanged(scea);
			}

		}

		/*
		 * Adds or Updates config file with new setting/value
		 */
		private static void AddUpdateAppSetting(string key, string value) {
			try {
				var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
				var settingsFile = configFile.AppSettings.Settings;
				if(settingsFile[key] == null) {
					settingsFile.Add(key, value);
				} else {
					settingsFile[key].Value = value;
				}
				configFile.Save(ConfigurationSaveMode.Modified);
				ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
			} catch(ConfigurationErrorsException e) {
				MessageBox.Show("Seadete salvestamise viga\n" + e.StackTrace, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

	}

	public class SettingsChangedEventArgs : EventArgs {
		public SettingsHandler.SETTING Setting;
		public string OldValue;
		public string NewValue;
	}

}

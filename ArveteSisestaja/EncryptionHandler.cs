using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ArveteSisestaja {
	static class EncryptionHandler {
		private static readonly string salt = "yj564k8d46aSD5G68E654dah4";

		static public string Protect(string password) {
			byte[] passwordBytes = Encoding.Unicode.GetBytes(password);
			byte[] saltBytes = Encoding.Unicode.GetBytes(salt);

			byte[] cipherBytes = ProtectedData.Protect(passwordBytes, saltBytes, DataProtectionScope.CurrentUser);

			return Convert.ToBase64String(cipherBytes);
		}

		static public string Unprotect(string cipher) {
			byte[] cipherBytes = Convert.FromBase64String(cipher);
			byte[] saltBytes = Encoding.Unicode.GetBytes(salt);
			byte[] passwordBytes = new byte[0];
			try {
				passwordBytes = ProtectedData.Unprotect(cipherBytes, saltBytes, DataProtectionScope.CurrentUser);
			} catch(CryptographicException ce) {
				Console.WriteLine(ce.ToString());
			}

			return Encoding.Unicode.GetString(passwordBytes);
		}
	}
}

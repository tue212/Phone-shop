using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PhoneShop
{
    public class Password
    {
        public static (string, string) Encrypt (string rawPassword)
        {
            byte[] passwordInBytes = Encoding.UTF8.GetBytes(rawPassword);
            byte[] entropy = new byte[20];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(entropy);
            }
            byte[] encryptedPassword = ProtectedData.Protect(passwordInBytes, entropy, DataProtectionScope.CurrentUser);
            return (Convert.ToBase64String(encryptedPassword), Convert.ToBase64String(entropy));
        }
        public static string Decrypt (string encryptedPassword, string entropy)
        {
            byte[] encryptedPasswordInBytes = Convert.FromBase64String(encryptedPassword);
            byte[] entropyInBytes = Convert.FromBase64String(entropy);
            byte[] decryptedPassword = ProtectedData.Unprotect(encryptedPasswordInBytes, entropyInBytes, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decryptedPassword);
        }
    }
}

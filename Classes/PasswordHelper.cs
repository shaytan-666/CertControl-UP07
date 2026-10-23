using System.Security.Cryptography;
using System.Text;

namespace CertControl.Classes
{
    internal static class PasswordHelper
    {
        public static string GetHash(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(password);

                byte[] hash = sha256.ComputeHash(bytes);

                StringBuilder result = new StringBuilder();

                foreach (byte b in hash)
                {
                    result.Append(b.ToString("X2"));
                }

                return result.ToString();
            }
        }
    }
}


using System.Text;

namespace GameServer.API.Helper
{
    public class PasswordHash
    {
        public static byte[] HashPassword(string password, string salt)
        {
            byte[] unhashedBytes = Encoding.Unicode.GetBytes(String.Concat(salt, password));

            var sha256 = System.Security.Cryptography.SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(unhashedBytes);

            return hashedBytes;
        }

        public static bool VerifyPassword(string attemptedPassword, byte[] hash, string salt)
        {
            string base64Hash = Convert.ToBase64String(hash);
            string base64AttemptedHash = Convert.ToBase64String(HashPassword(attemptedPassword, salt));

            return base64Hash == base64AttemptedHash;
        }
    }
}

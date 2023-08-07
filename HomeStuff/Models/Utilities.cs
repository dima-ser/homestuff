using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Text;

namespace HomeStuff.Models
{
    public class Utilities
    {
        public static string AdminUserName = "HomeStuff Admin";
        public static byte[] Salt = Encoding.ASCII.GetBytes("LemonTree");

        public static string GetHash(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: Salt,
               prf: KeyDerivationPrf.HMACSHA256,
               iterationCount: 100000,
               numBytesRequested: 256 / 8));
        }
    }
}

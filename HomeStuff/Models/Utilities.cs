using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace HomeStuff.Models
{
    public class Utilities
    {
        public static string AdminUserName = "HomeStuff Admin";
        public static byte[] Salt = Encoding.ASCII.GetBytes("LemonTree");
        private static string passwordFileName = "password.txt";
        public static string AttachmentDirName = "attachments";

        public static string GetHash(string password)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: Salt,
               prf: KeyDerivationPrf.HMACSHA256,
               iterationCount: 100000,
               numBytesRequested: 256 / 8));
        }

        public static string GetPasswordFilePath(IWebHostEnvironment environment, IConfiguration configuration)
        {
            string passwordFilePath = Path.Combine(environment.ContentRootPath, configuration.GetValue<string>("UserDataDirectory"));
            return Path.Combine(passwordFilePath, passwordFileName);
        }
    }
}

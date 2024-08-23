using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using System.Text;

namespace HomeStuff.Models
{
    public class Utilities
    {
        public static readonly string AdminUserName = "HomeStuff Admin";
        public static readonly byte[] Salt = Encoding.ASCII.GetBytes("LemonTree");
        public static readonly string AttachmentDirName = "attachments";
        public static readonly string ConfigUserDataDirectory = "UserDataDirectory";
        public static readonly string ConfigAvatarFileExtensions = "AvatarFileExtensions";

        private static readonly string passwordFileName = "password.txt";

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
            string passwordFilePath = Path.Combine(environment.ContentRootPath, configuration[ConfigUserDataDirectory]);
            return Path.Combine(passwordFilePath, passwordFileName);
        }

        public static bool IsDebugMode
        {
            get
            {
            #if DEBUG
                return true;
            #else
                return false;
            #endif
            }
        }

    }
}

using Microsoft.Extensions.Configuration;

namespace HomeStuff.Models
{
    public sealed class Config
    {
        //private IConfiguration configuration;
        private static readonly string UserDataDirectory = "UserDataDirectory";
        private static readonly string AvatarFileExtensions = "AvatarFileExtensions";
        private static readonly string ItemPageSize = "ItemPageSize";

        public static string GetUserDataDirectory(IConfiguration configuration)
        {
            if (configuration.GetValue<string>(UserDataDirectory) != null)
                return configuration.GetValue<string>(UserDataDirectory)!;
            else
                throw new Exception("Missing configuration from appsettings.json: " + UserDataDirectory);
        }

        public static string[] GetAvatarFileExtensions(IConfiguration configuration)
        {
            if (configuration.GetSection(AvatarFileExtensions).Get<string[]>() != null)
                return configuration.GetSection(AvatarFileExtensions).Get<string[]>()!;
            else
                throw new Exception("Missing configuration from appsettings.json: " + AvatarFileExtensions);
        }

        public static int GetItemPageSize(IConfiguration configuration)
        {
            if (configuration.GetSection(ItemPageSize).Get<int?>() != null)
                return configuration.GetSection(ItemPageSize).Get<int>()!;
            else
                throw new Exception("Missing configuration from appsettings.json: " + ItemPageSize);
        }
    }
}

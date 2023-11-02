using System.IO;
using Microsoft.Extensions.Configuration;

namespace myBackup.Utils
{
    public class ConfigUtils
    {
        public static IConfiguration Config;

        private ConfigUtils()
        {
        }

        public static void Init()
        {
            var builder = new ConfigurationBuilder();
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);
 
            Config = builder.Build();
        }
    }
}
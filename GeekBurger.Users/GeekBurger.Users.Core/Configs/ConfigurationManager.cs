using Microsoft.Extensions.Configuration;
using System.IO;

namespace GeekBurger.Users.Core.Configs
{
    public static class ConfigurationManager
    {
        public static IConfiguration Configuration { get; set; }

        static ConfigurationManager()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Api.Common
{
    public static class ConfigValue
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static string ConnectionString
        {
            get
            {
                return GetConfigValue("ConnectionString");
            }
        }

        public static string RepositoryAssembly
        {
            get
            {
                return GetConfigValue("RepositoryAssembly");
            }
        }

        public static string BusinessLogicAssembly
        {
            get
            {
                return GetConfigValue("BusinessLogicAssembly");
            }
        }

        public static IConfiguration Logging
        {
            get
            {
                return GetConfig("Logging");
            }
        }

        private static string GetConfigValue(string configKey)
        {
            return Configuration.GetSection(configKey).Value;
        }

        private static IConfiguration GetConfig(string configKey)
        {
            return Configuration.GetSection(configKey);
        }

    }
}

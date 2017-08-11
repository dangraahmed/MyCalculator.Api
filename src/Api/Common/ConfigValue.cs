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

        public static class TokenAuthOption
        {
            public static string Audience
            {
                get
                {
                    return GetConfigValue("TokenAuthOption:Audience");
                }
            }
            public static string Issuer
            {
                get
                {
                    return GetConfigValue("TokenAuthOption:Issuer");
                }
            }
            public static int ExpiresSpan
            {
                get
                {
                    return Convert.ToInt32(GetConfigValue("TokenAuthOption:ExpiresSpan"));
                }
            }
            public static string TokenType
            {
                get
                {
                    return GetConfigValue("TokenAuthOption:TokenType");
                }
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

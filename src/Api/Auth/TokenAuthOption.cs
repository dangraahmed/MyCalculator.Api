using Microsoft.IdentityModel.Tokens;
using System;
using Microsoft.Extensions.Configuration;
using Api.Common;

namespace Api.Auth
{
    public class TokenAuthOption
    {
        public static string Audience { get; } = ConfigValue.TokenAuthOption.Audience;
        public static string Issuer { get; } = ConfigValue.TokenAuthOption.Issuer;
        public static RsaSecurityKey Key { get; } = new RsaSecurityKey(RSAKeyHelper.GenerateKey());
        public static SigningCredentials SigningCredentials { get; } = new SigningCredentials(Key, SecurityAlgorithms.RsaSha256Signature);
        public static TimeSpan ExpiresSpan { get; } = TimeSpan.FromSeconds(ConfigValue.TokenAuthOption.ExpiresSpan);
        public static string TokenType { get; } = ConfigValue.TokenAuthOption.TokenType;
    }
}

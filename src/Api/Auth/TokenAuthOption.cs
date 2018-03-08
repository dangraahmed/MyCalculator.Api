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

    public abstract class ITokenAuthOptionWrapper
    {
        public virtual string Audience { get; }
        public virtual string Issuer { get; }
        public virtual RsaSecurityKey Key { get; }
        public virtual SigningCredentials SigningCredentials { get; } 
        public virtual TimeSpan ExpiresSpan { get; } 
        public virtual string TokenType { get; }
    }

    public class TokenAuthOptionWrapper : ITokenAuthOptionWrapper
    {
        public override string Audience { get; } = TokenAuthOption.Audience;
        public override string Issuer { get; } = TokenAuthOption.Issuer;
        public override RsaSecurityKey Key { get; } = TokenAuthOption.Key;
        public override SigningCredentials SigningCredentials { get; } = TokenAuthOption.SigningCredentials;
        public override TimeSpan ExpiresSpan { get; } = TokenAuthOption.ExpiresSpan;
        public override string TokenType { get; } = TokenAuthOption.TokenType;
    }
}

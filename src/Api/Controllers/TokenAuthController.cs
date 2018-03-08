using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Dto.Common;
using Dto.Object;
using Core;
using Core.Model;
using Core.Interface;
using AutoMapper;
using Newtonsoft.Json;
using Api.Auth;
using System.Security.Claims;
using System.Security.Principal;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace Api.Controllers
{
    [Route("[controller]")]
    public class TokenAuthController : Controller
    {
        private readonly IUserBL _UserBL;
        private readonly ITokenAuthOptionWrapper _TokenAuthOptionWrapper;
        public TokenAuthController(IUserBL userBL, ITokenAuthOptionWrapper tokenAuthOptionWrapper)
        {
            if (userBL == null)
            {
                throw new ArgumentNullException(nameof(userBL));
            }

            _UserBL = userBL;
            _TokenAuthOptionWrapper = tokenAuthOptionWrapper;
        }

        [Route("okok")]
        public string okok()
        {
            return "okokok";
        }

        [HttpPost]
        [Route("getAuthToken")]
        public string GetAuthToken([FromBody]UserMasterViewModel userMaster)
        {
            var existUser = _UserBL.GetUsers().FirstOrDefault(u => u.UserName == userMaster.UserId && u.UserPassword == userMaster.UserPassword);
            
            if (existUser != null)
            {
                var requestAt = DateTime.Now;
                var expiresIn = requestAt + _TokenAuthOptionWrapper.ExpiresSpan;
                var token = GenerateToken(existUser, expiresIn);

                var settings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                };

                return JsonConvert.SerializeObject(new RequestResult
                {
                    State = RequestState.Success,
                    Data = new RequestResultData()
                    {
                        RequertAt = requestAt,
                        ExpiresIn = _TokenAuthOptionWrapper.ExpiresSpan.TotalSeconds,
                        TokeyType = _TokenAuthOptionWrapper.TokenType,
                        AccessToken = token
                    }
                }, settings);
            }
            else
            {
                return JsonConvert.SerializeObject(new RequestResult
                {
                    State = RequestState.Failed,
                    Msg = "Username or password is invalid"
                });
            }
        }

        private string GenerateToken(UserMaster user, DateTime expires)
        {
            var handler = new JwtSecurityTokenHandler();

            ClaimsIdentity identity = new ClaimsIdentity(
                new GenericIdentity(user.UserName, "TokenAuth"),
                new[] {
                    new Claim("ID", user.Id.ToString())
                }
            );

            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = _TokenAuthOptionWrapper.Issuer,
                Audience = _TokenAuthOptionWrapper.Audience,
                SigningCredentials = _TokenAuthOptionWrapper.SigningCredentials,
                Subject = identity,
                Expires = expires
            });
            return handler.WriteToken(securityToken);
        }
    }
}

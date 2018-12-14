using AutoMapper;
using Core.Interface;
using Dto.Object;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserBL _userBL;
        private readonly IMapper _mapper;

        public UsersController(IUserBL userBL, IMapper mapper)
        {
            _userBL = userBL ?? throw new ArgumentNullException(nameof(userBL));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] UserMasterViewModel userParam)
        {
            var user = _mapper.Map<UserMasterViewModel>(_userBL.Authenticate(userParam.UserName, userParam.UserPassword));

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            user.UserPassword = null;

            user.Token = GenerateJSONWebToken(user);

            return Ok(user);
        }

        [HttpGet("getAll")]
        public IActionResult GetAll()
        {
            var users = _userBL.GetUsers();
            return Ok(users);
        }

        private string GenerateJSONWebToken(UserMasterViewModel userInfo)
        {
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("AhmedAminDangraSecretKey"); //TODO: remove hardcoding
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userInfo.UserName),
                    new Claim(ClaimTypes.Surname, userInfo.UserName),
                    new Claim(ClaimTypes.PostalCode, "400001"),
                    new Claim(ClaimTypes.DateOfBirth, DateTime.Now.ToString())
                }),
                Expires = DateTime.UtcNow.AddSeconds(100),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = "http://localhost:56788/" //TODO: remove hardcoding

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
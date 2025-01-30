using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LabelPadCoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace LabelPadCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("Authenticate")]      
        public async Task<IActionResult> Authenticate([FromBody]UserModel model)
        {
            UserModel login = new UserModel();
            login.UserName = model.UserName;
            login.Password = model.Password;
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(login);
            if (user != null)
            {
                var tokenStr =await GenerateJSONWebToken(user);
               // response = Ok( tokenStr);
            }
            return Ok(await GenerateJSONWebToken(user));
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;
            if (login.UserName == "admin" && login.Password == "labelpad")
            {
                user = new UserModel { UserName = "admin", EmailAddress = "admin@gmail.com", Password = "labelpad" };
            }
            return user;
        }

        private Task<string> GenerateJSONWebToken(UserModel userinfo)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,userinfo.UserName),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email,userinfo.EmailAddress),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(encodetoken);
        }

        //[Authorize]
        //[HttpPost("Post")]
        //public string Post()
        //{
        //    var Identity = HttpContext.User.Identity as ClaimsIdentity;
        //    IList<Claim> claim = Identity.Claims.ToList();
        //    var username = claim[0].Value;
        //    return "Welcome To: " + username;
        //}

        //[Authorize]
        [HttpGet("GetValue")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Value1", "Value2", "Value3" };
        }
    }
}
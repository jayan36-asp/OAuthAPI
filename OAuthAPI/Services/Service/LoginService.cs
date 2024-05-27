using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OAuthAPI.Models;
using OAuthAPI.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Configuration;
namespace OAuthAPI.Services.Service
{
    public class LoginService : ILoginService
    {
        public  readonly IConfiguration _config;

        public LoginService(IConfiguration config)
        {
            _config = config;
        }
        public loginResponse GenerateJwtToken(LoginRequest request)
        {
            var result = new loginResponse();

            if (users.Any(x => x.username.ToLower() == request.username.ToLower() && x.password == request.password))
            {
                var userDetails = users.Where(x => x.username.ToLower() == request.username.ToLower()).Select(x => x).FirstOrDefault();

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, request.username),
                    new Claim(ClaimTypes.Role, userDetails.role),
                    new Claim("Region", string.Join(",",userDetails.regions)),
                };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["jwt:key"]));
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["jwt:issuer"],
                    audience: _config["jwt:audience"],
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                );
                result.success = true;
                result.token = new JwtSecurityTokenHandler().WriteToken(token);
                return result;
            }
            else
            {
                result.success = false;
                return result;
            }
        }

        public UserModel GetUserDetail(string username)
        {
            return users.Where(x => x.username.ToLower() == username.ToLower()).Select(x => x).FirstOrDefault();
        }
        private readonly List<UserModel> users = new List<UserModel>
        {
            new UserModel { username = "Player", password = "player@23", role = "Player", regions = new string[]{ "b_game" } },
            new UserModel { username = "Admin", password = "admin@23", role = "Admin", regions = new string[]{ "b_game", "vip_character_personalize" } }
        };

    }
}

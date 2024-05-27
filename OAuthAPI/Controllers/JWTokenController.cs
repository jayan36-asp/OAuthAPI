using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAuthAPI.Models;
using OAuthAPI.Services.Interfaces;
using OAuthAPI.Services.Service;
using System.Diagnostics;

namespace OAuthAPI.Controllers
{
    public class JWTokenController : Controller
    {
        private readonly ILogger<JWTokenController> _logger;
        public ILoginService _loginService;

        public JWTokenController(ILogger<JWTokenController> logger, ILoginService loginService)
        {
            _logger = logger;
            _loginService = loginService;
        }

        public IActionResult LoginPage()
        {
            return View();
        }
       
        [HttpPost]
        public IActionResult Authorize([FromBody] LoginRequest request)
        {
            if (!string.IsNullOrEmpty(request.username) || !string.IsNullOrEmpty(request.password))
            {
                var result = _loginService.GenerateJwtToken(request);
                if (result.success)
                {
                    return Ok(new { result.success, result.token });
                }
            }
            return Unauthorized();
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUserDetails()
        {
            var username = User.Identity.Name;
            var user = _loginService.GetUserDetail(username);
            if (user == null)
                return Unauthorized();

            var userInfo = new
            {
                username = user.username,
                roles = new[] { user.role },
                regions = user.regions
            };
            return Ok(userInfo);
        }
        


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
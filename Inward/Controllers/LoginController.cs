using Inward.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Inward.Services.Abstraction;
using Inward.Common;

namespace Inward.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _userLoginService;
        private readonly IConfiguration _config;
        private readonly string _baseURL;
        private readonly string _Login;

        public LoginController(ILoginService userLoginService, IConfiguration config)
        {
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
            _config = config;
        }



        public IActionResult Login()
        {
            Response.Cookies.Delete("ProjectLevel");
            var cookieOptions = new CookieOptions();
            cookieOptions.Expires = DateTime.Now.AddDays(1);
            cookieOptions.Path = "/";

            Response.Cookies.Append("ProjectLevel", "0", cookieOptions);
            return View();
        }

        /// <summary>
        /// Authenticate whether user is correct or not
        /// </summary>
        /// <param name="userMaster">User's id & password</param>
        /// <returns>View</returns>
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> Login(UserMaster userMaster)

        {
            if (ModelState.IsValid)
            {
                UserMaster user = await _userLoginService.AuthenticateUser(userMaster);
                if (user == null || (user != null && user.UserId == 0) || (user != null && user.UserId == -2) || (user != null && user.UserId == -1))
                {
                    TempData["StatusMessage"] = "InvalidCredentials";
                    ViewBag.Message = CommonMethods.ConcatString("Invalid Credential",
                        Convert.ToString((int)CommonMethods.ResponseMsgType.error), "||");
                    return View(userMaster);
                }
                else
                {
                    string guid = Guid.NewGuid().ToString();

                    var cookieOptions = new CookieOptions();
                    cookieOptions.Expires = DateTime.Now.AddDays(1);
                    cookieOptions.Path = "/";
                    cookieOptions.HttpOnly = true;
                    cookieOptions.SameSite = SameSiteMode.Strict;

                    Response.Cookies.Append("AuthToken", guid, cookieOptions);
                    HttpContext.Session.SetString("AuthToken", guid.ToString());
                    var token = GenerateJSONWebToken();
                    {
                        var jwtToken = new JwtSecurityToken(token);
                        var claim = jwtToken.Claims;
                        var claims = new List<Claim>()
                   {
                      new Claim(ClaimTypes.NameIdentifier, Convert.ToString(user.UserId)),
                      new Claim(ClaimTypes.Name, user.UserName),

                      new Claim("ClaimAuthToken", guid),
                      new Claim("Token", token),
                      new Claim(JwtRegisteredClaimNames.Iss,jwtToken.Issuer),
                      new Claim(JwtRegisteredClaimNames.Aud,jwtToken.Audiences.FirstOrDefault()),



                  };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        var authproperties = new AuthenticationProperties
                        {
                            IsPersistent = false,
                            ExpiresUtc = DateTime.UtcNow.AddHours(2)
                        };
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
                            authproperties);


                    }
                }
            }
            return RedirectToAction("Index", "Home");

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Sign out the user from the authentication scheme
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear the session
            HttpContext.Session.Clear();

            // Delete relevant cookies (AuthToken, ProjectLevel, etc.)
            Response.Cookies.Delete("AuthToken");
            Response.Cookies.Delete("ProjectLevel");

            // Redirect to Login view
            return RedirectToAction("Login", "Login");
        }

        private string GenerateJSONWebToken()
        {
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtDurationMinutes = Convert.ToInt32(_config["Jwt:DurationInMinutes"]);

            if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer))
            {
                throw new InvalidOperationException("JWT configuration settings (Jwt:Key and Jwt:Issuer) are missing or empty.");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(jwtIssuer, jwtIssuer, null, expires: DateTime.Now.AddMinutes(jwtDurationMinutes), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

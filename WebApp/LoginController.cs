using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Collections.Generic;

namespace WebApp
{
    [Route("api")]
    public class LoginController : Controller
    {
        private readonly IAccountDatabase _db;

        public LoginController(IAccountDatabase db)
        {
            _db = db;
        }
        
        [HttpPost("sign-in")]
        public async Task<IActionResult> Login(string userName)
        {
            var account = await _db.FindByUserNameAsync(userName);
            if (account != null) {
                //*TODO 1: Generate auth cookie for user 'userName' with external id
                await authenticate(account);
                return Ok();
            } else {
                //*TODO 2: return 404 not found if user not found
                return NotFound();
            }
        }

        private async Task authenticate(Account user) {
            var claims = new List<Claim> {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.ExternalId),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),
            };
            ClaimsIdentity ci = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(ci));
        }
    }
}
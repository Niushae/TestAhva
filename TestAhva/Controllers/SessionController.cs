using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace TestAhva.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SessionController : ControllerBase
    {
        [HttpPost("keepalive")]
        public async Task<IActionResult> KeepAlive()
        {
            try
            {
                // Try to authenticate using the cookie scheme so we can re-issue the cookie with a refreshed expiration.
                var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                if (result?.Principal == null)
                {
                    return Unauthorized();
                }

                // Re-issue the authentication cookie with a new expiration to implement sliding expiration.
                var props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30) // match your configured session lifetime
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, result.Principal, props);
                return Ok();
            }
            catch
            {
                return StatusCode(500);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Controller
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }

    [Produces("application/json")]
    public class HelloController : ControllerBase
    {
        SignInManager<ApplicationUser> signInManager;
        public HelloController(SignInManager<ApplicationUser> s)
        {
            this.signInManager = s;
        }

        [Route("api/Hello")]
        public async Task<IActionResult> GetIndex()
        {
            var y = await signInManager.GetExternalAuthenticationSchemesAsync();
            var properties = signInManager.ConfigureExternalAuthenticationProperties(y.First().Name, "/api/Login");
            properties.Items.Add("response_type", "token");
            return new ChallengeResult(y.First().Name, properties);
            // return new JsonResult("Hello World");
        }

        [Route("/signin-facebook")]
        public async Task<IActionResult> GetIndex122(string code,string state)
        {
            HttpClient a = new HttpClient();
            var i = await a.GetAsync($"https://graph.facebook.com/v2.12/oauth/access_token?client_id=276094155791124&redirect_uri=http://localhost:5722/signin-facebook&client_secret=870c801b79236a0962cbd2c28ab9912b&code={code}");
            i.EnsureSuccessStatusCode();
            string responseBody = await i.Content.ReadAsStringAsync();
            string toke = "EAAD7Gy4UmxQBAISEmCgNebMOdEBQwvVNmfHcBMWQXhLjY5i9UPaRio6w0aJmOJs3EtcgZAoZBxxspB8ZCMtDEZCDtJ8Id9e5wJglUXen8ljFvCGt71pf7RPlAsH2ALNe7ZAqncMg5I7tFG3Enq3W2nZA1SX4s5D3IZD";
            var u = await a.GetAsync($"https://graph.facebook.com/v2.12/me?fields=id,name&access_token={toke}");
            responseBody = await u.Content.ReadAsStringAsync();

            var info = await signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return LocalRedirect("/api/Login");
            }

            // Sign in the user with this external login provider if the user already has a login.
            var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (result.Succeeded)
            {
                return LocalRedirect("/api/Protect");
            }
            return new LocalRedirectResult("/api/Protect");

            // return new JsonResult("signin-facebook");
        }


        [Route("api/Login")]
        public async Task<IActionResult> GetIndex12()
        {
            var info = await signInManager.GetExternalLoginInfoAsync();
            return new JsonResult("login");
        }

        [Authorize]
        [Route("api/Protect")]
        public IActionResult GetIndex1()
        {
            return new JsonResult("Protected");
        }
    }
}
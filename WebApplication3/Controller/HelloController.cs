using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApplication3.Controller
{
    [Produces("application/json")]
    public class HelloController : ControllerBase
    {
        [Route("api/Hello")]
        public async Task<IActionResult> GetIndex()
        {
            var properties = new AuthenticationProperties() { RedirectUri = "/api/Login" };
            properties.Items.Add("LoginProvider", "Facebook");
            return new ChallengeResult("Facebook", properties);
        }

        [Route("/signin-facebook")]
        public async Task<IActionResult> GetIndex122(string code,string state)
        {
            HttpClient a = new HttpClient();
            var i = await a.GetAsync($"https://graph.facebook.com/v2.12/oauth/access_token?client_id=276094155791124&redirect_uri=http://localhost:5722/signin-facebook&client_secret=870c801b79236a0962cbd2c28ab9912b&code={code}");
            string responseBody = await i.Content.ReadAsStringAsync();
            string toke = "EAAD7Gy4UmxQBAISEmCgNebMOdEBQwvVNmfHcBMWQXhLjY5i9UPaRio6w0aJmOJs3EtcgZAoZBxxspB8ZCMtDEZCDtJ8Id9e5wJglUXen8ljFvCGt71pf7RPlAsH2ALNe7ZAqncMg5I7tFG3Enq3W2nZA1SX4s5D3IZD";
            var u = await a.GetAsync($"https://graph.facebook.com/v2.12/me?fields=id,name&access_token={toke}");
            responseBody = await u.Content.ReadAsStringAsync();
            return new LocalRedirectResult("/api/Protect");
        }


        [Route("api/Login")]
        public async Task<IActionResult> GetIndex12()
        {
            return new JsonResult("login");
        }

        [ShouldBeFacebooked]
        [Route("api/Protect")]
        public IActionResult GetIndex1()
        {
            return new JsonResult("Protected");
        }
    }
}
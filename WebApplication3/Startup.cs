using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebApplication3.Controller;


namespace WebApplication3
{
    public class ShouldBeFacebooked : AuthorizeAttribute
    {
        public override bool IsDefaultAttribute()
        {
            return true;
        }

        public override bool Match(object obj)
        {
            return false;
        }
    }
       
    

    public class Startup
    {
      public void ConfigureServices(IServiceCollection services)
       {
            services.AddAuthentication().AddFacebook(facebookOption =>
            {
                facebookOption.AppId = "276094155791124";
                facebookOption.AppSecret = "870c801b79236a0962cbd2c28ab9912b";
            });

        ////services.AddAuthorization(options =>
        ////{
        ////    options.AddPolicy("ShouldBeFacebooked",
        ////        policy => policy.Requirements.Add(new ShouldBeFacebooked()));
        ////});


        services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
        }
    }
}

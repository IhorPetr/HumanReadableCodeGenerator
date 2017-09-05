using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using HUMANREADABLECODE.Algoritm;
using HUMANREADABLECODE.Services.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace HUMANREADABLECODE
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //JsonBuilder.ClearJson();
            //JsonBuilder.Add36Algoritm();
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("conf.json");
            // создаем конфигурацию
            AppConfiguration = builder.Build();
        }
        public IConfigurationRoot AppConfiguration { get; set; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRouting();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = AuthOptions.ISSUER,
                    ValidateAudience = true,
                    ValidAudience = AuthOptions.AUDIENCE,
                    ValidateLifetime = true,
                    IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                    ValidateIssuerSigningKey = true,
                }
            });
            RouteBuilder routeBuilder = new RouteBuilder(app);
            routeBuilder.MapGet("api/code/{id:int}", context =>
            {
                if (context.Authentication.HttpContext.User.Identity.IsAuthenticated)
                {
                    var res = AppConfiguration[context.GetRouteValue("id").ToString()];
                    return context.Response.WriteAsync(res == null ? (context.Response.StatusCode = 400).ToString() 
                    : $"{AppConfiguration["Codeprefix"]}{DateTime.Now.Month}{DateTime.Now.Year}-{res}");                   
                }
                return context.Response.WriteAsync((context.Response.StatusCode=401).ToString());
            });
            routeBuilder.MapPost("token/", context =>
            {
                var token = new  JwtTokenGenerator();
                var identy = token.GetIdentity(context.Request.Headers["username"], context.Request.Headers["password"]);
                if (identy == null)
                {
                    return context.Response.WriteAsync("Invalid username or password."); ;
                }
                var response = new
                {
                    access_token = token.GenerateToken(identy),
                    username = identy.Name
                };
                context.Response.ContentType= "application/json";
                return context.Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
            });

            app.UseRouter(routeBuilder.Build());
        }
    }

}

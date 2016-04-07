using System.IdentityModel.Tokens;
using System.Security.Cryptography;
using Microsoft.AspNet.Authentication.JwtBearer;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace WebAPIApplication
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddAuthentication();
            services.AddMvc();
            // services.AddMvc(config =>
            // {
//                var policy = new AuthorizationPolicyBuilder()
//                    .RequireAuthenticatedUser()
//                    .Build();
//                config.Filters.Add(new AuthorizeFilter(policy));
            // });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {                        
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsEnvironment("Development"))
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIISPlatformHandler();
             
            app.UseDefaultFiles();
            app.UseStaticFiles();

            var options = new JwtBearerOptions
            {
                Authority = "https://accounts.google.com",
                Audience = "342665198077-1fdticgpjke40gddj3r8vghltpgcvb5m.apps.googleusercontent.com",
                RequireHttpsMetadata = false,
                AutomaticAuthenticate = true,
                AutomaticChallenge = false,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = "accounts.google.com"
                }
            };

            app.UseJwtBearerAuthentication(options);

            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
    }
}

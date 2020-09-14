using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Configs;

namespace Portal
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            services.AddOptions();
#if DEBUG
            // Enable razor runtime compilation
            services.AddRazorPages().AddRazorRuntimeCompilation();
#endif
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
           .AddCookie("Cookies")
           .AddOpenIdConnect("oidc", options =>
           {

               var authorityServer = Configuration.GetSection("Authority").Get<AuthorityModel>();
#if DEBUG
               options.Authority = authorityServer.Debug;
#else
              options.Authority = authorityServer.Release;
#endif
               options.RequireHttpsMetadata = false;

               options.ClientId = authorityServer.ClientId;
               options.ClientSecret = authorityServer.ClientSecret;

               options.SaveTokens = authorityServer.SaveTokens;

               options.SignInScheme = authorityServer.SignInScheme;

               options.ResponseType = authorityServer.ResponseType;

               options.GetClaimsFromUserInfoEndpoint = authorityServer.GetClaimsFromUserInfoEndpoint;

               options.Events = new Microsoft.AspNetCore.Authentication.OpenIdConnect.OpenIdConnectEvents
               {
                   OnRedirectToIdentityProvider = context =>
                   {
                       context.ProtocolMessage.SetParameter("phone", context.Request.Query["phone"]);
                       context.ProtocolMessage.SetParameter("platform", context.Request.Query["platform"]);
                       return Task.FromResult(0);
                   }
               };

               options.Scope.Add("email");
               options.Scope.Add("profile");
               options.Scope.Add("roles");
               options.Scope.Add("openid");
           });

            // Config services
            services.AddCouchDBConfigService(Configuration);
            services.AddCouchDBContextService();
            services.AddAPILaoKYCService();
            services.AddMyAppService();
            services.AddApiResourceService();

            // Read configurations
            var dbConfig = new DBConfig();
            Configuration.GetSection("DBServer").Bind(dbConfig);
            services.AddSingleton(dbConfig);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

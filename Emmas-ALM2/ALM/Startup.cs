using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALM.Repository;
using ALM.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ALM
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Enviro { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            Enviro = env;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            if(Enviro.IsEnvironment("Development") || Enviro.IsEnvironment("Stage"))
            {
                services.AddTransient<IEmail, EmailSenderIntegrationStage>();
            }

            else if(Enviro.IsEnvironment("Production"))
            {
                services.AddTransient<IEmail, EmailSenderProduction>();
            }


            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSingleton<BankRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {

            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            else if (env.IsProduction())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

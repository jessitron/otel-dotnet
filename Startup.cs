using System;
using System.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace WebApplication
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
      
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddHttpClient();
          
          
           /* Tracing! */
            string headers = string.Format("x-honeycomb-team={0},x-honeycomb-dataset={1}", 
                                                        Environment.GetEnvironmentVariable("HONEYCOMB_API_KEY"),
                                                        Environment.GetEnvironmentVariable("HONEYCOMB_DATASET"));
            Console.WriteLine("Hello honeycomb! " + headers);  
            services.AddOpenTelemetryTracing((builder) => builder
                .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("my-service"))
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddConsoleExporter()          // console - uncomment this to see traces in the logs
                // honeycomb
                .AddOtlpExporter(otlpOptions =>
                {
                    otlpOptions.Endpoint = new Uri("https://api.honeycomb.io");
                    otlpOptions.Headers = headers;
                })
              );    
          /* end Tracing! */
            
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

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

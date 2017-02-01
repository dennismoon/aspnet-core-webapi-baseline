using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;

namespace AspNetCoreWebApiBaseline
{
    public class Startup
    {
        /// <summary>
        /// Configuration accesstor.
        /// </summary>
        public IConfigurationRoot Configuration { get; set; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // Add logging services
            services.AddLogging();

            // Needed for NLog.Web
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // Add MVC framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            // Add various loggers
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            loggerFactory.AddDebug();

            // Add NLog to .NET Core
            loggerFactory.AddNLog();

            // Enable ASP.NET Core features (NLog.web)
            app.AddNLogWeb();

            // Configure developer exception handling
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseStatusCodePages();
            }

            // Support loading static files
            app.UseStaticFiles();

            // Use default MVC routing rules
            app.UseMvcWithDefaultRoute();
        }
    }
}

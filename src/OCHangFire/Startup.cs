using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OCHangFire
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOrchardCms()
                .ConfigureServices(CmsTenantConfigureServices)
                .Configure(CmsTenantAppConfigure);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting(); // NOTE. Also don't use app.UseRouting(); in you main app startup, it is done when we built a tenant pipeline (per tenant)
            app.UseOrchardCore();
        }

        /// <summary>
        /// Extend OrchardCore service registrations.
        /// Reason:
        /// OrchardCore has a custom logic for the Dependency Injection to isolate registered services in it own tenant instance.
        /// So that, there are cases/library that can't resolve services from the OrchardCore.
        /// See problem: https://github.com/OrchardCMS/OrchardCore/issues/7437
        /// </summary>
        /// <param name="services"></param>
        private void CmsTenantConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(x => x.UseSqlServerStorage(_configuration.GetConnectionString("Hangfire")));
            services.AddHangfireServer();
        }

        /// <summary>
        /// Extend OrchardCore application/middleware builder
        /// Reason:
        /// OrchardCore has a custom logic for the Dependency Injection to isolate registered services in it own tenant instance.
        /// So that, there are cases/library that can't resolve services from the OrchardCore.
        /// See problem: https://github.com/OrchardCMS/OrchardCore/issues/7437
        /// </summary>
        /// <param name="app"></param>
        public void CmsTenantAppConfigure(IApplicationBuilder app)
        {
            app.UseHangfireServer();
            app.UseHangfireDashboard();
        }
    }
}

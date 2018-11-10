using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MicroService_Producer
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc();
            var consulclient = new ConsulClient(configuration =>
            {
                configuration.Datacenter = "dc1";
                configuration.Address = new Uri("http://192.168.1.23:8500");
            });
            var  pubport = 8800;
            consulclient.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                Address = "192.168.1.33",
                Port = pubport, 
                Check = new AgentServiceCheck()
                {
                    Interval = TimeSpan.FromSeconds(10), 
                    DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10), 
                    HTTP = $"http://192.168.1.33:{pubport}/api/health", 
                    Timeout = TimeSpan.FromSeconds(5)
                }, 
                ID = $"dotnetcoreservice{pubport}", Name = "dotnetcoreservice"
            });
        }
    }
}
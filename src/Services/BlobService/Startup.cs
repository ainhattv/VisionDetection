using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VDS.BlobService.Settings;
using Swashbuckle.AspNetCore.Swagger;
using VDS.BlobService.Common;
using AutoMapper;
using VDS.BlobService.Extensions;
using Microsoft.EntityFrameworkCore;
using VDS.BlobService.Data;
using VDS.BlobService.Adapters;
using VDS.BlobService.Interfaces;
using VDS.BlobService.Services;
using BlobService.ServiceBus;
using VDS.Logging;

namespace VDS.BlobService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<BlobContext>(options =>
                                      options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.Configure<BlobSettings>(Configuration.GetSection("BlobSetings"));
            services.Configure<ServiceBusSettings>(Configuration.GetSection("ServiceBusSettings"));

            services.AddTransient<IBlobAdapter, BlobAdapter>();
            services.AddTransient<IContainerService, ContainerService>();
            services.AddScoped(typeof(IAppLogger<>), typeof(LoggerAdapter<>));
            services.AddTransient<IServiceBusConsumer, ServiceBusConsumer>();

            // Add AutoMapper
            services.AddAutoMapper();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });

            services.AddMvcCore(options =>
            {
                options.Filters.Add(typeof(ValidateModelFilter));
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Blob API V1");
            });

            app.ConfigureExceptionHandler();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Register ServiceBus Receiver
            RegisterBlobReceiver(app);

            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void RegisterBlobReceiver(IApplicationBuilder app)
        {
            var bus = app.ApplicationServices.GetService<IServiceBusConsumer>();
            bus.RegisterOnMessageHandlerAndReceiveMessages();
        }
    }
}

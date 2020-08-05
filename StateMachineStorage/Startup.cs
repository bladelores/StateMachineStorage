using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using StateMachineStorage.Abstract.Repositories;
using StateMachineStorage.Abstract.Services;
using StateMachineStorage.Data;
using StateMachineStorage.Data.Repository;
using StateMachineStorage.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace StateMachineStorage
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "RSD StateMachine Storage Service API",
                    Version = $"v{Configuration["BuildVersion"].Split('.')[0]}"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.MapType(typeof(IFormFileCollection), () => new OpenApiSchema { Type = "file", Format = "binary" });
            });

            services.AddTransient<IEventLogRepository, EventLogRepository>();
            services.AddTransient<IStateMachineRepository, StateMachineRepository>();

            services.AddTransient<ILoggerService, LoggerService>();
            services.AddTransient<IStateMachineService, StateMachineService>();

            services.AddDbContext<StateMachineStorageContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:ConnectionString"]));

            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // Add our Config object so it can be injected
            //services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseDeveloperExceptionPage();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/v1/swagger.json", $"RSD StateMachine Storage Service API V{Configuration["BuildVersion"].Split('.')[0]}");
            });

            app.UseCors("AllowAll");

            app.UseMvc();

            var logger = loggerFactory.CreateLogger("Startup");
            logger.LogDebug("Service: " + Assembly.GetExecutingAssembly().GetName());
        }
    }
}

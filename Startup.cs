using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using DotNETCore_API_Documentation_Swagger.Controllers;
using DotNETCore_API_Documentation_Swagger.Controllers.V1;
using DotNETCore_API_Documentation_Swagger.Controllers.V2;

namespace DotNETCore_API_Documentation_Swagger
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
            services.AddRazorPages();
            services.AddMvcCore().AddApiExplorer();
            services.AddApiVersioning(c =>
            {
                c.ReportApiVersions = true;
                c.Conventions.Controller<LoginController>().HasApiVersion(new Microsoft.AspNetCore.Mvc.ApiVersion(1,0));
                c.Conventions.Controller<LoginV2Controller>().HasApiVersion(new Microsoft.AspNetCore.Mvc.ApiVersion(2, 0));
            });
            services.AddVersionedApiExplorer(c => // still need to know the difference between this and the api versioning
            {
                c.DefaultApiVersion = new ApiVersion(1, 0);
                c.AssumeDefaultVersionWhenUnspecified = true;
            });
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("V1", new OpenApiInfo
                { 
                    Version = "V1.0",
                    Title = "ToDo API"
                });
                c.SwaggerDoc("V2", new OpenApiInfo
                {
                    Version = "V2.0",
                    Title = "ToDo API"
                });

                c.ResolveConflictingActions(d => d.First());
                c.CustomSchemaIds(x=>x.FullName);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
           

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/V1/swagger.json", "V1.0");
                    c.SwaggerEndpoint("/swagger/V2/swagger.json", "V2.0");
                });

            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}"
                    );

            });
        }
    }
}

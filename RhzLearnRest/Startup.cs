using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RhzLearnRest.Data;
using RhzLearnRest.Domains.Interfaces;
using RhzLearnRest.Services;
using System;

namespace RhzLearnRest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string SampleAllowAnyOrigins = "_sampleAllowAnyOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;

            }).AddXmlDataContractSerializerFormatters();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddDbContext<RhzLearnRestContext>(options => options.UseSqlServer(
                Configuration.GetConnectionString("LocalData")));

            

            services.AddSwaggerGen(c =>
               c.SwaggerDoc("v1", new OpenApiInfo { Title = typeof(Startup).Namespace, Version = "V1" })
               );

            services.AddCors(options =>
                options.AddPolicy(SampleAllowAnyOrigins,
                builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                })
            );

            services.AddScoped<ICourseLibraryRepository, CourseLibraryRepository>();
            services.AddScoped<IDataManagerService, DataManagerService>();
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
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var exceptionHandlrFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlrFeature != null)
                        {
                            //do logging stuff
                        }
                        context.Response.StatusCode = 501;
                        await context.Response.WriteAsync("Something went wrong. Please try later.");
                    });
                });
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{typeof(Startup).Namespace} API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHttpsRedirection();

            app.UseCors(SampleAllowAnyOrigins);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

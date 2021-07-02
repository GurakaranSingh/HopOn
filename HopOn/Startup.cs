using HopOn.Data;
using HopOn.Model;
using HopOn.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HopOn
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //    services.Configure<IISServerOptions>(options =>
            //    {
            //        options.MaxRequestBodySize = null;// 3147483648;
            //    });

            //    services.Configure<KestrelServerOptions>(options =>
            //    {
            //        options.Limits.MaxRequestBodySize = null;// 3147483648; // if don't set default value is: 30 MB
            //    });
            //    services.Configure<FormOptions>(options =>
            //{
            //    options.ValueLengthLimit = int.MaxValue;
            //    options.MultipartBodyLengthLimit = 3147483648; // if don't set default value is: 128 MB
            //        options.MultipartHeadersLengthLimit = int.MaxValue;
            //});
            services.AddSignalR(e => {
                e.MaximumReceiveMessageSize = 1048576000;
            });
            //services.AddMvc(options => options.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddControllersWithViews();
            //services.AddHttpClient<IFileUploadServices, FileUploadServices>( client => client.BaseAddress = new Uri("https://localhost:44300/"));
            //services.AddCors(c =>
            //{
            //    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin());
            //});


            //services.AddProtectedBrowserStorage();
            #region Services 

            var fileHandlerType = Configuration.GetValue<string>("MySettings:FileHandlerType");
            switch (fileHandlerType.ToLower())
            {
                //case "minio":
                //    services.AddScoped<IFileHandler, MinioFileHandler>();
                //    break;
                case "awssdk":
                    services.AddScoped<IFileHandler, AWSFileHandler>();
                    break;
                    //case "awsapi":
                    //    services.AddScoped<IFileHandler, AWSApiFileHandler>();
                    //    break;
            }
            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContextPool<AppDBContext>(options => options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
            services.AddTransient<AppDBContext>();
            //services.AddSingleton<IUploadUtilityHelperServices, UploadUtilityHelperServices>();
            //services.AddHttpClient<IUploadUtilityHelperServices, UploadUtilityHelperServices>(client => client.BaseAddress = new Uri("https://localhost:44306/"));
            services.AddScoped<IUploadUtilityHelperServices, UploadUtilityHelperServices>();
            services.AddScoped<IProgressBarListServices, ProgressBarListServices>();

            #endregion

            #region Connection String  
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDBContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.Use(async (context, next) =>
            {
                context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = 1048576000;
                await next.Invoke();
            });
            db.Database.EnsureCreated();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors(options => options.AllowAnyOrigin());
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}

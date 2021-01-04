using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using DataLayer;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using DataLayer.Services;
using Microsoft.AspNetCore.Http;
using LettuceEncrypt;
using System.IO;
using UserMiddleware.Interfaces;
using UserMiddleware.Services;

namespace HTTPS_WebApiServer
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
            services.AddLettuceEncrypt().PersistDataToDirectory(new DirectoryInfo("D:/data/Lettuce/Ecnrypt"), "57247LaLol22"); ;
            //additional
            services.AddMvc();
            services.AddSession();
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IUserAccessService, UserAccessService>();
            services.AddTransient<IUserDataService, UserDataService>();
            services.AddMemoryCache();
            //Db connection
            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FreelancerContext>(OptionsBuilder
                => OptionsBuilder.UseNpgsql(sqlConnectionString)
                );

            services.AddControllersWithViews();
            //services.AddAuthentication(CookieA)
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            /*--------------------------ROUTING ZONE----------------------------*/
            app.UseHttpsRedirection();

            //add endpoint resolution middlware
            app.UseRouting();
            //security
            //app.UseAuthorization();

            /*-------------------------END ROUTING ZONE---------------------------*/
            app.UseEndpoints(endpoints =>
            {

                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("This main page of lab4 HTTPS Service." +
                        " Use /order for Customer and /applications for Freelancer");
                });
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

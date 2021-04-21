using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using DataLayer.Services;
using LettuceEncrypt;
using System.IO;
using UserMiddleware.Interfaces;
using UserMiddleware.Services;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System;
using FreelancerWeb.Authentication;
using FreelancerWeb.Authorization;

namespace FreelancerWeb
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
            services.AddMvc().AddJsonOptions(opts =>
            {
                opts.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            services.AddSession();

            //Add Auth middleware
            services.AddAuthentication()
                .AddFreelancerAuth<UserAccessService>(options =>
                {
                    options.Realm = "Freelancer App";
                    options.ForwardAuthenticate = FreelancerAuthDefaults.AuthenticationScheme;
                });
            services.AddAuthorization(options =>
            {
            });

            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IUserDataService, UserDataService>();
            services.AddMemoryCache();

            //Db connection
            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<FreelancerContext>(OptionsBuilder
                => OptionsBuilder.UseNpgsql(sqlConnectionString)
                );

            services.AddControllersWithViews();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                { 
                    Title = "Freelancer Rest Service",
                    Version = "v1"
                });

                var reference = new OpenApiReference
                {
                    Id = FreelancerAuthDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                };

                var secScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Auth",
                    Description = "Pass JWT Bearer token to the Authrozation section of header",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = reference
                };

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {secScheme, new string[] { } }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
            });
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
            app.UseAuthentication();
            app.UseAuthorization();

            /*-------------------------END ROUTING ZONE---------------------------*/
            app.UseEndpoints(endpoints =>
            {

                /*endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("This main page of lab4 HTTPS Service." +
                        " Use /order for Customer and /applications for Freelancer");
                });*/
                endpoints.MapDefaultControllerRoute();
            });

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                options.RoutePrefix = string.Empty;// "/swagger";
            });
        }
    }
}

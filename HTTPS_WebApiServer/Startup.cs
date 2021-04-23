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
            services.AddHttpsRedirection(options =>
            {
                options.HttpsPort = 443;
            });
            
            services.AddLettuceEncrypt(c =>
            {
                // Set domain names
                c.DomainNames = new[] { "hydra14.duckdns.org" };
                // Email to use as the contact
                c.EmailAddress = "glitchyhydra97@gmail.com";
                // Auto accept LetsEncrypt terms
                c.AcceptTermsOfService = true;
            }).PersistDataToDirectory(new DirectoryInfo(Directory.GetCurrentDirectory()), "57247LaLol22"); ;

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

            //Services
            services.AddTransient<IAuthorizationService, AuthorizationService>();
            services.AddTransient<IUserDataService, UserDataService>();
            services.AddMemoryCache();
            //Db connection
            const string connectionTemplate = "Host={0};Database={1};Username={2};Password={3}";
            var host = Configuration["DB_HOST"]     ?? "localhost";
            var db   = Configuration["DB_DATABASE"] ?? "freelancer_bd";
            var user = Configuration["DB_USER"]     ?? "postgres";
            var pass = Configuration["DB_PASS"]     ?? "5690";
            var sqlConnectionString = string.Format(connectionTemplate, host, db, user, pass);
            services.AddDbContext<FreelancerContext>(OptionsBuilder
                => OptionsBuilder.UseNpgsql(sqlConnectionString));
            //Controllers
            services.AddControllersWithViews();
            //Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                { 
                    Title = "Freelancer Rest Service",
                    Version = "v1"
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
                c.OperationFilter<AuthOperationAttribute>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                Console.WriteLine("Cert Directory: " + Directory.GetCurrentDirectory());
                app.UseDeveloperExceptionPage();
            }

            /*--------------------------ROUTING ZONE----------------------------*/
            //app.UseHttpsRedirection();

            //add endpoint resolution middlware
            app.UseRouting();

            //security
            app.UseAuthentication();
            app.UseAuthorization();

            /*-------------------------END ROUTING ZONE---------------------------*/
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            app.UseSwagger(c =>
            {
                
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Freelancer API V1");
                options.RoutePrefix = string.Empty;
            });
        }
    }
}

using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.IO;
using System.Net;
using System.Security.Authentication;

namespace HTTPS_WebApiServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
             var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddEnvironmentVariables()
            .AddJsonFile("certificate.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"certificate.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true)
            .Build();

            new WebHostBuilder()
                .UseKestrel(options =>
                {
                    var appServices = options.ApplicationServices;
                    options.ConfigureHttpsDefaults(h =>
                    {
                        h.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                        h.SslProtocols = SslProtocols.Tls12;
                        h.UseLettuceEncrypt(appServices);
                    });
                    options.Listen(IPAddress.Parse("192.168.0.200"), 443, listenOptions =>
                    {
                        var appServices = options.ApplicationServices;
                        listenOptions.UseHttps(h =>
                        {
                            h.UseLettuceEncrypt(appServices);
                        });
                    });
                    options.Listen(IPAddress.Parse("192.168.0.200"), 80, listenOptions =>
                    {
                    });
                })
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls("https://glitchyhydra.ddns.net:443", "http://glitchyhydra.ddns.net:80")
                .Build()
                .Run();
        }
    }
}

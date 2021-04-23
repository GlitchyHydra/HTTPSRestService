using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using System.IO;
using System.Net;
using System.Security.Authentication;

namespace FreelancerWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var HttpPort = 8002;
            var HttpsPort = 8003;
            var domainName = "hydra14.duckdns.org";
            var urlHttp = String.Format("http://{0}:{1}", domainName, HttpPort.ToString());
            var urlHttps = String.Format("http://{0}:{1}", domainName, HttpsPort.ToString());

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
                        h.UseLettuceEncrypt(appServices);
                        h.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                        h.SslProtocols = SslProtocols.Tls12;
                    });
                    options.ListenAnyIP(HttpPort, listenOptions =>
                    {

                    });
                    options.ListenAnyIP(HttpsPort, listenOptions =>
                    {
                        var appServices = options.ApplicationServices;
                        listenOptions.UseHttps(h =>
                        {
                            h.UseLettuceEncrypt(appServices);
                        });
                    });
                })
                .UseConfiguration(config)
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseStartup<Startup>()
                .UseUrls(urlHttp, urlHttps)
                .Build()
                .Run();
        }
    }
}

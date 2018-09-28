using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Tutorial
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .Build();

            string http = config["Binding:HttpPort"].ToString();
            string https = config["Binding:HttpsPort"].ToString();
            string cert_file = config["Binding:CertFileName"].ToString();
            string cert_pass = config["Binding:CertPassword"].ToString();
            string[] ports = new string[] { http, https, cert_file, cert_pass };

            BuildWebHost(args, ports).Run();
        }

        /*UseUrls no puede ser usado cuando se necesita habilitar un endpoint Https, para estos casos es necesario usar
         UseKestrel, aunque ahora bien, quizas todo esto no tenga sentido porque realmente no se deberia exponer Kestrel directamente
         sino un reverse proxy como nginx*/
        public static IWebHost BuildWebHost(string[] args, string[] ports) =>
            WebHost.CreateDefaultBuilder(args)    
                .UseKestrel(options =>
                {
                    options.Listen(IPAddress.Any, Int32.Parse(ports[0]));
                    options.Listen(IPAddress.Any, Int32.Parse(ports[1]), listenOptions =>
                    {
                        listenOptions.UseHttps(ports[2], ports[3]);
                    });
                })
                .UseStartup<Startup>()
                .Build();
    }
}

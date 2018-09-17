﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Tutorial.Models;
using Tutorial.Utils;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Tutorial
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //Se carga la configuracion desde el archivo appsettings
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Saco la cadena de conexion del archivo de configuracion
            string conn = Configuration["Database:ConnectionString"].ToString();
            //Agrego el archivo de configuracion para que sea accesible desde cualquier clase
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<DataContext>(options => options.UseSqlServer(conn));
            services.AddMvc().AddJsonOptions(options => options.SerializerSettings.ContractResolver = new  DefaultContractResolver())
                .AddMvcOptions(options => options.Filters.Add(typeof(ActionFilter)));

            //Se autentica el token en cada solicitud
            string strKey = Configuration["Security:Secret"].ToString();
            var key = Encoding.ASCII.GetBytes(strKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {                
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Para que incorpore la autenticacion por token 
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "", defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}

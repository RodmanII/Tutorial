using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Tutorial.Models;
using Tutorial.Utils;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using System.IO;

namespace Tutorial
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            //Se carga la configuracion desde el archivo appsettings
            var builder = new ConfigurationBuilder().SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }
        public CultureInfo DefaultCulture { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Saco la cadena de conexion del archivo de configuracion
            string conn = Configuration["Database:ConnectionString"].ToString();
            //Agrego el archivo de configuracion para que sea accesible desde cualquier clase
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddDbContext<DataContext>(options => options.UseSqlServer(conn));

            //Para que los mensajes de error generados por la validacion de modelos sean devueltos en español
            services.AddLocalization(options => { options.ResourcesPath = "Resources"; });

            services.AddMvc()
            .AddJsonOptions(options => options.SerializerSettings.ContractResolver = new  DefaultContractResolver())
            .AddMvcOptions(options => options.Filters.Add(typeof(ActionFilter))).AddDataAnnotationsLocalization(options =>
            {
                options.DataAnnotationLocalizerProvider = (type, factory) =>
                    factory.Create(typeof(SharedMessages));
            });

            //Establezco la cultura para que use el archivo de traducciones que define en Resources
            string cult = Configuration["Culture"].ToString();
            DefaultCulture = new CultureInfo(cult);

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("tutorial", new Info
                {
                    Title = "Net Core Tutorial",
                    Version = "1.0",
                    Description = "Api de practica creada para aprender NET CORE",
                    Contact = new Contact() { Email = "grijalva.rodrigo@treming.com", Url = "www.treming.com" }
                });

                /*Esto es para que a las peticiones les incorpore un campo para enviar la cabecera
                Authorization */
                c.OperationFilter<HeaderFilter>();

                //Se establece la ruta del archivo de comentarios para swagger
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseStaticFiles();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/tutorial/swagger.json", "API de Practica");
                //Esto para que en la ruta base de la api muestre a swagger
                c.RoutePrefix = "";
            });

            var supportedCultures = new[] { DefaultCulture };
            app.UseRequestLocalization(new RequestLocalizationOptions()
            {
                DefaultRequestCulture = new RequestCulture(DefaultCulture),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //Para que incorpore la autenticacion por token 
            app.UseAuthentication();

            /*Esto lo hacia para mostrar el contenido de un archivo, estatico, pero ahora en la ruta base va a estar swagger
            asi que ya no es necesario*/
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "", defaults: new { controller = "swagger" });
            });
        }
    }
}

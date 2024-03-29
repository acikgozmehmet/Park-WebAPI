using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Park.Data;
using Park.Repository;
using Park.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Park.ParkMapper;
using System.Reflection;
using System.IO;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Park
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(); 
            services.AddDbContext<ApplicationDbContext>
                (options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddAutoMapper(typeof(ParkMappings));
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;   
            });

            services.AddVersionedApiExplorer(options => options.GroupNameFormat = "'v'VVV");
            services.AddTransient <IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            var appsettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appsettingsSection);

            var appSettings = appsettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);


            services.AddAuthentication(x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(x=>
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


            //services.AddSwaggerGen(options => {
            //    options.SwaggerDoc("ParkOpenAPISpec", new Microsoft.OpenApi.Models.OpenApiInfo()
            //    {
            //        Title = "Park API",
            //        Version = "1",
            //        Description = "National Park API",
            //        Contact =  new Microsoft.OpenApi.Models.OpenApiContact()
            //        { 
            //            Email = "",
            //            Name = "Mehmet Acikgoz",
            //            Url = new Uri("https://github.com/acikgozmehmet")
            //        },
            //        License = new Microsoft.OpenApi.Models.OpenApiLicense()
            //        {
            //            Name = "MIT License",
            //            Url=new Uri("https://en.wikipedia.org/wiki/MIT License")
            //        }
            //    });

            //    var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}API.xml";
            //    var cmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
            //    options.IncludeXmlComments(cmlCommentFullPath);
            //});

            //services.AddControllers();
            // API Json response to C# Object with capital case properties first letter
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var desc in provider.ApiVersionDescriptions)            
                    options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());

                options.RoutePrefix = "";
            });

            //app.UseSwaggerUI(options =>
            //{
            //    options.SwaggerEndpoint("/swagger/ParkOpenAPISpec/swagger.json", "Park API");
            //    options.RoutePrefix= "";
            //});
            app.UseRouting();

            app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

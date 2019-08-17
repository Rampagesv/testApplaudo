using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Reflection;
using System.Text;
using testApplaudo.Models;

namespace testApplaudo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // Add-Migration InitialCreate
        // Update-Database
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // adicionando coneccionstring
            services.AddDbContext<testApplaudoContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("testApplaudoContext")));
            // Adicioando Seguridad
            services.AddIdentity<IdentityUser, IdentityRole>(
                Option =>
                {
                    Option.Password.RequireDigit = false;
                    Option.Password.RequiredLength = 8;
                    Option.Password.RequireNonAlphanumeric = false;
                    Option.Password.RequireUppercase = false;
                    Option.Password.RequireLowercase = false;
                }
                ).AddEntityFrameworkStores<testApplaudoContext>().AddDefaultTokenProviders();

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = Configuration["Jwt:Site"],
                    ValidIssuer = Configuration["Jwt:Site"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SigningKey"]))
                };
            });
            // Swagger documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "Test Applaudo Store API coding challenge",
                    Version = "v1"
                });

                // XML Documentation
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                // Annotations
                c.EnableAnnotations();
            });

        }
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwagger(c =>
            {
                c.RouteTemplate = "docs/{documentName}/docs.json";
            });

            // Swagger UI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/docs/v1/docs.json", "API v1");
                c.RoutePrefix = "docs";
            });



            app.UseHttpsRedirection();
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}

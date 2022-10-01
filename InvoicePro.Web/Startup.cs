using InvoicePro.Application.Helper;
using InvoicePro.Application.ViewModels;
using InvoicePro.Domain.Enums;
using InvoicePro.Infra.Data.Context;
using InvoicePro.Infra.IoC;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Rotativa.AspNetCore;
using System;
using System.Text;

namespace InvoicePro.Web
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
            services.AddDbContext<AssetProDbContext>(options =>
              options.UseSqlServer(Configuration.GetConnectionString(Enum.GetName(typeof(DbConnection), DbConnection.InvoiceProConnection_Local))));

            services.AddCors(options =>
            {
                options.AddPolicy("AllowsAll", builder =>
                {
                    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });

            RegisterServices(services);
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                x.RequireAuthenticatedSignIn = true;
            })
             .AddJwtBearer(options =>
             {
                 options.RequireHttpsMetadata = false;
                 options.SaveToken = true;
                 options.RequireHttpsMetadata = false;
                 options.TokenValidationParameters = JwtAuthentication.GetValidatorParameters(key);
             });

            ConfigureAutomapper(services);

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.Configure<MailSettings>(Configuration.GetSection("MailSettings"));
        }

        private void ConfigureAutomapper(IServiceCollection services)
        {
            AutomapperConfig.Config(services);
        }

        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, Microsoft.AspNetCore.Hosting.IHostingEnvironment _env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthorization();
            app.UseCors(options => options.AllowAnyOrigin());
            app.UseSession();
            app.Use(async (context, next) =>
            {
                var JWToken = context.Session.GetString("token");
                if (!string.IsNullOrEmpty(JWToken))
                {
                    context.Request.Headers.Add("Authorization", "Bearer " + JWToken);
                }
                await next();
            });
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            RotativaConfiguration.Setup(_env, _env.WebRootPath + "\\Rotativa");
        }

        private void RegisterServices(IServiceCollection services)
        {
            DependencyContainer.RegisterServices(services);
        }
    }
}
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebUI.CustomClaimProvider;
using WebUI.CustomValidation;
using WebUI.Models;

namespace AspNetCoreIdentiyCourse
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
            services.AddDbContext<UdemyIdentityContext>(opt =>
            {
                opt.UseSqlServer(Configuration["ConnectionStrings:DefaultConnectionString"]);
            });

            services.AddScoped<IClaimsTransformation, ClaimProvider>();

            services.AddAuthorization(opts =>
            {
                opts.AddPolicy("CityPolicy", policy =>
                {
                    policy.RequireClaim("city", "ankara");  //city ankara olmak zorunda.
                });
            });

            services.AddIdentity<AppUser, AppRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
                opts.User.AllowedUserNameCharacters = "abcdefghýijklmnoöpqrsþtuüvwxyzABCDEFGHIÝJKLMNOÖPQRSTUÜVWXYZ0123456789-._@+";
                opts.Password.RequiredLength = 6;
                opts.Password.RequireNonAlphanumeric = true;
                opts.Password.RequireDigit = true;
                opts.Password.RequiredUniqueChars = 1;
            })
                .AddPasswordValidator<CustomPasswordValidator>() //Password Validator Add
                .AddUserValidator<CustomUserValidator>() //User Validator Add
                .AddErrorDescriber<CustomIdentityErrorDescriber>() //Hata Mesajlarý Türkçeleþtirilmesi
                .AddDefaultTokenProviders() //Token servisi
                .AddEntityFrameworkStores<UdemyIdentityContext>();
            CookieBuilder cookieBuilder = new CookieBuilder();
            cookieBuilder.Name = "MyBlog";
            cookieBuilder.HttpOnly = false;
            cookieBuilder.SameSite = SameSiteMode.Lax; //(CSRF)Siteler Arasý Ýstek Hýrsýzlýðý Default deðerinde býraktýk.
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest; //Http ve Https gibi konfigürsayonlar için ayarlanýr.

            services.ConfigureApplicationCookie(opts =>
            {
                opts.LoginPath = "/Home/Login";
                opts.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                opts.Cookie = cookieBuilder;
                opts.SlidingExpiration = true; //Cookie süresi yarsýný geçtiyse ve kullanýcý sisteme yine bir login iþlemi yaparsa eski cookie tut.
                opts.AccessDeniedPath = new PathString("/Member/AccessDenied"); //Üyeler arasý  Rol bazlý sayfalar arasý kýsýtlama
            });

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}

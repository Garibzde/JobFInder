using JobFind.DAL;
using JobFind.Interfaces;
using JobFind.Models;
using JobFind.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace JobFind
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<JobFindContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
            builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
                opt.Password.RequiredLength = 3;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.MaxFailedAccessAttempts = 2;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            }).AddEntityFrameworkStores<JobFindContext>().AddDefaultTokenProviders();
            builder.Services.AddScoped<IEmailService,EmailService>();

            var app = builder.Build();

           
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.MapControllerRoute("areas", "{area:exists}/{controller=Admin}/{action=Index}/{id?}");


            app.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}

using BZLAND.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BZLAND
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AddDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.Configure<IdentityOptions>(opt =>
            {
                opt.Password.RequireNonAlphanumeric=true;
                opt.Password.RequireDigit=true;
                opt.Password.RequireLowercase=true;
                opt.Password.RequireUppercase=true;
                opt.Password.RequiredLength=8;

                opt.User.RequireUniqueEmail=true;

                opt.Lockout.AllowedForNewUsers=true;
                opt.User.RequireUniqueEmail=true;

            });
            var connectionString = builder.Configuration.GetConnectionString("Default");
            builder.Services.AddDbContext<AddDbContext>(x => x.UseSqlServer(connectionString));

            var app = builder.Build();


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {


                endpoints.MapControllerRoute(
                  name: "areas",
                  pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}"
                  );


                endpoints.MapDefaultControllerRoute();

                app.Run();
            });
        }
    }
}
    
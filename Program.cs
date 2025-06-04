using Microsoft.Extensions.FileProviders;
// using WebApp_Feed;
using WebApp_Landing;
using WebApp_Feed.Areas.Feed.Database;
using Microsoft.EntityFrameworkCore;


namespace WebApp_EFDB
{
    public static class MiddlewareStaticExtension
    {
        public static IApplicationBuilder UseLocalStaticFiles(this IApplicationBuilder app, string projectFolder)
        {
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), projectFolder, "wwwroot"))
            });
            return app;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Feed Controllers
            // builder.Services.AddFeedControllers();
            // builder.Services.AddFeedDatabase(builder.Configuration.GetConnectionString("SQLiteConnection"));

            

            builder.Services.AddDbContext<GreenswampContext>(options =>
                 options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection")));

            

            // Add Landing Pages
            builder.Services.AddLandingPages();

            builder.Services.AddHttpClient();


            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler();
            }
            

            // creating a database if there is none

            using (var scope = app.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<GreenswampContext>();
                dbContext.Database.EnsureCreated();
            }


            // Add static files from feed
            app.UseLocalStaticFiles("WebApp-Feed");

            // Add static files from landing
            app.UseLocalStaticFiles("WebApp-Landing");

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapAreaControllerRoute(
                    name: "feed_area",
                    areaName: "Feed",
                    pattern: "/Feed/{controller=Home}/{action=Index}/{id?}");

            });

            app.Run();
        }
    }
}
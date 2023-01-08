using Hangfire;
using HtmlToPdfConverter.BL.Services;
using HtmlToPdfConverter.DAL.Managers;
using HtmlToPdfConverter.WEB.Startup;

namespace HtmlToPdfConverter.WEB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var redisConnectionString = builder.Configuration.GetValue<string>("RedisConnectionString");
            builder.Services.AddRedis(redisConnectionString);

            builder.Services.AddScoped<IConverterService, ConverterService>();
            builder.Services.AddScoped<ISessionManager, RedisSessionManager>();
            builder.Services.AddScoped<IFileManager, MinioFileManager>();
            builder.Services.AddSingleton<IJobQueue, HangfireJobQueue>();

            var minioEndpoint = builder.Configuration.GetSection("Minio").GetValue<string>("Endpoint");
            var minioAccessKey = builder.Configuration.GetSection("Minio").GetValue<string>("AccessKey");
            var minioSecretKey = builder.Configuration.GetSection("Minio").GetValue<string>("SecretKey");
            builder.Services.AddMinio(minioEndpoint, minioAccessKey, minioSecretKey);

            var msSqlConnectionString = builder.Configuration.GetValue<string>("MSSQLConnectionString");
            builder.Services.AddHangfire(msSqlConnectionString);

            builder.Services.AddUploadSourceFileSaga();
            builder.Services.AddConvertToPdfSaga();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHangfireDashboard(); // Should be removed before deploying into prod. Dashboard should require an authentication and we don't have one.

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Convert}/{id?}");
            
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Convert}");
            
            app.Run();
        }
    }
}
using Microsoft.Extensions.DependencyInjection;
using ProjectLogistics.Core.Services;
using ProjectLogistics.Data;
using ProjectLogistics.Data.Repositories;

namespace ProjectLogistics.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddJsonFile("appsettings.json");

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.Configure<RouteOptions>(options =>
            {
                // Ensure all endspoints are lower case
                options.LowercaseUrls = true;
            });

            builder.Services.AddSingleton(new DatabaseConfiguration { ConnectionString = builder.Configuration["DatabaseConnectionString"] });
            builder.Services.AddSingleton<IPackageService, PackageService>();
            builder.Services.AddSingleton<IWarehouseService, WarehouseService>();
            builder.Services.AddSingleton<IPackageRepository, PackageRepository>();
            builder.Services.AddSingleton<IWarehouseRepository, WarehouseRepository>();
            builder.Services.AddSingleton<IDatabaseSetup, DatabaseSetup>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Call the database setup method
            app.Services.GetService<IDatabaseSetup>()!.Setup();

            app.Run();
        }
    }
}
using Amazon.DynamoDBv2;
using Amazon.SimpleNotificationService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using WebAdvert.Api.Mapping;
using WebAdvert.Api.Services;

namespace WebAdvert.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();

            builder.Services.AddHealthChecks().AddCheck<StorageHealthCheck>("st");


            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(AdvertProfile));
            builder.Services.AddSingleton<IAmazonDynamoDB, AmazonDynamoDBClient>();
            builder.Services.AddSingleton<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
            builder.Services.AddTransient<IAdvertStorageService, DynamoAdvertStorageService>();
            builder.Services.AddSingleton<IMessagePublisher, MessagePublisher>();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
           
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
            }
            app.UseSwaggerUI();
            app.UseAuthorization();

            app.UseHealthChecks("/health/check");
            app.MapControllers();

            app.Run();
        }
    }
}
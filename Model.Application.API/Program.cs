using Model.Application.API.Extensions;
using Serilog;

namespace Model.Application.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, loggerConfig) =>
            loggerConfig.ReadFrom.Configuration(context.Configuration));

            builder.Services
                .AddConfig(builder.Configuration)
                .AddDB(builder.Configuration)
                .AddInterfaces()
                .UseTimeouts();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowOrigin");

            app.UseHttpsRedirection();

            app.UseSerilogRequestLogging();

            app.UseRequestTimeouts();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}


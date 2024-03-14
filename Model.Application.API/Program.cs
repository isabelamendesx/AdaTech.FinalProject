
using IdempotentAPI.Cache.DistributedCache.Extensions.DependencyInjection;
using Identity;
using Identity.Interfaces;
using Identity.Services;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Application.API.Extensions;
using Model.Application.API.Filters;
using Model.Application.API.Middlewares;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using Model.Infra.Data.Repositories;
using Model.Service.Services;
using Serilog;
using System.Net;
using Way2Commerce.Api.Extensions;

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

            app.UseExceptionHandler();

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


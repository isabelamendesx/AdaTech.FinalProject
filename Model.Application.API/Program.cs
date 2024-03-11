
using IdempotentAPI.Cache.DistributedCache.Extensions.DependencyInjection;
using Identity;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.EntityFrameworkCore;
using Model.Application.API.Filters;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using Model.Infra.Data.Repositories;
using Model.Service.Services;

namespace Model.Application.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
            builder.Services.AddProblemDetails();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddIdempotentAPIUsingDistributedCache();


            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddDbContext<IdentityDataContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IRefundService, RefundService>();
            builder.Services.AddScoped<IRuleService, RuleService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IRepository<Refund>, RefundRepository>();
            builder.Services.AddScoped<IRepository<RefundOperation>, RefundOperationRepository>();
            builder.Services.AddScoped<IRepository<Category>, CategoryRepository>();
            builder.Services.AddScoped<IRepository<Rule>, RuleRepository>();
            builder.Services.AddScoped<ILogger, Logger<Refund>>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}

using IdempotentAPI.Cache.DistributedCache.Extensions.DependencyInjection;
using Identity;
using Identity.Interfaces;
using Identity.Services;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Model.Application.API.Filters;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using Model.Infra.Data.Repositories;
using Model.Service.Services;
using System.Net;
using Way2Commerce.Api.Extensions;

namespace Model.Application.API.Extensions
{
    public static class ServiceSetup
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllers();
            services.AddRequestTimeouts(options =>
            {
                options.DefaultPolicy = new RequestTimeoutPolicy()
                {
                    Timeout = TimeSpan.FromMilliseconds(5000),
                    TimeoutStatusCode = (int) HttpStatusCode.RequestTimeout
                };
            });
            services.AddSwagger();
            services.AddAuthorizationPolicies();
            services.AddAuthentication(config);
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();

            services.AddDistributedMemoryCache();
            services.AddIdempotentAPIUsingDistributedCache();

            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<IdentityDataContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyHeader()
                               .AllowAnyMethod();
                    });
            });

            return services;
        }

        public static IServiceCollection AddInterfaces (this IServiceCollection services)
        {
            services.AddDefaultIdentity<IdentityUser>()
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<IdentityDataContext>()
                            .AddDefaultTokenProviders();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IRefundService, RefundService>();
            services.AddScoped<IRuleService, RuleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRepository<Refund>, RefundRepository>();
            services.AddScoped<IRepository<RefundOperation>, RefundOperationRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Rule>, RuleRepository>();
            services.AddScoped<ILogger, Logger<Refund>>();

            return services;
        }
    }
}

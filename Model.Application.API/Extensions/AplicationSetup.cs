using IdempotentAPI.Cache.DistributedCache.Extensions.DependencyInjection;
using IdempotentAPI.Extensions.DependencyInjection;
using Identity;
using Identity.Interfaces;
using Identity.Services;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Model.Application.API.Filters;
using Model.Application.API.Util;
using Model.Domain.Entities;
using Model.Domain.Interfaces;
using Model.Infra.Data.Context;
using Model.Infra.Data.Repositories;
using Model.Service.Services;
using System.Net;
using Way2Commerce.Api.Extensions;

namespace Model.Application.API.Extensions
{
    public static class AplicationSetup
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config) 
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ExceptionFilter>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });   
            services.AddSwagger();
            services.AddAuthorizationPolicies();
            services.AddAuthentication(config);
            services.AddProblemDetails();

            services.AddIdempotentAPI();
            services.AddDistributedMemoryCache();
            services.AddIdempotentAPIUsingDistributedCache();

            services.AddHttpContextAccessor();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowOrigin",
                                       builder =>
                                       {
                        builder.AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader();
                    });
            });

            return services;
        }

        public static IServiceCollection AddDB(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DataContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<IdentityDataContext>(options =>
            {
                options.UseNpgsql(config.GetConnectionString("DefaultConnection"));
            });

            services.AddDefaultIdentity<IdentityUser>()
                            .AddRoles<IdentityRole>()
                            .AddEntityFrameworkStores<IdentityDataContext>()
                            .AddDefaultTokenProviders();

            return services;
        }

        public static IServiceCollection AddInterfaces (this IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IRefundService, RefundService>();
            services.AddScoped<IRuleService, RuleService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IRepository<Refund>, RefundRepository>();
            services.AddScoped<IRepository<RefundOperation>, RefundOperationRepository>();
            services.AddScoped<IRepository<Category>, CategoryRepository>();
            services.AddScoped<IRepository<Rule>, RuleRepository>();
            services.AddScoped<ILogger, Logger<Refund>>();
            services.AddScoped<IUserAccessor, UserAccessor>();

            return services;
        }
    }
}

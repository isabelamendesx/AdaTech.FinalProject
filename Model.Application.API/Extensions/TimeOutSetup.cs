using Microsoft.AspNetCore.Http.Timeouts;
using System.Net;

namespace Model.Application.API.Extensions
{
    public static class TimeOutSetup
    {
        public static IServiceCollection UseTimeouts(this IServiceCollection services)
        {
            services.AddRequestTimeouts(options =>
            {
                options.DefaultPolicy = new RequestTimeoutPolicy()
                {
                    Timeout = TimeSpan.FromMilliseconds(5000),
                    TimeoutStatusCode = (int)HttpStatusCode.RequestTimeout
                };
            });

            return services;
        }
    }
}

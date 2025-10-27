using Org.BouncyCastle.Security;
using System.Threading.RateLimiting;

namespace Api.Extensions
{
    internal static class RateLimiterExtensions
    {
        internal static IServiceCollection AddRateLimiterServices(this IServiceCollection services)
        {
            const string GetPolicyName = "SlidingGet";
            const string ModifyPolicyName = "SlidingModify";

            services.AddRateLimiter(options =>
            {
                options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
                
                options.AddPolicy(GetPolicyName, httpContext =>
                    RateLimitPartition.GetSlidingWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: partition => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 100,
                            Window = TimeSpan.FromMinutes(1),
                            SegmentsPerWindow = 6                           
                        }
                    )
                );

                options.AddPolicy(ModifyPolicyName, httpContext =>
                    RateLimitPartition.GetSlidingWindowLimiter(
                        partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                        factory: partition => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 20,
                            Window = TimeSpan.FromMinutes(1),
                            SegmentsPerWindow = 4
                        }
                    )
                );
            });

            return services;
        }
    }
}

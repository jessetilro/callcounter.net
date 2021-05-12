using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Callcounter.Net
{
    public static class CallcounterMiddleWareExtensions
    {
        public static IServiceCollection AddCallcounter(this IServiceCollection serviceCollection)
        {
            return serviceCollection.AddHttpClient();
        }
        
        public static IApplicationBuilder UseCallcounter(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CallcounterMiddleWare>();
        }
    }
}
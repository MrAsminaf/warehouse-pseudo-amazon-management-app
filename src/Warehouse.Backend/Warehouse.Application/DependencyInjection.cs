using Microsoft.Extensions.DependencyInjection;

namespace Warehouse.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        return services;
    }
}
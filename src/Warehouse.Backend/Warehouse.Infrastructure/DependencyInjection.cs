using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Warehouse.Infrastructure.Data;

namespace Warehouse.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, 
        IConfiguration configuration)
    {
        if (!string.IsNullOrEmpty(configuration.GetConnectionString("WarehouseDb")))
        {
            services.AddDbContext<ApplicationContext>(options => 
                options.UseNpgsql(configuration.GetConnectionString("WarehouseDb")));
        }

        return services;
    }
}
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MudBlazor.Services
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a Drawer Service as a Scoped instance.
        /// </summary>
        /// <param name="services">IServiceCollection</param>
        /// <returns>Continues the IServiceCollection chain.</returns>
        public static IServiceCollection AddMudBlazorDrawerService(this IServiceCollection services)
        {
            services.TryAddScoped<IDrawerService, DrawerService>();
            return services;
        }
    }
}

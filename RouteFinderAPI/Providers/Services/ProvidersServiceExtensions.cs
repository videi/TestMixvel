namespace RouteFinderAPI.Providers.Services
{
    public static class ProvidersServiceExtensions
    {
        /// <summary>
        /// Метод расширения для настройки сервисов провайдера в контейнере зависимостей.
        /// </summary>
        /// <param name="services">Коллекция сервисов, в которую будут добавлены новые сервисы.</param>
        /// <returns>
        /// Возвращает обновлённую коллекцию сервисов <see cref="IServiceCollection"/>.
        /// </returns>
        public static IServiceCollection ConfigureProviderServices(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(One.Mappings.SearchProfile));
            services.AddAutoMapper(typeof(Two.Mappings.SearchProfile));

            services.AddHttpClient<One.Services.SearchService>(nameof(One.Services.SearchService));
            services.AddHttpClient<Two.Services.SearchService>(nameof(Two.Services.SearchService));

            services.AddScoped<ISearchService, One.Services.SearchService>();
            services.AddScoped<ISearchService, Two.Services.SearchService>();

            return services;
        }
    }
}

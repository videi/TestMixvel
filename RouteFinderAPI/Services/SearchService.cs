using Microsoft.Extensions.Caching.Memory;
using RouteFinderAPI.Data.Models;
using Route = RouteFinderAPI.Data.Models.Route;

namespace RouteFinderAPI.Services
{
    public class SearchService : ISearchService
    {
        private readonly IEnumerable<Providers.Services.ISearchService> _providers;
        private readonly IMemoryCache _cache;
        
        public SearchService(IEnumerable<Providers.Services.ISearchService> providers, IMemoryCache cache)
        {
            _providers = providers;
            _cache = cache;
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            var isAvailableProviders = await Task.WhenAll(_providers.Select(p => p.IsAvailableAsync(cancellationToken)));
            return isAvailableProviders.Any(isAvailable => isAvailable);
        }

        public async Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken)
        {
            SearchResponse retSearchResponse;
            Route[] routes;

            cancellationToken.ThrowIfCancellationRequested();

            var cacheKey = $"{request.Origin}-{request.Destination}-{request.OriginDateTime}-{request.Filters?.DestinationDateTime}-{request.Filters?.MaxPrice}";

            if (request.Filters != null && request.Filters.OnlyCached == true)
            {
                if (_cache.TryGetValue(cacheKey, out SearchResponse cachedSearchResponse))
                {
                    routes = cachedSearchResponse.Routes;

                    if (request.Filters?.MinTimeLimit != null)
                    {
                        routes = cachedSearchResponse.Routes.Where(r => r.TimeLimit >= request.Filters.MinTimeLimit).ToArray();
                    }

                    if (routes.Any())
                    {
                        return new SearchResponse
                        {
                            Routes = routes,
                            MinPrice = routes.Min(r => r.Price),
                            MaxPrice = routes.Max(r => r.Price),
                            MinMinutesRoute = (int)routes.Min(r => (r.DestinationDateTime - r.OriginDateTime).TotalMinutes),
                            MaxMinutesRoute = (int)routes.Max(r => (r.DestinationDateTime - r.OriginDateTime).TotalMinutes),
                        };
                    }
                }

                return new SearchResponse 
                { 
                    Routes = Array.Empty<Route>() 
                }; 
            }

            var tasks = _providers.Select(
                provider =>
                {
                    if (provider.IsAvailableAsync(cancellationToken).Result)
                        return provider.SearchAsync(request, cancellationToken);

                    return Task.FromResult(new SearchResponse());
                });

            var results = await Task.WhenAll(tasks);

            routes = results.SelectMany(result => result.Routes).ToArray();

            retSearchResponse = new SearchResponse
            {
                Routes = routes,
                MinPrice = routes.Min(r => r.Price),
                MaxPrice = routes.Max(r => r.Price),
                MinMinutesRoute = (int)routes.Min(r => (r.DestinationDateTime - r.OriginDateTime).TotalMinutes),
                MaxMinutesRoute = (int)routes.Max(r => (r.DestinationDateTime - r.OriginDateTime).TotalMinutes),
            };

            var cacheExpirationTime = routes.Max(r => r.TimeLimit).ToUniversalTime() - DateTime.UtcNow;

            if (cacheExpirationTime > TimeSpan.Zero)
            {
                _cache.Set(cacheKey, retSearchResponse, cacheExpirationTime);
            }

            return retSearchResponse;
        }
    }
}
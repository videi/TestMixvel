using AutoMapper;
using RouteFinderAPI.Providers.Services;
using RouteFinderAPI.Providers.Two.Models;

namespace RouteFinderAPI.Providers.Two.Services
{
    public class SearchService : ISearchService
    {
        private readonly HttpClient _httpClient;
        private readonly string _uri = "http://provider-two/api/v1";
        private readonly IMapper _mapper;

        public SearchService(HttpClient httpClient, IMapper mapper)
        {
            _httpClient = httpClient;
            _mapper = mapper;
        }

        public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync($"{_uri}/ping", cancellationToken);
            return response.IsSuccessStatusCode;
        }

        public async Task<Data.Models.SearchResponse> SearchAsync(Data.Models.SearchRequest request, CancellationToken cancellationToken)
        {
            var providerRequest = _mapper.Map<SearchRequest>(request);

            var response = await _httpClient.PostAsJsonAsync($"{_uri}/search", providerRequest, cancellationToken);
            response.EnsureSuccessStatusCode();

            var providerResponse = await response.Content.ReadFromJsonAsync<SearchResponse>();

            if (providerResponse != null && request.Filters?.MaxPrice != null)
                providerResponse.Routes = providerResponse.Routes.Where(r => r.Price <= request.Filters.MaxPrice).ToArray();

            var searchResponse = _mapper.Map<Data.Models.SearchResponse>(providerResponse);

            return searchResponse;
        }
    }
}

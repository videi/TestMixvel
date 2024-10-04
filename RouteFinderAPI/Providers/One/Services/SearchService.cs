using AutoMapper;
using RouteFinderAPI.Providers.One.Models;
using RouteFinderAPI.Providers.Services;

namespace RouteFinderAPI.Providers.One.Services
{
    public class SearchService : ISearchService
    {
        private readonly HttpClient _httpClient;
        private readonly string _uri = "http://provider-one/api/v1";
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

            if (providerResponse != null && request.Filters?.MinTimeLimit != null)
                providerResponse.Routes = providerResponse.Routes.Where(r => r.TimeLimit >= request.Filters.MinTimeLimit).ToArray();

            var searchResponse = _mapper.Map<Data.Models.SearchResponse>(providerResponse);

            return searchResponse;
        }
    }
}

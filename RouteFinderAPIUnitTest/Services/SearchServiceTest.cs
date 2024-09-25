using Moq;
using Microsoft.Extensions.Caching.Memory;
using RouteFinderAPI.Services;
using RouteFinderAPI.Data.Models;
using ISearchService = RouteFinderAPI.Providers.Services.ISearchService;

namespace RouteFinderAPIUnitTest.Services
{
    public class SearchServiceTest
    {
        private readonly Mock<IMemoryCache> _mockMemoryCache;
        private readonly Mock<ICacheEntry> _mockCacheEntry;

        private readonly Mock<ISearchService> _mockProviderOne;
        private readonly Mock<ISearchService> _mockProviderTwo;
        
        private readonly SearchService _searchService;
        
        private SearchRequest _searchRequest = new SearchRequest
        {
            Origin = "Origin",
            Destination = "Destination",
            OriginDateTime = DateTime.Now,
            Filters = new SearchFilters
            {
                DestinationDateTime = DateTime.Now.AddHours(1),
                MaxPrice = 10,
                MinTimeLimit = DateTime.Now,
                OnlyCached = false
            }
        };

        public SearchServiceTest()
        {
            _mockMemoryCache = new Mock<IMemoryCache>();
            _mockCacheEntry = new Mock<ICacheEntry>();

            _mockProviderOne = new Mock<ISearchService>();
            _mockProviderTwo = new Mock<ISearchService>();

            var providers = new List<ISearchService>
            {
                _mockProviderOne.Object,
                _mockProviderTwo.Object
            };

            _searchService = new SearchService(providers, _mockMemoryCache.Object);
        }

        [Fact]
        public async Task SearchAsync_Cache_Exist()
        {
            var searchResponse = new SearchResponse()
            { 
                Routes = new[]
                { 
                    new Route()
                    {
                        Id = Guid.NewGuid(),
                        TimeLimit = DateTime.Now.AddDays(1),
                    }
                } 
            };
            _searchRequest.Filters ??= new SearchFilters();
            _searchRequest.Filters.OnlyCached = true;

            object? cachedResponse = searchResponse;
            _mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out cachedResponse))
                .Returns(true);

            var result = await _searchService.SearchAsync(_searchRequest, It.IsAny<CancellationToken>());

            Assert.Equal(searchResponse.Routes, result.Routes);
        }

        [Fact]
        public async Task SearchAsync_Cache_NotExist()
        {
            _searchRequest.Filters ??= new SearchFilters();
            _searchRequest.Filters.OnlyCached = true;

            _mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
                .Returns(false);

            var result = await _searchService.SearchAsync(_searchRequest, CancellationToken.None);

            Assert.Empty(result.Routes);
        }

        [Fact]
        public async Task SearchAsync_AggregateProviders()
        {
            var providerOneResponse = new SearchResponse 
            { 
                Routes = new[] 
                { 
                    new Route()
                    {
                        Id = Guid.NewGuid(),
                        TimeLimit = DateTime.Now.AddDays(1)
                    }
                } 
            };
            var providerTwoResponse = new SearchResponse 
            { 
                Routes = new[] 
                {
                    new Route()
                    {
                        Id = Guid.NewGuid(),
                        TimeLimit = DateTime.Now.AddDays(1)
                    }
                } 
            };

            _mockProviderOne
               .Setup(x => x.IsAvailableAsync(It.IsAny<CancellationToken>()))
               .ReturnsAsync(true);

            _mockProviderOne
                .Setup(x => x.SearchAsync(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(providerOneResponse);

            _mockProviderTwo
              .Setup(x => x.IsAvailableAsync(It.IsAny<CancellationToken>()))
              .ReturnsAsync(true);

            _mockProviderTwo
                .Setup(x => x.SearchAsync(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(providerTwoResponse);

            _mockMemoryCache
                .Setup(x => x.TryGetValue(It.IsAny<object>(), out It.Ref<object?>.IsAny))
                .Returns(false);

            _mockMemoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(_mockCacheEntry.Object);
            _mockCacheEntry.SetupProperty(entry => entry.Value);

            var result = await _searchService.SearchAsync(_searchRequest, CancellationToken.None);
            Assert.NotNull(result);
            Assert.Equal(providerOneResponse.Routes.Length + providerTwoResponse.Routes.Length, result.Routes.Length);

            bool isContains = providerTwoResponse.Routes.All(item => result.Routes.Contains(item));
            Assert.True(isContains);

            var searchResponse = new SearchResponse
            {
                Routes = providerOneResponse.Routes.Union(providerTwoResponse.Routes).ToArray()
            };

            Assert.NotNull(_mockCacheEntry.Object.Value);
            Assert.Equal(searchResponse.Routes, (_mockCacheEntry.Object.Value as SearchResponse)?.Routes);
        }

        [Fact]
        public async Task IsAvailableAsync_AllProvidersAvailable()
        {
            // Arrange
            _mockProviderOne
                .Setup(x => x.IsAvailableAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockProviderTwo
                .Setup(x => x.IsAvailableAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _searchService.IsAvailableAsync(CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task IsAvailableAsync_AllProvidersNotAvailable()
        {
            // Arrange
            _mockProviderOne
                .Setup(x => x.IsAvailableAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            _mockProviderTwo
                .Setup(x => x.IsAvailableAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _searchService.IsAvailableAsync(CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task SearchAsync_ThrowsOperationCanceledException()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();

            await Assert.ThrowsAsync<OperationCanceledException>(() =>
                _searchService.SearchAsync(_searchRequest, cts.Token));
        }
    }
}

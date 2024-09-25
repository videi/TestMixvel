using Moq.Protected;
using Moq;
using System.Net;
using RouteFinderAPI.Providers.One.Models;
using RouteFinderAPI.Providers.One.Services;
using AutoMapper;
using System.Text.Json;
using RouteFinderAPI.Providers.One.Mappings;

namespace RouteFinderAPIUnitTest.Providers.One
{
    public class SearchServiceTest
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly SearchService _provider;
        private readonly IMapper _mapper;

        RouteFinderAPI.Data.Models.SearchRequest searchRequest = new()
        {
            Origin = "Origin",
            Destination = "Destination",
            OriginDateTime = DateTime.Now,
            Filters = new RouteFinderAPI.Data.Models.SearchFilters
            {
                DestinationDateTime = DateTime.Now.AddHours(1),
            }
        };

        public SearchServiceTest()
        {
            // Настройка mock HttpMessageHandler для тестирования HttpClient
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var httpClient = new HttpClient(_httpMessageHandlerMock.Object)
            {
                BaseAddress = new Uri("http://provider-one/")
            };

            // Настройка AutoMapper для маппинга между контрактами
            var config = new MapperConfiguration(cfg => cfg.AddProfile<SearchProfile>());

            _mapper = config.CreateMapper();
            _provider = new SearchService(httpClient, _mapper);
        }

        [Fact]
        public async Task SearchAsync_ProviderReturnRoutes()
        {
            var apiResponse = new SearchResponse
            {
                Routes = new[]
                {
                    new Route
                    {
                        From = "From",
                        To = "To",
                        DateFrom = DateTime.Now,
                        DateTo = DateTime.Now.AddHours(1),
                        Price = 100
                    }
                }
            };

            _httpMessageHandlerMock.Protected() 
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(apiResponse))
                });

            var result = await _provider.SearchAsync(searchRequest, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Single(result.Routes);
            Assert.Equal(100, result.Routes[0].Price);
        }

        [Fact]
        public async Task SearchAsync_ProviderReturnsServerError()
        {
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            await Assert.ThrowsAsync<HttpRequestException>(() => _provider.SearchAsync(searchRequest, CancellationToken.None));
        }

        [Fact]
        public async Task IsAvailableAsync_ProviderIsAvailable()
        {
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().EndsWith("/api/v1/ping")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            var isAvailable = await _provider.IsAvailableAsync(CancellationToken.None);

            Assert.True(isAvailable);
        }

        [Fact]
        public async Task IsAvailableAsync_ProviderIsNotAvailable()
        {
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri != null && req.RequestUri.ToString().EndsWith("/api/v1/ping")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.InternalServerError
                });

            var isAvailable = await _provider.IsAvailableAsync(CancellationToken.None);

            Assert.False(isAvailable);
        }
    }
}

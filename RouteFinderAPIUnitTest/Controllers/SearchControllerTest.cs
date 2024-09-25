using Microsoft.AspNetCore.Mvc;
using Moq;
using RouteFinderAPI.Controllers;
using RouteFinderAPI.Data.Models;
using ISearchService = RouteFinderAPI.Services.ISearchService;

namespace RouteFinderAPIUnitTest.Controllers
{
    public class SearchControllerTest
    {
        private readonly Mock<ISearchService> _mockSearchService;
        private readonly SearchController _searchController;

        public SearchControllerTest()
        {
            _mockSearchService = new Mock<ISearchService>();
            _searchController = new SearchController(_mockSearchService.Object);
        }

        [Fact]
        public async Task Search_Ok()
        {
            var searchRequest = new SearchRequest
            {
                Origin = "Origin",
                Destination = "Destination",
                OriginDateTime = DateTime.Now,
                Filters = new SearchFilters
                {
                    DestinationDateTime = DateTime.Now.AddHours(1),
                    MaxPrice = 10,
                    OnlyCached = false
                }
            };
            var searchResponse = new SearchResponse()
            {
                Routes = new[]
                {
                    new Route()
                    {
                        Origin = searchRequest.Origin,
                        Destination = searchRequest.Destination,
                        OriginDateTime = searchRequest.OriginDateTime,
                        DestinationDateTime = searchRequest.Filters.DestinationDateTime.Value,
                        Price = 1,
                    }
                },
            };

            _mockSearchService.Setup(x => x.SearchAsync(It.IsAny<SearchRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(searchResponse);

            var result = await _searchController.Search(searchRequest, It.IsAny<CancellationToken>());

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(searchResponse, okResult.Value);
        }

        [Fact]
        public async Task Ping_IsAvailable()
        {
            _mockSearchService.Setup(x => x.IsAvailableAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var result = await _searchController.Ping(It.IsAny<CancellationToken>());

            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Ping_IsNotAvailable()
        {
            _mockSearchService.Setup(x => x.IsAvailableAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var result = await _searchController.Ping(It.IsAny<CancellationToken>());

            var statusCodeResult = Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(500, statusCodeResult.StatusCode);
        }
    }
}

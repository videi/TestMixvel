using Microsoft.AspNetCore.Mvc;
using RouteFinderAPI.Data.Models;
using RouteFinderAPI.Services;

namespace RouteFinderAPI.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        /// <summary>
        /// Обрабатывает запрос на выполнение асинхронного поиска на основе данных, 
        /// переданных в теле запроса.
        /// </summary>
        /// <param name="request">Объект, содержащий параметры для поиска.</param>
        /// <param name="cancellationToken">Токен отмены для управления выполнением операции.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>, который содержит результаты поиска 
        /// или сообщение об ошибке.
        /// </returns>
        /// <remarks>
        /// Этот метод принимает запрос в формате JSON и выполняет асинхронный поиск. 
        /// Он поддерживает отмену операции с использованием указанного токена. 
        /// Если токен отмены активирован, выполнение будет прервано, и метод может 
        /// выбросить исключение <see cref="OperationCanceledException"/>.
        /// </remarks>
        [HttpPost()]
        public async Task<IActionResult> Search([FromBody] SearchRequest request, CancellationToken cancellationToken)
        {
            var response = await _searchService.SearchAsync(request, cancellationToken);
            return Ok(response);
        }

        /// <summary>
        /// Обрабатывает запрос на проверку доступности сервиса.
        /// </summary>
        /// <param name="cancellationToken">Токен отмены для управления выполнением операции.</param>
        /// <returns>
        /// Возвращает <see cref="IActionResult"/>, указывающий на состояние сервиса. 
        /// В случае успешного выполнения возвращает статус 200 OK.
        /// </returns>
        /// <remarks>
        /// Этот метод позволяет клиентам проверить, доступен ли сервис. 
        /// Поддерживает отмену операции с использованием указанного токена. 
        /// Если токен отмены активирован, выполнение будет прервано, и метод может 
        /// выбросить исключение <see cref="OperationCanceledException"/>.
        /// </remarks>
        [HttpGet("ping")]
        public async Task<IActionResult> Ping(CancellationToken cancellationToken)
        {
            var isAvailable = await _searchService.IsAvailableAsync(cancellationToken);
            return isAvailable ? Ok() : StatusCode(500);
        }

    }
}

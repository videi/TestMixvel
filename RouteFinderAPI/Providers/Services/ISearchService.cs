using RouteFinderAPI.Data.Models;

namespace RouteFinderAPI.Providers.Services;

public interface ISearchService
{
    /// <summary>
    /// Проверяет доступность ресурса асинхронно.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для управления выполнением операции.</param>
    /// <returns>Возвращает true, если ресурс доступен; в противном случае false.</returns>
    /// <remarks>
    /// Этот метод выполняет асинхронную проверку состояния ресурса и поддерживает отмену 
    /// операции с использованием указанного токена. Если токен отмены активирован, 
    /// выполнение будет прервано, и метод может выбросить исключение <see cref="OperationCanceledException"/>.
    /// </remarks>
    Task<bool> IsAvailableAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Выполняет асинхронный поиск на основе указанного запроса.
    /// </summary>
    /// <param name="request">Объект, содержащий параметры поиска.</param>
    /// <param name="cancellationToken">Токен отмены для управления выполнением операции.</param>
    /// <returns>
    /// Возвращает объект <see cref="SearchResponse"/>, содержащий результаты поиска.
    /// </returns>
    /// <remarks>
    /// Этот метод выполняет асинхронный поиск и поддерживает отмену операции с 
    /// использованием указанного токена. Если токен отмены активирован, 
    /// выполнение будет прервано, и метод может выбросить исключение <see cref="OperationCanceledException"/>.
    /// </remarks>
    Task<SearchResponse> SearchAsync(SearchRequest request, CancellationToken cancellationToken);
}
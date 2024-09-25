using System.ComponentModel.DataAnnotations;

namespace RouteFinderAPI.Providers.One.Models
{
    /// <summary>
    /// Ответ на поиск маршрутов.
    /// </summary>
    public class SearchResponse
    {
        /// <summary>
        /// Возвращает или задает массив маршрутов.
        /// </summary>
        [Required]
        public Route[] Routes { get; set; } = Array.Empty<Route>();
    }
}

using System.ComponentModel.DataAnnotations;

namespace RouteFinderAPI.Data.Models
{
    /// <summary>
    /// Запрос на поиск маршрутов.
    /// </summary>
    public class SearchRequest
    {
        /// <summary>
        /// Возвращает или задает начальную точку маршрута, например Москва.
        /// </summary>
        [Required]
        public string Origin { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает или задает конечную точку маршрута, например Сочи.
        /// </summary>
        [Required]
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает или задает дату начала маршрута.
        /// </summary>
        [Required]
        public DateTime OriginDateTime { get; set; }

        /// <summary>
        /// Возвращает или задает дополнительные параметры фильтра.
        /// </summary>
        public SearchFilters? Filters { get; set; }
    }
}

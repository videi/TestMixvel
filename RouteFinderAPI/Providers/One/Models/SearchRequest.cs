using System.ComponentModel.DataAnnotations;

namespace RouteFinderAPI.Providers.One.Models
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
        public string From { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает или задает конечную точку маршрута, например Сочи.
        /// </summary>
        [Required]
        public string To { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает или задает дату начала маршрута.
        /// </summary>
        [Required]
        public DateTime DateFrom { get; set; }

        /// <summary>
        /// Возвращает или задает дату окончания маршрута.
        /// </summary>
        public DateTime? DateTo { get; set; }

        /// <summary>
        /// Возвращает или задает максимальную цену маршрута.
        /// </summary>
        public decimal? MaxPrice { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace RouteFinderAPI.Providers.Two.Models
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
        public string Departure { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает или задает конечную точку маршрута, например Сочи.
        /// </summary>
        [Required]
        public string Arrival { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает или задает дату начала маршрута.
        /// </summary>
        [Required]
        public DateTime DepartureDate { get; set; }

        /// <summary>
        /// Возвращает или задает минимальное значение лимита времени для маршрута.
        /// </summary>
        public DateTime? MinTimeLimit { get; set; }
    }
}

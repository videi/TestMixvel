using System.ComponentModel.DataAnnotations;

namespace RouteFinderAPI.Data.Models
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

        /// <summary>
        /// Возвращает или задает cамый дешевый маршрут.
        /// </summary>
        [Required]
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Возвращает или задает cамый дорогой маршрут.
        /// </summary>
        [Required]
        public decimal MaxPrice { get; set; }

        /// <summary>
        /// Возвращает или задает cамый быстрый маршрут.
        /// </summary>
        [Required]
        public int MinMinutesRoute { get; set; }

        /// <summary>
        /// Возвращает или задает cамый длинный маршрут.
        /// </summary>
        [Required]
        public int MaxMinutesRoute { get; set; }
    }
}

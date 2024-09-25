using System.ComponentModel.DataAnnotations;

namespace RouteFinderAPI.Providers.One.Models
{
    /// <summary>
    /// Маршрут.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Возвращает или задает начальную точку маршрута.
        /// </summary>
        [Required]
        public string From { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает или задает конечную точку маршрута.
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
        [Required]
        public DateTime DateTo { get; set; }

        /// <summary>
        /// Возвращает или задает стоимость маршрута.
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// Возвращает или задает Лимит времени. 
        /// После его истечения маршрут становится неактуальным.
        /// </summary>
        [Required]
        public DateTime TimeLimit { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace RouteFinderAPI.Providers.Two.Models
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
        public Point Departure { get; set; } = new Point();

        /// <summary>
        /// Возвращает или задает конечную точку маршрута.
        /// </summary>
        [Required]
        public Point Arrival { get; set; } = new Point();

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

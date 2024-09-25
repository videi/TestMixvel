using System.ComponentModel.DataAnnotations;

namespace RouteFinderAPI.Data.Models
{
    /// <summary>
    /// Маршрут.
    /// </summary>
    public class Route
    {
        /// <summary>
        /// Возвращает или задает идентификатор всего маршрута.
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Возвращает или задает начальную точку маршрута.
        /// </summary>
        [Required]
        public string Origin { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает или задает конечную точку маршрута.
        /// </summary>
        [Required]
        public string Destination { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает или задает дату начала маршрута.
        /// </summary>
        [Required]
        public DateTime OriginDateTime { get; set; }

        /// <summary>
        /// Возвращает или задает дату окончания маршрута.
        /// </summary>
        [Required]
        public DateTime DestinationDateTime { get; set; }

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

using System.ComponentModel.DataAnnotations;

namespace RouteFinderAPI.Providers.Two.Models
{
    /// <summary>
    /// Точка маршрута.
    /// </summary>
    public class Point
    {
        /// <summary>
        /// Возвращает или задает название пункта, например Москва\Сочи.
        /// </summary>
        [Required]
        public string NameOfPoint { get; set; } = string.Empty;

        /// <summary>
        /// Возвращает или задает дату для точки в маршруте, например, Точка = Москва, Дата = 2023-01-01 15-00-00.
        /// </summary>
        [Required]
        public DateTime Date { get; set; }
    }
}

namespace RouteFinderAPI.Data.Models
{
    /// <summary>
    /// Фильтр поиска маршрутов.
    /// </summary>
    public class SearchFilters
    {
        /// <summary>
        /// Возвращает или задает дату окончания маршрута.
        /// </summary>
        public DateTime? DestinationDateTime { get; set; }

        /// <summary>
        /// Возвращает или задает максимальную цену маршрута.
        /// </summary>
        public decimal? MaxPrice { get; set; }

        /// <summary>
        /// Возвращает или задает минимальное значение лимита времени для маршрута.
        /// </summary>
        public DateTime? MinTimeLimit { get; set; }

        /// <summary>
        /// Возвращает или задает принудительный поиск в кэшированных данных.
        /// </summary>
        public bool? OnlyCached { get; set; }
    }
}

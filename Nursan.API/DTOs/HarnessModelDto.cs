namespace Nursan.API.DTOs
{
    /// <summary>
    /// DTO за модел на кабелен комплект
    /// </summary>
    public class HarnessModelDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Префикс
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// Семейство
        /// </summary>
        public string? Family { get; set; }

        /// <summary>
        /// Суфикс
        /// </summary>
        public string? Suffix { get; set; }

        /// <summary>
        /// Пълно име на модела
        /// </summary>
        public string? HarnessModelName { get; set; }

        /// <summary>
        /// Релиз
        /// </summary>
        public string? Release { get; set; }

        /// <summary>
        /// Активен ли е
        /// </summary>
        public bool? Activ { get; set; }

        /// <summary>
        /// ID на семейството
        /// </summary>
        public int? FamilyId { get; set; }

        /// <summary>
        /// Име на семейството
        /// </summary>
        public string? FamilyName { get; set; }
    }
}

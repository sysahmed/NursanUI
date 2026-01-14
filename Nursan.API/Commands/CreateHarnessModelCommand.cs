using MediatR;
using Nursan.Domain.Entity;

namespace Nursan.API.Commands
{
    /// <summary>
    /// Команда за създаване на нов модел на кабелен комплект
    /// </summary>
    public class CreateHarnessModelCommand : IRequest<OrHarnessModel>
    {
        /// <summary>
        /// Префикс на модела
        /// </summary>
        public string? Prefix { get; set; }

        /// <summary>
        /// Семейство
        /// </summary>
        public string? Family { get; set; }

        /// <summary>
        /// Суфикс на модела
        /// </summary>
        public string? Suffix { get; set; }

        /// <summary>
        /// Релиз
        /// </summary>
        public string? Release { get; set; }

        /// <summary>
        /// ID на семейството
        /// </summary>
        public int? FamilyId { get; set; }

        /// <summary>
        /// Активен ли е моделът
        /// </summary>
        public bool? Activ { get; set; } = true;
    }
}

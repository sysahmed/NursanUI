using MediatR;
using Nursan.Domain.Entity;

namespace Nursan.API.Queries
{
    /// <summary>
    /// Заявка за получаване на всички модели на кабелни комплекти
    /// </summary>
    public class GetHarnessModelsQuery : IRequest<List<OrHarnessModel>>
    {
        /// <summary>
        /// Филтриране по активност
        /// </summary>
        public bool? IsActive { get; set; }

        /// <summary>
        /// Филтриране по семейство
        /// </summary>
        public int? FamilyId { get; set; }
    }
}

using Common.Dtos;
using Common.Dtos.Commands;

namespace Services.Interfaces
{
    public interface IConsultantRoleService
    {
        Task<ConsultantRoleDto> AssignRoleAsync(AssignRoleDto assignRole, CancellationToken cancellationToken = default);
    }
}

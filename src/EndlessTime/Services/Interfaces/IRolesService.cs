using Common.Dtos;
using Common.Dtos.Commands;

namespace Services.Interfaces
{
    public interface IRolesService
    {
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createRole, CancellationToken cancellationToken = default);

        Task<List<RoleDto>> GetRolesAsync(CancellationToken cancellationToken = default);
    }
}

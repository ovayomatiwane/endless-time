using AutoMapper;
using Common.Dtos;
using Common.Dtos.Commands;
using Common.Exceptions;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class RolesService(
        ApplicationDataContext databaseContext,
        IMapper mapper) : IRolesService
    {
        public async Task<RoleDto> CreateRoleAsync(CreateRoleDto createRole, CancellationToken cancellationToken = default)
        {
            ValidateCreateRoleDto(createRole);

            var existingRole = await databaseContext.Roles
                                                    .SingleOrDefaultAsync(x => x.Name == createRole.Name, cancellationToken);

            if (existingRole is not null)
            {
                string message = $"A role with the name {createRole.Name} already exists";
                throw new EntityAlreadyExistsException(message);
            }

            Role role = new()
            {
                Id = Guid.NewGuid(),
                Name = createRole.Name,
                CreatedDate = DateTime.UtcNow
            };

            databaseContext.Roles.Add(role);
            await databaseContext.SaveChangesAsync(cancellationToken);
            
            return mapper.Map<RoleDto>(role);
        }

        public async Task<List<RoleDto>> GetRolesAsync(CancellationToken cancellationToken = default)
        {
            var roles = await databaseContext.Roles.ToListAsync(cancellationToken);

            return mapper.Map<List<RoleDto>>(roles);
        }

        private void ValidateCreateRoleDto(CreateRoleDto createRole)
        {
            string message;

            if (createRole is null)
            {
                message = $"Null argument {nameof(createRole)}.";
                throw new ArgumentNullException(nameof(createRole), message);
            }

            if (string.IsNullOrEmpty(createRole.Name))
            {
                message = $"Invalid name provided. Name cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }
        }
    }
}

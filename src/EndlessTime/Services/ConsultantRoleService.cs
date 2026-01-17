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
    public class ConsultantRoleService(
        ApplicationDataContext databaseContext,
        IMapper mapper) : IConsultantRoleService
    {
        public async Task<ConsultantRoleDto> AssignRoleAsync(AssignRoleDto assignRole, CancellationToken cancellationToken = default)
        {
            ValidateAssignRoleDto(assignRole);

            DateTime timeNow = DateTime.UtcNow;
            string message;

            var consultant = await databaseContext.Consultants
                                                  .Include(x => x.ConsultantRoles)
                                                  .SingleOrDefaultAsync(x => x.Id == assignRole.ConsultantId, cancellationToken);

            if (consultant is null)
            {
                message = $"consultant with id: {assignRole.ConsultantId} not found.";
                throw new EntityNotFoundException(message);
            }

            var role = await databaseContext.Roles.SingleOrDefaultAsync(
                                                                x => x.Id == assignRole.RoleId || 
                                                                (assignRole.RoleName != string.Empty && x.Name == assignRole.RoleName), cancellationToken);

            if (role is null)
            {
                message = $"Role with id: {assignRole.RoleId} not found.";
                throw new EntityNotFoundException(message);
            }

            foreach(var pastConsultantRole in consultant.ConsultantRoles)
            {
                pastConsultantRole.IsCurrent = false;
                databaseContext.ConsultantRoles.Update(pastConsultantRole);
            }

            ConsultantRole consultantRole = new()
            {
                Id = Guid.NewGuid(),
                RoleId = assignRole.RoleId,
                ConsultantId = consultant.Id,
                IsCurrent = true,
                CreatedDate = timeNow,
            };

            databaseContext.ConsultantRoles.Add(consultantRole);

            await databaseContext.SaveChangesAsync(cancellationToken);

            var updatedConsultantRole = await databaseContext.ConsultantRoles
                                                             .Include(x => x.Role)
                                                             .Include(x => x.Consultant)
                                                             .SingleOrDefaultAsync(x => x.Id == consultantRole.Id, cancellationToken);

            return mapper.Map<ConsultantRoleDto>(updatedConsultantRole);
        }

        public async Task<List<ConsultantRoleDto>> GetAllCurrentAsync(CancellationToken cancellationToken = default)
        {
            var consultantRoles = await databaseContext.ConsultantRoles
                                                       .Include(x => x.Role)
                                                       .Include(x => x.Consultant)
                                                       .Where(x => x.IsCurrent)
                                                       .ToListAsync(cancellationToken);

            return mapper.Map<List<ConsultantRoleDto>>(consultantRoles);
        }

        private void ValidateAssignRoleDto(AssignRoleDto assignRole)
        {
            string message;

            if (assignRole is null)
            {
                message = $"Null argument {nameof(assignRole)}.";
                throw new ArgumentNullException(nameof(assignRole), message);
            }
        }
    }
}

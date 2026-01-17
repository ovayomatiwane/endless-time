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
    public class AssignmentsService(
        ApplicationDataContext databaseContext, 
        IMapper mapper) : IAssignmentsService
    {
        public async Task<int> GetAssignedHoursAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var assignment = await databaseContext.Assignments
                                            .Include(x => x.ConsultantAssignments)
                                            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (assignment is null)
            {
                return 0;
            }

            int assignedHours = 0;
            foreach (var consultantAssignment in assignment.ConsultantAssignments)
            {
                assignedHours += consultantAssignment.HoursAssigned;
            }

            return assignedHours;
        }

        public async Task<int> GetAvailableHoursAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var assignment = await databaseContext.Assignments
                                            .Include(x => x.ConsultantAssignments)
                                            .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (assignment is null)
            {
                return 0;
            }

            int assignedHours = 0;
            foreach (var consultantAssignment in assignment.ConsultantAssignments)
            {
                assignedHours += consultantAssignment.HoursAssigned;
            }

            int availableHours = assignment.MaxDurtion - assignedHours;

            return availableHours;
        }

        public async Task<AssignmentDto> CreateAssignmentAsync(CreateAssignmentDto createAssignment, CancellationToken cancellationToken = default)
        {
            ValidateCreateAssignmentDto(createAssignment);

            DateTime timeNow = DateTime.UtcNow;

            Assignment assignment = new()
            {
                Id = Guid.NewGuid(),
                Name = createAssignment.Name,
                Description = createAssignment.Description,
                MaxDurtion = createAssignment.MaxDuration,
                CreatedDate = timeNow,
            };

            databaseContext.Assignments.Add(assignment);

            await databaseContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<AssignmentDto>(assignment);
        }

        public async Task<List<AssignmentDto>> GetAssignmentsAsync(CancellationToken cancellationToken = default)
        {
            var assignments = await databaseContext.Assignments
                                                   .ToListAsync(cancellationToken);

            return mapper.Map<List<AssignmentDto>>(assignments);
        }

        public async Task<AssignmentDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {

            var assignment = await databaseContext.Assignments
                                                   .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (assignment is null)
            {
                string message = $"Assignment with Id: {id} does not exist.";
                throw new EntityNotFoundException(message);
            }

            return mapper.Map<AssignmentDto>(assignment);
        }

        public async Task<List<AssignmentDto>> GetUnassignedAsync(CancellationToken cancellationToken = default)
        {
            // TODO: Add functionality for tasks that have been partially assigned
            var unassigned = await databaseContext.Assignments
                                                       .Where(x => !databaseContext.ConsultantAssignments.Any(y => x.Id == y.AssignmentId))
                                                       .ToListAsync(cancellationToken);
            
            return mapper.Map<List<AssignmentDto>>(unassigned);
        }

        private void ValidateCreateAssignmentDto(CreateAssignmentDto createAssignment)
        {
            string message;

            if (createAssignment is null)
            {
                message = $"Null argument {nameof(createAssignment)}.";
                throw new ArgumentNullException(nameof(createAssignment), message);
            }

            if (string.IsNullOrEmpty(createAssignment.Name))
            {
                message = $"Invalid name provided. Name cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }

            if (createAssignment.MaxDuration <= 0)
            {
                message = $"Max duration has to be a positive number greater than zero.";
                throw new Exception(message);
            }
        }
    }
}

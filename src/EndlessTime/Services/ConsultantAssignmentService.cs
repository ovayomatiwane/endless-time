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
    public class ConsultantAssignmentService(
        ApplicationDataContext databaseContext,
        IConsultantsService consultantsService,
        IRatesService ratesService,
        IAssignmentsService assignmentsService,
        IMapper mapper) : IConsultantAssignmentService
    {
        const int MaxDailyHours = 12;

        public async Task<ConsultantAssignmentDto> AssignTaskAsync(AssignTaskDto assignTask, CancellationToken cancellationToken = default)
        {
            ValidateAssignTaskDto(assignTask);

            DateTime dateNow = DateTime.UtcNow;

            string message; 

            var consultant = await consultantsService.GetByIdAsync(assignTask.ConsultantId, cancellationToken);
            if (consultant is null)
            {
                message = $"Consultant with Id: {assignTask.ConsultantId} not found.";
                throw new EntityNotFoundException(message);
            }

            var rate = await ratesService.GetByIdAsync(assignTask.RateId, cancellationToken);
            if (rate is null)
            {
                message = $"Role rate with Id: {assignTask.RateId} not found.";
                throw new EntityNotFoundException(message);
            }

            var assignment = await assignmentsService.GetByIdAsync(assignTask.AssignmentId, cancellationToken);
            if (assignment is null)
            {
                message = $"Assignemnt (Task) with Id: {assignTask.AssignmentId} not found.";
                throw new EntityNotFoundException(message);
            }

            // Calculate hours already assigned
            int assignedHours = await assignmentsService.GetAssignedHoursAsync(assignTask.AssignmentId, cancellationToken);
            if (assignedHours >= assignment.MaxDurtion)
            {
                message = $"Maximum number of hours of {assignment.MaxDurtion} ({assignedHours}) has been assigned.";
                throw new NumericalValueOutOfAllowableBoundsException(message);
            }

            // Calculate allowable hours to be assigned
            int availableHours = await assignmentsService.GetAvailableHoursAsync(assignTask.AssignmentId, cancellationToken);
            if (availableHours < assignTask.AssignedHours)
            {
                message = $"Assigning hours {assignTask.AssignedHours} are higher than the available task hours: ({availableHours}).";
                throw new NumericalValueOutOfAllowableBoundsException(message);
            }
            
            int consultantAssignedHours = await consultantsService.GetDayAssignedHoursAsync(assignTask.ConsultantId, cancellationToken);
            if (consultantAssignedHours >= MaxDailyHours)
            {
                message = $"Consultant with Id: {assignTask.ConsultantId} has been assigned the maximum hours already ({consultantAssignedHours}).";
                throw new NumericalValueOutOfAllowableBoundsException(message);
            }

            // Calculate max number of hours we can assign here
            int consultantAvailableHours = MaxDailyHours - consultantAssignedHours;
            int totalAssigningHours = assignTask.AssignedHours <= consultantAvailableHours ? assignTask.AssignedHours : consultantAvailableHours;

            Guid id = Guid.NewGuid();

            ConsultantAssignment newAssignment = new() {
                Id = id,
                RateId = assignTask.RateId,
                ConsultantId = assignTask.ConsultantId,
                AssignmentId = assignTask.AssignmentId,
                CreatedDate = dateNow,
                HoursAssigned = totalAssigningHours,
                HoursCompleted = 0
            };

            databaseContext.ConsultantAssignments.Add(newAssignment);
            
            await databaseContext.SaveChangesAsync(cancellationToken);

            var createdAssignment = await databaseContext.ConsultantAssignments
                                         .Include(x => x.Consultant)
                                         .Include(x => x.Assignment)
                                         .Include(x => x.Rate)
                                            .ThenInclude(y => y.Role)
                                         .SingleOrDefaultAsync(x => x.Id == id, cancellationToken);


            return mapper.Map<ConsultantAssignmentDto>(createdAssignment);
        }

        public async Task<ConsultantAssignmentDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var consultantAssignment = await databaseContext.ConsultantAssignments
                                                             .Include(x => x.Assignment)
                                                             .Include(x => x.Consultant)
                                                             .Include(x => x.Rate)
                                                                .ThenInclude(y => y.Role)
                                                             .SingleOrDefaultAsync(x => x.Id == id);

            if (consultantAssignment is null)
            {
                string message = $"Consultant assignment with Id: {id} not found";
                throw new EntityNotFoundException(message);
            }
            
            return mapper.Map<ConsultantAssignmentDto>(consultantAssignment);
        }

        public async Task<List<ConsultantAssignmentDto>> GetConsultantAssignmentsAsync(CancellationToken cancellationToken = default)
        {
            var consultantAssignments = await databaseContext.ConsultantAssignments
                                                             .Include(x => x.Assignment)
                                                             .Include(x => x.Consultant)
                                                             .Include(x => x.Rate)
                                                                .ThenInclude(y => y.Role)
                                                             .ToListAsync(cancellationToken);

            return mapper.Map<List<ConsultantAssignmentDto>>(consultantAssignments);
        }

        private void ValidateAssignTaskDto(AssignTaskDto assignTask)
        {
            string message;

            if (assignTask is null)
            {
                message = $"Null argument {nameof(assignTask)}.";
                throw new ArgumentNullException(nameof(assignTask), message);
            }

            if (assignTask.AssignedHours <= 0)
            {
                message = $"Number of hours assigned has to be a positive number greater than zero.";
                throw new Exception(message);
            }
        }
    }
}

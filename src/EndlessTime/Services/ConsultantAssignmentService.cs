using AutoMapper;
using Azure.Core;
using Common.Dtos;
using Common.Dtos.Commands;
using Common.Dtos.Responses;
using Common.Exceptions;
using Common.Utils;
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

        public async Task<MoneyOwedDto> GetMoneyOwedAsync(MoneyOwedRequestDto request, CancellationToken cancellationToken = default)
        {
            ValidateMoneyOwedRequestDto(request);

            var consultant = await databaseContext.Consultants.SingleOrDefaultAsync(x => x.Id == request.ConsultantId, cancellationToken);
            if (consultant is null)
            {
                string message = $"Consultant with Id: {request.ConsultantId} not found";
                throw new EntityNotFoundException(message);
            }

            DateTime dateStart = request.FromDate.StartOfDay();
            DateTime dateEnd = request.ToDate.EndOfDay();



            var consultantAssignments = await databaseContext.ConsultantAssignments
                                                            .Include(x => x.Consultant)
                                                            .Include(x => x.Rate)
                                                            .Include(x => x.Assignment)
                                                            .Where(x => x.ConsultantId == request.ConsultantId && x.CreatedDate >= dateStart && x.CreatedDate <= dateEnd)
                                                            .ToListAsync(cancellationToken);

            decimal totalValue = 0;
            List<AssignmentOwedDto> assignmentsOwed = [];

            foreach(var consultantAssignment in consultantAssignments)
            {
                var totalOwed = consultantAssignment.HoursCompleted * consultantAssignment.Rate.HourlyRate;
                if (totalOwed > 0)
                {
                    totalValue += totalOwed;

                    assignmentsOwed.Add(new()
                    {
                        AssignmentId = consultantAssignment.AssignmentId,
                        AssignmentName = consultantAssignment.Assignment.Name,
                        AmountOwed = totalOwed,
                    });
                }
            }

            var consultantAssign = consultantAssignments.FirstOrDefault();

            if (consultantAssign != null)
            {
                return new()
                {
                    FullName = $"{consultantAssign.Consultant.FirstName} {consultantAssign.Consultant.LastName}",
                    EmailAddress = consultantAssign.Consultant.EmailAddress ?? string.Empty,
                    TotalOwed = totalValue,
                    FromDate = dateStart,
                    ToDate = dateEnd,
                    AssignmentsOwed = assignmentsOwed
                };
            };

            return new()
            {
                FullName = $"{consultant.FirstName} {consultant.LastName}",
                EmailAddress = consultant.EmailAddress ?? string.Empty,
                TotalOwed = totalValue,
                FromDate = dateStart,
                ToDate = dateEnd,
                AssignmentsOwed = assignmentsOwed
            };
        }

        public async Task<ConsultantAssignmentDto> CompleteHoursAsync(CompleteHoursDto completeRequest, CancellationToken cancellationToken = default)
        {
            ValidateCompleteHourstDto(completeRequest);

            DateTime timeNow = DateTime.UtcNow;

            string message;

            var consultantAssignment = await databaseContext.ConsultantAssignments
                                                            .Include(x => x.Consultant)
                                                            .Include(x => x.Rate)
                                                                .ThenInclude(x => x.Role)
                                                            .Include(x => x.Assignment)
                                                            .SingleOrDefaultAsync(x => x.Id == completeRequest.ConsultantAssignmentId,cancellationToken);

            if (consultantAssignment is null)
            {
                message = $"Assignment with id {completeRequest.ConsultantAssignmentId} does not exist.";
                throw new EntityNotFoundException(message);
            }

            if (consultantAssignment.HoursCompleted >= completeRequest.HoursCompleted)
            {
                message = $"Number of hours already completed ({consultantAssignment.HoursCompleted}) cannot be greater or equal to number of hours to be completed ({completeRequest.HoursCompleted}). Remaining: {consultantAssignment.HoursAssigned - consultantAssignment.HoursCompleted}";
                throw new NumericalValueOutOfAllowableBoundsException(message);
            }

            if (consultantAssignment.HoursAssigned < completeRequest.HoursCompleted)
            {
                message = $"Number of hours assigned ({consultantAssignment.HoursAssigned}) cannot be less than number of hours to be completed ({completeRequest.HoursCompleted}). Remaining: {consultantAssignment.HoursAssigned - consultantAssignment.HoursCompleted}";
                throw new NumericalValueOutOfAllowableBoundsException(message);
            }

            consultantAssignment.HoursCompleted = completeRequest.HoursCompleted;
            databaseContext.Update(consultantAssignment);
            await databaseContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<ConsultantAssignmentDto>(consultantAssignment);
        }

        private void ValidateMoneyOwedRequestDto(MoneyOwedRequestDto request)
        {
            string message;

            if (request is null)
            {
                message = $"Null argument {nameof(request)}.";
                throw new ArgumentNullException(nameof(request), message);
            }

            if (request.FromDate >= request.ToDate)
            {
                message = $"FromDate has to be before the ToDate";
                throw new Exception(message);
            }
        }

        private void ValidateCompleteHourstDto(CompleteHoursDto completeRequest)
        {
            string message;

            if (completeRequest is null)
            {
                message = $"Null argument {nameof(completeRequest)}.";
                throw new ArgumentNullException(nameof(completeRequest), message);
            }

            if (completeRequest.HoursCompleted <= 0)
            {
                message = $"Number of hours completed have to be positive and greater than zero.";
                throw new NumericalValueOutOfAllowableBoundsException(message);
            }
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

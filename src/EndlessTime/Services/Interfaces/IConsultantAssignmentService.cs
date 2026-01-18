using Common.Dtos;
using Common.Dtos.Commands;
using Common.Dtos.Responses;

namespace Services.Interfaces
{
    public interface IConsultantAssignmentService
    {
        Task<List<ConsultantAssignmentDto>> GetConsultantAssignmentsAsync(CancellationToken cancellationToken = default);

        Task<ConsultantAssignmentDto> AssignTaskAsync(AssignTaskDto assignTask, CancellationToken cancellationToken = default);

        Task<ConsultantAssignmentDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

        Task<MoneyOwedDto> GetMoneyOwedAsync(MoneyOwedRequestDto request, CancellationToken cancellationToken = default);

        Task<ConsultantAssignmentDto> CompleteHoursAsync(CompleteHoursDto request, CancellationToken cancellationToken = default);
    }
}

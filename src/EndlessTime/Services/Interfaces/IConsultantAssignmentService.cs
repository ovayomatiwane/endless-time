using Common.Dtos;
using Common.Dtos.Commands;

namespace Services.Interfaces
{
    public interface IConsultantAssignmentService
    {
        Task<List<ConsultantAssignmentDto>> GetConsultantAssignmentsAsync(CancellationToken cancellationToken = default);

        Task<ConsultantAssignmentDto> AssignTaskAsync(AssignTaskDto assignTask, CancellationToken cancellationToken = default);

        Task<ConsultantAssignmentDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

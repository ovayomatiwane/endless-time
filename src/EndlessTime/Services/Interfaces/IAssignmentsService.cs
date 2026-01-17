using Common.Dtos;
using Common.Dtos.Commands;

namespace Services.Interfaces
{
    public interface IAssignmentsService
    {
        Task<AssignmentDto> CreateAssignmentAsync(CreateAssignmentDto createAssignment, CancellationToken cancellationToken = default);
        
        Task<List<AssignmentDto>> GetAssignmentsAsync(CancellationToken cancellationToken = default);
        
        Task<AssignmentDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        
        Task<List<AssignmentDto>> GetUnassignedAsync(CancellationToken cancellationToken = default);

        Task<int> GetAssignedHoursAsync(Guid id, CancellationToken cancellationToken = default);

        Task<int> GetAvailableHoursAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

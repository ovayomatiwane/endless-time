using Common.Dtos;
using Common.Dtos.Commands;

namespace Services.Interfaces
{
    public interface IConsultantsService
    {
        Task<ConsultantDto> CreateConsultantAsync(CreateConsultantDto createConsultant, CancellationToken cancellationToken = default);

        Task<List<ConsultantDto>> GetAllAsync(CancellationToken cancellationToken = default);

        Task<ConsultantDto> GetByIdAsync(Guid consultantId, CancellationToken cancellationToken = default);

        Task<int> GetDayAssignedHoursAsync(Guid consultantId, CancellationToken cancellationToken = default);
    }
}

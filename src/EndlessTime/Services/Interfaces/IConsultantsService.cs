using Common.Dtos;
using Common.Dtos.Commands;

namespace Services.Interfaces
{
    public interface IConsultantsService
    {
        Task<ConsultantDto> CreateConsultantAsync(CreateConsultantDto createConsultant, CancellationToken cancellationToken = default);

        Task<List<ConsultantDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}

using Common.Dtos;
using Common.Dtos.Commands;

namespace Services.Interfaces
{
    public interface IRatesService
    {
        Task<List<RateDto>> GetActiveAsync(CancellationToken cancellationToken = default);

        Task<RateDto> CreateRateAsync(CreateRateDto createRate, CancellationToken cancellationToken = default);

        Task<List<RateDto>> GetRatesAsync(CancellationToken cancellationToken = default);

        Task<RateDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}

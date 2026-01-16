using Common.Dtos;

namespace Services.Interfaces
{
    public interface IConsultantsService
    {
        Task<IEnumerable<ConsultantDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}

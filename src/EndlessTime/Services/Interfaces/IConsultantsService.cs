using Common.Dtos;

namespace Services.Interfaces
{
    public interface IConsultantsService
    {
        Task<List<ConsultantDto>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}

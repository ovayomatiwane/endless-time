using Common.Dtos;

namespace Services.Interfaces
{
    public interface IConsultantsService
    {
        Task<IEnumerable<ConsultantDto>> GetAll(CancellationToken cancellationToken = default);
    }
}

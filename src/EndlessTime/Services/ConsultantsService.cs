using AutoMapper;
using Common.Dtos;
using Domain;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class ConsultantsService(
        IMapper mapper,
        ApplicationDataContext databaseContext) : IConsultantsService
    {
        public async Task<IEnumerable<ConsultantDto>> GetAll(CancellationToken cancellationToken = default)
        {
            var result = await databaseContext.Consultants
                                              .ToListAsync(cancellationToken);

            return mapper.Map<List<ConsultantDto>>(result);
        }
    }
}

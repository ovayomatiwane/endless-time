using AutoMapper;
using Common.Dtos;
using Common.Dtos.Commands;
using Common.Exceptions;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class RatesService(ApplicationDataContext databaseContext, IMapper mapper) : IRatesService
    {
        public async Task<RateDto> CreateRateAsync(CreateRateDto createRate, CancellationToken cancellationToken = default)
        {
            ValidateCreateRateDto(createRate);

            DateTime timeNow = DateTime.UtcNow;

            var role = await databaseContext.Roles
                                            .Include(x => x.Rates)
                                            .SingleOrDefaultAsync(x => x.Id == createRate.RoleId, cancellationToken);

            if (role is null)
            {
                string message = $"Role with Id: {createRate.RoleId} not found.";
                throw new EntityNotFoundException(message);
            }

            foreach(var rate in role.Rates)
            {
                rate.IsCurrent = false;
                databaseContext.Rates.Update(rate);
            }

            Rate newRate = new()
            {
                Id = Guid.NewGuid(),
                RoleId = createRate.RoleId,
                HourlyRate = createRate.HourlyRate,
                CreatedDate = timeNow,
                IsCurrent = true
            };

            databaseContext.Rates.Add(newRate);

            await databaseContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<RateDto>(newRate);
        }

        public async Task<List<RateDto>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            var rates = await databaseContext.Rates
                                             .Include(x => x.Role)
                                             .Where(x => x.IsCurrent)
                                             .ToListAsync(cancellationToken);

            return mapper.Map<List<RateDto>>(rates);
        }

        public async Task<RateDto> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var rate = await databaseContext.Rates.SingleOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (rate is null)
            {
                string message = $"Role rate with Id: {id} does not exist";
                throw new EntityNotFoundException(message);
            }

            return mapper.Map<RateDto>(rate);
        }

        public async Task<List<RateDto>> GetRatesAsync(CancellationToken cancellationToken = default)
        {
            var rates = await databaseContext.Rates
                                             .Include(x => x.Role)
                                             .ToListAsync(cancellationToken);

            return mapper.Map<List<RateDto>>(rates);
        }

        private void ValidateCreateRateDto(CreateRateDto createRate)
        {
            string message; 

            if (createRate is null)
            {
                message = $"Null argument {nameof(createRate)}.";
                throw new ArgumentNullException(nameof(createRate), message);
            }

            if (createRate.HourlyRate <= 0)
            {
                message = $"Hourly rate has to be a positive number greater than zero.";
                throw new Exception(message);
            }
        }
    }
}

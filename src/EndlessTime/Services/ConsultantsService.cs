using AutoMapper;
using Common.Dtos;
using Common.Dtos.Commands;
using Common.Exceptions;
using Common.Utils;
using Domain;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;

namespace Services
{
    public class ConsultantsService(
        IMapper mapper,
        ApplicationDataContext databaseContext) : IConsultantsService
    {
        public async Task<ConsultantDto> CreateConsultantAsync(CreateConsultantDto createConsultant, CancellationToken cancellationToken = default)
        {
            ValidateCreateConsultantDto(createConsultant);

            DateTime timeNow = DateTime.UtcNow;

            string message;

            string? emailLowercase = createConsultant?.EmailAddress?.ToLowerInvariant();

            var existsConsultant = await databaseContext.Consultants
                                                        .SingleOrDefaultAsync(x => x.EmailAddress == emailLowercase);

            if (existsConsultant is not null)
            {
                message = $"Consultant with email {emailLowercase} already exists.";
                throw new EntityAlreadyExistsException(message);
            }

            Consultant newConsultant = new()
            {
                Id = Guid.NewGuid(),
                FirstName = createConsultant!.FirstName!,
                LastName = createConsultant!.LastName!,
                EmailAddress = createConsultant!.EmailAddress,
            };

            databaseContext.Consultants.Add(newConsultant);
            await databaseContext.SaveChangesAsync(cancellationToken);

            return mapper.Map<ConsultantDto>(newConsultant);
        }

        public async Task<List<ConsultantDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await databaseContext.Consultants
                                              .ToListAsync(cancellationToken);

            return mapper.Map<List<ConsultantDto>>(result);
        }

        public async Task<ConsultantDto> GetByIdAsync(Guid consultantId, CancellationToken cancellationToken = default)
        {
            var consultant = await databaseContext.Consultants
                                                   .SingleOrDefaultAsync(x => x.Id == consultantId, cancellationToken);

            if (consultant is null)
            {
                string message = $"Consultanr with Id: {consultantId} does not exist.";
                throw new EntityNotFoundException(message);
            }

            return mapper.Map<ConsultantDto>(consultant);
        }

        public async Task<int> GetDayAssignedHoursAsync(Guid consultantId, CancellationToken cancellationToken = default)
        {
            var (dayStart, dayEnd) = DateUtils.GetUtcDayRange(DateTime.UtcNow);

            var consultantAssignemts = await databaseContext.ConsultantAssignments
                                                   .Where(x => x.ConsultantId == consultantId && x.CreatedDate >= dayStart && x.CreatedDate <= dayEnd)
                                                   .ToListAsync(cancellationToken);

            
            if (consultantAssignemts is null)
            {
                return 0;
            }

            int assignedHours = 0;

            foreach(var consultantAssignemt in consultantAssignemts)
            {
                assignedHours += consultantAssignemt.HoursAssigned;
            }

            return assignedHours;
        }

        private void ValidateCreateConsultantDto(CreateConsultantDto createConsultant)
        {
            string message;

            if (createConsultant is null)
            {
                message = $"Null argument {nameof(createConsultant)}.";
                throw new ArgumentNullException(nameof(createConsultant), message);
            }

            if (string.IsNullOrEmpty(createConsultant.FirstName))
            {
                message = $"Invalid name provided. Name cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }

            if (string.IsNullOrEmpty(createConsultant.LastName))
            {
                message = $"Invalid surname provided. Surname cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }

            if (string.IsNullOrEmpty(createConsultant.EmailAddress))
            {
                message = $"Invalid Email address provided. Email address cannot be null or empty.";
                throw new RequiredNullOrEmptyStringException(message);
            }
        }
    }
}

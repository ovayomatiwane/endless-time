using AutoMapper;
using Domain;
using Common.Dtos;
using Domain.Entities;

namespace WebApi.Mapping
{
    public class ApplicationMappingProfile : Profile
    {
        public ApplicationMappingProfile() 
        { 
            //One way mappings
            CreateMap<Consultant, ConsultantDto>();

            //Two-way mappings

            //Custom mappings
        }
    }
}

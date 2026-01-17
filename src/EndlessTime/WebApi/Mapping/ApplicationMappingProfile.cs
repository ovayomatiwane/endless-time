using AutoMapper;
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
            CreateMap<User, UserDto>();
            CreateMap<Role, RoleDto>();
            CreateMap<Rate, RateDto>();
            CreateMap<ConsultantRole, ConsultantRoleDto>();
            CreateMap<Assignment, AssignmentDto>();
            CreateMap<ConsultantAssignment, ConsultantAssignmentDto>();

            //Two-way mappings

            //Custom mappings
        }
    }
}

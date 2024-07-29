using AutoMapper;
using DataStore.Core.DTOs.SubcommitteeMembership;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class SubcommitteeMembershipProfile : Profile
    {
        public SubcommitteeMembershipProfile()
        {
            CreateMap<CreateSubcommitteeMembershipDTO, SubcommitteeMembership>();
            CreateMap<SubcommitteeMembership, ReadSubcommitteeMembershipDTO>();
            CreateMap<UpdateSubcommitteeMembershipDTO, SubcommitteeMembership>();
        }
    }
}

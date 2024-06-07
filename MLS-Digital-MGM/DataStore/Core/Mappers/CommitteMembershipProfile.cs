using AutoMapper;
using DataStore.Core.DTOs.CommitteMember;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class CommitteeMembershipProfile : Profile
    {
        public CommitteeMembershipProfile()
        {
            CreateMap<CreateCommitteMemberShipDTO, CommitteeMembership>();
            CreateMap<CommitteeMembership, ReadCommitteMemberShipDTO>();
            CreateMap<UpdateCommitteMemberShipDTO, CommitteeMembership>();
        }
    }
}

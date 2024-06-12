using AutoMapper;
using DataStore.Core.DTOs.CommitteeMemberShip;
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
    public class CommitteeMembershipProfile : Profile
    {
        public CommitteeMembershipProfile()
        {
            CreateMap<CreateCommitteeMemberShipDTO, CommitteeMembership>();
            CreateMap<CommitteeMembership, ReadCommitteeMemberShipDTO>();
            CreateMap<UpdateCommitteeMemberShipDTO, CommitteeMembership>();
        }
    }
}

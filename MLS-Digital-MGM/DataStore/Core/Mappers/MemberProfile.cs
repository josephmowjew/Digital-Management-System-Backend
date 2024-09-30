//create member profile mapper
using AutoMapper;
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.DTOs;
using DataStore.Core.DTOs.Member;

namespace DataStore.Core.Mappers
{
    public class MemberProfile : Profile
    {
        public MemberProfile() 
        {
            CreateMap<Member, ReadMemberDTO>()
             .ForMember(dest => dest.DateOfAdmissionToPractice, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateOfAdmissionToPractice ?? DateTime.Now)));
            CreateMap<CreateMemberDTO, Member>();
            CreateMap<UpdateMemberDTO, Member>();
        }
    }
}

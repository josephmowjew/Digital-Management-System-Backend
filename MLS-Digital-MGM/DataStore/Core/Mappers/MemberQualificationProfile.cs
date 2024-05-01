//create member qualification profile mapper
using AutoMapper;
using DataStore.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataStore.Core.DTOs;
using DataStore.Core.DTOs.Member;
using DataStore.Core.DTOs.MemberQualification;

namespace DataStore.Core.Mappers
{
  public class MemberQualificationProfile : Profile
  {
    public MemberQualificationProfile()
    {
      CreateMap<MemberQualification, ReadMemberQualificationDTO>()
      .ForMember(dest => dest.DateObtained, opt => opt.MapFrom(src => DateOnly.FromDateTime(src.DateObtained)));;
      CreateMap<CreateMemberQualificationDTO, MemberQualification>()
       .ForMember(dest => dest.Attachments, opt => opt.Ignore());; 
      CreateMap<UpdateMemberQualificationDTO, MemberQualification>()
       .ForMember(dest => dest.Attachments, opt => opt.Ignore());;
    }
  }
}

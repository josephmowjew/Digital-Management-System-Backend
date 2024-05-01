using AutoMapper;
using DataStore.Core.DTOs.QualificationType; 
using DataStore.Core.Models;

namespace DataStore.Core.Mappers
{
  public class QualificationTypeProfile : Profile 
  {
    public QualificationTypeProfile()
    {
      CreateMap<CreateQualificationTypeDTO, QualificationType>();
      CreateMap<QualificationType, ReadQualificationTypeDTO>(); 
      CreateMap<UpdateQualificationTypeDTO, QualificationType>();
    }
  }
}

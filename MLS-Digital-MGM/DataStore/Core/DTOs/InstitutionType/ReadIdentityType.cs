namespace DataStore.Core.DTOs.InstitutionType
{
    public class ReadInstitutionTypeDTO
    {
        // Define properties here to represent the data fields of the DTO
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string Status { get; set; }
        public int? InstitutionTypeId { get; set; }
        public virtual ReadInstitutionTypeDTO InstitutionType { get; set; }
    }
}
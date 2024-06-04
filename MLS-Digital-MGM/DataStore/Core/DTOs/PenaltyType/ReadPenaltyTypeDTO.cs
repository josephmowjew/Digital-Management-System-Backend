using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.PenaltyType
{
    public class ReadPenaltyTypeDTO
    {
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
    }
}

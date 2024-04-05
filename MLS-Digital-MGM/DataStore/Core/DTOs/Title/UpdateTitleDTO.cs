using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Title
{
    public class UpdateTitleDTO
    {
        [Required]
        public int Id { get; set; }
        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string Name { get; set; }

    }
}
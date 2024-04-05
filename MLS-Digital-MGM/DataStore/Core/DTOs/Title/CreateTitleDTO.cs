using System.ComponentModel.DataAnnotations;

namespace DataStore.Core.DTOs.Title
{
    public class CreateTitleDTO
    {
        [StringLength(maximumLength: 100, MinimumLength = 2)]
        public string Name { get; set; }
    }
}
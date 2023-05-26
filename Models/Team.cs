using System.ComponentModel.DataAnnotations;

namespace BZLAND.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string ImagePath { get; set; }
        public bool IsDeleted { get; set; }
    }
}

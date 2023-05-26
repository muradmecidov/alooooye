using System.ComponentModel.DataAnnotations;

namespace BZLAND.Areas.Admin.ViewModel
{
    public class UpdateTeamVM
    {
        public int Id { get; set; }
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}

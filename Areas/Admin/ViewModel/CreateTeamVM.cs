using System.ComponentModel.DataAnnotations;

namespace BZLAND.Areas.Admin.ViewModel
{
    public class CreateTeamVM
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}

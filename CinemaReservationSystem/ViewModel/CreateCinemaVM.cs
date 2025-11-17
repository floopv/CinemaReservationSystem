using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaReservationSystem.ViewModel
{
    public class CreateCinemaVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "required yastaa")]
        [MinLength(3)]
        //[MaxLength(25)]
        public string Name { get; set; }
        //[MaxLength(150)]
        public string Description { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public IFormFile? ImgFile { get; set; }
    }
}

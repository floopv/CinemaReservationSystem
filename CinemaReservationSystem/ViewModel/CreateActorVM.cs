using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaReservationSystem.ViewModel
{
    public class CreateActorVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        [NotMapped]
        public IFormFile? ImgFile { get; set; }
    }
}

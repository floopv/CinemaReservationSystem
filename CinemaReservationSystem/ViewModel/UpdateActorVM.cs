using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaReservationSystem.ViewModel
{
    public class UpdateActorVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Img { get; set; }
        [NotMapped]
        public IFormFile? ImgFile { get; set; }
    }
}

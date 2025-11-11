using System.ComponentModel.DataAnnotations;

namespace CinemaReservationSystem.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Required yastaa")]
        [MinLength(3)]
        //[MaxLength(25)]
        public string Name { get; set; }
        //[MaxLength(250)]
        public string Description { get; set; }
        public bool Status { get; set; }
        public List<Movie>? Movies { get; set; }
    }
}

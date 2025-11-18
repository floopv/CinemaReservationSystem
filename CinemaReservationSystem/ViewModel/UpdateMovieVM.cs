using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaReservationSystem.ViewModel
{
    public class UpdateMovieVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? MainImg { get; set; }
        [NotMapped]
        public IFormFile? FormMainImg { get; set; }
        [NotMapped]
        public ICollection<IFormFile>? FormSubImgs { get; set; }
        public int CategoryId { get; set; }
        public int CinemaId { get; set; }
        public List<Category>? Categories { get; set; }
        public List<Cinema>? Cinemas { get; set; }
        public List<Actor>? Actors { get; set; }
        public List<int>? SelectedActorIds { get; set; } = new List<int>();
        public List<MovieSubImg>? MovieSubImgs { get; set; }
    }
}

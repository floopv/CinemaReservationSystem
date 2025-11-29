namespace CinemaReservationSystem.ViewModel
{
    public class MovieWithRelatedMoviesVM
    {
        public Movie Movie { get; set; }
        public IEnumerable<Movie> RelatedMovies { get; set; }
    }
}

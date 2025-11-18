namespace CinemaReservationSystem.Models
{
    public class ActorMovie
    {
        public int ActorsId { get; set; }
        public Actor Actor { get; set; }
        public int MoviesId { get; set; }
        public Movie Movie { get; set; }

    }
}

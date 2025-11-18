using Microsoft.AspNetCore.Identity;

namespace CinemaReservationSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address{ get; set; }
        public int? Age{ get; set; }
    }
}

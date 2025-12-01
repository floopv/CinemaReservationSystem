namespace CinemaReservationSystem.Models
{
    public class ApplicationUserPromotionCode
    {
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int PromotionCodeId { get; set; }
        public Promotion PromotionCode { get; set; }
    }
}

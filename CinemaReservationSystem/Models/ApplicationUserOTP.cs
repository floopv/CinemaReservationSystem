namespace CinemaReservationSystem.Models
{
    public class ApplicationUserOTP
    {
        public string Id { get; set; }
        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; }
        public string OTP { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsValid { get; set; }
        public ApplicationUserOTP()
        {
        }
        public ApplicationUserOTP(string ApplicationUserId,string OTP)
        {
            Id = Guid.NewGuid().ToString();
            CreatedAt = DateTime.UtcNow;
            IsValid = true;
            this.ApplicationUserId = ApplicationUserId;
            this.OTP = OTP;
            ExpireAt = CreatedAt.AddMinutes(10);
        }
    } 
}

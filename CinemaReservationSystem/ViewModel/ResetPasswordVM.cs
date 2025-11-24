using System.ComponentModel.DataAnnotations;

namespace CinemaReservationSystem.ViewModel
{
    public class ResetPasswordVM
    {
        public int Id { get; set; }
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [DataType(DataType.Password) , Compare(nameof(NewPassword))]
        public string ConfirmNewPassword { get; set; }
        public string ApplicationUserId { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace MAMBY.UI.Models
{
    public class ResetPasswordViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Şifreler uyuşmadı!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }
}

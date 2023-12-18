
using System.ComponentModel.DataAnnotations;

namespace MAMBY.UI.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Bu alan boş bırakılamaz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Şifreler uyuşmadı!")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
        public string ImageUrl { get; set; }
    }
}

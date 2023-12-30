
using System.ComponentModel.DataAnnotations;

namespace MAMBY.UI.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Ad alanı boş bırakılamaz.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Soyad alanı boş bırakılamaz.")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Doğum tarihi alanı boş bırakılamaz.")]
        [DataType(DataType.Date, ErrorMessage = "Geçersiz tarih formatı.")]
        public DateTime BirthDate { get; set; }
        [Required(ErrorMessage = "Adres alanı boş bırakılamaz.")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Kullanıcı adı alanı boş bırakılamaz.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Telefon numarası alanı boş bırakılamaz.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "Telefon Numarası 11 haneli olmalıdır.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "E-posta alanı boş bırakılamaz.")]
        [EmailAddress(ErrorMessage = "Geçersiz e-posta adresi.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Şifre alanı boş bırakılamaz.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Şifre tekrarı boş bırakılamaz.")]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}

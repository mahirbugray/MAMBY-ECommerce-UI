using System.ComponentModel.DataAnnotations;

namespace MAMBY.UI.Models
{
    public class PaymentViewModel
    {
        public UserViewModel User { get; set; }
        public List<CardLineViewModel> cardLines { get; set; }
        public decimal totalPrice { get; set; }
        [Required(ErrorMessage = "Şehir alanı boş bırakılamaz.")]
        public string city { get; set; }

        [Required(ErrorMessage = "Mahalle alanı boş bırakılamaz.")]
        public string neighbourhood { get; set; }

        [Required(ErrorMessage = "Apartman Numarası alanı boş bırakılamaz.")]
        public string aptNo { get; set; }

        [Required(ErrorMessage = "Posta Kodu alanı boş bırakılamaz.")]
        [StringLength(5, MinimumLength = 5, ErrorMessage = "Posta Kodu 5 haneli olmalıdır.")]
        public string postCode { get; set; }

    }
}

namespace MAMBY.UI.Models
{
    public class PaymentViewModel
    {
        public UserViewModel User { get; set; }
        public List<CardLineViewModel> cardLines { get; set; }
        public decimal totalPrice { get; set; }
        public string city { get; set; }
        public string neighbourhood { get; set; }
        public string aptNo { get; set; }
        public string postCode { get; set; }

    }
}

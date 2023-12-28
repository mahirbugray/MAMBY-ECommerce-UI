namespace MAMBY.UI.Models
{
    public class CardLineViewModel
    {
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string ProductName { get; set; }
        public decimal TotalPrice { get; set; }
        public int CardId { get; set; }
        public CardViewModel CardViewModel { get; set; }
        public int ProductId { get; set; }
        public ProductViewModel ProductViewModel { get; set; }
    }
}

namespace MAMBY.UI.Models
{
    public class CardLineViewModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal ProductPrice { get; set; }

        public List<CardLineViewModel> AddCard (List<CardLineViewModel> card, CardLineViewModel cardLine)
        {
            if (card.Any(cd => cd.ProductId == cardLine.ProductId))
            {
                foreach (CardLineViewModel model in card)
                {
                    if(model.ProductId == cardLine.ProductId) 
                    {
                        model.Quantity += cardLine.Quantity;
                    }
                }
            }
            else
            {
                card.Add(cardLine);
            }
            return card;
        }
        public List<CardLineViewModel> DeleteCard(List<CardLineViewModel> card, int id) 
        {
            card.RemoveAll(cd => cd.ProductId == id);
            return card;
        }
        public int TotalQuantity(List<CardLineViewModel> card)
        {
            var totalQuantity = card.Sum(cd => cd.Quantity);
            return totalQuantity;
        }
        public decimal TotalPrice(List<CardLineViewModel> card)
        {
            var totalPrice = card.Sum(c => c.Quantity * c.ProductPrice);
            return totalPrice;
        }
    }
}

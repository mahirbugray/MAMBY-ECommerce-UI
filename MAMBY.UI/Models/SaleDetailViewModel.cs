namespace MAMBY.UI.Models
{
    public class SaleDetailViewModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public int ProductId { get; set; }
        public int SaleId { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Neighbourhood { get; set; }
        public string ZipCode { get; set; }
        public string AptNumber { get; set; }
        public string CardOwner { get; set; }
        public string CardNumber { get; set; }


        //Relation
        public ProductViewModel ProductViewModel { get; set; }
        public SaleViewModel SaleViewModel { get; set; }
    }
}

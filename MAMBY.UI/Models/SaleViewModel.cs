namespace MAMBY.UI.Models
{
    public class SaleViewModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public decimal TotalPrice { get; set; }
        public int TotalQuantity { get; set; }
        public int UserId { get; set; }

        //Relation
        public List<SaleDetailViewModel> SaleDetailViewModel { get; set; }
    }
}

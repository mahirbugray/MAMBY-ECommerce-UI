namespace MAMBY.UI.Models
{
    public class CardViewModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public decimal TotalPrice { get; set; }
        public int UserId { get; set; }

        public List<CardLineViewModel> Lines { get; set; }

    }
}

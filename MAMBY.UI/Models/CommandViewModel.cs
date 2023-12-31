namespace MAMBY.UI.Models
{
    public class CommandViewModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public bool? IsDeleted { get; set; }
        public string Content { get; set; }

        public int UserId { get; set; }
        public int ProductId { get; set; }
        //public ProductViewModel ProductViewModel { get; set; }
    }
}

namespace MAMBY.UI.Models
{
    public class ProductFeatureViewModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; }
        public string Key { get; set; }
        public string value { get; set; }
        public int ProductId { get; set; }

        //Relation
        public virtual ProductViewModel Products { get; set; }
    }
}

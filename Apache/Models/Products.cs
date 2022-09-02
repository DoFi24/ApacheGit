namespace Apache.Models
{
    public class Products
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? CategoryId { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}

namespace Apache.Models
{
    public class Places
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? PlaceImage { get; set; }
        public override string ToString()
        {
            return Name!;
        }
    }
}

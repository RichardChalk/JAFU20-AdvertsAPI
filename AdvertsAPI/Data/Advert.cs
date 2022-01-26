namespace AdvertsAPI.Data
{
    public class Advert
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;

        public decimal Price { get; set; }
        public DateTime DateAdded { get; set; }
    }
}

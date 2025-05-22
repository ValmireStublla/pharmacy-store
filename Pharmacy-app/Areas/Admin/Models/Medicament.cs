namespace Pharmacy_app.Areas.Admin.Models
{
    public class Medicament
    {
        public Guid ID { get; set; }

        public string? Title { get; set; }
        public float? Price { get; set; }

        public string? Description { get; set; }

        public byte[]? Photo { get; set; }

        public DateTime? DataSkadences { get; set; }
    }
}

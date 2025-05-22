namespace Pharmacy_app.Areas.Admin.Models
{
    public class Pharmacists
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Degree { get; set; }

        public int YearsOfExperience { get; set; }

        public byte[]? ProfilePicture { get; set; }
    }
}

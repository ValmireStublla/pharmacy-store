using System.ComponentModel.DataAnnotations;

namespace Pharmacy_app.Areas.Admin.ViewModels
{
    public class PharmacistsViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Ju lutem shenoni emrin e punetorit.")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Ju lutem shenoni graden.")]
        public string? Degree { get; set; }

        [Required(ErrorMessage = "Ju lutem shenoni vitet e eksperiences.")]
        public int? YearsOfExperience { get; set; }

        public IFormFile ProfilePicture { get; set; }
    }
}

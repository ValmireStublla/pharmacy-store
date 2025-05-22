using System.ComponentModel.DataAnnotations;

namespace Pharmacy_app.Areas.Admin.ViewModels
{
    public class MedicamentViewModel
    {
        public Guid ID { get; set; }

        [Required(ErrorMessage = "Ju lutem shenoni emrin e medikamentit.")]
        public string? Title { get; set; }

        [Required(ErrorMessage = "Ju lutem shenoni cmimin.")]
        public float? Price { get; set; }

        [Required(ErrorMessage = "Ju lutem shenoni pershkrimin.")]
        public string? Description { get; set; }

        public IFormFile Photo { get; set; }

        [Required(ErrorMessage = "Ju lutem shenoni daten e skadences.")]
        public DateTime? DataSkadences { get; set; }
    }
}

using Libs.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _21dthc1DemoAPI.Models
{
    public class InsertBMIModel
    {
        [Required(ErrorMessage = "Cân nặng là bắt buộc.")]
        [Range(1, 500, ErrorMessage = "Cân nặng phải nằm trong khoảng từ 1 đến 500 kg.")]
        public double Weight { get; set; }

        [Required(ErrorMessage = "Chiều cao là bắt buộc.")]
        [Range(0.5, 2.5, ErrorMessage = "Chiều cao phải nằm trong khoảng từ 0.5 đến 2.5 mét.")]
        public double Height { get; set; }

        [Required(ErrorMessage = "HealthRecordId là bắt buộc.")]
        public Guid HealthRecordId { get; set; }
    }
}

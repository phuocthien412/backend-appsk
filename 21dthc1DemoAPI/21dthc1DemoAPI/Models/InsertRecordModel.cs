using Libs.Entity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace _21dthc1DemoAPI.Models
{
    public class InsertRecordModel
    {
        [Required]
        public double BloodPressure { get; set; }

        [Required]
        public DateTime DateCreated { get; set; } = DateTime.UtcNow; // UTC nhất quán

        public List<BMI>? BMIs { get; set; }
    }
}

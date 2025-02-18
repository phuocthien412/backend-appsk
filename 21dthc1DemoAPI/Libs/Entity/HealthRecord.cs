using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Entity
{
    public class HealthRecord
    {
        [Key] // Chỉ định khoá chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid RecordId { get; set; }

        
        public double BloodPressure { get; set; }

        
        public DateTime DateCreated { get; set; } = DateTime.UtcNow; // UTC nhất quán

        public List<BMI>? BMIs { get; set; }
    }
}

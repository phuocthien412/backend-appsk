using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Entity
{
    public class BMI
    {
        [Key] // Chỉ định khoá chính
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid BmiId { get; set; }

        
        [Range(1, 500)] // Ràng buộc giá trị hợp lý
        public double Weight { get; set; }

        
        [Range(0.5, 2.5)] // Chiều cao hợp lý (0.5m - 2.5m)
        public double Height { get; set; }

        [NotMapped] // Không lưu trong cơ sở dữ liệu
        public double BmiValue => Math.Round(Weight / (Height * Height), 2); // Làm tròn 2 chữ số thập phân

        
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;

        
        public Guid HealthRecordId { get; set; } // Khóa ngoại

        // Mối quan hệ N-1 với HealthRecord
        [ForeignKey("HealthRecordId")]
        public HealthRecord HealthRecord { get; set; }
    }

}

using Libs.Entity;

namespace _21dthc1DemoAPI.Models
{
    public class UpdateRecordModel
    {
    
        public double BloodPressure { get; set; }


        public DateTime DateCreated { get; set; } = DateTime.UtcNow; // UTC nhất quán

        public List<BMI>? BMIs { get; set; }


    }
}

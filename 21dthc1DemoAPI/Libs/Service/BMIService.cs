using Libs.Entity;
using Libs.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Libs.Repository.HealthRecordRepository;

namespace Libs.Service
{
   
    public class BMIService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly BMIRepository bMIRepository;
        private readonly HealthRecordRepository healthRecordRepository;


        public BMIService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            this.bMIRepository = new BMIRepository(dbContext);
            this.healthRecordRepository = new HealthRecordRepository(dbContext); 
        }

        void save()
        {
            dbContext.SaveChanges();
        }

        public async Task<List<BMI>> GetAllBMIsAsync()
        {
            return await bMIRepository.GetAllBMIsAsync();
        }

        public async Task<BMI> GetBMIByIdAsync(Guid bmiId)
        {
            var bmi = await bMIRepository.GetBMIByIdAsync(bmiId);
            if (bmi == null)
                throw new KeyNotFoundException($"BMI with ID {bmiId} not found");
            return bmi;
        }

        public async Task AddBMIAsync(BMI bmi)
        {
            await bMIRepository.AddBMIAsync(bmi);
            save();
        }

        public async Task UpdateBMIAsync(BMI bmi) {

            var existingBMI = await bMIRepository.GetBMIByIdAsync(bmi.BmiId);
            if (existingBMI == null)
            {
                throw new KeyNotFoundException($"BMI with ID {bmi.BmiId} not found.");
            }

            await bMIRepository.UpdateBMIAsync(bmi);
            save();
        }

        // Xóa BMI theo ID
        public async Task DeleteBMIAsync(Guid bmiId)
        {
            var bmi = await bMIRepository.GetBMIByIdAsync(bmiId);
            if (bmi == null)
            {
                throw new KeyNotFoundException($"BMI with ID {bmiId} not found.");
            }

            await bMIRepository.DeleteBMIAsync(bmiId);
            save();
        }
    }

}

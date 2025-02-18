using Libs.Data;
using Libs.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libs.Repository
{
    public interface IBMIRepository : IRepository<BMI>
    {
        Task<BMI> GetBMIByIdAsync(Guid bmiId);
        Task<List<BMI>> GetAllBMIsAsync();
        Task AddBMIAsync(BMI bmi);
        Task UpdateBMIAsync(BMI bmi);
        Task DeleteBMIAsync(Guid bmiId);

        Task<List<BMI>> GetBMIDetailsByRecordIdAsync(Guid recordId);
    }

    public class BMIRepository : RepositoryBase<BMI>, IBMIRepository
    {
        public BMIRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<BMI>> GetBMIDetailsByRecordIdAsync(Guid recordId)
        {
            return await _dbContext.BMI
                .Where(b => b.HealthRecordId == recordId)
                .Include(b => b.HealthRecord) // Nếu cần thông tin HealthRecord liên quan
                .ToListAsync();
        }

        public async Task<BMI> GetBMIByIdAsync(Guid bmiId)
        {
            return await _dbContext.BMI
                .Include(b => b.HealthRecord)
                .FirstOrDefaultAsync(b => b.BmiId == bmiId);
        }

        public async Task<List<BMI>> GetAllBMIsAsync()
        {
            return await _dbContext.BMI
                .Include(b => b.HealthRecord)
                .OrderByDescending(b => b.DateCreated)
                .ToListAsync();
        }

        public async Task AddBMIAsync(BMI bmi)
        {
            await _dbContext.BMI.AddAsync(bmi);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateBMIAsync(BMI bmi)
        {
            _dbContext.BMI.Update(bmi);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteBMIAsync(Guid bmiId)
        {
            var bmi = await GetBMIByIdAsync(bmiId);
            if (bmi != null)
            {
                _dbContext.BMI.Remove(bmi);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
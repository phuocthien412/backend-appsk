using Libs.Data;
using Libs.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Libs.Repository.HealthRecordRepository;

namespace Libs.Repository
{
    public class HealthRecordRepository : RepositoryBase<HealthRecord>, IHealthRecordRepository
    {
        public interface IHealthRecordRepository : IRepository<HealthRecord>
        {
            Task<List<HealthRecord>> GetAllHealthRecordsAsync();
            Task<HealthRecord> GetHealthRecordByIdAsync(Guid recordId);
            Task AddHealthRecordAsync(HealthRecord healthRecord);
            Task UpdateHealthRecordAsync(HealthRecord healthRecord);
            Task DeleteHealthRecordAsync(Guid recordId);
            Task<List<BMI>> GetBMIsByRecordIdAsync(Guid recordId);
        }
        public HealthRecordRepository(ApplicationDbContext dbContext) : base(dbContext) 
        { 
        }
        public async Task<List<HealthRecord>> GetAllHealthRecordsAsync()
        {
            return await _dbContext.HealthRecord
                .Include(hr => hr.BMIs) // Include danh sách BMI liên kết
                .OrderByDescending(hr => hr.DateCreated)
                .ToListAsync();
        }

        public async Task<HealthRecord> GetHealthRecordByIdAsync(Guid recordId)
        {
            return await _dbContext.HealthRecord
             .Include(hr => hr.BMIs)
                .FirstOrDefaultAsync(hr => hr.RecordId == recordId);

        }

        public async Task AddHealthRecordAsync(HealthRecord healthRecord)
        {
            await _dbContext.HealthRecord.AddAsync(healthRecord);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateHealthRecordAsync(HealthRecord healthRecord)
        {
            _dbContext.HealthRecord.Update(healthRecord);
            await _dbContext.SaveChangesAsync();
        }


        public async Task DeleteHealthRecordAsync(Guid recordId)
        {
            var record = new HealthRecord { RecordId = recordId };
            _dbContext.HealthRecord.Remove(record);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<BMI>> GetBMIsByRecordIdAsync(Guid recordId)
        {
            return await _dbContext.BMI
                .Where(b => b.HealthRecordId == recordId)
                .ToListAsync();
        }

    }
}

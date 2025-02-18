using Libs.Entity;
using Libs.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Libs.Repository.HealthRecordRepository;

namespace Libs.Service
{
    public class HealthRecordService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IBMIRepository _bmiRepository;
        private readonly IHealthRecordRepository _healthRecordRepository;

        public HealthRecordService(ApplicationDbContext dbContext, IBMIRepository bmiRepository, IHealthRecordRepository healthRecordRepository)
        {
            _dbContext = dbContext;
            _bmiRepository = bmiRepository;
            _healthRecordRepository = healthRecordRepository;
        }

        private void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task<List<HealthRecord>> GetAllHealthRecordsAsync()
        {
            return await _healthRecordRepository.GetAllHealthRecordsAsync();
        }

        public async Task AddHealthRecordAsync(HealthRecord healthRecord)
        {
            await _healthRecordRepository.AddHealthRecordAsync(healthRecord);
            SaveChanges();
        }

        public async Task UpdateHealthRecordAsync(HealthRecord healthRecord)
        {
            var existingHealthRecord = await _healthRecordRepository.GetHealthRecordByIdAsync(healthRecord.RecordId);
            if (existingHealthRecord == null)
            {
                throw new KeyNotFoundException($"HealthRecord with ID {healthRecord.RecordId} not found.");
            }

            await _healthRecordRepository.UpdateHealthRecordAsync(healthRecord);
            SaveChanges();
        }

        public async Task DeleteHealthRecordAsync(Guid recordId)
        {
            var existingRecord = await _healthRecordRepository.GetHealthRecordByIdAsync(recordId);
            if (existingRecord == null)
            {
                throw new KeyNotFoundException($"HealthRecord with ID {recordId} not found.");
            }

            await _healthRecordRepository.DeleteHealthRecordAsync(recordId);
            SaveChanges();
        }

        public async Task<HealthRecord> GetHealthRecordByIdAsync(Guid recordId)
        {
            var healthRecord = await _healthRecordRepository.GetHealthRecordByIdAsync(recordId);
            if (healthRecord == null)
            {
                throw new KeyNotFoundException($"HealthRecord with ID {recordId} not found.");
            }

            return healthRecord;
        }

        public async Task<List<BMI>> GetBMIsByRecordIdAsync(Guid recordId)
        {
            // Ensure HealthRecord exists before querying BMI
            var existingRecord = await _healthRecordRepository.GetHealthRecordByIdAsync(recordId);
            if (existingRecord == null)
            {
                throw new KeyNotFoundException($"HealthRecord with ID {recordId} not found.");
            }

            return await _dbContext.BMI
                .Where(b => b.HealthRecordId == recordId)
                .ToListAsync();
        }
    }
}

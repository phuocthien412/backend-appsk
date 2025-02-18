using _21dthc1DemoAPI.Models;
using Azure;
using Libs.Entity;
using Libs.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _21dthc1DemoAPI.Controllers.api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecordController : ControllerBase
    {
        private readonly HealthRecordService _healthRecordService;

        public RecordController(HealthRecordService healthRecord)
        {
            _healthRecordService = healthRecord;
        }

        // Lấy tất cả 
        [HttpGet("get-all-HealthRecord")]
        //[Authorize(Policy = "BMI.View")]
        public async Task<IActionResult> GetAllHealthRecords()
        {
            try
            {
                var healthRecords = await _healthRecordService.GetAllHealthRecordsAsync();

                // Sắp xếp theo DateCreated giảm dần
                var sortedRecords = healthRecords
                    .OrderByDescending(hr => hr.DateCreated) // Sắp xếp theo ngày
                    .Select(hr => new
                    {
                        hr.RecordId,
                        hr.BloodPressure,
                        hr.DateCreated,
                        BmiIds = hr.BMIs?.Select(bmi => bmi.BmiId).ToList() // Trả về danh sách BMIId
                    });

                return Ok(new
                {
                    status = true,
                    message = "Lấy danh sách HealthRecords thành công",
                    totalRecords = sortedRecords.Count(),
                    data = sortedRecords
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }



        // Lấy thông tin BMI theo ID
        [HttpGet("get-healthrecord/{id}")]
        public async Task<IActionResult> GetHealthRecordById(Guid id)
        {
            try
            {
                var healhRecordList = await _healthRecordService.GetHealthRecordByIdAsync(id);
                if (healhRecordList == null)
                {
                    return NotFound(new { status = false, message = "Không tìm thấy HEALHRECORD với ID được cung cấp." });
                }
                return Ok(new
                {
                    status = true,
                    message = "Lấy danh sách HealthRecords thành công",
                    data = healhRecordList
                });


            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }


        // them moi
        [HttpPost("insert-healthrecord")]
        public async Task<IActionResult> InsertHealthRecord(InsertRecordModel healthRecord)
        {
            try
            {
                HealthRecord healthRecord1 = new HealthRecord
                {

                    RecordId = Guid.NewGuid(),
                    BloodPressure = healthRecord.BloodPressure,
                    DateCreated = DateTime.UtcNow,
                    BMIs = healthRecord.BMIs,

                };

                await _healthRecordService.AddHealthRecordAsync(healthRecord1);
                return Ok(new { status = true, message = "Thêm HealthRecord thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        [HttpGet("{recordId}/bmis")]
        public async Task<IActionResult> GetBMIsByRecordId(Guid recordId)
        {
            try
            {
                var bmis = await _healthRecordService.GetBMIsByRecordIdAsync(recordId); // Fetch full BMI details
                return Ok(new
                {
                    status = true,
                    message = "Fetched BMI details successfully.",
                    data = bmis
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { status = false, message = $"Error: {ex.Message}" });
            }
        }






        ////[Authorize(Policy = "BMI.Edit")]
        //// Cập nhật HealthRecord
        //[HttpPut("update-healthrecord/{id}")]

        //public async Task<IActionResult> UpdateHealthRecord(Guid id,UpdateRecordModel recordModel)
        //{
        //    try
        //    {

        //        var existingRecord = await _healthRecordService.GetHealthRecordByIdAsync(id);
        //        if (existingRecord == null)
        //        {
        //            return NotFound(new { status = false, message = $"HealthRecord with ID {id} not found." });
        //        }

        //        existingRecord.BloodPressure = recordModel.BloodPressure;
        //        existingRecord.DateCreated = recordModel.DateCreated;

        //        await _healthRecordService.UpdateHealthRecordAsync(existingRecord);

        //        return Ok(new
        //        {
        //            status = true,
        //            message = "Cập nhật HealthRecord thành công!",
        //            data = new
        //            {

        //                existingRecord.BloodPressure,
        //                existingRecord.DateCreated
        //            }
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { status = false, message = $"Có lỗi xảy ra: {ex.Message}" });
        //    }
        //}




        //// Xóa HealthRecord
        //[HttpDelete("delete-healthrecord/{id}")]
        //public async Task<IActionResult> DeleteHealthRecord(Guid id)
        //{
        //    try
        //    {
        //        // Kiểm tra xem HealthRecord có tồn tại không
        //        var existingRecord = await _healthRecordService.GetHealthRecordByIdAsync(id);
        //        if (existingRecord == null)
        //        {
        //            return NotFound(new { status = false, message = $"Không tìm thấy HealthRecord với ID {id} để xóa." });
        //        }

        //        // Thực hiện xóa
        //        await _healthRecordService.DeleteHealthRecordAsync(id);
        //        return Ok(new { status = true, message = "Xóa HealthRecord thành công!" });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Xử lý lỗi và trả về phản hồi
        //        return BadRequest(new { status = false, message = $"Có lỗi xảy ra: {ex.Message}" });
        //    }
        //}

    }
}
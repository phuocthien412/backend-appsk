using _21dthc1DemoAPI.Models;
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
    public class BMIController : ControllerBase
    {
        private readonly BMIService _bmiService;

        public BMIController(BMIService bmiService)
        {
            _bmiService = bmiService;
        }

        // Lấy tất cả BMI 
        [HttpGet("get-all-bmi")]
        //[Authorize(Policy = "BMI.View")]
        public async Task<IActionResult> GetAllBMIs()
        {
            try
            {
                var bmiList = await _bmiService.GetAllBMIsAsync();

                // Tính toán và phân loại BMI cho từng đối tượng
                var response = bmiList.Select(bmi =>
                {
                    var result = ClassifyBMI(bmi.Weight, bmi.Height);
                    return new
                    {
                        BmiId = bmi.BmiId,
                        Weight = bmi.Weight,
                        Height = bmi.Height,
                        BmiValue = result.BmiValue,
                        Classification = result.Classification,
                        DateCreated = bmi.DateCreated,
                        HealthRecordId = bmi.HealthRecordId
                    };
                }).ToList();

                return Ok(new
                {
                    status = true,
                    message = "Lấy danh sách BMI thành công",
                    totalRecord = response.Count,
                    data = response
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }


        // Lấy thông tin BMI theo ID
        [HttpGet("get-bmi/{id}")]
        public async Task<IActionResult> GetBMIById(Guid id)
        {
            try
            {
                var bmi = await _bmiService.GetBMIByIdAsync(id);
                if (bmi == null)
                {
                    return NotFound(new { status = false, message = "Không tìm thấy BMI với ID được cung cấp." });
                }

                var result = ClassifyBMI(bmi.Weight, bmi.Height);
                return Ok(new
                {
                    BmiId = bmi.BmiId,
                    Weight = bmi.Weight,
                    Height = bmi.Height,
                    BmiValue = result.BmiValue,
                    Classification = result.Classification,
                    DateCreated = bmi.DateCreated,
                    HealthRecordId = bmi.HealthRecordId
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }



        // Thêm mới BMI
        [HttpPost("insert-bmi")]
        public async Task<IActionResult> InsertBMI(InsertBMIModel bmiModel)
        {
            try
            {
                BMI bmi = new BMI
                {
                    BmiId = Guid.NewGuid(),
                    Weight = bmiModel.Weight,
                    Height = bmiModel.Height,
                    DateCreated = DateTime.UtcNow,
                    HealthRecordId = bmiModel.HealthRecordId
                };

                await _bmiService.AddBMIAsync(bmi);
                return Ok(new { status = true, message = "Thêm BMI thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }



        //[Authorize(Policy = "BMI.Edit")]
        // Cập nhật BMI
        //"weight": 70
    //"height": 1.75,
    //"healthRecordId": "5a873d56-b984-4eb8-b6c6-bad1c0d6066e"
   
    [HttpPut("update-bmi/{id}")]
        public async Task<IActionResult> UpdateBMI(Guid id,UpdateBMIModel bmiModel)
        {
            try
            {
                var existingBMI = await _bmiService.GetBMIByIdAsync(id);
                if (existingBMI == null)
                {
                    return NotFound(new { status = false, message = "Không tìm thấy BMI để cập nhật." });
                }

                existingBMI.Weight = bmiModel.Weight;
                existingBMI.Height = bmiModel.Height;

                await _bmiService.UpdateBMIAsync(existingBMI);
                return Ok(new { status = true, message = "Cập nhật BMI thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        // Xóa BMI
        [HttpDelete("delete-bmi/{id}")]
        public async Task<IActionResult> DeleteBMI(Guid id)
        {
            try
            {
                var existingBMI = await _bmiService.GetBMIByIdAsync(id);
                if (existingBMI == null)
                {
                    return NotFound(new { status = false, message = "Không tìm thấy BMI để xóa." });
                }

                await _bmiService.DeleteBMIAsync(id);
                return Ok(new { status = true, message = "Xóa BMI thành công!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { status = false, message = $"Có lỗi xảy ra: {ex.Message}" });
            }
        }

        public (double BmiValue, string Classification) ClassifyBMI(double weight, double height)
        {
            double bmiValue = Math.Round(weight / (height * height), 2);
            string classification;

            if (bmiValue < 18.5)
                classification = "Nhẹ cân";
            else if (bmiValue >= 18.5 && bmiValue < 23)
                classification = "Bình thường";
            else if (bmiValue >= 23 && bmiValue < 25)
                classification = "Thừa cân";
            else
                classification = "Béo phì";

            return (bmiValue, classification);
        }


    }
}
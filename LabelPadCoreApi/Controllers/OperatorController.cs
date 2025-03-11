using LabelPad.Domain.Models;
using LabelPad.Repository.OperatorManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LabelPadCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperatorController : ControllerBase
    {
        private readonly IOperatorRepository _operatorRepository;

        public OperatorController(IOperatorRepository operatorRepository)
        {
            _operatorRepository = operatorRepository;
        }

        [HttpGet("GetAllOperators")]
        public async Task<ActionResult<IEnumerable<OperatorDetails>>> GetAllOperators(int LoginId,int RoleId)
        {
            try
            {

            var OperatorDetails = await _operatorRepository.GetAllOperators(LoginId, RoleId);
                return Ok(OperatorDetails);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        [HttpGet("GetOperatorById")]
        public async Task<ActionResult<OperatorDetails>> GetOperatorById(int id)
        {
            try
            {

            var operatorDetail = await _operatorRepository.GetOperatorById(id);
            if (operatorDetail == null) return NotFound();
                return Ok(operatorDetail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
          [HttpPost("CreateOperator")]
        public async Task<IActionResult> CreateOperator(
     [FromForm] OperatorDetails operatorDetail, // Use [FromForm] instead of [FromBody]
     IFormFile Profile, // File input for profile image
     IFormFile HelpImage // File input for help image
 )
        {
            try
            {
                if (operatorDetail == null)
                {
                    return BadRequest("Operator details cannot be null.");
                }

                // Validate required fields
                if (string.IsNullOrEmpty(operatorDetail.FirstName) ||
                    string.IsNullOrEmpty(operatorDetail.LastName) ||
                    string.IsNullOrEmpty(operatorDetail.Email))
                {
                    return BadRequest("First Name, Last Name, and Email are required.");
                }

                // Process profile image (if provided)
                if (Profile != null)
                {
                    var profilePath = Path.Combine("Uploads", Profile.FileName);
                    using (var stream = new FileStream(profilePath, FileMode.Create))
                    {
                        await Profile.CopyToAsync(stream);
                    }
                    operatorDetail.Profile = profilePath; // Save path in DB
                }

                // Process help image (if provided)
                if (HelpImage != null)
                {
                    var helpImagePath = Path.Combine("Uploads", HelpImage.FileName);
                    using (var stream = new FileStream(helpImagePath, FileMode.Create))
                    {
                        await HelpImage.CopyToAsync(stream);
                    }
                    operatorDetail.HelpImage = helpImagePath; // Save path in DB
                }

                var createdOperator = await _operatorRepository.CreateOperator(operatorDetail);
                return Ok(createdOperator);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
        }
        [HttpPut("UpdateOperators")]
        public async Task<IActionResult> UpdateOperators(
  [FromForm] OperatorDetails operatorDetail, // Use [FromForm] instead of [FromBody]
  IFormFile Profiles, // File input for profile image
  IFormFile HelpImages // File input for help image
)
        {
            try
            {
                if (operatorDetail == null)
                {
                    return BadRequest("Operator details cannot be null.");
                }

                // Validate required fields
                if (string.IsNullOrEmpty(operatorDetail.FirstName) ||
                    string.IsNullOrEmpty(operatorDetail.LastName) ||
                    string.IsNullOrEmpty(operatorDetail.Email))
                {
                    return BadRequest("First Name, Last Name, and Email are required.");
                }

                // Process profile image (if provided)
                if (Profiles != null)
                {
                    var profilePath = Path.Combine("Uploads", Profiles.FileName);
                    using (var stream = new FileStream(profilePath, FileMode.Create))
                    {
                        await Profiles.CopyToAsync(stream);
                    }
                    operatorDetail.Profile = profilePath; // Save path in DB
                }

                // Process help image (if provided)
                if (HelpImages != null)
                {
                    var helpImagePath = Path.Combine("Uploads", HelpImages.FileName);
                    using (var stream = new FileStream(helpImagePath, FileMode.Create))
                    {
                        await HelpImages.CopyToAsync(stream);
                    }
                    operatorDetail.HelpImage = helpImagePath; // Save path in DB
                }

                var createdOperator = await _operatorRepository.UpdateOperator(operatorDetail);
                return Ok(createdOperator);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred.");
            }
        }

        [HttpPost("CreateOperators")]
        public async Task<ActionResult<OperatorDetails>> CreateOperators(OperatorDetails operatorDetail)
        {
            try
            { 
            var createdOperator = await _operatorRepository.CreateOperator(operatorDetail);
                return Ok(createdOperator);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPut("UpdateOperator")]
        public async Task<IActionResult> UpdateOperator(OperatorDetails operatorDetail)
        {
            try
            { 
            var updatedOperator = await _operatorRepository.UpdateOperator(operatorDetail);
            if (updatedOperator == null) 
                return NotFound();
                return Ok(updatedOperator);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpDelete("DeleteOperator")]
        public async Task<IActionResult> DeleteOperator(int id)
        {
            try
            {

                var result = await _operatorRepository.DeleteOperator(id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}


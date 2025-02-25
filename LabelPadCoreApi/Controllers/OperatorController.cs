using LabelPad.Domain.Models;
using LabelPad.Repository.OperatorManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OperatorDetails>>> GetAllOperators()
        {
            try
            {

            var OperatorDetails = await _operatorRepository.GetAllOperators();
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
        public async Task<ActionResult<OperatorDetails>> CreateOperator(OperatorDetails operatorDetail)
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


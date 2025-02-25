using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.OperatorManagement
{
    public class OperatorRepository : IOperatorRepository
    {
        private readonly LabelPadDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public OperatorRepository(LabelPadDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<IEnumerable<OperatorDetails>> GetAllOperators()
        {
            try
            {

            var OperatorDetails = await _dbContext.OperatorDetail.Where(o => !o.IsDeleted).ToListAsync();
                return OperatorDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OperatorDetails> GetOperatorById(int id)
        {
            try
            {

            var OperatorDetails = await _dbContext.OperatorDetail.FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);
                return OperatorDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OperatorDetails> CreateOperator(OperatorDetails operatorDetails)
        {
            try
            {

            operatorDetails.IsActive = true;
            operatorDetails.IsDeleted = false;
            operatorDetails.CreatedOn = DateTime.Now;
            _dbContext.OperatorDetail.Add(operatorDetails);
            await _dbContext.SaveChangesAsync();
                return operatorDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<OperatorDetails> UpdateOperator(OperatorDetails operatorDetails)
        {
            try
            {

                var existingOperator = await _dbContext.OperatorDetail.FindAsync(operatorDetails.Id);
                if (existingOperator == null) return null;

                existingOperator.FirstName = operatorDetails.FirstName;
                existingOperator.LastName = operatorDetails.LastName;
                existingOperator.OperatorName = operatorDetails.OperatorName;
                existingOperator.Email = operatorDetails.Email;
                existingOperator.ContactctNumber = operatorDetails.ContactctNumber;
                existingOperator.RegisteredAddress = operatorDetails.RegisteredAddress;
                existingOperator.TradingAddress = operatorDetails.TradingAddress;
                existingOperator.RegisteredCity = operatorDetails.RegisteredCity;
                existingOperator.TradingCity = operatorDetails.TradingCity;
                existingOperator.RegisteredCounty = operatorDetails.RegisteredCounty;
                existingOperator.TradingCounty = operatorDetails.TradingCounty;
                existingOperator.RegisteredCountryId = operatorDetails.RegisteredCountryId;
                existingOperator.TradingCountryId = operatorDetails.TradingCountryId;
                existingOperator.RegisteredZipCode = operatorDetails.RegisteredZipCode;
                existingOperator.TradingZipCode = operatorDetails.TradingZipCode;
                existingOperator.VatRegistered = operatorDetails.VatRegistered;
                existingOperator.VatNumber = operatorDetails.VatNumber;
                existingOperator.RoleId = operatorDetails.RoleId;
                existingOperator.Date = DateTime.Now;
                existingOperator.Notes = operatorDetails.Notes;
                existingOperator.Profile = operatorDetails.Profile;
                existingOperator.HelpImage = operatorDetails.HelpImage;
                existingOperator.Content = operatorDetails.Content;
                existingOperator.Heading = operatorDetails.Heading;
                existingOperator.IsMicrosoftAccount = operatorDetails.IsMicrosoftAccount;
                operatorDetails.IsActive = true;
                operatorDetails.IsDeleted = false;
                existingOperator.UpdatedOn = DateTime.Now;
                existingOperator.UpdatedBy = operatorDetails.UpdatedBy;

                _dbContext.OperatorDetail.Update(existingOperator);
                await _dbContext.SaveChangesAsync();
                return existingOperator;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<dynamic> DeleteOperator(int id)
        {
            try
            {

            
            var operatorDetail = await _dbContext.OperatorDetail.FindAsync(id);
            if (operatorDetail == null)
            {
                return new { Message = "Operator not found" };
            }

            operatorDetail.IsDeleted = true;
            _dbContext.OperatorDetail.Update(operatorDetail);
            await _dbContext.SaveChangesAsync();

                return new { Message = "Operator deleted successfully" };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}

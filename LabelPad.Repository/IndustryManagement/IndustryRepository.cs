using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.IndustryManagement
{
    public class IndustryRepository : IIndustryRepository
    {
        private readonly LabelPadDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public IndustryRepository(LabelPadDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<IEnumerable<Industries>> GetAllIndustries()
        {
            try
            {

                var IndustryDetails = await _dbContext.Industry.Where(o => !o.IsDelete).ToListAsync();
                return IndustryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Industries> GetIndustryById(int id)
        {
            try
            {

                var IndustryDetails = await _dbContext.Industry.FirstOrDefaultAsync(o => o.Id == id && !o.IsDelete);
                return IndustryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Industries> InsertIndustry(Industries industry)
        {
            try
            {

                industry.IsActive = true;
                industry.IsDelete = false;
                industry.CreatedBy = "";
                // industry.CreatedDate = DateTime.Now;
                _dbContext.Industry.Add(industry);
                await _dbContext.SaveChangesAsync();
                return industry;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Industries> UpdateIndustry(Industries industry)
        {
            try
            {

                var existingIndusty = await _dbContext.Industry.FindAsync(industry.Id);
                if (existingIndusty == null) return null;

                existingIndusty.IndustryName = industry.IndustryName;
                existingIndusty.IsActive = true;
                existingIndusty.IsDelete = false;
                existingIndusty.UpdatedDate = DateTime.Now;
                existingIndusty.UpdatedBy = "";

                _dbContext.Industry.Update(existingIndusty);
                await _dbContext.SaveChangesAsync();
                return existingIndusty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<Industries> DeleteIndustry(int id)
        {
            try
            {


                var industryDetails = await _dbContext.Industry.FindAsync(id);
                if (industryDetails == null)
                {
                    return null;
                }
                industryDetails.IsActive = false;
                industryDetails.IsDelete = true;
                industryDetails.UpdatedBy = "";
                industryDetails.UpdatedDate = DateTime.Now;
                _dbContext.Industry.Update(industryDetails);
                await _dbContext.SaveChangesAsync();

                return industryDetails;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

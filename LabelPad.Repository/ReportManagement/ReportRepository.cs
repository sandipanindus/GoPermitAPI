using LabelPad.Domain.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LabelPad.Repository.ReportManagement
{
   public class ReportRepository:IReportRepository
    {
        private readonly LabelPadDbContext _dbContext;
        public ReportRepository(LabelPadDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<dynamic> GetSupportList()
        {
            var support = (from s in _dbContext.Supports
                           join r in _dbContext.RegisterUsers on s.RegisterUserId equals r.Id
                           select new
                           {
                               s.Id,
                               s.Issue,
                               r.FirstName,
                               r.LastName,
                               r.MobileNumber,
                               r.Email
                           }).ToList();
          //  var support = await _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false).OrderByDescending(x => x.Id).ToListAsync();

            return support;
        }
    }
}

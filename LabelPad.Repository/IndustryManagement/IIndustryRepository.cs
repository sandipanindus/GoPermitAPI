using LabelPad.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.IndustryManagement
{
    public interface IIndustryRepository
    {
        Task<IEnumerable<Industries>> GetAllIndustries();
        Task<Industries> GetIndustryById(int id);
        Task<Industries> InsertIndustry(Industries industry);
        Task<Industries> UpdateIndustry(Industries industry);
        Task<Industries> DeleteIndustry(int id);
    }
}

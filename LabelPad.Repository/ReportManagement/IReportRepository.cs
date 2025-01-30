using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.ReportManagement
{
   public interface IReportRepository
    {
        Task<dynamic> GetSupportList();
    }
}

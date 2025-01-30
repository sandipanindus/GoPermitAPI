using LabelPad.Domain.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.PermissionManagement
{
   public interface IPermissionRepository
    {
        Task<dynamic> GetScreens(int RoleId, int ClientId, int LoginId);
        Task<dynamic> GetModuleScreens(int RoleId);
        Task<dynamic> SavePermissionData(List<GetModulesAc> objinput);
        Task<dynamic> GetPingReports(int PageNo, int PageSize, int SiteId, int UserId, string FromDate, string ToDate);
    }
}

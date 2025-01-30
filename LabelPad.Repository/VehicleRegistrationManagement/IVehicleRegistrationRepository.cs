using LabelPad.Domain.ApplicationClasses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.VehicleRegistrationManagement
{
   public interface IVehicleRegistrationRepository
    {
        Task<dynamic> AddVehicles(List<AddVehicleRegistrationAc> objinput);
        Task<dynamic> GetTenantsBySite(int SiteId);
        Task<dynamic> GetVehicleDetails();
    }
}

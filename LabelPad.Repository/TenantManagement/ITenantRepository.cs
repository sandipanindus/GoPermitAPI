using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.TenantManagement
{
   public  interface ITenantRepository
    {
        Task<dynamic> UpdateVisitorParking(UpdateVistorsParkingRequest parkingRequest);
        Task<dynamic> AddVehicles(List<AddVehicleRegistrationAc> objinput);
        Task<dynamic> AddVehicle_New(List<AddVehicleRegistrationAc> objinput);
        Task<dynamic> UpdateVehicle(List<AddVehicleRegistrationAc> objinput);
        Task<dynamic> AddVehicleTimeSlot(List<AddVehicleTimeSlotAc> objinput);

        Task<Support> AddSupport(AddSupportAc objinput);
        Task<dynamic> GetVehicleDetails(string tenantid);
        Task<dynamic> Getbaynobytenant(string tenantid);
        Task<dynamic> Getvehiclecountsdetails(string tenantid, string bayno);
        Task<dynamic> getvehcilecountsbydatesvrm(string tenantid, string bayno, string dates, string vrm);
        Task<dynamic> GetvehiclecountsdetailsById(string tenantid, string bayno, int Id);
        Task<dynamic> getvehcilecountsbydates(string tenantid,string bayno,string dates);
        Task<dynamic> getvehcilelistcountsbydates(string tenantid, string bayno, string dates);
        Task<dynamic> GetSupportList(int TenantId);
        Task<dynamic> GetSupportById(int Id, int TicketId, int TenantId);
        Task<dynamic> ReplySupport(AddSupportAc objinput);

        Task<dynamic> getversions();



    }
}

using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.VisitorParkingManagement
{
   public interface IVisitorParkingRepository
    {
        Task<dynamic> AddVisitorParking(AddVisitorParkingAc objvisitor);
        Task<dynamic> UpdateVisitorParking(AddVisitorParkingAc objvisitor);
        Task<dynamic> UpdateNotification(int ID);

        Task<List<VisitorParking>> GetVisitorParkings(int TenantId);
        Task<dynamic> GetVisitorParkingById(int Id);
        Task<dynamic> GetVisitorParkingBysiteId(int Id);
        Task<dynamic> GetVisitordetailsById(int Id);
        Task<dynamic> GetVisitorParkingBysiteIddate(int Id,string date);


        bool GetExistsVehicleParking(AddVisitorParkingAc objvisitor);
        Task<dynamic> DeleteVisitorParking(int Id);
        Task<dynamic> GetVisitorBayNos(int SiteId);
        Task<dynamic> GetVisitorBayNosEdit(int SiteId, int TenantId);
        Task<dynamic> GetVisitorSlot(int Id);
        Task<dynamic> UpdateVisitorSlot(UpdateVisitorSlot obj);
    }
}

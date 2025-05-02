using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace LabelPad.Repository.VehicleRegistrationManagement
{
    public class VehicleRegistrationRepository:IVehicleRegistrationRepository
    {
        private readonly LabelPadDbContext _dbContext;
        public VehicleRegistrationRepository(LabelPadDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<dynamic> GetVehicleDetails()
        {
            var vehicles = (from v in _dbContext.VehicleRegistrations
                            join r in _dbContext.RegisterUsers on v.RegisterUserId equals r.Id
                            join s in _dbContext.Sites on r.SiteId equals s.Id
                            where v.IsActive == true && v.IsDeleted == false 
                            select  new 
                            {
                                FirstName=r.FirstName,
                                LastName=r.LastName,
                                SiteName=s.SiteName,
                                ParkingBay=r.ParkingBay,
                                Id=r.Id
                            }).Distinct().OrderByDescending(x=>x.Id).ToList();
            return vehicles;
        }
        public async Task<dynamic> GetTenantsBySite(int SiteId)
        {
            List<RegisterUser> users =await _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == SiteId && x.RoleId==3).ToListAsync();
            List<RegisterUser> user = new List<RegisterUser>();
            for (int i = 0; i < users.Count; i++)
            {
                RegisterUser model = new RegisterUser();
                int id = users[i].Id;
                VehicleRegistration registration =  _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == id).FirstOrDefault();
                if (registration == null)
                {
                    model.Address = users[i].Address;
                    model.Address2 = users[i].Address2;
                    model.City = users[i].City;
                    model.ClientId = users[i].ClientId;
                    model.CountryId = users[i].CountryId;
                    model.CreatedBy = users[i].CreatedBy;
                    model.CreatedOn = users[i].CreatedOn;
                    model.Email = users[i].Email;
                    model.EmailCode = users[i].EmailCode;
                    model.FirstName = users[i].FirstName;
                    model.Id = users[i].Id;
                    model.IsActive = users[i].IsActive;
                    model.IsDeleted = users[i].IsDeleted;
                    model.IsVerified = users[i].IsVerified;
                    model.LastName = users[i].LastName;
                    model.MobileNumber = users[i].MobileNumber;
                    model.ParentId = users[i].ParentId;
                    model.ParkingBay = users[i].ParkingBay;
                    model.Password = users[i].Password;
                    model.RoleId = users[i].RoleId;
                    model.SiteId = users[i].SiteId;
                    model.State = users[i].State;
                    model.UpdatedBy = users[i].UpdatedBy;
                    user.Add(model);
                }
            }
            return user;
        }
        public async Task<dynamic> AddVehicles(List<AddVehicleRegistrationAc> objinput)
        {
            try
            {
                string uniqueVehicleId = Guid.NewGuid().ToString();
                for (int i = 0; i < objinput.Count; i++)
                {
                    VehicleRegistration vehicle =await _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.Model == objinput[i].Model && x.RegisterUserId == objinput[i].TenantId).FirstOrDefaultAsync();
                    if (vehicle == null)
                    {
                        VehicleRegistration reg = new VehicleRegistration();
                        if (objinput[i].StartDate == "")
                        {

                            reg.StartDate = null;
                        }
                        else
                        {
                            reg.StartDate = Convert.ToDateTime(objinput[i].StartDate);
                        }
                        if (objinput[i].EndDate == "")
                        {

                            reg.EndDate = null;
                        }
                        else
                        {
                            reg.EndDate = Convert.ToDateTime(objinput[i].EndDate);
                        }
                        reg.ParkingBayNo = Convert.ToInt32(objinput[i].bayno);
                        reg.Make = objinput[i].Make;
                        reg.Model = objinput[i].Model;
                        reg.VRM = objinput[i].vrm;
                        reg.RegisterUserId = objinput[i].TenantId;
                        reg.IsActive = true;
                        reg.IsDeleted = false;
                        reg.CreatedBy = objinput[i].LoginId;
                        reg.CreatedOn = DateTime.Now;
                        reg.UniqueVehicleId = uniqueVehicleId;
                        _dbContext.VehicleRegistrations.Add(reg);
                        await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        vehicle.Make = objinput[i].Make;
                        vehicle.ParkingBayNo = Convert.ToInt32(objinput[i].bayno);
                        vehicle.Model = objinput[i].Model;
                        vehicle.VRM = objinput[i].vrm;
                        if (objinput[i].StartDate == "")
                        {

                            vehicle.StartDate = null;
                        }
                        else
                        {
                            vehicle.StartDate = Convert.ToDateTime(objinput[i].StartDate);
                        }
                        if (objinput[i].EndDate == "")
                        {

                            vehicle.EndDate = null;
                        }
                        else
                        {
                            vehicle.EndDate = Convert.ToDateTime(objinput[i].EndDate);
                        }
                       
                        vehicle.UpdatedBy = objinput[i].LoginId;
                        vehicle.UpdatedOn = DateTime.Now;
                        _dbContext.VehicleRegistrations.Add(vehicle);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {

            }
            return new { Message = "Saved Successfully" };
        }
    }
}

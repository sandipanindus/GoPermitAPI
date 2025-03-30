using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.VisitorParkingManagement
{
    public class VisitorParkingRepository : IVisitorParkingRepository
    {
        private readonly LabelPadDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public VisitorParkingRepository(LabelPadDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<dynamic> GetVisitorBayNos(int SiteId)
        {
            // var visitorbaynos = await _dbContext.VisitorBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == SiteId && x.RegisterUserId == 0).ToListAsync();
            return null;
        }
        public async Task<dynamic> GetVisitorBayNosEdit(int SiteId, int UserId)
        {
            //  var visitorbaynos = await _dbContext.VisitorBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == SiteId && (x.RegisterUserId == 0 || x.RegisterUserId == UserId)).ToListAsync();
            return null;
        }
        public async Task<dynamic> DeleteVisitorParking(int Id)
        {
            try
            {
                var visitor = _dbContext.VisitorParkingTemps.Where(x => x.IsActive == false && x.IsDeleted == false && x.Id == Id).FirstOrDefault();
                if (visitor != null)
                {
                    //var visitorvehicle = _dbContext.VisitorParkingVehicleDetails.Where(x => x.IsActive == true && x.IsDeleted == false && x.VisitorParkingId == Id).ToList();
                    //for (int i = 0; i < visitorvehicle.Count; i++)
                    //{
                    //    int bayid = visitorvehicle[i].VisitorBayNoId;
                    //    int registeruserid = visitor.RegisterUserId;
                    //    VisitorBayNo bayno = new VisitorBayNo();

                    //}
                    visitor.IsActive = false;
                    visitor.IsDeleted = true;
                    visitor.UpdatedBy = visitor.RegisterUserId;
                    visitor.UpdatedOn = DateTime.Now;
                    _dbContext.VisitorParkingTemps.Update(visitor);
                    _dbContext.SaveChanges();
                    return new { Message = "Deleted Successfully" };
                }
                else if(visitor == null)
                {
                 var visitor1 = _dbContext.VisitorParkings.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Id).FirstOrDefault();
                    if(visitor1!= null)
                    {
                        visitor1.IsActive = false;
                        visitor1.IsDeleted = true;
                        visitor1.UpdatedBy = visitor1.RegisterUserId;
                        visitor1.UpdatedOn = DateTime.Now;
                        _dbContext.VisitorParkings.Update(visitor1);
                        _dbContext.SaveChanges();
                        return new { Message = "Deleted Successfully" };
                    }
                }
                return new { Message = "Deleted Successfully" };

            }
            catch
            {
                return null;
            }
        }


        public async Task<dynamic> GetVisitorSlot(int Id)
        {
            var visitorparktemp = _dbContext.VisitorParkingTemps.Where(x => x.Id == Id && x.IsActive==false && x.IsDeleted==false).FirstOrDefault();
            if (visitorparktemp != null)
            {
                DateTime createddate = visitorparktemp.StartDate.AddMinutes(20);
                DateTime currentdate = DateTime.Now;
                if (createddate >= currentdate)
                {
                    int SiteId = visitorparktemp.SiteId;
                    ArrayList array = new ArrayList();
                    array.Add(SiteId);
                    array.Add(visitorparktemp.StartTime);
                    string dt = visitorparktemp.StartDate.ToString("yyyy-MM-dd") + " " + visitorparktemp.StartTime;
                    array.Add(Convert.ToDateTime(dt));
                    AdminBusiness business = new AdminBusiness();
                    string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
                    DataSet ds = business.GetVisitorBaynos(array, strConnection);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        int visitorbayId = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
                        var visitorbaynos = _dbContext.VisitorBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == SiteId && x.Id == visitorbayId).ToList();
                        if (visitorbaynos != null)
                        {
                            int visitorbaynoid = visitorbaynos[0].Id;
                            string visitorbayname = visitorbaynos[0].BayName;
                            var visitor = (from v in _dbContext.VisitorParkingTemps
                                           join s in _dbContext.Sites on v.SiteId equals s.Id
                                           where v.IsActive == false && v.IsDeleted == false && v.Id == Id
                                           select new
                                           {
                                               PropertyName = s.SiteName,
                                               vrm = v.VRMNumber,
                                               FirstName = v.Name,
                                               Surname = v.Surname,
                                               Email = v.Email,
                                               Contact = v.MobileNumber,
                                               Date = v.StartDate.ToString("dd-MM-yyyy"),
                                               StartTime = v.StartTime,
                                               EndTime = v.EndTime,
                                               Duration = v.Duration,
                                               bayid = visitorbaynoid,
                                               bayname = visitorbayname
                                           }).FirstOrDefault();

                            return visitor;
                        }

                       
                    }
                    else
                    {
                        return false;
                    }
                   
                }
                else
                {
                    return false;
                }
            }
            
           
            return null;
           
        }

        public async Task<dynamic> UpdateVisitorSlot(UpdateVisitorSlot obj)
        {
            var visitor = _dbContext.VisitorParkingTemps.Where(x => x.IsActive == false && x.IsDeleted == false && x.Id == obj.Id).FirstOrDefault();
            if (visitor != null)
            {
                

                visitor.VRMNumber = obj.vrm;
                visitor.IsActive = true;
                visitor.UpdatedBy = visitor.Id;
                visitor.UpdatedOn = DateTime.Now;
                _dbContext.VisitorParkingTemps.Update(visitor);
                _dbContext.SaveChanges();
                VisitorParking parking = new VisitorParking();
                parking.IsActive = true;
                parking.IsDeleted = false;
                parking.CreatedOn = DateTime.Now;
                parking.CreatedBy = visitor.CreatedBy;
                parking.SiteId = visitor.SiteId;
                parking.RegisterUserId = visitor.RegisterUserId;
                parking.Name = obj.firstname;
                parking.Surname = obj.lastname;
                parking.Email = visitor.Email;
                parking.MobileNumber = obj.contact;
                parking.VRMNumber = obj.vrm;
                parking.VisitorBayNoId = obj.visitorbayid;
                parking.Duration = visitor.Duration;
                parking.SessionUnit = visitor.SessionUnit;
                parking.StartDate = visitor.StartDate;
                parking.EndDate = visitor.EndDate;
                parking.StartTime = visitor.StartTime;
                parking.EndTime = visitor.EndTime;
                parking.CCtome = visitor.CCtome;
                _dbContext.VisitorParkings.Add(parking);
                _dbContext.SaveChanges();
            }
            return new { Message = "Visitor slot updated" };
        }
        public async Task<dynamic> AddVisitorParking(AddVisitorParkingAc objvisitor)
        {
            try
            {
                VisitorParkingTemp visitor = new VisitorParkingTemp();
                string[] Date = objvisitor.Date.Split('T');
                visitor.IsActive = false;
                visitor.IsDeleted = false;
                visitor.CreatedBy = objvisitor.TenantId;
                visitor.CreatedOn = DateTime.Now;
                visitor.Email = objvisitor.Email;
                visitor.Name = objvisitor.Name==null?"": System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objvisitor.Name.ToLower());
                visitor.Make = objvisitor.Make;
                visitor.MobileNumber = objvisitor.MobileNumber;
                visitor.RegisterUserId = objvisitor.TenantId;
                visitor.SiteId = objvisitor.SiteId;
                visitor.Model = objvisitor.Model;
                visitor.VRMNumber = objvisitor.VRM==null?"": objvisitor.VRM.ToUpper();
                visitor.StartTime = objvisitor.StartTime;
                visitor.Surname = objvisitor.Surname==null?"": System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(objvisitor.Surname.ToLower()); ;
                visitor.CCtome = objvisitor.cctome;
               // visitor.VisitorBayNoId = objvisitor.VisitorBayNoId;
                int duration = Convert.ToInt32(objvisitor.Duration);
                string dt = string.Empty;
                if (objvisitor.SessionUnit == "Minutes")
                {
                    dt = Convert.ToDateTime(objvisitor.StartTime).AddMinutes(duration).ToString("HH:mm");
                }
                else if (objvisitor.SessionUnit == "Hours")
                {
                    dt = Convert.ToDateTime(objvisitor.StartTime).AddHours(duration).ToString("HH:mm");
                }
                objvisitor.EndTime = dt;
                visitor.EndTime = objvisitor.EndTime;
                DateTime datenew = Convert.ToDateTime(Date[0]);
                string StartDate = datenew.ToString("yyyy-MM-dd") + " " + objvisitor.StartTime;
                string EndDate = datenew.ToString("yyyy-MM-dd") + " " + objvisitor.EndTime;
                visitor.StartDate = Convert.ToDateTime(StartDate);
                visitor.EndDate = Convert.ToDateTime(EndDate);
                visitor.SessionUnit = objvisitor.SessionUnit;
                visitor.Duration = objvisitor.Duration;
                // visitor.Zipcode = objvisitor.Zipcode;
                _dbContext.VisitorParkingTemps.Add(visitor);
                _dbContext.SaveChanges();
                objvisitor.Id = visitor.Id;
                //var visitorbayno = _dbContext.VisitorBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == objvisitor.SiteId && x.Id == Convert.ToInt32(objvisitor.VisitorBayNoId)).FirstOrDefault();
                //if (visitorbayno != null)
                //{
                //    visitorbayno.StartDate = Convert.ToDateTime(objvisitor.StartTime);
                //    visitorbayno.EndDate = Convert.ToDateTime(objvisitor.EndTime);
                //    visitorbayno.RegisterUserId = Convert.ToInt32(objvisitor.TenantId);
                //    _dbContext.VisitorBayNos.Update(visitorbayno);
                //    _dbContext.SaveChanges();
                //}

            }
            catch (Exception ex)
            {

            }
            return new { Message = "Visitor Parking saved successfully" };
        }

        public async Task<List<VisitorParking>> GetVisitorParkings(int TenantId)
        {
            List<VisitorParking> visitors = await _dbContext.VisitorParkings.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == TenantId).OrderByDescending(x=>x.StartDate).ToListAsync();
            return visitors;
        }

        public bool GetExistsVehicleParking(AddVisitorParkingAc objvisitor)
        {

            VisitorParking visitors = _dbContext.VisitorParkings.FirstOrDefault(x => x.MobileNumber.ToLower().Trim() == objvisitor.Email.ToLower().Trim() && x.Email.ToLower().Trim() == objvisitor.MobileNumber.ToLower().Trim() && x.IsDeleted == false && x.CreatedBy == objvisitor.TenantId);
            return (visitors != null);
        }

        public async Task<dynamic> UpdateVisitorParking(AddVisitorParkingAc objvisitor)
        {
            var visitor = _dbContext.VisitorParkings.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == objvisitor.Id).FirstOrDefault();
            if (visitor != null)
            {
                //  visitor.FirstName = objvisitor.FirstName;
                //   visitor.LastName = objvisitor.LastName;
                visitor.MobileNumber = objvisitor.MobileNumber;
                visitor.Email = objvisitor.Email;
                //  visitor.Address = objvisitor.Address;
                //  visitor.City = objvisitor.City;
                //   visitor.State = objvisitor.State;
                //   visitor.Zipcode = objvisitor.Zipcode;
                visitor.UpdatedBy = objvisitor.TenantId;
                visitor.UpdatedOn = DateTime.Now;
                _dbContext.VisitorParkings.Update(visitor);
                _dbContext.SaveChanges();

            }
            return new { Message = "Visitor parking updated successfully" };
        }
        public async Task<dynamic> UpdateNotification(int ID)
        {
            var visitor = _dbContext.AuditLogs.Where(x => x.TenantId ==ID).ToList();
            if (visitor!=null) {
                visitor.ForEach(x =>
                {
                    x.Isread = true;
                    
                });
                _dbContext.AuditLogs.UpdateRange(visitor);
                _dbContext.SaveChanges();
            }
            
        
        //var ValidCustomers = Auditlog.Where(c => c.IsValid).ToList();
        //    ValidCustomers.ForEach(c => c.CreditLimit = 1000);
        //    _dbContext.VisitorParkings.Update(visitor);
        //       _dbContext.SaveChanges();


            //if (visitor != null)
            //{
            //    visitor.foreach{
            //        visitor.is= objvisitor.;
            //    }
            //    visitor.i = objvisitor.;
            //    visitor.Email = objvisitor.Email;
            //    visitor.UpdatedBy = objvisitor.TenantId;
            //    visitor.UpdatedOn = DateTime.Now;
            //    _dbContext.VisitorParkings.Update(visitor);
            //    _dbContext.SaveChanges();

            //}
            return new { Message = "Visitor parking updated successfully" };
        }

        public async Task<dynamic> GetVisitorParkingById(int Id)
        {
            var visitors = (from v in _dbContext.VisitorParkings
                            where v.IsActive == true && v.IsDeleted == false && v.Id == Id
                            select new
                            {
                                v.Id,
                                v.IsActive,
                                v.IsDeleted,
                                //  v.LastName,
                                v.MobileNumber,
                                v.RegisterUser,
                                v.SiteId,
                                //   v.State,
                                v.UpdatedBy,
                                v.UpdatedOn,
                                //  v.Zipcode,
                                //  v.FirstName,
                                v.Email,
                                v.CreatedOn,
                                v.CreatedBy,
                                //  v.City,
                                //   v.Address,
                                vehiclelists = (from l in _dbContext.VisitorParkingVehicleDetails
                                                where l.VisitorParkingId == v.Id && l.IsActive == true && l.IsDeleted == false
                                                select new
                                                {
                                                    l.Id,
                                                    l.CreatedBy,
                                                    l.CreatedOn,
                                                    enddate = l.EndDate,
                                                    l.IsActive,
                                                    l.IsDeleted,
                                                    make = l.Make,
                                                    model = l.Model,
                                                    startdate = l.StartDate,
                                                    l.UpdatedBy,
                                                    l.UpdatedOn,
                                                    bayid = l.VisitorBayNo,
                                                    vrm = l.VRMNumber,
                                                    l.VisitorParkingId
                                                }).ToList()
                            }).FirstOrDefault();
            return visitors;
        }


        public async Task<dynamic> GetVisitorParkingBysiteId(int Id)
        {
            var visitors = (from v in _dbContext.VisitorParkings
                            where v.IsActive == true && v.IsDeleted == false && v.SiteId == Id
                            select new
                            {
                                v.Id,
                                v.StartDate
                            }).ToList();
            return visitors;
        }


        public async Task<dynamic> GetVisitorParkingBysiteIddate(int Id, String date)
        {

            List<object> baseresponce = new List<object>();


            var visitorbays = _dbContext.VisitorBayNos.Where(x =>  x.IsDeleted == false && x.IsActive == true && x.SiteId==Id).ToList();

            //var visitorbays = (from v in _dbContext.VisitorBayNos
            //                   join vb in _dbContext.VisitorBayNos on v.VisitorBayNoId equals vb.Id
            //                   where v.IsActive == true && v.IsDeleted == false && v.SiteId == Id
            //                   select new
            //                   {
            //                       v.VisitorBayNoId,
            //                       vb.BayName

            //                   }).Distinct().ToList();

            foreach (var bayno in visitorbays)
            {
                modelresponce response = new modelresponce();

                var visitors = (from v in _dbContext.VisitorParkings
                                join s in _dbContext.Sites on v.SiteId equals s.Id
                                join vb in _dbContext.VisitorBayNos on v.VisitorBayNoId equals vb.Id
                                where v.IsActive == true && v.IsDeleted == false && v.SiteId == Id && v.StartDate.Date == (Convert.ToDateTime(date).Date) && v.VisitorBayNoId == bayno.Id
                                select new
                                {
                                    v.Id,
                                    v.StartDate,
                                    s.SiteName,
                                    v.VRMNumber,
                                    v.EndDate,
                                    v.StartTime,
                                    v.EndTime,
                                    vb.BayName

                                }).ToList();

                response.id = bayno.Id;
                response.bayname = bayno.BayName;
                response.result = visitors;

                baseresponce.Add(response);

            }



            return baseresponce;
        }

        public async Task<dynamic> GetVisitordetailsById(int Id)
        {
            var visitors = (from v in _dbContext.VisitorParkings
                            join vb in _dbContext.VisitorBayNos on v.VisitorBayNoId equals vb.Id
                            join r in _dbContext.RegisterUsers on v.RegisterUserId equals r.Id

                            where v.IsActive == true && v.IsDeleted == false && v.Id == Id
                            select new
                            {
                                id = v.Id,
                                drivername = v.Name + " " + v.Surname,
                                bayNO = vb.BayName,
                                vrmno = v.VRMNumber,
                                email = v.Email,
                                phno = v.MobileNumber,
                                tenantname = r.FirstName + " " + r.LastName,
                                tenantmobile = r.MobileNumber,
                                startDate=v.StartDate,
                                endDate=v.EndDate

                            }).ToList();
            return visitors;
        }
    }
    public class modelresponce
    {
        public int id { get; set; }
        public string bayname { get; set; }
        public object result { get; set; }
    }
}

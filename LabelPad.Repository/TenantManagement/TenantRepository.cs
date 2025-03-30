using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using LabelPad.Repository.UserManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.TenantManagement
{
    public class TenantRepository : ITenantRepository
    {
        private readonly LabelPadDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        public TenantRepository(LabelPadDbContext dbContext, IConfiguration configuration, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<dynamic> GetVehicleDetails(string tenantid)
        {
            // var vehicles = await _dbContext.VehicleRegistrations.Where(x=>x.IsActive==true && x.IsDeleted==false && x.RegisterUserId==Convert.ToInt32(tenantid)).ToListAsync();
            var vehicles = (from v in _dbContext.VehicleRegistrations
                            join pb in _dbContext.ParkingBayNos on v.ParkingBayNo equals pb.Id
                            where v.RegisterUserId == Convert.ToInt32(tenantid) && v.IsActive == true && v.IsDeleted == false
                            select new
                            {
                                vrm = v.VRM,
                                parkingBayNo = pb.BayName,
                                startDate = v.StartDate,
                                endDate = v.EndDate,
                            });


            return vehicles;
        }

        public async Task<dynamic> GetvehiclecountsdetailsById(string tenantid, string bayno, int Id)
        {
            DateTime todaydate = DateTime.Now;

            // Get maxVehiclesPerBay for the specified parking bay
            var maxVehiclesPerBay = _dbContext.ParkingBayNos
                                              .Where(pb => pb.Id == Convert.ToInt32(bayno) && pb.IsActive && !pb.IsDeleted)
                                              .Select(pb => pb.MaxVehiclesPerBay)
                                              .FirstOrDefault();

            var vehicles = (from v in _dbContext.VehicleRegistrations
                            join pb in _dbContext.ParkingBayNos on v.ParkingBayNo equals pb.Id
                            where v.RegisterUserId == Convert.ToInt32(tenantid) && v.ParkingBayNo == Convert.ToInt32(bayno)
                            && v.IsActive == true && v.IsDeleted == false && pb.IsActive == true && pb.IsDeleted == false && v.EndDate > todaydate
                            select new
                            {
                                id = v.Id,
                                vrm = v.VRM,
                                Make = v.Make,
                                Model = v.Model,
                                parkingBayNo = pb.BayName,
                                startDate = v.StartDate,
                                bayconfig = v.ConfigNo,
                                issavecount = v.IsSaveCount,
                                configno = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == Convert.ToInt32(tenantid) && x.ParkingBayNo == Convert.ToInt32(bayno)).Max(x => x.ConfigNo),
                                maxissavecount = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == Convert.ToInt32(tenantid) && x.ParkingBayNo == Convert.ToInt32(bayno)).Max(x => x.IsSaveCount),
                                selectedddates = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.IsActive == true && x.IsDeleted == false && x.VehicleRegistrationId == v.Id).ToList(),
                                updateEndDate = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Convert.ToInt32(tenantid)).FirstOrDefault().UpdateEnddate,
                                endDate = v.EndDate,
                            }).ToList();

            // Filter vehicles based on maxVehiclesPerBay
            if (maxVehiclesPerBay == 1)
            {
                // Return the record with matching Id
                var singleVehicle = vehicles.FirstOrDefault(v => v.id == Id);
                return singleVehicle != null ? new { message = "singledata", data = singleVehicle } : new { message = "nodata", data = "NULL" };
            }
            else if (maxVehiclesPerBay == 2)
            {
                // Return the top 2 records
                return new { message = "twodata", data = vehicles.Take(2).ToList() };
            }

            return new { message = "nodata", data = "NULL" };
        }


        public async Task<dynamic> Getvehiclecountsdetails(string tenantid, string bayno)
        {
            DateTime todaydate = DateTime.Now;
       
            var vehicles = (from v in _dbContext.VehicleRegistrations
                                join pb in _dbContext.ParkingBayNos on v.ParkingBayNo equals pb.Id
                                where v.RegisterUserId == Convert.ToInt32(tenantid) && v.ParkingBayNo == Convert.ToInt32(bayno)
                                && v.IsActive == true && v.IsDeleted == false && pb.IsActive == true && pb.IsDeleted == false && v.EndDate > todaydate
                                select new
                                {
                                    id = v.Id,
                                    vrm = v.VRM,
                                    Make = v.Make,
                                    Model = v.Model,
                                    parkingBayNo = pb.BayName,
                                    startDate = v.StartDate,
                                    bayconfig = v.ConfigNo,
                                    issavecount = v.IsSaveCount,
                                    configno = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == Convert.ToInt32(tenantid) && x.ParkingBayNo == Convert.ToInt32(bayno)).Max(x => x.ConfigNo),
                                    maxissavecount = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == Convert.ToInt32(tenantid) && x.ParkingBayNo == Convert.ToInt32(bayno)).Max(x => x.IsSaveCount),
                                    selectedddates = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.IsActive == true && x.IsDeleted == false && x.VehicleRegistrationId == v.Id).ToList(),
                                    updateEndDate= _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Convert.ToInt32(tenantid)).FirstOrDefault().UpdateEnddate,


                                    endDate = v.EndDate,
                                }).ToList();

                if (vehicles != null && vehicles.Count != 0)
                {
                    if (vehicles[0].issavecount > 0)
                    {
                        List<object> baseresponce = new List<object>();
                        returndata basemodel = new returndata();
                        if (vehicles[0].maxissavecount > 0)
                        {

                            for (int i = 1; i <= vehicles[0].maxissavecount; i++)
                            {
                                List<object> responce = new List<object>();

                                for (int k = 0; k < vehicles.Count(); k++)
                                {

                                    if (vehicles[k].issavecount == i)
                                    {
                                        var data = new object();
                                        data = vehicles[k];
                                        responce.Add(data);
                                    }
                                }
                                // baseresponce[0] = "multiple data";
                                baseresponce.Add(responce);
                            }
                            //baseresponce[0] = "multiple data";
                            basemodel.message = "mutilpledata";
                            basemodel.data = baseresponce;
                            return basemodel;

                        }
                    }
                }
                //var object1 = vehicles.ToList();
                // List<string> selectedCollection = selected.ToList();


                return vehicles;
           
          

            // var vehicles = await _dbContext.VehicleRegistrations.Where(x=>x.IsActive==true && x.IsDeleted==false && x.RegisterUserId==Convert.ToInt32(tenantid)).ToListAsync();

        }



        public async Task<dynamic> getvehcilecountsbydates(string tenantid, string bayno, string dates)
        {
            // var vehicles = await _dbContext.VehicleRegistrations.Where(x=>x.IsActive==true && x.IsDeleted==false && x.RegisterUserId==Convert.ToInt32(tenantid)).ToListAsync();
            var vehicles = (from v in _dbContext.VehicleRegistrations
                            join pb in _dbContext.VehicleRegistrationTimeSlots on v.Id equals pb.VehicleRegistrationId
                            where v.RegisterUserId == Convert.ToInt32(tenantid) && v.ParkingBayNo == Convert.ToInt32(bayno)
                            && v.IsActive == true && v.IsDeleted == false && pb.FromDate.Date == (Convert.ToDateTime(dates).Date)
                             && pb.IsActive == true && pb.IsDeleted == false
                            select new
                            {

                                id = v.Id,
                                vrm = v.VRM,
                                Make = v.Make,
                                Model = v.Model,
                                // parkingBayNo = pb.BayName,
                                startDate = pb.FromDate,
                                bayconfig = v.ConfigNo,
                                // issavecount = v.IsSaveCount,
                                configno = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == Convert.ToInt32(tenantid) && x.ParkingBayNo == Convert.ToInt32(bayno)).Max(x => x.ConfigNo),
                                // maxissavecount = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == Convert.ToInt32(tenantid) && x.ParkingBayNo == Convert.ToInt32(bayno)).Max(x => x.IsSaveCount),
                                //selectedddates = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.IsActive == true && x.IsDeleted == false && x.VehicleRegistrationId == v.Id).ToList(),

                                endDate = pb.ToDate,
                            }).ToList();




            return vehicles;
        }

        public class returndata
        {
            public string message { get; set; }
            public object data { get; set; }


        }

        public async Task<dynamic> Getbaynobytenant(string tenantid)
        {
            int tenantIdInt = Convert.ToInt32(tenantid);

            var parkingbays = await _dbContext.ParkingBayNos
                .Where(x => x.IsActive == true && x.IsDeleted == false &&
                           (x.RegisterUserId == tenantIdInt || x.UpdatedBy == tenantIdInt))
                .Select(x => new
                {
                    bayNo = x.Id,
                    bayname = x.BayName,
                    maxbayno = x.MaxVehiclesPerBay,
                    startdate = x.StartDate,
                    endate = x.EndDate
                })
                .ToListAsync();

            return parkingbays;
        }

        public async Task<dynamic> ReplySupport(AddSupportAc objinput)
        {
           
                var updatestaus = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == objinput.TicketId).FirstOrDefault();
                if (updatestaus != null)
                {
                    updatestaus.Status = "In Progress";
                    _dbContext.Supports.Update(updatestaus);
                    _dbContext.SaveChanges();
                }
                var support = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == objinput.Id).FirstOrDefault();
                if (support != null)
                {
                    support.IsRead = true;
                    support.UpdatedBy = objinput.TenantId;
                    support.UpdatedOn = DateTime.Now;
                    _dbContext.Supports.Update(support);
                    _dbContext.SaveChanges();
                }
                Support obj = new Support();
                obj.RegisterUserId = objinput.TenantId;
                obj.Subject = objinput.Subject;
                obj.IsRead = false;
                obj.ParentId = objinput.Id;
                obj.IsActive = true;
                obj.IsDeleted = false;
                obj.Issue = objinput.Issue;
                obj.TicketId = objinput.TicketId;
                obj.CreatedBy = objinput.TenantId;
                obj.CreatedOn = DateTime.Now;
                _dbContext.Supports.Add(obj);
                _dbContext.SaveChanges();
            
            return new { Message = "Saved Successfully" };
        }

        public async Task<dynamic> UpdateVisitorParking(UpdateVistorsParkingRequest parkingRequest)
        {
            

                var existingparking = await _dbContext.VisitorParkingTemps.FindAsync(parkingRequest.Id);
                if (existingparking == null) return null;

                existingparking.StartTime = parkingRequest.StartTime;
                existingparking.EndTime = parkingRequest.EndTime;
                existingparking.VRMNumber = parkingRequest.VRMNumber;
                existingparking.IsActive = true;
                existingparking.IsDeleted = false;
                existingparking.UpdatedOn = DateTime.Now;
                existingparking.UpdatedBy = 1;

                _dbContext.VisitorParkingTemps.Update(existingparking);
                await _dbContext.SaveChangesAsync();
                return existingparking;
            
        }

        public async Task<dynamic> GetSupportById(int Id, int TicketId, int TenantId)
        {

            var updatechat = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.IsRead == false && x.TicketId == TicketId).OrderByDescending(x => x.Id).ToList();
            foreach (var item in updatechat)
            {
                if (item != null)
                {
                    int createdby = item.CreatedBy;
                    int roleid = 0;
                    var userl = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == createdby).FirstOrDefault();
                    if (userl != null)
                    {
                        roleid = userl.RoleId;
                    }
                    if (roleid == 1)
                    {
                        item.IsRead = true;

                    }
                    else
                    {
                        item.IsRead = false;

                    }
                    item.UpdatedBy = TenantId;
                    item.UpdatedOn = DateTime.Now;
                    _dbContext.Supports.Update(item);
                    _dbContext.SaveChanges();
                }
            }

            List<GetPastQuery> list = new List<GetPastQuery>();
            string path = _configuration["ProfilePath"];
            int SupportId = 0;
            var support = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && (x.Id == TicketId || x.TicketId == TicketId)).ToList();
            for (int i = 0; i < support.Count; i++)
            {
                if (support[i].ParentId == 0)
                {
                    SupportId = support[i].Id;
                    GetPastQuery model = new GetPastQuery();
                    model.Date = support[i].CreatedOn.ToString("dd-MM-yyyy HH:mm");
                    // model.Name = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == support[i].RegisterUserId).FirstOrDefault().FirstName;
                    var user = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == support[i].RegisterUserId).FirstOrDefault();
                    if (user != null)
                    {
                        model.Name = user.FirstName + " " + user.LastName;
                        if (user.ProfilePath != null)
                        {
                            model.ProfilePath = path + user.ProfilePath;
                        }
                        else
                        {
                            model.ProfilePath = user.ProfilePath;
                        }
                    }
                    model.Issue = support[i].Issue;
                    model.Id = support[i].Id;
                    model.CreatedBy = support[i].CreatedBy;
                    int roleid = 0;
                    var userl = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == model.CreatedBy).FirstOrDefault();
                    if (userl != null)
                    {
                        roleid = userl.RoleId;
                    }
                    model.RoleId = roleid;
                    model.Class = "reply_issue";
                    model.Subject = support[i].Subject;
                    list.Add(model);
                }
                else if (support[i].ParentId != 0)
                {
                    int parentid = support[i].Id;
                    var response = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == parentid).FirstOrDefault();
                    if (response != null)
                    {
                        SupportId = support[i].Id;
                        GetPastQuery model = new GetPastQuery();
                        model.Date = response.CreatedOn.ToString("dd-MM-yyyy HH:mm");
                        // model.Name = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == response.CreatedBy).FirstOrDefault().FirstName;
                        var user = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == support[i].RegisterUserId).FirstOrDefault();
                        if (user != null)
                        {
                            model.Name = user.FirstName + " " + user.LastName;
                            if (user.ProfilePath != null)
                            {
                                model.ProfilePath = path + user.ProfilePath;
                            }
                            else
                            {
                                model.ProfilePath = user.ProfilePath;
                            }
                        }
                        model.Issue = response.Issue;
                        model.Id = response.Id;
                        model.CreatedBy = response.CreatedBy;
                        int roleid = 0;
                        var userl = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == model.CreatedBy).FirstOrDefault();
                        if (userl != null)
                        {
                            roleid = userl.RoleId;
                        }
                        model.RoleId = roleid;
                        if (model.RoleId == 1)
                        {
                            model.Class = "response_issue";
                        }
                        else
                        {
                            model.Class = "reply_issue";
                        }


                        model.Subject = response.Subject;
                        list.Add(model);
                    }
                }
            }
            return list;
        }

        public async Task<dynamic> GetSupportList(int TenantId)
        {
            List<GetSupportAc> list = new List<GetSupportAc>();
            var support = (from d in _dbContext.Supports
                           where d.IsActive == true && d.IsDeleted == false && d.RegisterUserId == TenantId && d.ParentId == 0
                           select new
                           {
                               Date = d.CreatedOn.ToString("dd-MM-yyyy"),
                               Id = d.Id,
                               Subject = d.Subject,
                               IsRead = d.IsRead,
                               message = d.Issue,
                           }).OrderByDescending(x => x.Id).ToList();
            for (int i = 0; i < support.Count; i++)
            {
                string subject = support[i].Subject;
                int ticketid = support[i].Id;
                //  var objres = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Subject == subject).ToList();
                var objres = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == ticketid).ToList();

                var lastcoment = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.TicketId == ticketid).OrderByDescending(x => x.TicketId)
                             .Take(1)
                             .Select(x => x.Issue)
                             .ToList()
                             .FirstOrDefault(); ;

                GetSupportAc model = new GetSupportAc();
                model.Date = support[i].Date;
                model.Subject = support[i].Subject;
                model.TicketId = support[i].Id;
                model.LastComment = lastcoment;

                if (objres.Count > 0)
                {
                    model.Status = objres[0].Status;
                    int count = objres.Count;
                    if (objres[count - 1].CreatedBy == 1)
                    {
                        if (objres[count - 1].IsRead == true)
                        {
                            model.IsRead = true;
                            model.Id = objres[count - 1].Id;
                        }
                        else
                        {
                            model.IsRead = false;
                            model.Id = objres[count - 1].Id;
                        }

                    }
                    else
                    {
                        model.Id = support[i].Id;
                        model.IsRead = true;
                    }
                    list.Add(model);
                }
            }

            return list;
        }
        public async Task<dynamic> AddSupport(AddSupportAc objinput)
        {
           
                Support obj = new Support();
                obj.RegisterUserId = objinput.TenantId;
                obj.Subject = objinput.Subject;
                obj.IsRead = false;
                obj.ParentId = 0;
                obj.IsActive = true;
                obj.IsDeleted = false;
                obj.Issue = objinput.Issue;
                obj.CreatedBy = objinput.TenantId;
                obj.CreatedOn = DateTime.Now;
                obj.Status = "New";
                _dbContext.Supports.Add(obj);
                _dbContext.SaveChanges();
                var support = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == obj.Id).FirstOrDefault();
                if (support != null)
                {
                    support.TicketId = obj.Id;
                    _dbContext.Supports.Update(support);
                    _dbContext.SaveChanges();
                }
           
            return new { Message = "Saved Successfully" };
        }
        public async Task<dynamic> AddVehicles(List<AddVehicleRegistrationAc> objinput)
        {
            bool iscancelwhitelist = false;
            // Get EmailSettings from Configuration
            EmailSettings emailSettings = _configuration.GetSection("EmailSettings").Get<EmailSettings>();

            // Wrap it inside Options.Create to match IOptions<T>
            IOptions<EmailSettings> optionsEmailSettings = Options.Create(emailSettings);

            // Pass it correctly to the UserRepository constructor
            UserManagement.UserRepository usrepo = new UserManagement.UserRepository(_dbContext, _configuration, optionsEmailSettings);

            var Reguserssiteid = _dbContext.RegisterUsers.Where(x => x.Id == objinput[0].TenantId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault().SiteId;

           
                var siteparkingbayno = Convert.ToInt32(objinput[0].bayno);
                var vrmcompare = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == objinput[0].TenantId && x.ParkingBayNo == siteparkingbayno).FirstOrDefault();
                

                if (vrmcompare != null)
                {
                    if (DateTime.Parse(objinput[0].StartDate) == (vrmcompare.StartDate) && DateTime.Parse(objinput[0].EndDate) == vrmcompare.EndDate && objinput[0].vrm == vrmcompare.VRM)
                    {
                        iscancelwhitelist = false;
                    }
                    else
                    {
                        iscancelwhitelist = true;
                    }

                }
                else
                {
                    iscancelwhitelist = false;
                }
                int tenantid1 = objinput[0].TenantId;
                int bayno1 = Convert.ToInt32(objinput[0].bayno);
                var ParkingBayNo = _dbContext.ParkingBayNos.Where(x => x.Id == bayno1 && x.IsDeleted == false && x.IsActive == true).FirstOrDefault();
                if (ParkingBayNo != null)
                {
                    var enddate = ParkingBayNo.EndDate;
                    var newenddate = Convert.ToDateTime(objinput[0].EndDate);
                    bool updatevehicle = false;
                    var isregistered = _dbContext.ParkingBayNos.Where(x => x.Id == bayno1 && (x.StartDate >= enddate) && (x.EndDate <= newenddate) && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    if (isregistered != null)
                    {
                        if (isregistered.RegisterUserId == tenantid1)
                        {
                            updatevehicle = true;
                        }
                        else
                        {

                            if (isregistered.RegisterUserId == 0)
                            {
                                updatevehicle = true;
                            }
                            else
                            {
                                updatevehicle = false;

                                return new { Message = "Can not be updated as the the bayname is already booked for respective dates" };
                            }

                        }
                    }
                    else
                    {
                        updatevehicle = true;
                    }

                    if (updatevehicle)
                    {
                        
                        if (objinput[0].TenantId != 0)
                        {
                            
                            int tenantid = objinput[0].TenantId;
                            int bayno = Convert.ToInt32(objinput[0].bayno);
                            var vehicleregistration = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == tenantid && x.ParkingBayNo == bayno && x.IsDeleted == false && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (vehicleregistration != null)
                            {
                                int parkingbayno = vehicleregistration.ParkingBayNo;
                                var parkingbaynos = _dbContext.ParkingBayNos.Where(x => x.Id == parkingbayno).FirstOrDefault();
                                if (parkingbaynos != null)
                                {
                                    int maxvehicle = parkingbaynos.MaxVehiclesPerBay;
                                    if (maxvehicle == 1)
                                    {
                                        if (vrmcompare != null)
                                        {
                                            var oldvrm = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == objinput[0].TenantId && x.ParkingBayNo == siteparkingbayno).FirstOrDefault().VRM;

                                        if (iscancelwhitelist==true) {
                                        usrepo.Cancelwhitelistvehicle(Convert.ToInt32(Reguserssiteid), oldvrm);
                                        }
                                        }


                                        int vehicleregistrationid = vehicleregistration.Id;
                                        var timeslots = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.VehicleRegistrationId == vehicleregistrationid).ToList();
                                        if (timeslots != null)
                                        {
                                            timeslots.ForEach(x =>
                                            {
                                                x.IsActive = false;
                                                x.IsDeleted = true;
                                                x.UpdatedBy = x.CreatedBy;
                                                x.UpdatedOn = DateTime.Now;
                                            });
                                            _dbContext.VehicleRegistrationTimeSlots.UpdateRange(timeslots);
                                            _dbContext.SaveChanges();

                                            vehicleregistration.IsActive = false;
                                            vehicleregistration.IsDeleted = true;
                                            vehicleregistration.UpdatedBy = tenantid;
                                            vehicleregistration.UpdatedOn = DateTime.Now;
                                            _dbContext.VehicleRegistrations.Update(vehicleregistration);
                                            _dbContext.SaveChanges();
                                        }
                                    }
                                }
                                if (objinput[0].Update == true)
                                {
                                    var parkingbay = _dbContext.ParkingBayNos.Where(x => x.RegisterUserId == tenantid && x.Id == bayno && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                    if (parkingbay != null)
                                    {
                                        parkingbay.StartDate = Convert.ToDateTime(objinput[0].StartDate);
                                        parkingbay.EndDate = Convert.ToDateTime(objinput[0].EndDate);
                                        parkingbay.UpdatedBy = tenantid;
                                        parkingbay.UpdatedOn = DateTime.Now;
                                        _dbContext.ParkingBayNos.Update(parkingbay);
                                        _dbContext.SaveChanges();

                                    }
                                    int vehicleregistrationid = vehicleregistration.Id;
                                    var timeslots = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.VehicleRegistrationId == vehicleregistrationid).ToList();
                                    if (timeslots != null)
                                    {
                                        timeslots.ForEach(x =>
                                        {
                                            x.IsActive = false;
                                            x.IsDeleted = true;
                                            x.UpdatedBy = x.CreatedBy;
                                            x.UpdatedOn = DateTime.Now;
                                        });
                                        _dbContext.VehicleRegistrationTimeSlots.UpdateRange(timeslots);
                                        _dbContext.SaveChanges();

                                        vehicleregistration.IsActive = false;
                                        vehicleregistration.IsDeleted = true;
                                        vehicleregistration.UpdatedBy = tenantid;
                                        vehicleregistration.UpdatedOn = DateTime.Now;
                                        _dbContext.VehicleRegistrations.Update(vehicleregistration);
                                        _dbContext.SaveChanges();
                                    }
                                }
                            }
                        }
                        DateTime todaydate = DateTime.Now;
                        if (objinput[0].dates != "")
                        {
                            string[] values = objinput[0].dates.Split(',');
                            //DateTime modifieddate = Convert.ToDateTime(date1.Year + "-" + date1.Month + "-" + date1.Day + " " + date2.Hour + ":" + date2.Minute);
                            for (int i = 0; i < values.Length; i++)
                            {
                                values[i] = values[i].Trim();
                                string testdate = values[i].Substring(0, 16);
                                DateTime testdate1 = Convert.ToDateTime(testdate);


                                var responce = (from b in _dbContext.VehicleRegistrations
                                                join pb in _dbContext.VehicleRegistrationTimeSlots on b.Id equals pb.VehicleRegistrationId
                                                where b.RegisterUserId == objinput[0].TenantId && b.ParkingBayNo == Convert.ToInt32(objinput[0].bayno)
                                                && pb.FromDate.Date == (Convert.ToDateTime(testdate1).Date)

                                                select new
                                                {
                                                    id = pb.Id,

                                                }).ToList();
                                if (responce != null)
                                {
                                    responce.ForEach(a =>
                                    {
                                        var visitorbayno = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == a.id).FirstOrDefault();
                                        if (visitorbayno != null)
                                        {
                                            visitorbayno.IsDeleted = true;
                                            visitorbayno.IsActive = false;
                                            visitorbayno.UpdatedOn = DateTime.Now;
                                            _dbContext.VehicleRegistrationTimeSlots.Update(visitorbayno);
                                            _dbContext.SaveChanges();
                                        }

                                    //var vehicle = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == a.vehicleid).FirstOrDefault();
                                    //if (vehicle != null)
                                    //{
                                    //    vehicle.IsDeleted = true;
                                    //    vehicle.IsActive = false;
                                    //    vehicle.UpdatedOn = DateTime.Now;
                                    //    _dbContext.VehicleRegistrations.Update(vehicle);
                                    //    _dbContext.SaveChanges();
                                    //}

                                });
                                }
                            }


                            if (objinput[0].Issavecount == 1)
                            {
                                var vehilce = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == objinput[0].TenantId && x.ParkingBayNo == Convert.ToInt32(objinput[0].bayno)).ToList();
                                if (vehilce != null)
                                {

                                    vehilce.ForEach(a =>
                                    {
                                        var timeslots = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.IsActive == true && x.IsDeleted == false && x.VehicleRegistrationId == a.Id && x.FromDate.Date > (todaydate.Date)).ToList();
                                        if (timeslots != null)
                                        {
                                            timeslots.ForEach(x =>
                                            {
                                                x.IsActive = false;
                                                x.IsDeleted = true;
                                                x.UpdatedBy = x.CreatedBy;
                                                x.UpdatedOn = DateTime.Now;
                                            });
                                            _dbContext.VehicleRegistrationTimeSlots.UpdateRange(timeslots);
                                            _dbContext.SaveChanges();
                                        }


                                        a.UpdatedOn = DateTime.Now;
                                        a.EndDate = DateTime.Now;
                                        _dbContext.VehicleRegistrations.Update(a);
                                        _dbContext.SaveChanges();
                                    });
                                }
                            }

                        }
                        else
                        {
                            var vehilce = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == objinput[0].TenantId && x.ParkingBayNo == Convert.ToInt32(objinput[0].bayno)).ToList();
                            if (vehilce != null)
                            {
                                // DateTime todaydate = DateTime.Now;
                                vehilce.ForEach(a =>
                                {
                                    var timeslots = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.IsActive == true && x.IsDeleted == false && x.VehicleRegistrationId == a.Id && x.FromDate.Date > todaydate.Date).ToList();
                                    if (timeslots != null)
                                    {
                                        timeslots.ForEach(x =>
                                        {
                                            x.IsActive = false;
                                            x.IsDeleted = true;
                                            x.UpdatedBy = x.CreatedBy;
                                            x.UpdatedOn = DateTime.Now;
                                        });
                                        _dbContext.VehicleRegistrationTimeSlots.UpdateRange(timeslots);
                                        _dbContext.SaveChanges();
                                    }


                                    a.UpdatedOn = DateTime.Now;
                                    a.EndDate = DateTime.Now;
                                    _dbContext.VehicleRegistrations.Update(a);
                                    _dbContext.SaveChanges();
                                });
                            }
                        }

                        for (int i = 0; i < objinput.Count; i++)
                        {
                            if (objinput[i].dates != "")
                            {

                            }
                            else
                            {
                                objinput[i].Issavecount = 0;
                            }

                            VehicleRegistration reg = new VehicleRegistration();
                            if (objinput[i].StartDate == "")
                            {

                                reg.StartDate = null;
                            }
                            else
                            {
                                //DateTime mystartDate = DateTime.ParseExact(objinput[i].StartDate, "yyyy-MM-dd HH:mm",
                                //               System.Globalization.CultureInfo.InvariantCulture);
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
                            reg.ConfigNo = objinput[i].Id.ToString();
                            int tenantid = objinput[0].TenantId;
                            int bayno = Convert.ToInt32(objinput[0].bayno);
                            var vehicleregistration = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == tenantid && x.ParkingBayNo == bayno).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (vehicleregistration != null)
                            {
                                int parkingbayno = vehicleregistration.ParkingBayNo;
                                var parkingbaynos = _dbContext.ParkingBayNos.Where(x => x.Id == parkingbayno).FirstOrDefault();
                                if (parkingbaynos != null)
                                {
                                    int maxvehicle = parkingbaynos.MaxVehiclesPerBay;
                                    if (maxvehicle == 1)
                                    {
                                        reg.IsSaveCount = 1;
                                    }
                                    else
                                    {

                                        reg.IsSaveCount = objinput[i].Issavecount;
                                    }
                                }
                            }
                            var parkingbay = _dbContext.ParkingBayNos.Where(x => x.RegisterUserId == tenantid && x.Id == bayno && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                            if (parkingbay != null)
                            {
                                parkingbay.StartDate = Convert.ToDateTime(objinput[0].StartDate);
                                parkingbay.EndDate = Convert.ToDateTime(objinput[0].EndDate);
                                parkingbay.UpdatedBy = tenantid;
                                parkingbay.UpdatedOn = DateTime.Now;
                                _dbContext.ParkingBayNos.Update(parkingbay);
                                _dbContext.SaveChanges();
                            }
                           


                            reg.IsActive = true;
                            reg.IsDeleted = false;
                            reg.CreatedBy = objinput[i].LoginId;
                            reg.CreatedOn = DateTime.Now;
                            _dbContext.VehicleRegistrations.Add(reg);
                            await _dbContext.SaveChangesAsync();
                            if (objinput[i].dates != "")
                            {
                                savemutliplevehciledates(reg.Id, objinput[i].dates, reg.StartDate, reg.EndDate, objinput[i].Issavecount, objinput[i].TenantId, Convert.ToInt32(objinput[i].bayno));
                            }
                            else
                            {
                                savemutliplevehciletime(reg.Id, reg.StartDate, reg.EndDate, objinput[i].Issavecount);
                            }
                                    //UserManagement.UserRepository usrepo = new UserManagement.UserRepository( _dbContext,_configuration);
                                    
                                    if (iscancelwhitelist == true)
                                    {
                                        //usrepo.Cancelwhitelistvehicle(Convert.ToInt32(Reguserssiteid), oldvrm);
                                        usrepo.whitelistvehicle(Convert.ToInt32(Reguserssiteid), objinput[0].vrm);

                                    }
                                    else
                                    {
                                        usrepo.whitelistvehicle(Convert.ToInt32(Reguserssiteid), objinput[0].vrm);
                                    }
                        }
                    }
                }
               
            
            return new { Message = "Saved Successfully" };
        }


        public async Task<dynamic> UpdateVehicle(List<AddVehicleRegistrationAc> objinput)
        {
            var tenantBayList = objinput.Select(x => new { x.TenantId, BayNo = Convert.ToInt32(x.bayno) }).ToList();
            var results = new List<string>();

            foreach (var input in objinput)
            {
                var user = _dbContext.RegisterUsers
                    .FirstOrDefault(x => !x.IsDeleted && x.Id == input.TenantId && x.IsActive);

                if (user == null)
                {
                    results.Add("User not found for Tenant ID: " + input.TenantId);
                    continue;
                }

                int bayno = Convert.ToInt32(input.bayno);

                if (!DateTime.TryParse(input.StartDate, out DateTime newStartDate) ||
                    !DateTime.TryParse(input.EndDate, out DateTime newEndDate))
                {
                    results.Add("Invalid date format for record with ID: " + input.Id);
                    continue;
                }

                var requestIds = objinput.Select(x => x.Id).ToList();

                var conflictingRecords = await _dbContext.VehicleRegistrations
                    .AnyAsync(v => v.ParkingBayNo == bayno
                                && !requestIds.Contains(v.Id)
                                && v.IsActive
                                && newStartDate < v.EndDate   // Overlaps start
                                && newEndDate > v.StartDate); // Overlaps end

                if (conflictingRecords)
                {
                    results.Add("Range Already Exist");
                    return new UpdateVehicleResponse { Messages = results };
                }
            }
            
            foreach (var input in objinput)
            {
                var user = _dbContext.RegisterUsers
                    .FirstOrDefault(x => !x.IsDeleted && x.Id == input.TenantId && x.IsActive);

                if (user == null)
                {
                    results.Add("User not found for Tenant ID: " + input.TenantId);
                    continue;
                }

                int bayno = Convert.ToInt32(input.bayno);

                var existingRecord = await _dbContext.VehicleRegistrations
                    .FirstOrDefaultAsync(v => v.Id == input.Id && v.ParkingBayNo == bayno);

                if (existingRecord != null)
                {
                    existingRecord.VRM = input.vrm;
                    existingRecord.StartDate = Convert.ToDateTime(input.StartDate);
                    existingRecord.EndDate = Convert.ToDateTime(input.EndDate);
                    existingRecord.UpdatedOn = DateTime.Now;
                    existingRecord.UpdatedBy = user.Id;

                    _dbContext.VehicleRegistrations.Update(existingRecord);
                    await _dbContext.SaveChangesAsync();

                    var responce = _dbContext.VehicleRegistrationTimeSlots
                        .Where(pb => pb.VehicleRegistrationId == existingRecord.Id)
                        .ToList();

                    if (responce.Any())
                    {
                        foreach (var slot in responce)
                        {
                            if (slot.IsActive && !slot.IsDeleted)
                            {
                                slot.IsDeleted = true;
                                slot.IsActive = false;
                                slot.UpdatedOn = DateTime.Now;
                            }
                        }

                        _dbContext.VehicleRegistrationTimeSlots.UpdateRange(responce);
                        await _dbContext.SaveChangesAsync();
                    }
                    if (input.dates != "")
                    {
                        savemutliplevehciledates(existingRecord.Id, input.dates, existingRecord.StartDate, existingRecord.EndDate, 0, user.Id, bayno);
                    }
                    else
                    {
                        savemutliplevehciletime(existingRecord.Id, existingRecord.StartDate, existingRecord.EndDate, 0);
                    }
                    results.Add("Record updated successfully");
                }
                else
                {
                    results.Add("Record not found for ID: " + input.Id);
                }
            }

            return new UpdateVehicleResponse { Messages = results };
        }
        public async Task<dynamic> AddVehicle_New(List<AddVehicleRegistrationAc> objinput)
        {
            var tenantBayList = objinput.Select(x => new { x.TenantId, BayNo = Convert.ToInt32(x.bayno) }).ToList();

            var existingVehicles = _dbContext.VehicleRegistrations
                .Where(v => v.IsActive)
                .AsEnumerable()
                .Where(v => tenantBayList.Any(tb => tb.TenantId == v.RegisterUserId && tb.BayNo == v.ParkingBayNo))
                .ToList();

            // Update existing time slots
            var existingVehicleIds = existingVehicles.Select(v => v.Id).ToList();
            var existingTimeSlots = _dbContext.VehicleRegistrationTimeSlots
                .Where(x => existingVehicleIds.Contains(x.VehicleRegistrationId))
                .ToList();




            foreach (var input in objinput)
            {
                int bayno = Convert.ToInt32(input.bayno);
                DateTime newStartDate = Convert.ToDateTime(input.StartDate);
                DateTime newEndDate = Convert.ToDateTime(input.EndDate);

                // Strict Overlap Check
                var hasOverlap = _dbContext.VehicleRegistrations
                 .Any(v => v.ParkingBayNo == bayno &&
                  v.IsActive &&
                 !v.IsDeleted &&
              (
                  ((v.StartDate.HasValue && v.StartDate.Value.TimeOfDay != TimeSpan.Zero) ||
                   (v.EndDate.HasValue && v.EndDate.Value.TimeOfDay != TimeSpan.Zero)) && // Only check records with time
                  (
                      (newStartDate >= v.StartDate && newStartDate < v.EndDate) ||  // Starts inside existing range
                      (newEndDate > v.StartDate && newEndDate <= v.EndDate) ||      // Ends inside existing range
                      (newStartDate <= v.StartDate && newEndDate >= v.EndDate)      // Covers the entire existing range
                  )
              ));

                if (hasOverlap)
                {
                    return new AddVehicleResponse { Message = "Range Already Exist" };
                }
                else
                {
                    int SelBayno = Convert.ToInt32(objinput[0].bayno);
                    var resp = _dbContext.VehicleRegistrations
              .Where(v => v.ParkingBayNo == SelBayno)
              .ToList();

                    if (resp.Any())
                    {
                        foreach (var slot in resp)
                        {
                            // Check if both StartDate and EndDate exist and have time as 00:00
                            bool isMidnight = slot.StartDate.HasValue && slot.StartDate.Value.TimeOfDay == TimeSpan.Zero &&
                                              slot.EndDate.HasValue && slot.EndDate.Value.TimeOfDay == TimeSpan.Zero;

                            if (slot.IsActive && !slot.IsDeleted && isMidnight)
                            {
                                slot.IsDeleted = true;
                                slot.IsActive = false;
                                slot.UpdatedOn = DateTime.Now;
                            }
                        }

                        _dbContext.VehicleRegistrations.UpdateRange(resp);
                        await _dbContext.SaveChangesAsync();
                    }
                }

            }



            foreach (var input in objinput)
            {
                var user = _dbContext.RegisterUsers
                    .FirstOrDefault(x => !x.IsDeleted && x.Id == input.TenantId && x.IsActive);

                if (user == null) continue;

                int bayno = Convert.ToInt32(input.bayno);

                var parkingBay = _dbContext.ParkingBayNos
                    .FirstOrDefault(pb => pb.Id == bayno && !pb.IsDeleted && pb.IsActive);

                if (parkingBay != null)
                {
                    parkingBay.Status = true;
                    parkingBay.IsActive = true;
                    parkingBay.IsDeleted = false;
                    parkingBay.UpdatedBy = user.Id;
                    parkingBay.UpdatedOn = DateTime.Now;

                    _dbContext.ParkingBayNos.Update(parkingBay);
                    await _dbContext.SaveChangesAsync();
                }



                var newVehicle = new VehicleRegistration
                {
                    IsActive = true,
                    IsDeleted = false,
                    IsSaveCount = 1,
                    RegisterUserId = user.Id,
                    VRM = input.vrm,
                    CreatedOn = DateTime.Now,
                    CreatedBy = user.Id,
                    StartDate = Convert.ToDateTime(input.StartDate),
                    EndDate = Convert.ToDateTime(input.EndDate),
                    ParkingBayNo = bayno
                };

                _dbContext.VehicleRegistrations.Add(newVehicle);
                await _dbContext.SaveChangesAsync();

                var responce = _dbContext.VehicleRegistrationTimeSlots
                      .Where(pb => pb.VehicleRegistrationId == newVehicle.Id)
                      .ToList();

                if (responce.Any())
                {
                    foreach (var slot in responce)
                    {
                        if (slot.IsActive && !slot.IsDeleted)
                        {
                            slot.IsDeleted = true;
                            slot.IsActive = false;
                            slot.UpdatedOn = DateTime.Now;
                        }
                    }

                    _dbContext.VehicleRegistrationTimeSlots.UpdateRange(responce);
                    await _dbContext.SaveChangesAsync();

                }

                //if (DateTime.TryParse(input.dates, out DateTime parsedDate))
                //{
                savemutliplevehciledates(newVehicle.Id, input.dates, newVehicle.StartDate, newVehicle.EndDate, 0, user.Id, bayno);
              //  _userRepository.whitelistvehicle(Convert.ToInt32(user.SiteId), objinput[0].vrm);

                //}
                //else
                //{
                //   // savemutliplevehciledates(newVehicle.Id, input.dates, newVehicle.StartDate, newVehicle.EndDate, 0, user.Id, bayno);

                //    savemutliplevehciletime(newVehicle.Id, newVehicle.StartDate, newVehicle.EndDate, 0);
                //}
            }

            return new AddVehicleResponse { Message = "Vehicles updated and new records inserted successfully" };
        }


        //public async Task<dynamic> AddVehicle_New(List<AddVehicleRegistrationAc> objinput)
        //{
        //    var tenantBayList = objinput.Select(x => new { x.TenantId, BayNo = Convert.ToInt32(x.bayno) }).ToList();

        //    var existingVehicles = _dbContext.VehicleRegistrations
        //        .Where(v => v.IsActive)
        //        .AsEnumerable()
        //        .Where(v => tenantBayList.Any(tb => tb.TenantId == v.RegisterUserId && tb.BayNo == v.ParkingBayNo))
        //        .ToList();

        //    // Update existing time slots
        //    var existingVehicleIds = existingVehicles.Select(v => v.Id).ToList();
        //    var existingTimeSlots = _dbContext.VehicleRegistrationTimeSlots
        //        .Where(x => existingVehicleIds.Contains(x.VehicleRegistrationId))
        //        .ToList();




        //    foreach (var input in objinput)
        //    {
        //        int bayno = Convert.ToInt32(input.bayno);
        //        DateTime newStartDate = Convert.ToDateTime(input.StartDate);
        //        DateTime newEndDate = Convert.ToDateTime(input.EndDate);

        //        // Strict Overlap Check
        //        var hasOverlap = _dbContext.VehicleRegistrations
        //         .Any(v => v.ParkingBayNo == bayno &&
        //          v.IsActive &&
        //         !v.IsDeleted &&
        //      (
        //          ((v.StartDate.HasValue && v.StartDate.Value.TimeOfDay != TimeSpan.Zero) ||
        //           (v.EndDate.HasValue && v.EndDate.Value.TimeOfDay != TimeSpan.Zero)) && // Only check records with time
        //          (
        //              (newStartDate >= v.StartDate && newStartDate < v.EndDate) ||  // Starts inside existing range
        //              (newEndDate > v.StartDate && newEndDate <= v.EndDate) ||      // Ends inside existing range
        //              (newStartDate <= v.StartDate && newEndDate >= v.EndDate)      // Covers the entire existing range
        //          )
        //      ));

        //        if (hasOverlap)
        //        {
        //            return new AddVehicleResponse { Message = "Range Already Exist" };
        //        }
        //        else
        //        {
        //            int SelBayno = Convert.ToInt32(objinput[0].bayno);
        //            var resp = _dbContext.VehicleRegistrations
        //      .Where(v => v.ParkingBayNo == SelBayno)
        //      .ToList();

        //            if (resp.Any())
        //            {
        //                foreach (var slot in resp)
        //                {
        //                    // Check if both StartDate and EndDate exist and have time as 00:00
        //                    bool isMidnight = slot.StartDate.HasValue && slot.StartDate.Value.TimeOfDay == TimeSpan.Zero &&
        //                                      slot.EndDate.HasValue && slot.EndDate.Value.TimeOfDay == TimeSpan.Zero;

        //                    if (slot.IsActive && !slot.IsDeleted && isMidnight)
        //                    {
        //                        slot.IsDeleted = true;
        //                        slot.IsActive = false;
        //                        slot.UpdatedOn = DateTime.Now;
        //                    }
        //                }

        //                _dbContext.VehicleRegistrations.UpdateRange(resp);
        //                await _dbContext.SaveChangesAsync();
        //            }
        //        }

        //    }



        //    foreach (var input in objinput)
        //    {
        //        var user = _dbContext.RegisterUsers
        //            .FirstOrDefault(x => !x.IsDeleted && x.Id == input.TenantId && x.IsActive);

        //        if (user == null) continue;

        //        int bayno = Convert.ToInt32(input.bayno);

        //        var parkingBay = _dbContext.ParkingBayNos
        //            .FirstOrDefault(pb => pb.Id == bayno && !pb.IsDeleted && pb.IsActive);

        //        if (parkingBay != null)
        //        {
        //            parkingBay.Status = true;
        //            parkingBay.IsActive = true;
        //            parkingBay.IsDeleted = false;
        //            parkingBay.UpdatedBy = user.Id;
        //            parkingBay.UpdatedOn = DateTime.Now;

        //            _dbContext.ParkingBayNos.Update(parkingBay);
        //            await _dbContext.SaveChangesAsync();
        //        }



        //        var newVehicle = new VehicleRegistration
        //        {
        //            IsActive = true,
        //            IsDeleted = false,
        //            IsSaveCount = 1,
        //            RegisterUserId = user.Id,
        //            VRM = input.vrm,
        //            CreatedOn = DateTime.Now,
        //            CreatedBy = user.Id,
        //            StartDate = Convert.ToDateTime(input.StartDate),
        //            EndDate = Convert.ToDateTime(input.EndDate),
        //            ParkingBayNo = bayno
        //        };

        //        _dbContext.VehicleRegistrations.Add(newVehicle);
        //        await _dbContext.SaveChangesAsync();

        //        var responce = _dbContext.VehicleRegistrationTimeSlots
        //              .Where(pb => pb.VehicleRegistrationId == newVehicle.Id)
        //              .ToList();

        //        if (responce.Any())
        //        {
        //            foreach (var slot in responce)
        //            {
        //                if (slot.IsActive && !slot.IsDeleted)
        //                {
        //                    slot.IsDeleted = true;
        //                    slot.IsActive = false;
        //                    slot.UpdatedOn = DateTime.Now;
        //                }
        //            }

        //            _dbContext.VehicleRegistrationTimeSlots.UpdateRange(responce);
        //            await _dbContext.SaveChangesAsync();

        //        }

        //        //if (DateTime.TryParse(input.dates, out DateTime parsedDate))
        //        //{
        //         savemutliplevehciledates(newVehicle.Id, input.dates, newVehicle.StartDate, newVehicle.EndDate, 0, user.Id, bayno);
        //        //}
        //        //else
        //        //{
        //        //   // savemutliplevehciledates(newVehicle.Id, input.dates, newVehicle.StartDate, newVehicle.EndDate, 0, user.Id, bayno);

        //        //    savemutliplevehciletime(newVehicle.Id, newVehicle.StartDate, newVehicle.EndDate, 0);
        //        //}
        //    }

        //    return new AddVehicleResponse { Message = "Vehicles updated and new records inserted successfully" };
        //}



        private async Task<bool> savemutliplevehciletime(int id, DateTime? startdate, DateTime? enddate, int issavecount)
        {
            DateTime date1 = Convert.ToDateTime(startdate);
            DateTime date2 = Convert.ToDateTime(enddate);

            DateTime modifieddate = Convert.ToDateTime(date1.Year + "-" + date1.Month + "-" + date1.Day + " " + date2.Hour + ":" + date2.Minute);
            var diff = (date2 - date1).TotalDays;
            //  ArrayList DATES = new ArrayList();
            for (int i = 0; i < diff; i++)
            {
                VehicleRegistrationTimeSlot reg = new VehicleRegistrationTimeSlot();

                reg.IsActive = true;
                reg.IsDeleted = false;
                reg.CreatedBy = 0;
                reg.CreatedOn = DateTime.Now;
                reg.VehicleRegistrationId = id;
                reg.IsSaveCount = issavecount;
                reg.FromDate = date1.AddDays(i);
                reg.ToDate = modifieddate.AddDays(i);

                _dbContext.VehicleRegistrationTimeSlots.Add(reg);
                _dbContext.SaveChanges();

                //DATES.Add(date1.AddDays(i));
                //DATES.Add(modifieddate.AddDays(i));

            }
            //var Listdat = DATES;


            return true;
        }
        //save multiple dates from datepicker 
        private async Task<bool> savemutliplevehciledates(int id, string dates, DateTime? startdate, DateTime? enddate, int issavecount, int tenantid, int bayid)
        {


            // var responce = await _dbContext.VehicleRegistrationTimeSlots.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == Convert.ToInt32(tenantid)).ToListAsync();






            string[] values = dates.Split(',');

            DateTime date1 = Convert.ToDateTime(startdate);
            DateTime date2 = Convert.ToDateTime(enddate);

            //DateTime modifieddate = Convert.ToDateTime(date1.Year + "-" + date1.Month + "-" + date1.Day + " " + date2.Hour + ":" + date2.Minute);

            for (int i = 0; i < values.Length; i++)
            {
                values[i] = values[i].Trim();
                string testdate = values[i].Substring(0, 16);
                DateTime testdate1 = Convert.ToDateTime(testdate);


                //    var responce = (from b in _dbContext.VehicleRegistrations
                //                       join pb in _dbContext.VehicleRegistrationTimeSlots on b.Id equals pb.VehicleRegistrationId
                //                       where b.RegisterUserId == tenantid && b.ParkingBayNo == bayid 
                //                       && pb.FromDate.Date == (Convert.ToDateTime(testdate1).Date)
                //                       && pb.VehicleRegistrationId!=id
                //                       select new
                //                       {
                //                           id = pb.Id,

                //}). ToList();
                //    if (responce!=null)
                //    {
                //        responce.ForEach(a =>
                //        {
                //            var visitorbayno = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == a.id).FirstOrDefault();
                //            if (visitorbayno != null)
                //            {
                //                visitorbayno.IsDeleted = true;
                //                visitorbayno.IsActive = false;
                //                visitorbayno.UpdatedOn = DateTime.Now;
                //                _dbContext.VehicleRegistrationTimeSlots.Update(visitorbayno);
                //                _dbContext.SaveChanges();
                //            }
                //        });
                //    }

                ///DateTime testdate1 = DateTime.ParseExact(values[i], "yyyy-MM-dd HH:mm",
                //   System.Globalization.CultureInfo.InvariantCulture);

                DateTime modifiedfromdate = Convert.ToDateTime(testdate1.Year + "-" + testdate1.Month + "-" + testdate1.Day + " " + date1.Hour + ":" + date1.Minute);
                DateTime modifietodate = Convert.ToDateTime(testdate1.Year + "-" + testdate1.Month + "-" + testdate1.Day + " " + date2.Hour + ":" + date2.Minute);

                VehicleRegistrationTimeSlot reg = new VehicleRegistrationTimeSlot();

                reg.IsActive = true;
                reg.IsDeleted = false;
                reg.CreatedBy = 0;
                reg.CreatedOn = DateTime.Now;
                reg.VehicleRegistrationId = id;
                reg.FromDate = modifiedfromdate;
                reg.ToDate = modifietodate;
                reg.IsSaveCount = issavecount;
                //reg.IsSentToZatPark = issentzatpark;

                _dbContext.VehicleRegistrationTimeSlots.Add(reg);
                _dbContext.SaveChanges();
            }


            return true;
        }

        public async Task<dynamic> AddVehicleTimeSlot(List<AddVehicleTimeSlotAc> objinput)
        {
            try
            {
                for (int i = 0; i < objinput.Count; i++)
                {

                    VehicleRegistrationTimeSlot reg = new VehicleRegistrationTimeSlot();

                    reg.ParkingBayNo = Convert.ToInt32(objinput[i].BayNo);
                    reg.VehicleNo = objinput[i].VehicleNo;
                    reg.TimeSlot = objinput[i].TimeSlot;
                    reg.Status = objinput[i].Status;
                    reg.RegisterUserId = objinput[i].TenantId;
                    reg.Status = objinput[i].Status;
                    reg.IsActive = true;
                    reg.IsDeleted = false;
                    reg.CreatedBy = objinput[i].TenantId;
                    reg.CreatedOn = DateTime.Now;
                    _dbContext.VehicleRegistrationTimeSlots.Add(reg);
                    await _dbContext.SaveChangesAsync();

                }
            }
            catch (Exception)
            {

            }
            return new { Message = "Saved Successfully" };
        }

        public async  Task<dynamic> getversions()
        {
            //try {
           

            var versions = await _dbContext.SoftwareVersions.Where(x => x.Status == true).ToListAsync();
            
                return versions;
            //return versions;
            //}

            // catch (Exception)
            //{

            //}

        }


    }
}

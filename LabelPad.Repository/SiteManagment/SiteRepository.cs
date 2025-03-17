using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.SiteManagment
{
    public class SiteRepository : ISiteRepository
    {
        private readonly LabelPadDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public SiteRepository(LabelPadDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<List<Country>> GetCountries()
        {
            List<Country> countries = await _dbContext.Countries.Where(x => x.IsActive == true && x.IsDeleted == false).ToListAsync();
            return countries;
        }
        public async Task<dynamic> CloseTicket(int Id)
        {
            var support = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Id).FirstOrDefault();
            if (support != null)
            {
                support.Status = "Closed";
                support.UpdatedBy = 1;
                support.UpdatedOn = DateTime.Now;
                _dbContext.Supports.Update(support);
                _dbContext.SaveChanges();
            }
            return new { Message = "Ticket Closed Successfully" };
        }

        public async Task<dynamic> GetParkingBayNos(int SiteId, string Date, string EndDate)
        {

            DateTime dt = Convert.ToDateTime(Date);
            DateTime enddate = Convert.ToDateTime(EndDate);
            //  dt = dt.AddDays(1);
            //var parkingbaynos = (from p in _dbContext.ParkingBayNos
            //                     where p.SiteId == SiteId && p.IsActive == true && p.IsDeleted == false && (( p.EndDate < dt || p.EndDate ==null) ||((p.StartDate > enddate || p.StartDate == null)))
            //                     select p).ToList();

            // var parkingbaynosendate = (from p in _dbContext.ParkingBayNos
            //                      where p.SiteId == SiteId && p.IsActive == true && p.IsDeleted == false && (p.StartDate > enddate || p.StartDate == null)
            //                      select p).ToList();
            //var data= ScrambledEquals(parkingbaynos, parkingbaynosendate);

            //var parkingbaynoswithoutdates = (from p in _dbContext.ParkingBayNos
            //                     where p.SiteId == SiteId && p.IsActive == true && p.IsDeleted == false &&  p.StartDate == null
            //                     select p).ToList();

            //var enddateexp = (from p in _dbContext.ParkingBayNos
            //                    where p.SiteId == SiteId && p.IsActive == true && p.IsDeleted == false && (p.EndDate > dt)
            //                    select p).ToList();


            //var startdateexp = (from p in _dbContext.ParkingBayNos
            //                   where p.SiteId == SiteId && p.IsActive == true && p.IsDeleted == false && (p.StartDate > enddate )
            //                   select p).ToList();
            //if (enddateexp.Count == startdateexp.Count)
            //{
            //    return parkingbaynoswithoutdates.Concat(startdateexp);
            //}
            //else
            //{
            //    return parkingbaynoswithoutdates;

            //}
            List<object> responce = new List<object>();



            ArrayList array = new ArrayList();
            array.Add(SiteId);
            array.Add(dt);
            array.Add(enddate);

            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.Getbaynamesbydates(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt1 = ds.Tables[0];



                var nums = "0123456789".ToCharArray();


                var convertedList = (from rw in dt1.AsEnumerable()
                                     orderby rw["BayName"] ascending
                                     select new
                                     {
                                         id = Convert.ToString(rw["BayName"]),
                                         bayName = Convert.ToString(rw["BayName"]),

                                     }).OrderBy(x => x.bayName.LastIndexOfAny(nums))
                                        .ThenBy(x => x.bayName).ToList();








                //var query = convertedList.DistinctBy(p => new { p.Id, p.Name });


                return convertedList;

            }

            return responce;

            // parkingbaynos = parkingbaynos.Where(x => x.Status==false).ToList();
        }




        public async Task<dynamic> GetParkingBayNobysiteid(int SiteId)
        {
            List<object> responce = new List<object>();


            ArrayList array = new ArrayList();
            array.Add(SiteId);

            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.Getbaynametogropdown(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt1 = ds.Tables[0];



                var nums = "0123456789".ToCharArray();


                var convertedList = (from rw in dt1.AsEnumerable()

                                     select new
                                     {
                                         registerUserId = Convert.ToString(rw["RegisterUserId"]),
                                         id = Convert.ToString(rw["Id"]),
                                        // bayName = Convert.ToString(rw["BayName"]) + " " + Convert.ToString(rw["FirstName"]) + " " + Convert.ToString(rw["LastName"]),
                                         bayName = Convert.ToString(rw["BayName"]),
                                         updatedBy = Convert.ToString(rw["UpdatedBy"])
                                       


                                     }).OrderBy(x => x.bayName.LastIndexOfAny(nums))
                                        .ThenBy(x => x.bayName).ToList();

                return convertedList;



            }

            //    var nums = "0123456789".ToCharArray();
            //var parkingbaynos = (from s in _dbContext.ParkingBayNos
            //                     join r in _dbContext.RegisterUsers on s.RegisterUserId equals r.Id into temp
            //                     from r in temp.DefaultIfEmpty()
            //                     where  s.IsActive == true && s.IsDeleted == false &&  s.SiteId == SiteId
            //                     select new
            //                     {
            //                         registerUserId= s.RegisterUserId,
            //                         id=s.Id,
            //                         bayName=s.BayName +"-"+ r?.FirstName+" "+r?.LastName

            //                     }).ToList();


            //var parkingbaynos1 = await _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == SiteId).ToListAsync();
            return responce;

        }

        public async Task<dynamic> UpdateSupport(GetSupportCls obj)
        {
            var support = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == obj.Id).FirstOrDefault();
            if (support != null)
            {
                Support suppobj = new Support();
                suppobj.CreatedBy = 1;
                suppobj.CreatedOn = DateTime.Now;
                suppobj.IsActive = true;
                suppobj.IsDeleted = false;
                suppobj.Issue = obj.Response;
                suppobj.ParentId = obj.Id;
                suppobj.RegisterUserId = 1;
                suppobj.Subject = obj.Subject;
                suppobj.TicketId = obj.Id;
                _dbContext.Supports.Add(suppobj);
                _dbContext.SaveChanges();
            }
            return new { Message = "Resonse send successfully" };
        }
        public async Task<dynamic> GetSearchSupportList(int PageNo, int PageSize, int SiteId, string SiteName, string Name, string Email, string MobileNumber, string Subject)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            List<GetSupportList> list = new List<GetSupportList>();
            List<GetSearchSupport> supports = new List<GetSearchSupport>();
            ArrayList array = new ArrayList();
            array.Add(SiteName);
            array.Add(Name);
            array.Add(Email);
            array.Add(MobileNumber);
            array.Add(Subject);
            if (SiteId == 0)
            {
                array.Add(null);
            }
            else
            {
                array.Add(SiteId);
            }
            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.GetSearchSupports(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {


                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    supports.Add(new GetSearchSupport
                    {
                        Name = ds.Tables[0].Rows[i]["Name"].ToString(),
                        MobileNo = ds.Tables[0].Rows[i]["MobileNumber"].ToString(),
                        Email = ds.Tables[0].Rows[i]["Email"].ToString(),
                        Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                        SiteId = Convert.ToInt32(ds.Tables[0].Rows[i]["SiteId"]),
                        Subject = ds.Tables[0].Rows[i]["Subject"].ToString(),
                        IsRead = Convert.ToBoolean(ds.Tables[0].Rows[i]["IsRead"]),
                        SiteName = ds.Tables[0].Rows[i]["SiteName"].ToString()
                    });
                }
            }
            int totalitems = supports.Count();
            double totalpa = (double)totalitems / (double)PageSize;
            double totalpage = Math.Round(totalpa);
            if (supports.Count > 0)
            {
                for (int i = 0; i < supports.Count; i++)
                {
                    GetSupportList model = new GetSupportList();
                    string subject = supports[i].Subject;
                    var result = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Subject == subject).FirstOrDefault();
                    if (result != null)
                    {
                        int ticketid = result.Id;
                        var objres = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.TicketId == ticketid).ToList();
                        if (objres.Count > 0)
                        {
                            int count = objres.Count();
                            model.Name = supports[i].Name;
                            model.MobileNo = supports[i].MobileNo;
                            model.Email = supports[i].Email;
                            model.Subject = supports[i].Subject;
                            model.TicketId = ticketid;
                            model.Status = objres[0].Status;
                            model.TotalItem = totalitems;
                            model.TotalPage = totalpage;
                            model.SiteName = supports[i].SiteName;
                            if (objres[count - 1].CreatedBy != 1)
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
                                model.Id = supports[i].Id;
                                model.IsRead = true;
                            }
                            list.Add(model);
                        }
                        else
                        {
                            model.Name = supports[i].Name;
                            model.MobileNo = supports[i].MobileNo;
                            model.Email = supports[i].Email;
                            model.Subject = supports[i].Subject;
                            model.Id = supports[i].Id;
                            model.TicketId = ticketid;
                            model.IsRead = supports[i].IsRead;
                            model.TotalItem = totalitems;
                            model.TotalPage = totalpage;
                            model.SiteName = supports[i].SiteName;
                            list.Add(model);
                        }
                    }
                }
            }
            list = list.Skip(count2).Take(count1).ToList();
            return list;
        }
        public async Task<dynamic> GetIsReadNotifications(int RoleId, int LoginId, int SiteId)
        {
            var url = "http://smartpermitapi.eisappserver.net";
            var supports = (from s in _dbContext.Supports
                            join r in _dbContext.RegisterUsers on s.RegisterUserId equals r.Id
                            join m in _dbContext.Sites on r.SiteId equals m.Id
                            where s.IsActive == true && s.IsDeleted == false && r.IsActive == true && r.IsDeleted == false && s.IsRead == false
                            select new
                            {
                                ProfilePath = url + r.ProfilePath,
                                s.Id,
                                s.RegisterUserId,
                                s.Subject,
                                s.Issue,
                                s.IsRead,
                                s.TicketId,
                                SiteId = m.Id,
                                Date = s.CreatedOn.ToString("dd.MM.yyyy"),
                                Time = s.CreatedOn.ToString("HH:mm")

                            }).OrderByDescending(a => a.Id).ToList();
            if (RoleId == 1 && LoginId == 1 && SiteId == 0)
            {
                supports = supports.Where(x => x.RegisterUserId != 1).ToList();
            }
            else if (SiteId != 0)
            {
                supports = supports.Where(x => x.SiteId == SiteId).ToList();
            }
            return supports;
        }
        public async Task<dynamic> GetSupportListAdmin(int PageNo, int PageSize, int SiteId)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;

            List<GetSupportList> list = new List<GetSupportList>();
            var supports = (from s in _dbContext.Supports
                            join r in _dbContext.RegisterUsers on s.RegisterUserId equals r.Id
                            join m in _dbContext.Sites on r.SiteId equals m.Id
                            where s.IsActive == true && s.IsDeleted == false
                            && r.IsDeleted == false && r.IsActive == true && s.ParentId == 0 && r.RoleId == 2
                            && m.IsActive == true && m.IsDeleted == false
                            select new
                            {
                                pageNo=PageNo,
                                Name = r.FirstName + " " + r.LastName,
                                MobileNo = r.MobileNumber,
                                r.Email,
                                s.Id,
                                SiteId = m.Id,
                                s.Subject,
                                s.IsRead,
                                m.SiteName
                            }).OrderByDescending(x => x.Id).ToList();

            if (SiteId != 0)
            {
                supports = supports.Where(x => x.SiteId == SiteId).ToList();
            }
            int totalitems = supports.Count();
            double totalpa = (double)totalitems / (double)PageSize;
            double totalpage = Math.Round(totalpa);
            if (supports.Count > 0)
            {
                for (int i = 0; i < supports.Count; i++)
                {
                    GetSupportList model = new GetSupportList();
                    int subject = supports[i].Id;
                    var result = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == subject).FirstOrDefault();
                    if (result != null)
                    {
                        int ticketid = result.Id;
                        var objres = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.TicketId == ticketid).ToList();
                        if (objres.Count > 0)
                        {

                            int count = objres.Count();
                            model.pageNo = PageNo;
                            model.Name = supports[i].Name;
                            model.MobileNo = supports[i].MobileNo;
                            model.Email = supports[i].Email;
                            model.Subject = supports[i].Subject;
                            model.TicketId = ticketid;
                            model.Status = objres[0].Status;
                            model.TotalItem = totalitems;
                            model.TotalPage = totalpage;
                            model.SiteName = supports[i].SiteName;
                            int createdby = objres[count - 1].CreatedBy;
                            var userl = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == createdby).FirstOrDefault();
                            int roleid = 0;
                            if (userl != null)
                            {
                                roleid = userl.RoleId;
                            }

                            if (roleid != 1)
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
                                model.Id = supports[i].Id;
                                model.IsRead = true;
                            }
                            list.Add(model);
                        }
                        else
                        {
                            model.pageNo = PageNo;
                            model.Name = supports[i].Name;
                            model.MobileNo = supports[i].MobileNo;
                            model.Email = supports[i].Email;
                            model.Subject = supports[i].Subject;
                            model.Id = supports[i].Id;
                            model.TicketId = ticketid;
                            model.IsRead = supports[i].IsRead;
                            model.TotalItem = totalitems;
                            model.TotalPage = totalpage;
                            model.SiteName = supports[i].SiteName;
                            list.Add(model);
                        }
                    }
                }
            }
            list = list.Skip(count2).Take(count1).ToList();
            return list;
        }
        public async Task<dynamic> GetSupportAdminById(int Id)
        {


            var updatechat = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.IsRead == false && x.TicketId == Id).OrderByDescending(x => x.Id).ToList();
            foreach (var item in updatechat)
            {
                if (item != null)
                {
                    int createdby = item.CreatedBy;
                    int RoleId = 0;
                    var userl = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == createdby).FirstOrDefault();
                    if (userl != null)
                    {
                        RoleId = userl.RoleId;
                    }
                    if (RoleId != 1)
                    {
                        item.IsRead = true;

                    }
                    else
                    {
                        item.IsRead = false;

                    }
                    item.UpdatedOn = DateTime.Now;
                    _dbContext.Supports.Update(item);
                    _dbContext.SaveChanges();
                }
            }



            string path = _configuration["ProfilePath"];
            List<GetPastQuery> list = new List<GetPastQuery>();
            int SupportId = 0;
            var support = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && (x.Id == Id || x.TicketId == Id)).ToList();
            for (int i = 0; i < support.Count; i++)
            {
                if (support[i].ParentId == 0)
                {
                    SupportId = support[i].Id;
                    GetPastQuery model = new GetPastQuery();
                    model.Date = support[i].CreatedOn.ToString("dd-MM-yyyy HH:mm");
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
                    model.Class = "reply_issue";
                    model.Subject = support[i].Subject;
                    model.Status = support[0].Status;
                    var userl = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == model.CreatedBy).FirstOrDefault();
                    if (userl != null)
                    {
                        model.RoleId = userl.RoleId;
                    }
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
                        var userl = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == model.CreatedBy).FirstOrDefault();
                        if (userl != null)
                        {
                            model.RoleId = userl.RoleId;
                        }
                        model.Status = support[0].Status;
                        if (model.RoleId == 1)
                        {
                            model.Class = "response_issue";
                        }
                        else
                        {
                            model.Class = "reply_issue";
                        }


                        model.Subject = response.Subject;
                     //   model.RoleId = 1;
                        list.Add(model);
                    }
                }
            }
            return list;
        }
        public async Task<dynamic> GetSupportById(int Id)
        {
            GetSupportCls obj = new GetSupportCls();
            var support = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Id).FirstOrDefault();
            if (support != null)
            {
                support.IsRead = true;
                support.UpdatedBy = 1;
                support.UpdatedOn = DateTime.Now;
                _dbContext.Supports.Update(support);
                _dbContext.SaveChanges();
            }
            string name = string.Empty;
            string email = string.Empty;
            var user = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == support.RegisterUserId).FirstOrDefault();
            if (user != null)
            {
                name = user.FirstName + " " + user.LastName;
                email = user.Email;
            }
            obj.Name = name;
            obj.Email = email;
            obj.Subject = support.Subject;
            obj.Issue = support.Issue;
            obj.PropertyName = _dbContext.Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == support.RegisterUser.SiteId).FirstOrDefault().SiteName;
            obj.RegisterUserId = support.RegisterUserId;
            obj.Id = support.Id;
            var response = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParentId == support.Id).FirstOrDefault();
            if (response != null)
            {
                obj.Response = response.Issue;
            }
            return obj;
        }

        public async Task<dynamic> GetParkingBayNosEdit(int SiteId, int UserId)
        {
            var parkingbaynos = await _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == SiteId && (x.RegisterUserId == 0 || x.RegisterUserId == UserId)).ToListAsync();
            return parkingbaynos;
        }
        DateTime RoundUp(DateTime dt, TimeSpan d)
        {
            return new DateTime((dt.Ticks + d.Ticks - 1) / d.Ticks * d.Ticks, dt.Kind);
        }
        

            public async Task<dynamic> GetVisitorParkingsById(string TenantId, string id)
            {
            DateTime todayday = DateTime.UtcNow.Date;

            var visitors2 = (from l in _dbContext.VisitorParkingTemps
                             join r in _dbContext.RegisterUsers on l.RegisterUserId equals r.Id
                             where l.RegisterUserId == Convert.ToInt32(TenantId) && l.Id == Convert.ToInt32(id) 
                             && l.IsActive == true && l.IsDeleted == false
                             && l.StartDate >= todayday
                             select new
                             {
                                 Date = l.StartDate.ToString("dd-MM-yyyy"),
                                 vrm = l.VRMNumber,
                                 Name = l.Name + " " + l.Surname,
                                 Contact = r.MobileNumber,
                                 Starttime = l.StartTime,
                                 Endtime = l.EndTime,
                                 Id = l.Id,
                                 status = false
                             }).OrderByDescending(x => x.Id).ToList();


            return (visitors2);
        }
        public async Task<dynamic> GetVisitorParkings(string TenantId)
        {
            DateTime todayday = DateTime.UtcNow.Date;
            var visitors = (from l in _dbContext.VisitorParkings
                            join r in _dbContext.RegisterUsers on l.RegisterUserId equals r.Id
                            where l.RegisterUserId == Convert.ToInt32(TenantId)
                            && l.IsActive == true && l.IsDeleted == false
                            && l.StartDate >= todayday
                            select new
                            {
                                Date = l.StartDate.ToString("dd-MM-yyyy"),
                                vrm = l.VRMNumber,
                                Name = l.Name + " " + l.Surname,
                                Contact = r.MobileNumber,
                                Starttime = l.StartTime,
                                Endtime = l.EndTime,
                                Id = l.Id,
                                status = true
                            }).OrderByDescending(x => x.Id).ToList();

            var visitors2 = (from l in _dbContext.VisitorParkingTemps
                             join r in _dbContext.RegisterUsers on l.RegisterUserId equals r.Id
                             where l.RegisterUserId == Convert.ToInt32(TenantId)
                             && l.IsActive == true && l.IsDeleted == false
                             && l.StartDate >= todayday
                             select new
                             {
                                 Date = l.StartDate.ToString("dd-MM-yyyy"),
                                 vrm = l.VRMNumber,
                                 Name = l.Name + " " + l.Surname,
                                 Contact = r.MobileNumber,
                                 Starttime = l.StartTime,
                                 Endtime = l.EndTime,
                                 Id = l.Id,
                                 status = false
                             }).OrderByDescending(x => x.Id).ToList();


            return visitors.Concat(visitors2);
        }

        public async Task<dynamic> DeleteManageParking(int Id, int Bayno)
        {
            try
            {
                var vehicleregistration = _dbContext.VehicleRegistrations.Where(x => x.Id == Id && x.ParkingBayNo == Bayno && x.IsActive==true && x.IsDeleted==false).FirstOrDefault();
                if (vehicleregistration != null)
                {
                    int parkingbayno = vehicleregistration.ParkingBayNo;
                    var parkingbaynos = _dbContext.ParkingBayNos.Where(x => x.Id == parkingbayno).FirstOrDefault();
                    if (parkingbaynos != null)
                    {
                        int maxvehicle = parkingbaynos.MaxVehiclesPerBay;

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
                            vehicleregistration.UpdatedBy = vehicleregistration.CreatedBy;
                            vehicleregistration.UpdatedOn = DateTime.Now;
                            _dbContext.VehicleRegistrations.Update(vehicleregistration);
                            _dbContext.SaveChanges();
                        }

                    }
                }
                return new { Message = "Deleted Successfully" };
            }
            catch
            {
                return null;
            }
        }


        public async Task<dynamic> GetManageParkings(string TenantId)
        {
            DateTime todayday = DateTime.UtcNow.Date;
            var visitors = (from l in _dbContext.VehicleRegistrations
                            join b in _dbContext.ParkingBayNos on l.ParkingBayNo equals b.Id
                            where l.RegisterUserId == Convert.ToInt32(TenantId)
                            && l.IsActive == true && l.IsDeleted == false
                            //&& l.StartDate >= todayday
                            select new
                            {
                                vrm = l.VRM,
                                bayName = b.BayName,
                                Startdate = l.StartDate,
                                Endate = l.EndDate,
                                Id = l.Id,
                                bayno = l.ParkingBayNo,
                                maxvehicle=b.MaxVehiclesPerBay,
                                registerUserId=l.RegisterUserId
                            }).OrderByDescending(x => x.Id).ToList();

            return visitors;
        }
        public async Task<dynamic> BindTimeSlots(string duration, string sessionunit, DateTime date, string SiteId)
        {
            List<TimeSlotCls> model = new List<TimeSlotCls>();
            List<TimeSlotCls> list = new List<TimeSlotCls>();
            try
            {
                double result = 0;
                var dt2 = "";
                string starttime = string.Empty;
                string datefrom = date.ToString("yyyy-MM-dd");
                int durationnew = Convert.ToInt32(duration);
                string datetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                if (datefrom == DateTime.Now.ToString("yyyy-MM-dd"))
                {
                    if (sessionunit == "Minutes")
                    {
                        var dt1 = RoundUp(DateTime.Parse(datetime), TimeSpan.FromMinutes(durationnew));
                        starttime = dt1.ToString("HH:mm");
                    }
                    else if (sessionunit == "Hours")
                    {
                        var dt1 = RoundUp(DateTime.Parse(datetime), TimeSpan.FromHours(durationnew));
                        starttime = dt1.ToString("HH:mm");
                    }

                }
                else
                {
                    starttime = "00:00";
                }

                int j = 1;
                list.Add(new TimeSlotCls
                {
                    Id = j,
                    Time = starttime
                });
                string endtime = "23:50";

                DateTime dtstart = Convert.ToDateTime(starttime);
                DateTime dtend = Convert.ToDateTime(endtime);
                TimeSpan subttot = dtend.Subtract(dtstart);
                TimeSpan span = new TimeSpan(subttot.Hours, ((subttot.Minutes) + 10), 0);
                if (sessionunit == "Minutes")
                {
                    result = (span.TotalMinutes / durationnew);
                }
                else if (sessionunit == "Hours")
                {
                    result = (span.TotalHours / durationnew);
                }
                else if (sessionunit == "Days")
                {

                }
                for (int i = 0; i < result; i++)
                {
                    if (sessionunit == "Minutes")
                    {
                        DateTime dtim1 = Convert.ToDateTime(starttime).AddMinutes(Convert.ToDouble(duration));
                        string dtnw1 = dtim1.ToString("HH:mm");
                        j++;
                        list.Add(new TimeSlotCls
                        {
                            Id = j,
                            Time = dtnw1
                        });
                        starttime = dtnw1;
                    }
                    else if (sessionunit == "Hours")
                    {
                        DateTime dtim1 = Convert.ToDateTime(starttime).AddHours(Convert.ToDouble(duration));
                        string dtnw1 = dtim1.ToString("HH:mm");
                        j++;
                        list.Add(new TimeSlotCls
                        {
                            Id = j,
                            Time = dtnw1
                        });
                        starttime = dtnw1;
                    }
                }


                for (int i = 0; i < list.Count; i++)
                {
                    string time = list[i].Time;
                    DateTime dttime = Convert.ToDateTime(time).AddMinutes(-1);
                    string newtime = dttime.ToString("HH:mm");
                    ArrayList array = new ArrayList();
                    array.Add(Convert.ToInt32(SiteId));
                    array.Add(time);
                    string dt = date.ToString("yyyy-MM-dd") + " " + newtime;
                    array.Add(Convert.ToDateTime(dt));
                    AdminBusiness business = new AdminBusiness();
                    string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
                    DataSet ds = business.GetVisitorBaynos(array, strConnection);
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        if (sessionunit == "Minutes")
                        {
                            dttime = Convert.ToDateTime(time).AddMinutes(durationnew).AddMinutes(1);
                            newtime = dttime.ToString("HH:mm");
                            array = new ArrayList();
                            array.Add(Convert.ToInt32(SiteId));
                            array.Add(time);
                            dt = date.ToString("yyyy-MM-dd") + " " + newtime;
                            array.Add(Convert.ToDateTime(dt));
                            DataSet ds1 = business.GetVisitorBaynos(array, strConnection);
                            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                            {
                                model.Add(new TimeSlotCls
                                {
                                    Id = list[i].Id,
                                    Time = list[i].Time
                                });
                            }
                            else
                            {
                                dttime = Convert.ToDateTime(time).AddMinutes(durationnew).AddMinutes(-1);
                                newtime = dttime.ToString("HH:mm");
                                array = new ArrayList();
                                array.Add(Convert.ToInt32(SiteId));
                                array.Add(time);
                                dt = date.ToString("yyyy-MM-dd") + " " + newtime;
                                array.Add(Convert.ToDateTime(dt));
                                DataSet ds2 = business.GetVisitorBaynos(array, strConnection);
                                if (ds2 != null && ds2.Tables.Count > 0 && ds2.Tables[0].Rows.Count > 0)
                                {
                                    model.Add(new TimeSlotCls
                                    {
                                        Id = list[i].Id,
                                        Time = list[i].Time
                                    });
                                }
                            }
                        }
                        else
                        {
                            dttime = Convert.ToDateTime(time).AddMinutes(1);
                            newtime = dttime.ToString("HH:mm");
                            array = new ArrayList();
                            array.Add(Convert.ToInt32(SiteId));
                            array.Add(time);
                            dt = date.ToString("yyyy-MM-dd") + " " + newtime;
                            array.Add(Convert.ToDateTime(dt));
                            DataSet ds1 = business.GetVisitorBaynos(array, strConnection);
                            if (ds1 != null && ds1.Tables.Count > 0 && ds1.Tables[0].Rows.Count > 0)
                            {


                                model.Add(new TimeSlotCls
                                {
                                    Id = list[i].Id,
                                    Time = list[i].Time
                                });


                            }
                        }
                    }
                    else
                    {
                        dttime = Convert.ToDateTime(time).AddMinutes(1);
                        newtime = dttime.ToString("HH:mm");
                        array = new ArrayList();
                        array.Add(Convert.ToInt32(SiteId));
                        array.Add(time);
                        dt = date.ToString("yyyy-MM-dd") + " " + newtime;
                        array.Add(Convert.ToDateTime(dt));
                        DataSet ds3 = business.GetVisitorBaynos(array, strConnection);
                        if (ds3 != null && ds3.Tables.Count > 0 && ds3.Tables[0].Rows.Count > 0)
                        {
                            model.Add(new TimeSlotCls
                            {
                                Id = list[i].Id,
                                Time = list[i].Time
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }




            return model;
        }

        public async Task<dynamic> GetVisitorBayNo(string starttime, string siteid, string date)
        {


            DateTime ddate = Convert.ToDateTime(date);
            DateTime dttime = Convert.ToDateTime(starttime).AddMinutes(1);
            ArrayList array = new ArrayList();
            array.Add(Convert.ToInt32(siteid));
            array.Add(dttime.ToString("HH:mm"));
            string dt = ddate.ToString("yyyy-MM-dd") + " " + dttime.ToString("HH:mm");
            array.Add(Convert.ToDateTime(dt));
            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.GetVisitorBaynos(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {

                int Id = Convert.ToInt32(ds.Tables[0].Rows[0]["Id"]);
                var visitorbaynos = _dbContext.VisitorBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == Convert.ToInt32(siteid) && x.Id == Id).ToList();
                return visitorbaynos[0];
            }
            return null;

        }
        public async Task<dynamic> AddSite(AddSiteAc objsite)
        {

            Site site = new Site();
            site.SiteName = objsite.SiteName;
            site.ZatparkSitecode = objsite.SiteCode;
            site.SiteAddress = objsite.SiteAddress;
            site.City = objsite.City;
            site.State = objsite.State;
            site.Zipcode = objsite.Zipcode;
            site.ContactPersonName = objsite.ContactPersonName;
            site.Email = objsite.Email;
            site.ContactNumber = objsite.ContactNumber;
            site.MobileNumber = objsite.MobileNumber;
            site.TenantParkingBays = objsite.TenantParkingBays;
            site.NoOfTotalBay = Convert.ToInt32(objsite.TenantParkingBays);
            site.VisitorParkingBays = objsite.VisitorParkingBays;
            site.ManageParkingAvailble = objsite.ManageParkingAvailble;
            site.visitorParkingAvailble = objsite.VisitorParkingAvailble;
            site.Zatparklogs24hrs = objsite.Zatparklogs24hrs;
            site.IsActive = objsite.Active;
            site.IsDeleted = false;
            site.CreatedBy = objsite.LoginId;
            site.CreatedOn = DateTime.Now;
            site.MaxVehiclesPerBay = objsite.VehiclesPerBay;
            site.ParkingBaySeperator = objsite.Seperator;
            site.ParkingBaySectionsOrFloors = Convert.ToInt32(objsite.Section);
            site.VisitorSeperator = objsite.VSeperator;
            site.IndustryId = objsite.IndustryId;
            site.OperatorId = objsite.OperatorId;
            site.EnforcementService = objsite.EnforcementService;
            site.VisitorSectionsOrFloors = Convert.ToInt32(objsite.VSection);
            _dbContext.Sites.Add(site);
            _dbContext.SaveChanges();
            int j = 0;
            for (int i = 0; i < objsite.ParkingBays.Count; i++)
            {
                j++;
                ParkingBay bay = new ParkingBay();
                bay.Prefix = objsite.ParkingBays[i].prefix;
                bay.From = Convert.ToInt32(objsite.ParkingBays[i].from);
                bay.To = Convert.ToInt32(objsite.ParkingBays[i].to);
                bay.count = Convert.ToInt32(objsite.ParkingBays[i].count);
                bay.IsActive = true;
                bay.Section = j;
                bay.IsDeleted = false;
                bay.CreatedBy = 1;
                bay.CreatedOn = DateTime.Now;
                bay.SiteId = site.Id;
                _dbContext.ParkingBays.Add(bay);
                _dbContext.SaveChanges();
                string prefix = objsite.ParkingBays[i].prefix;
                int from = Convert.ToInt32(objsite.ParkingBays[i].from);
                int to = Convert.ToInt32(objsite.ParkingBays[i].to);
                for (int k = from; k <= to; k++)
                {
                    if (objsite.Seperator == "None")
                    {
                        objsite.Seperator = "";
                    }
                    else if (objsite.Seperator == "EmptySpace")
                    {
                        objsite.Seperator = " ";
                    }
                    string bayname = prefix + objsite.Seperator + k;
                    ParkingBayNo bayno = new ParkingBayNo();

                    bayno.BayName = bayname;
                    bayno.CreatedBy = 1;
                    bayno.MaxVehiclesPerBay = objsite.VehiclesPerBay;
                    bayno.CreatedOn = DateTime.Now;
                    bayno.IsActive = true;
                    bayno.IsDeleted = false;
                    bayno.ParkingBayId = bay.Id;
                    bayno.SiteId = site.Id;
                    bayno.Section = j.ToString();
                    _dbContext.ParkingBayNos.Add(bayno);
                    _dbContext.SaveChanges();
                }
            }


            j = 0;
            for (int i = 0; i < objsite.VisitorBays.Count; i++)
            {
                j++;
                VisitorBay bay = new VisitorBay();
                bay.Prefix = objsite.VisitorBays[i].prefix;
                bay.From = Convert.ToInt32(objsite.VisitorBays[i].from);
                bay.To = Convert.ToInt32(objsite.VisitorBays[i].to);
                bay.count = Convert.ToInt32(objsite.VisitorBays[i].count);
                bay.MaxParkingSessionAllowed = Convert.ToInt32(objsite.maxparkingsession);
                bay.TimeUnit = objsite.TimeUnit;
                bay.IsActive = true;
                bay.Section = j;
                bay.IsDeleted = false;
                bay.CreatedBy = 1;
                bay.CreatedOn = DateTime.Now;
                bay.SiteId = site.Id;
                _dbContext.VisitorBays.Add(bay);
                _dbContext.SaveChanges();
                string prefix = objsite.VisitorBays[i].prefix;
                int from = Convert.ToInt32(objsite.VisitorBays[i].from);
                int to = Convert.ToInt32(objsite.VisitorBays[i].to);
                for (int k = from; k <= to; k++)
                {
                    if (objsite.VSeperator == "None")
                    {
                        objsite.VSeperator = "";
                    }
                    else if (objsite.VSeperator == "EmptySpace")
                    {
                        objsite.VSeperator = " ";
                    }
                    string bayname = prefix + objsite.VSeperator + k;
                    VisitorBayNo bayno = new VisitorBayNo();

                    bayno.BayName = bayname;
                    bayno.CreatedBy = 1;
                    bayno.CreatedOn = DateTime.Now;
                    bayno.IsActive = true;
                    bayno.IsDeleted = false;
                    //   bayno.MaxVehiclesPerBay = objsite.VehiclesPerBay;
                    bayno.VisitorBayId = bay.Id;
                    bayno.SiteId = site.Id;
                    bayno.Section = j.ToString();
                    _dbContext.VisitorBayNos.Add(bayno);
                    _dbContext.SaveChanges();
                }
            }
            for (int i = 0; i < objsite.VisitorSessions.Count; i++)
            {
                VisitorBaySession session = new VisitorBaySession();
                session.SiteId = site.Id;
                session.Duration = objsite.VisitorSessions[i].duration;
                session.SessionUnit = objsite.VisitorSessions[i].sessionunit;
                session.CreatedBy = 1;
                session.CreatedOn = DateTime.Now;
                session.IsActive = true;
                session.IsDeleted = false;
                _dbContext.VisitorBaySessions.Add(session);
                _dbContext.SaveChanges();
            }
            return new { Message = "Site saved successfully" };
        }

        public async Task<dynamic> DeleteSite(int Id)
        {
            Site site = _dbContext.Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Id).FirstOrDefault();
            if (site != null)
            {

                var parkingbaynos = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == Id).ToList();
                if (parkingbaynos != null)
                {
                    parkingbaynos.ForEach(x =>
                    {
                        x.IsActive = false;
                        x.IsDeleted = true;
                        x.UpdatedBy = x.CreatedBy;
                        x.UpdatedOn = DateTime.Now;
                    });
                    _dbContext.ParkingBayNos.UpdateRange(parkingbaynos);
                    _dbContext.SaveChanges();
                }
                var parkingbayno = _dbContext.ParkingBays.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == Id).ToList();
                if (parkingbayno != null)
                {
                    parkingbayno.ForEach(x =>
                    {
                        x.IsActive = false;
                        x.IsDeleted = true;
                        x.UpdatedBy = x.CreatedBy;
                        x.UpdatedOn = DateTime.Now;
                    });
                    _dbContext.ParkingBays.UpdateRange(parkingbayno);
                    _dbContext.SaveChanges();

                }
                var visitorbaynos = _dbContext.VisitorBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == Id).ToList();
                if (visitorbaynos != null)
                {
                    visitorbaynos.ForEach(x =>
                    {
                        x.IsActive = false;
                        x.IsDeleted = true;
                        x.UpdatedBy = x.CreatedBy;
                        x.UpdatedOn = DateTime.Now;
                    });
                    _dbContext.VisitorBayNos.UpdateRange(visitorbaynos);
                    _dbContext.SaveChanges();
                }
                var visitorbayno = _dbContext.VisitorBays.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == Id).ToList();
                if (visitorbayno != null)
                {
                    visitorbayno.ForEach(x =>
                    {
                        x.IsActive = false;
                        x.IsDeleted = true;
                        x.UpdatedBy = x.CreatedBy;
                        x.UpdatedOn = DateTime.Now;
                    });
                    _dbContext.VisitorBays.UpdateRange(visitorbayno);
                    _dbContext.SaveChanges();

                }
                var sessions = _dbContext.VisitorBaySessions.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == Id).ToList();
                if (sessions != null)
                {
                    sessions.ForEach(x =>
                    {
                        x.IsActive = false;
                        x.IsDeleted = true;
                        x.UpdatedBy = x.CreatedBy;
                        x.UpdatedOn = DateTime.Now;
                    });
                    _dbContext.VisitorBaySessions.UpdateRange(sessions);
                    _dbContext.SaveChanges();

                }
                site.IsDeleted = true;
                site.IsActive = false;
                site.UpdatedOn = DateTime.Now;
                _dbContext.Sites.Update(site);
                _dbContext.SaveChanges();
                return new { Message = "Site deleted successfully" };

            }
            else
            {
                return new { Message = "No data found" };
            }

        }

        public async Task<dynamic> GetSiteById(int Id)
         {
            var site = (from s in _dbContext.Sites
                        where s.Id == Id
                        select new
                        {
                            s.City,
                            s.ContactNumber,
                            s.ContactPersonName,
                            s.CreatedBy,
                            s.CreatedOn,
                            MaxVehiclesPerBay = s.MaxVehiclesPerBay.ToString(),
                            s.Email,
                            s.Id,
                            s.IsActive,
                            s.ManageParkingAvailble,
                            s.visitorParkingAvailble,
                            s.Zatparklogs24hrs,
                            s.IsDeleted,
                            s.MobileNumber,
                            s.NoOfTotalBay,
                            SectionsOrFloors = s.ParkingBaySectionsOrFloors,
                            Seperator = s.ParkingBaySeperator,
                            s.SiteAddress,
                            s.SiteName,
                            SiteCode = s.ZatparkSitecode,
                            s.State,
                            s.TenantParkingBays,
                            s.UpdatedBy,
                            s.UpdatedOn,
                            s.VisitorParkingBays,
                            s.Zipcode,
                            s.VisitorSectionsOrFloors,
                            s.VisitorSeperator,
                            s.IndustryId,
                            s.OperatorId,
                            s.EnforcementService,
                            bays = (from p in _dbContext.ParkingBays
                                    where p.SiteId == s.Id && p.IsActive == true && p.IsDeleted == false
                                    select new
                                    {
                                        Prefix = p.Prefix,
                                        From = p.From.ToString(),
                                        To = p.To.ToString(),
                                        p.SiteId,
                                        p.Section,
                                        Id = p.Id,
                                        Count = p.count.ToString()
                                    }).ToList(),
                            visitorbays = (from v in _dbContext.VisitorBays
                                           where v.SiteId == s.Id && v.IsActive == true && v.IsDeleted == false
                                           select new
                                           {
                                               Prefix = v.Prefix,
                                               From = v.From.ToString(),
                                               To = v.To.ToString(),
                                               v.SiteId,
                                               v.Section,
                                               Id = v.Id,
                                               Count = v.count.ToString(),
                                               MaxParkingSession = v.MaxParkingSessionAllowed.ToString(),
                                               v.TimeUnit
                                           }).ToList(),
                            visitorsessions = (from v in _dbContext.VisitorBaySessions
                                               where v.SiteId == s.Id && v.IsActive == true && v.IsDeleted == false
                                               select new
                                               {
                                                   Id = v.Id,
                                                   Duration = v.Duration,
                                                   SessionUnit = v.SessionUnit,
                                                   SiteId = v.SiteId
                                               }).ToList()
                        }).FirstOrDefault();
            //Site site = await _dbContext.Sites.FirstOrDefaultAsync(x => x.IsActive == true && x.IsDeleted == false && x.Id == Id);
            return site;
        }
        public async Task<dynamic> GetSearchSites(int PageNo, int PageSize, string SiteName, string Email, string MobileNumber, int SiteId)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            List<GetSearchSiteCls> list = new List<GetSearchSiteCls>();
            ArrayList array = new ArrayList();
            array.Add(SiteName);
            array.Add(Email);
            array.Add(MobileNumber);
            if (SiteId == 0)
            {
                array.Add(null);
            }
            else
            {
                array.Add(SiteId);
            }
            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.GetSearchSites(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int totalitems = ds.Tables[0].Rows.Count;
                double totalpa = (double)totalitems / (double)PageSize;
                double totalpage = Math.Round(totalpa);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    list.Add(new GetSearchSiteCls
                    {
                        pageNo=PageNo,
                        TotalItem = totalitems,
                        TotalPage = totalpage + 1,
                        Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                        SiteName = ds.Tables[0].Rows[i]["SiteName"].ToString(),
                        Email = ds.Tables[0].Rows[i]["Email"].ToString(),
                        MobileNumber = ds.Tables[0].Rows[i]["MobileNumber"].ToString()
                    });
                }
            }
            list = list.Skip(count2).Take(count1).ToList();
            return list;
        }
        public async Task<dynamic> GetSearchAuditReport(int PageNo, int PageSize, string Username, string Sitename, string Fromdate, string Todate, int SiteId)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            List<GetSearchAuditLogs> list = new List<GetSearchAuditLogs>();
            ArrayList array = new ArrayList();
            array.Add(Username);
            array.Add(Sitename);
            array.Add(Fromdate);
            array.Add(Todate);
            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.GetSearchAuditLog(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int totalitems = ds.Tables[0].Rows.Count;
                double totalpa = (double)totalitems / (double)PageSize;
                double totalpage = Math.Round(totalpa);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    list.Add(new GetSearchAuditLogs
                    {
                        pageNo = PageNo,
                        TotalItem = totalitems,
                        TotalPage = totalpage + 1,
                        Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                        SiteName = ds.Tables[0].Rows[i]["SiteName"].ToString(),
                        Username = ds.Tables[0].Rows[i]["Username"].ToString(),
                        Function = ds.Tables[0].Rows[i]["Function"].ToString(),
                        Date = ds.Tables[0].Rows[i]["Date"].ToString(),
                        Operation = ds.Tables[0].Rows[i]["Operation"].ToString(),
                        Agent = ds.Tables[0].Rows[i]["Agent"].ToString(),
                        SiteId = Convert.ToInt32(ds.Tables[0].Rows[i]["SiteId"])
                    }) ;
                }
            }

            //  return list;
            if (SiteId == 0)
            {
                list = list.Skip(count2).Take(count1).ToList();
                return list;
            }
            else
            {
                list = list.Where(x => x.SiteId == SiteId).ToList();
                list = list.Skip(count2).Take(count1).ToList();
                return list;
            }

        }
        public async Task<dynamic> GetSearchZatpark(int PageNo, int PageSize, string Tenant, string Sitename, string BayNo, string Fromdate, string Todate, int SiteId)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            List<GetSearchZatpark> list = new List<GetSearchZatpark>();
            ArrayList array = new ArrayList();
            array.Add(Tenant);
            array.Add(Sitename);
            array.Add(BayNo);
            array.Add(Fromdate);
            array.Add(Todate);
            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.GetSearchZatpark(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int totalitems = ds.Tables[0].Rows.Count;
                double totalpa = (double)totalitems / (double)PageSize;
                double totalpage = Math.Round(totalpa);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    list.Add(new GetSearchZatpark
                    {
                        TotalItem = totalitems,
                        TotalPage = totalpage + 1,
                        Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                        RegistrationTimeSlotId = Convert.ToInt32(ds.Tables[0].Rows[i]["RegistrationTimeSlotId"]),
                        ParkingBayNoId = Convert.ToInt32(ds.Tables[0].Rows[i]["ParkingBayNoId"]),
                        SiteId = Convert.ToInt32(ds.Tables[0].Rows[i]["SiteId"]),
                        SiteName = ds.Tables[0].Rows[i]["SiteName"].ToString(),
                        Tenant = ds.Tables[0].Rows[i]["Tenant"].ToString(),
                        Request = ds.Tables[0].Rows[i]["Request"].ToString(),
                        FromDate = ds.Tables[0].Rows[i]["FromDate"].ToString(),
                        ToDate = ds.Tables[0].Rows[i]["ToDate"].ToString(),
                        BayName = ds.Tables[0].Rows[i]["BayName"].ToString(),
                        vrm = ds.Tables[0].Rows[i]["VRM"].ToString(),
                        ZatparkResponse = ds.Tables[0].Rows[i]["ZatparkResponse"].ToString()
                    });
                }
            }

            //  return list;
            if (SiteId == 0)
            {
                list = list.Skip(count2).Take(count1).ToList();
                return list;
            }
            else
            {
                list = list.Where(x => x.SiteId == SiteId).ToList();
                list = list.Skip(count2).Take(count1).ToList();
                return list;
            }

        }

        public async Task<dynamic> GetAuditLogs(int PageNo, int PageSize, int RoleId, int SiteId)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            List<GetSearchAuditLogs> list = new List<GetSearchAuditLogs>();
            ArrayList array = new ArrayList();
            array.Add("");
            array.Add("");
            array.Add(DateTime.Now.ToString("yyyy-MM-dd"));
            array.Add(DateTime.Now.ToString("yyyy-MM-dd"));
            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.GetSearchAuditLog(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int totalitems = ds.Tables[0].Rows.Count;
                double totalpa = (double)totalitems / (double)PageSize;
                double totalpage = Math.Round(totalpa);
                int pageNo = PageNo;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    list.Add(new GetSearchAuditLogs
                    {
                        pageNo=PageNo,
                        TotalItem = totalitems,
                        TotalPage = totalpage + 1,
                        Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                        SiteName = ds.Tables[0].Rows[i]["SiteName"].ToString(),
                        Username = ds.Tables[0].Rows[i]["Username"].ToString(),
                        Function = ds.Tables[0].Rows[i]["Function"].ToString(),
                        Date = ds.Tables[0].Rows[i]["Date"].ToString(),
                        Operation = ds.Tables[0].Rows[i]["Operation"].ToString(),
                        Agent = ds.Tables[0].Rows[i]["Agent"].ToString(),
                        SiteId = Convert.ToInt32(ds.Tables[0].Rows[i]["SiteId"])
                    });
                }
            }

            //  return list;
            if (RoleId == 1)
            {
                list = list.Skip(count2).Take(count1).ToList();
                return list;
            }
            else
            {
                list = list.Where(x => x.SiteId == SiteId).ToList();
                list = list.Skip(count2).Take(count1).ToList();
                return list;
            }

        }
        public async Task<dynamic> GetTenantLogs(int PageNo, int PageSize, int SiteId, int TenantId)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            List<GetSearchAuditLogs> list = new List<GetSearchAuditLogs>();
            ArrayList array = new ArrayList();
            array.Add(TenantId);
            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.GetSearchTenantLog(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int totalitems = ds.Tables[0].Rows.Count;
                double totalpa = (double)totalitems / (double)PageSize;
                double totalpage = Math.Round(totalpa);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    list.Add(new GetSearchAuditLogs
                    {
                        TotalItem = totalitems,
                        TotalPage = totalpage + 1,
                        Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                        SiteName = ds.Tables[0].Rows[i]["SiteName"].ToString(),
                        Username = ds.Tables[0].Rows[i]["Username"].ToString(),
                        Function = ds.Tables[0].Rows[i]["Function"].ToString(),
                        Date = ds.Tables[0].Rows[i]["Date"].ToString(),
                        Operation = ds.Tables[0].Rows[i]["Operation"].ToString(),
                        Agent = ds.Tables[0].Rows[i]["Agent"].ToString(),
                        SiteId = Convert.ToInt32(ds.Tables[0].Rows[i]["SiteId"])
                    });
                }
            }

            return list;
            //if (RoleId == 1)
            //{
            //    list = list.Skip(count2).Take(count1).ToList();
            //    return list;
            //}
            //else
            //{
            //    list = list.Where(x => x.SiteId == SiteId).ToList();
            //    list = list.Skip(count2).Take(count1).ToList();
            //    return list;
            //}

        }



        public async Task<dynamic> GetZatParkLogs(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            List<GetSearchZatpark> list = new List<GetSearchZatpark>();
            ArrayList array = new ArrayList();
            array.Add("");
            array.Add("");
            array.Add("");
            array.Add(DateTime.Now.ToString("yyyy-MM-dd"));
            array.Add(DateTime.Now.ToString("yyyy-MM-dd"));
            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.GetSearchZatpark(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int totalitems = ds.Tables[0].Rows.Count;
                double totalpa = (double)totalitems / (double)PageSize;
                double totalpage = Math.Round(totalpa);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    list.Add(new GetSearchZatpark
                    {
                        TotalItem = totalitems,
                        TotalPage = totalpage + 1,
                        Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                        RegistrationTimeSlotId = Convert.ToInt32(ds.Tables[0].Rows[i]["RegistrationTimeSlotId"]),
                        ParkingBayNoId = Convert.ToInt32(ds.Tables[0].Rows[i]["ParkingBayNoId"]),
                        SiteId = Convert.ToInt32(ds.Tables[0].Rows[i]["SiteId"]),
                        SiteName = ds.Tables[0].Rows[i]["SiteName"].ToString(),
                        Tenant = ds.Tables[0].Rows[i]["Tenant"].ToString(),
                        Request = ds.Tables[0].Rows[i]["Request"].ToString(),
                        FromDate = ds.Tables[0].Rows[i]["FromDate"].ToString(),
                        ToDate = ds.Tables[0].Rows[i]["ToDate"].ToString(),
                        BayName = ds.Tables[0].Rows[i]["BayName"].ToString(),
                        vrm = ds.Tables[0].Rows[i]["VRM"].ToString(),
                        ZatparkResponse = ds.Tables[0].Rows[i]["ZatparkResponse"].ToString()
                    });
                }
            }

            //  return list;
            if (RoleId == 1)
            {
                list = list.Skip(count2).Take(count1).ToList();
                return list;
            }
            else
            {
                list = list.Where(x => x.SiteId == SiteId).ToList();
                list = list.Skip(count2).Take(count1).ToList();
                return list;
            }

        }


        public async Task<dynamic> GetSites(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {

            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            if (RoleId == 1)
            {

                if (PageSize == 0)
                {
                    List<Site> sites1 = _dbContext.Sites.Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
                    //var sites1 = (from s in _dbContext.Sites
                    //             where s.IsActive == true && s.IsDeleted == false
                    //             select new
                    //             {
                    //                 s.Id,
                    //                 SiteName = s.SiteName,
                    //             }).OrderByDescending(x => x.Id).ToList();
                    return sites1;
                }
                else
                {
                    int totalitems = _dbContext.Sites.Where(x => x.IsDeleted == false).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var sites = (from s in _dbContext.Sites
                                 where s.IsActive == true && s.IsDeleted == false
                                 select new
                                 {   
                                     PageNo = PageNo,
                                     s.Id,
                                     SiteName = s.SiteName,
                                      s.Email,
                                     s.MobileNumber,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage,
                                 }).OrderByDescending(x => x.Id).ToList();
                 


                    sites = sites.Skip(count2).Take(count1).ToList();
             
                    return sites;
                }

            }
           else if (RoleId != 1)
            {
                var users = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.Id == LoginId).OrderByDescending(x => x.Id).FirstOrDefault();

                if (PageSize == 0)
                {


                    List<Site> sites1 = _dbContext.Sites.Where(x => x.IsDeleted == false && x.OperatorId== users.OperatorId).OrderByDescending(x => x.Id).ToList();
                    //var sites1 = (from s in _dbContext.Sites
                    //             where s.IsActive == true && s.IsDeleted == false
                    //             select new
                    //             {
                    //                 s.Id,
                    //                 SiteName = s.SiteName,
                    //             }).OrderByDescending(x => x.Id).ToList();
                    return sites1;
                }
                else
                {
                    int totalitems = _dbContext.Sites.Where(x => x.IsDeleted == false).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var sites = (from s in _dbContext.Sites
                                 where s.IsActive == true && s.IsDeleted == false && s.OperatorId== users.OperatorId
                                 select new
                                 {
                                     PageNo = PageNo,
                                     s.Id,
                                     SiteName = s.SiteName,
                                     s.Email,
                                     s.MobileNumber,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage,
                                 }).OrderByDescending(x => x.Id).ToList();



                    sites = sites.Skip(count2).Take(count1).ToList();

                    return sites;
                }

            }

            else
            {

                int totalitems = 0;
                int totalpage = 0;
                if (PageSize != 0)
                {
                    if (SiteId == 0)
                    {
                        totalitems = _dbContext.Sites.Where(x => x.IsDeleted == false).Count();
                        totalpage = totalitems / PageSize;
                        var sites = (from s in _dbContext.Sites
                                     where s.IsActive == true && s.IsDeleted == false
                                     select new
                                     {
                                         PageNo = PageNo,
                                         s.Id,
                                         s.SiteName,
                                         s.Email,
                                         s.MobileNumber,
                                         TotalItem = totalitems,
                                         TotalPage = totalpage,
                                     }).OrderByDescending(x => x.Id).ToList();
                        sites = sites.Skip(count2).Take(count1).ToList();
                        return sites;
                    }
                    else
                    {
                        totalitems = _dbContext.Sites.Where(x => x.IsDeleted == false && x.Id == SiteId).Count();
                        totalpage = totalitems / PageSize;

                        var sites = (from s in _dbContext.Sites
                                     where s.IsActive == true && s.IsDeleted == false && s.Id == SiteId
                                     select new
                                     {
                                         PageNo = PageNo,
                                         s.Id,
                                         s.SiteName,
                                         s.Email,
                                         s.MobileNumber,
                                         TotalItem = totalitems,
                                         TotalPage = totalpage,
                                     }).OrderByDescending(x => x.Id).ToList();
                        sites = sites.Skip(count2).Take(count1).ToList();
                        return sites;
                    }
                }
                else
                {
                    if (SiteId == 0)
                    {

                        List<Site> sites1 = _dbContext.Sites.Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToList();
                        return sites1;
                    }
                    else
                    {

                        List<Site> sites1 = _dbContext.Sites.Where(x => x.IsDeleted == false && x.Id == SiteId).OrderByDescending(x => x.Id).ToList();
                        return sites1;
                    }

                }
            }
        }

        public async Task<dynamic> GetSitesbyoperatorid(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId,int OperatorId)
        {

            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
           

                if (OperatorId != 0)
                {
                    List<Site> sites1 = _dbContext.Sites.Where(x => x.IsDeleted == false && x.OperatorId== OperatorId).OrderByDescending(x => x.Id).ToList();
                    //var sites1 = (from s in _dbContext.Sites
                    //             where s.IsActive == true && s.IsDeleted == false
                    //             select new
                    //             {
                    //                 s.Id,
                    //                 SiteName = s.SiteName,
                    //             }).OrderByDescending(x => x.Id).ToList();
                    return sites1;
                }
            else
            {
                return null;
            }
               

           
        }

        public async Task<dynamic> GetNotificatiosList(int tenantId)
        {
            var count = (from s in _dbContext.AuditLogs
                         where s.TenantId==tenantId && s.Isread==false
                         select new
                         {
                           s.Id,
                           s.Function,
                           s.Date,
                           s.Operation,
                           s.Isread
                          
                         }).ToList();

            return count;
        }

        public async Task<List<Site>> GetSiteslogin(int LoginId)
        {
            int siteconfig = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.Id == LoginId).OrderByDescending(x => x.Id).FirstOrDefault().SiteId;

            if (siteconfig == 0)
            {
                List<Site> sites = await _dbContext.Sites.Where(x => x.IsDeleted == false).OrderByDescending(x => x.Id).ToListAsync();
                return sites;
            }
            else
            {
                List<Site> sites = await _dbContext.Sites.Where(x => x.IsDeleted == false && x.Id == siteconfig).OrderByDescending(x => x.Id).ToListAsync();
                return sites;
            }

        }
        public async Task<List<VisitorBaySession>> GetVisitorBaySessions(int SiteId)
        {
            List<VisitorBaySession> sessions = await _dbContext.VisitorBaySessions.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == SiteId).ToListAsync();
            return sessions;
        }
        public async Task<dynamic> UpdateSite(AddSiteAc objsite)
        {
            Site site = _dbContext.Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == objsite.Id).FirstOrDefault();
            if (site != null)
            {
                site.SiteName = objsite.SiteName;
                site.ZatparkSitecode = objsite.SiteCode;
                site.SiteAddress = objsite.SiteAddress;
                site.City = objsite.City;
                site.State = objsite.State;
                site.Zipcode = objsite.Zipcode;
                site.ContactPersonName = objsite.ContactPersonName;
                site.ContactNumber = objsite.ContactNumber;
                site.Email = objsite.Email;
                site.MobileNumber = objsite.MobileNumber;
                site.IsActive = objsite.Active;
                site.ManageParkingAvailble = objsite.ManageParkingAvailble;
                site.visitorParkingAvailble = objsite.VisitorParkingAvailble;
                site.Zatparklogs24hrs = objsite.Zatparklogs24hrs;
                site.UpdatedBy = objsite.LoginId;
                site.MaxVehiclesPerBay = objsite.VehiclesPerBay;
                site.OperatorId = objsite.OperatorId;
                site.IndustryId = objsite.IndustryId;
                site.UpdatedOn = DateTime.Now;
                site.EnforcementService = objsite.EnforcementService;
                if (objsite.ParkingBays.Count != 0)
                {
                    site.ParkingBaySeperator = objsite.Seperator;
                    site.ParkingBaySectionsOrFloors = Convert.ToInt32(objsite.Section);
                    site.TenantParkingBays = objsite.TenantParkingBays;
                }
                if (objsite.VisitorBays.Count != 0)
                {
                    site.VisitorSeperator = objsite.VSeperator;
                    site.VisitorSectionsOrFloors = Convert.ToInt32(objsite.VSection);
                    site.VisitorParkingBays = objsite.VisitorParkingBays;
                }
                _dbContext.Sites.Update(site);
                _dbContext.SaveChanges();
                if (objsite.ParkingBays.Count != 0)
                {
                    var baynos = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == objsite.Id).ToList();
                    if (baynos != null || baynos.Count != 0)
                    {
                        baynos.ForEach(x =>
                        {
                            x.IsActive = false;
                            x.IsDeleted = true;
                            x.UpdatedBy = x.CreatedBy;
                            x.UpdatedOn = DateTime.Now;
                        });
                        _dbContext.ParkingBayNos.UpdateRange(baynos);
                        _dbContext.SaveChanges();
                    }

                    var bayno = _dbContext.ParkingBays.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == objsite.Id).ToList();
                    if (bayno != null || bayno.Count != 0)
                    {
                        bayno.ForEach(x =>
                        {
                            x.IsActive = false;
                            x.IsDeleted = true;
                            x.UpdatedBy = x.CreatedBy;
                            x.UpdatedOn = DateTime.Now;
                        });
                        _dbContext.ParkingBays.UpdateRange(bayno);
                        _dbContext.SaveChanges();
                    }
                    int j = 0;
                    for (int i = 0; i < objsite.ParkingBays.Count; i++)
                    {
                        j++;
                        ParkingBay bay = new ParkingBay();
                        bay.Prefix = objsite.ParkingBays[i].prefix;
                        bay.From = Convert.ToInt32(objsite.ParkingBays[i].from);
                        bay.To = Convert.ToInt32(objsite.ParkingBays[i].to);
                        bay.count = Convert.ToInt32(objsite.ParkingBays[i].count);
                        bay.IsActive = true;
                        bay.Section = j;
                        bay.IsDeleted = false;
                        bay.CreatedBy = 1;
                        bay.CreatedOn = DateTime.Now;
                        bay.SiteId = site.Id;
                        _dbContext.ParkingBays.Add(bay);
                        _dbContext.SaveChanges();
                        string prefix = objsite.ParkingBays[i].prefix;
                        int from = Convert.ToInt32(objsite.ParkingBays[i].from);
                        int to = Convert.ToInt32(objsite.ParkingBays[i].to);
                        for (int k = from; k <= to; k++)
                        {
                            if (objsite.Seperator == "None")
                            {
                                objsite.Seperator = "";
                            }
                            else if (objsite.Seperator == "EmptySpace")
                            {
                                objsite.Seperator = " ";
                            }
                            string bayname = prefix + objsite.Seperator + k;
                            ParkingBayNo baynoobj = new ParkingBayNo();

                            baynoobj.BayName = bayname;
                            baynoobj.MaxVehiclesPerBay = objsite.VehiclesPerBay;
                            baynoobj.CreatedBy = 1;
                            baynoobj.CreatedOn = DateTime.Now;
                            baynoobj.IsActive = true;
                            baynoobj.IsDeleted = false;
                            baynoobj.ParkingBayId = bay.Id;
                            baynoobj.SiteId = site.Id;
                            baynoobj.Section = j.ToString();
                            _dbContext.ParkingBayNos.Add(baynoobj);
                            _dbContext.SaveChanges();
                        }
                    }

                }
                if (objsite.VisitorBays.Count != 0)
                {
                    var visitorbaynos = _dbContext.VisitorBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == objsite.Id).ToList();
                    if (visitorbaynos != null || visitorbaynos.Count != 0)
                    {
                        visitorbaynos.ForEach(x =>
                        {
                            x.IsActive = false;
                            x.IsDeleted = true;
                            x.UpdatedBy = x.CreatedBy;
                            x.UpdatedOn = DateTime.Now;
                        });
                        _dbContext.VisitorBayNos.UpdateRange(visitorbaynos);
                        _dbContext.SaveChanges();
                    }

                    var visitorbay = _dbContext.VisitorBays.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == objsite.Id).ToList();
                    if (visitorbay != null || visitorbay.Count != 0)
                    {
                        visitorbay.ForEach(x =>
                        {
                            x.IsActive = false;
                            x.IsDeleted = true;
                            x.UpdatedBy = x.CreatedBy;
                            x.UpdatedOn = DateTime.Now;
                        });
                        _dbContext.VisitorBays.UpdateRange(visitorbay);
                        _dbContext.SaveChanges();
                    }
                    int j = 0;
                    for (int i = 0; i < objsite.VisitorBays.Count; i++)
                    {
                        j++;
                        VisitorBay bay = new VisitorBay();
                        bay.Prefix = objsite.VisitorBays[i].prefix;
                        bay.From = Convert.ToInt32(objsite.VisitorBays[i].from);
                        bay.To = Convert.ToInt32(objsite.VisitorBays[i].to);
                        bay.count = Convert.ToInt32(objsite.VisitorBays[i].count);
                        bay.MaxParkingSessionAllowed = Convert.ToInt32(objsite.maxparkingsession);
                        bay.TimeUnit = objsite.TimeUnit;
                        bay.IsActive = true;
                        bay.Section = j;
                        bay.IsDeleted = false;
                        bay.CreatedBy = 1;
                        bay.CreatedOn = DateTime.Now;
                        bay.SiteId = site.Id;
                        _dbContext.VisitorBays.Add(bay);
                        _dbContext.SaveChanges();
                        string prefix = objsite.VisitorBays[i].prefix;
                        int from = Convert.ToInt32(objsite.VisitorBays[i].from);
                        int to = Convert.ToInt32(objsite.VisitorBays[i].to);
                        for (int k = from; k <= to; k++)
                        {
                            if (objsite.VSeperator == "None")
                            {
                                objsite.VSeperator = "";
                            }
                            else if (objsite.VSeperator == "EmptySpace")
                            {
                                objsite.VSeperator = " ";
                            }
                            string bayname = prefix + objsite.VSeperator + k;
                            VisitorBayNo baynoobjv = new VisitorBayNo();

                            baynoobjv.BayName = bayname;
                            //   baynoobjv.MaxVehiclesPerBay = objsite.VehiclesPerBay;
                            baynoobjv.CreatedBy = 1;
                            baynoobjv.CreatedOn = DateTime.Now;
                            baynoobjv.IsActive = true;
                            baynoobjv.IsDeleted = false;
                            baynoobjv.VisitorBayId = bay.Id;
                            baynoobjv.SiteId = site.Id;
                            baynoobjv.Section = j.ToString();
                            _dbContext.VisitorBayNos.Add(baynoobjv);
                            _dbContext.SaveChanges();
                        }
                    }

                    for (int i = 0; i < objsite.VisitorSessions.Count; i++)
                    {
                        int Id = objsite.VisitorSessions[i].Id;
                        var session = _dbContext.VisitorBaySessions.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Id).FirstOrDefault();
                        if (session != null)
                        {
                            session.Duration = objsite.VisitorSessions[i].duration;
                            session.SessionUnit = objsite.VisitorSessions[i].sessionunit;
                            session.UpdatedBy = 1;
                            session.UpdatedOn = DateTime.Now;
                            _dbContext.VisitorBaySessions.Update(session);
                            _dbContext.SaveChanges();
                        }
                    }

                }
                return new { Message = "Site updated successfully" };
            }
            else
            {
                return new { Message = "No data Found" };
            }

        }
        public bool GetExistsSite(AddSiteAc objsite)
        {

            Site site = _dbContext.Sites.FirstOrDefault(x => x.SiteName.ToLower().Trim() == objsite.SiteName.ToLower().Trim() && x.IsDeleted == false && x.CreatedBy == objsite.LoginId);
            return (site != null);

        }

        public async Task<dynamic> saveauditlog(Auditlog req)
        {

            AuditLogs bay = new AuditLogs();
            bay.Agent = req.Agent;
            bay.IsActive = true;
            bay.IsDeleted = false;
            bay.CreatedBy = 1;
            bay.CreatedOn = DateTime.Now;
            bay.Date = DateTime.Now;
            bay.RegisterUserId = Convert.ToInt32(req.RegisterUserId);
            bay.RoleId = Convert.ToInt32(req.RoleId);
            bay.Operation = req.Operation;
            bay.Function = req.Function;
            bay.TenantId = Convert.ToInt32(req.TenantId);
            _dbContext.AuditLogs.Add(bay);
            _dbContext.SaveChanges();

            return new { Message = "Ticket Closed Successfully" };
        }

        public async Task<dynamic> saveauditlogfornotification(Auditlog req)
        {

            AuditLogs bay = new AuditLogs();
            bay.Agent = req.Agent;
            bay.IsActive = true;
            bay.IsDeleted = false;
            bay.CreatedBy = 1;
            bay.CreatedOn = DateTime.Now;
            bay.Date = DateTime.Now;
            bay.RegisterUserId = Convert.ToInt32(req.RegisterUserId);
            bay.RoleId = Convert.ToInt32(req.RoleId);
            bay.Operation = req.Operation;
            bay.Function = req.Function;
            bay.TenantId = Convert.ToInt32(req.TenantId);
            bay.Isread = true;
            _dbContext.AuditLogs.Add(bay);
            _dbContext.SaveChanges();

            return new { Message = "Ticket Closed Successfully" };
        }

    }
}

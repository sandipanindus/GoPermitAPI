using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Repository.PermissionManagement
{
    public class PermissionRepository : IPermissionRepository
    {

        private readonly LabelPadDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public PermissionRepository(LabelPadDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<dynamic> GetModuleScreens(int RoleId)
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            if (RoleId == 0) {
                menuItems = (from m in _dbContext.Modules
                               where m.IsActive == true && m.IsDeleted == false
                             orderby m.SequenceNo ascending
                             select new MenuItem
                               {
                                   ModuleId = m.Id,
                                   id = m.ModuleName.ToLower().Replace(" ", ""),
                                   icon = m.Icon,
                                   label = m.Label,
                                   to=m.To,
                                   Subs = (from s in _dbContext.Screens

                                           where s.IsActive == true && s.IsDeleted == false
                                           && s.ModuleId == m.Id
                                           orderby s.SequenceNo ascending
                                           select new ScreenItems
                                           {
                                               ModuleId = s.ModuleId,
                                               ScreenId = s.Id,
                                               ScreenName = s.ScreenName,
                                               icon = s.Icon,
                                               label = s.Label,
                                               to = s.To
                                           }).ToList()
                               }).ToList();
            }
            else
            {
                string RoleName = _dbContext.Roles.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == RoleId).FirstOrDefault().Name;
                menuItems = (from m in _dbContext.Modules
                             where m.IsActive == true && m.IsDeleted == false
                             orderby m.SequenceNo ascending
                             select new MenuItem
                             {
                                 ModuleId = m.Id,
                                 id = m.ModuleName.ToLower().Replace(" ", ""),
                                 icon = m.Icon,
                                 label = m.Label,
                                 to=m.To,
                                 Subs = (from s in _dbContext.Screens
                                         join r in _dbContext.RoleScreens on s.Id equals r.ScreenId
                                         where s.IsActive == true && s.IsDeleted == false
                                         && s.ModuleId == m.Id && (r.View == true || r.Add == true || r.Edit == true
                                         || r.Delete == true || r.Approve == true || r.Reject == true) && r.RoleId==RoleId
                                         && r.ClientId==r.ClientId orderby s.SequenceNo ascending
                                         select new ScreenItems
                                         {
                                             ModuleId = s.ModuleId,
                                             ScreenId = s.Id,
                                             ScreenName = s.ScreenName,
                                             icon = s.Icon,
                                             label = s.Label,
                                             to = s.To
                                         }).ToList()
                             }).ToList();
                menuItems = menuItems.Where(x => x.Subs.Count() != 0).ToList();
                int count = menuItems.Where(x => x.Subs.Count() != 0).ToList().Count();
                if (count == 0)
                {
                    if (RoleName == "Bo admin")
                    {
                        menuItems = (from m in _dbContext.Modules
                                     where m.IsActive == true && m.IsDeleted == false && m.Label=="menu.userconfig"
                                     select new MenuItem
                                     {
                                         ModuleId = m.Id,
                                         id = m.ModuleName.ToLower().Replace(" ", ""),
                                         icon = m.Icon,
                                         label = m.Label,
                                         to=m.To,
                                         Subs = (from s in _dbContext.Screens

                                                 where s.IsActive == true && s.IsDeleted == false
                                                 && s.ModuleId == m.Id && s.ScreenTableName== "RoleScreens"
                                                 select new ScreenItems
                                                 {
                                                     ModuleId = s.ModuleId,
                                                     ScreenId = s.Id,
                                                     ScreenName = s.ScreenName,
                                                     icon = s.Icon,
                                                     label = s.Label,
                                                     to = s.To
                                                 }).ToList()
                                     }).ToList();
                    }
                }
              
            }
            return menuItems;
        }
        public async Task<dynamic> GetScreens(int RoleId, int ClientId, int LoginId)
        {
            var Modules = (from m in _dbContext.Modules
                           where m.IsActive == true && m.IsDeleted == false
                           select new
                           {
                               ModuleId = m.Id,
                               ModuleName = m.ModuleName,
                               ModuleIcon = m.Icon,
                               Screens = (from s in _dbContext.Screens

                                          where s.IsActive == true && s.IsDeleted == false
                                          && s.ModuleId == m.Id
                                          select new
                                          {
                                              ModuleId = s.ModuleId,
                                              ScreenId = s.Id,
                                              ScreenName = s.ScreenName,
                                              ScreenIcon = s.Icon,  
                                              Label=s.Label
                                          }).ToList()
                           }).ToList();
            List<GetModulesAc> listmodule = new List<GetModulesAc>();
            List<ScreensModel> listscreen = new List<ScreensModel>();
            for (int i = 0; i < Modules.Count; i++)
            {                
                GetModulesAc model = new GetModulesAc();
                model.ModuleId = Modules[i].ModuleId;
                model.ModuleName = Modules[i].ModuleName;
                model.ModuleIcon = Modules[i].ModuleIcon;
                listscreen = new List<ScreensModel>();
                for (int j = 0; j < Modules[i].Screens.Count; j++)
                {
                   
                    ScreensModel objscreen = new ScreensModel();
                    RoleScreen obj = _dbContext.RoleScreens.Where(x => x.IsActive == true && x.IsDeleted == false && x.ScreenId == Modules[i].Screens[j].ScreenId && x.RoleId==RoleId).FirstOrDefault();
                    if (obj != null)
                    {
                        objscreen.ScreenId = Modules[i].Screens[j].ScreenId;
                        objscreen.ScreenName = Modules[i].Screens[j].ScreenName;
                        objscreen.ModuleId = Modules[i].Screens[j].ModuleId;
                        objscreen.ScreenIcon = Modules[i].Screens[j].ScreenIcon;
                        objscreen.RoleId = RoleId;
                        objscreen.RoleScreenId = obj.Id;
                        objscreen.ClientId = ClientId;
                        objscreen.View = obj.View;
                        objscreen.Add = obj.Add;
                        objscreen.Edit = obj.Edit;
                        objscreen.Delete = obj.Delete;
                        objscreen.Approve = obj.Approve;
                        objscreen.Reject = obj.Reject;
                        objscreen.LoginId = 0;
                        objscreen.Label= Modules[i].Screens[j].Label;
                    }
                    else
                    {
                        objscreen.ScreenId = Modules[i].Screens[j].ScreenId;
                        objscreen.ScreenName = Modules[i].Screens[j].ScreenName;
                        objscreen.ModuleId = Modules[i].Screens[j].ModuleId;
                        objscreen.ScreenIcon = Modules[i].Screens[j].ScreenIcon;
                        objscreen.RoleId = RoleId;
                        objscreen.RoleScreenId = 0;
                        objscreen.ClientId = ClientId;
                        objscreen.View = false;
                        objscreen.Add = false;
                        objscreen.Edit = false;
                        objscreen.Delete = false;
                        objscreen.Approve = false;
                        objscreen.Reject = false;
                        objscreen.LoginId = 0;
                        objscreen.Label = Modules[i].Screens[j].Label;
                    }
                    listscreen.Add(objscreen);
                }
                model.ScreensModel = listscreen;
                listmodule.Add(model);
            }
            return listmodule;
        }

        public async Task<dynamic> SavePermissionData(List<GetModulesAc> objinput)
        {
            for (int i = 0; i < objinput.Count; i++)
            {
                for (int j = 0; j < objinput[i].ScreensModel.Count; j++)
                {
                    RoleScreen roleScreen = await _dbContext.RoleScreens.Where(x => x.IsActive == true && x.IsDeleted == false && x.RoleId == objinput[i].ScreensModel[j].RoleId && x.ScreenId == objinput[i].ScreensModel[j].ScreenId).FirstOrDefaultAsync();
                    if (roleScreen != null)
                    {
                        roleScreen.View = objinput[i].ScreensModel[j].View;
                        roleScreen.IsActive = true;
                        roleScreen.IsDeleted = false;
                        roleScreen.Add = objinput[i].ScreensModel[j].Add;
                        roleScreen.Edit = objinput[i].ScreensModel[j].Edit;
                        roleScreen.Delete = objinput[i].ScreensModel[j].Delete;
                        roleScreen.Approve = objinput[i].ScreensModel[j].Approve;
                        roleScreen.Reject = objinput[i].ScreensModel[j].Reject;
                        roleScreen.UpdatedBy = objinput[i].ScreensModel[j].LoginId;
                        roleScreen.UpdatedOn = DateTime.Now;
                        roleScreen.ClientId = objinput[i].ScreensModel[j].ClientId;
                        roleScreen.ScreenId = objinput[i].ScreensModel[j].ScreenId;
                        roleScreen.RoleId= objinput[i].ScreensModel[j].RoleId;
                        _dbContext.RoleScreens.Update(roleScreen);
                       await _dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        roleScreen = new RoleScreen();
                        roleScreen.IsActive = true;
                        roleScreen.IsDeleted = false;
                        roleScreen.View = objinput[i].ScreensModel[j].View;
                        roleScreen.Add = objinput[i].ScreensModel[j].Add;
                        roleScreen.Edit = objinput[i].ScreensModel[j].Edit;
                        roleScreen.Delete = objinput[i].ScreensModel[j].Delete;
                        roleScreen.Approve = objinput[i].ScreensModel[j].Approve;
                        roleScreen.Reject = objinput[i].ScreensModel[j].Reject;
                        roleScreen.CreatedBy = objinput[i].ScreensModel[j].ClientId;
                        roleScreen.CreatedOn = DateTime.Now;
                        roleScreen.ClientId = objinput[i].ScreensModel[j].ClientId;
                        roleScreen.ScreenId = objinput[i].ScreensModel[j].ScreenId;
                        roleScreen.RoleId = objinput[i].ScreensModel[j].RoleId;
                        _dbContext.RoleScreens.Add(roleScreen);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            return objinput;
        }

        public async Task<dynamic> GetPingReports(int PageNo, int PageSize, int SiteId, int UserId, string FromDate, string ToDate)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            string subtracthour = _configuration["SubtractHour"];
            //var count = _repositoryPaymentReceived.Count(x => x.IsActive == true && x.IsDeleted == false);
            //  var getall = await _paymentreceivedManager.GetAllList();
            string newtime = string.Empty;
            string starttime1 = string.Empty;
            string SiteName = string.Empty;
            string UserName = string.Empty;
            int id = 0;
            string sitenameobj = string.Empty;
            string sitecode = string.Empty;
            int sitemasterid = 0;
            int siteuserid = 0;
            string username = string.Empty;
            string date = string.Empty;
            List<GetPingReport> list = new List<GetPingReport>();
            // string date = string.Empty;
            AdminBusiness business = new AdminBusiness();

            if (SiteId == null || SiteId == 0)
            {
                SiteId = 0;
                SiteName = "";
            }
            else
            {
                var newsites = _dbContext.Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == SiteId).FirstOrDefault();
                if (newsites != null)
                {
                    SiteName = newsites.SiteName;
                }
            }

            if (UserId == null || UserId == 0)
            {
                UserId = 0;
                UserName = "";
            }
            if (FromDate == "undefined")
            {
                DateTime dt = DateTime.Now;
                FromDate = dt.ToString("yyyy-MM-dd");
                ToDate = "";
            }
            else
            {
                DateTime dt = Convert.ToDateTime(FromDate);
                FromDate = dt.ToString("yyyy-MM-dd");
            }
            if (ToDate == "undefined" && FromDate != "undefined")
            {
                ToDate = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (ToDate == "")
                {
                    DateTime dt = DateTime.Now;
                    ToDate = dt.ToString("yyyy-MM-dd");
                }
                else
                {
                    DateTime dt = Convert.ToDateTime(ToDate);
                    ToDate = dt.ToString("yyyy-MM-dd");
                }
            }
            string strConnection = _configuration["ConnectionStrings:Default"];
            list = new List<GetPingReport>();
            ArrayList array = new ArrayList();
            if (SiteId == 0)
            {
                array.Add("");
            }
            else
            {
                array.Add(SiteId);
            }

            if (UserId == 0)
            {
                array.Add("");
            }
            else
            {
                array.Add(UserId);
            }
            array.Add(FromDate);
            array.Add(ToDate);
            DataSet ds = business.GetSearchPings(array, strConnection);
            string newtime1 = string.Empty;
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DateTime dt = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"]);
                    date = dt.ToString("dd-MM-yyyy");


                    string time = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"]).ToString("HH:mm");

                    if (newtime1 == time)
                    {

                    }
                    else
                    {
                        list.Add(new GetPingReport
                        {
                            Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                            SiteCode = ds.Tables[0].Rows[i]["SiteCode"].ToString(),
                            SiteName = ds.Tables[0].Rows[i]["SiteName"].ToString(),
                            SiteId = Convert.ToInt32(ds.Tables[0].Rows[i]["SiteId"]),
                            RegisterUserId = Convert.ToInt32(ds.Tables[0].Rows[i]["RegisterUserId"]),
                            UserName = ds.Tables[0].Rows[i]["Username"].ToString(),
                            Date = date,
                            Time = time,
                            Status = "Connected"
                        });
                    }
                    newtime1 = Convert.ToDateTime(ds.Tables[0].Rows[i]["Date"]).ToString("HH:mm");
                }
            }

            string status = string.Empty;
            List<GetPingReport> model = new List<GetPingReport>();
            var sites = _dbContext.Sites.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
            if (sites.Count > 0)
            {
                for (int i = 0; i < sites.Count; i++)
                {
                    int siteid = sites[i].Id;
                    var finallist = list.Where(x => x.Id == siteid).ToList();
                    if (finallist.Count > 0)
                    {
                        starttime1 = finallist[0].Time;
                        newtime = finallist[0].Time;
                        id = finallist[0].Id;
                        sitecode = finallist[0].SiteCode;
                        sitenameobj = finallist[0].SiteName;
                        sitemasterid = finallist[0].SiteId;
                        siteuserid = finallist[0].RegisterUserId;
                        username = finallist[0].UserName;
                        date = finallist[0].Date;
                        model.Add(new GetPingReport
                        {
                            Id = finallist[0].Id,
                            SiteCode = finallist[0].SiteCode,
                            SiteName = finallist[0].SiteName,
                            SiteId = finallist[0].SiteId,
                            RegisterUserId = finallist[0].RegisterUserId,
                            UserName = finallist[0].UserName,
                            Date = finallist[0].Date,
                            Time = newtime,
                            Status = "Connected"
                        });
                        for (int j = 1; j < finallist.Count; j++)
                        {

                            DateTime dtim = Convert.ToDateTime(newtime).AddMinutes(10);
                            string dtnw = dtim.ToString("HH:mm");
                            if (finallist[j].Time == dtnw)
                            {
                                newtime = finallist[j].Time;
                                model.Add(new GetPingReport
                                {
                                    Id = finallist[j].Id,
                                    SiteCode = finallist[j].SiteCode,
                                    SiteName = finallist[j].SiteName,
                                    SiteId = finallist[j].SiteId,
                                    RegisterUserId = finallist[j].RegisterUserId,
                                    UserName = finallist[j].UserName,
                                    Date = finallist[j].Date,
                                    Time = finallist[j].Time,
                                    Status = "Connected"
                                });
                            }
                            else
                            {
                                model.Add(new GetPingReport
                                {
                                    Id = finallist[j].Id,
                                    SiteCode = finallist[j].SiteCode,
                                    SiteName = finallist[j].SiteName,
                                    SiteId = finallist[j].SiteId,
                                    RegisterUserId = finallist[j].RegisterUserId,
                                    UserName = finallist[j].UserName,
                                    Date = finallist[j].Date,
                                    Time = dtnw,
                                    Status = "Disconnected"
                                });
                                DateTime dtstart = Convert.ToDateTime(dtnw);
                                DateTime dtend = Convert.ToDateTime(finallist[j].Time);
                                TimeSpan subttot = dtend.Subtract(dtstart);
                                TimeSpan span = new TimeSpan(subttot.Hours, subttot.Minutes, 0);
                                double result = span.TotalMinutes / 10;
                                for (int k = 0; k < result; k++)
                                {

                                    dtim = Convert.ToDateTime(dtstart).AddMinutes(10);
                                    dtnw = dtim.ToString("HH:mm");
                                    if (dtnw == finallist[j].Time)
                                    {
                                        status = "Connected";
                                    }
                                    else
                                    {
                                        status = "Disconnected";
                                    }
                                    model.Add(new GetPingReport
                                    {
                                        Id = finallist[j].Id,
                                        SiteCode = finallist[j].SiteCode,
                                        SiteName = finallist[j].SiteName,
                                        SiteId = finallist[j].SiteId,
                                        RegisterUserId = finallist[j].RegisterUserId,
                                        UserName = finallist[j].UserName,
                                        Date = finallist[j].Date,
                                        Time = dtnw,
                                        Status = status
                                    });
                                    dtstart = Convert.ToDateTime(dtnw);
                                }
                                newtime = dtnw;
                            }
                        }

                        if (FromDate == DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            var timezones = TimeZoneInfo.GetSystemTimeZones();
                            var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                            DateTime ukTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, ukTimeZone);
                            // var ukTimeZone1 = TimeZoneInfo.FindSystemTimeZoneById("GMT Daylight Time");
                            //  DateTime ukTime1 = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, ukTimeZone);
                            DateTime dt = DateTime.Now.AddHours(Convert.ToInt32(subtracthour)).AddMinutes(-DateTime.Now.Minute % 10);
                            DateTime currentdate = DateTime.Now.AddHours(Convert.ToInt32(subtracthour));
                            string currenttime = currentdate.ToString("HH:mm");
                            DateTime dtstart = Convert.ToDateTime(newtime);
                            DateTime dtend = Convert.ToDateTime(currenttime);
                            TimeSpan subttot = dtstart.Subtract(dtend);
                            if (subttot.ToString().Contains('-'))
                            {
                                subttot = dtend.Subtract(dtstart);

                            }
                            if (dt.ToString("HH:mm") != newtime)
                            {
                                TimeSpan span = new TimeSpan(subttot.Hours, subttot.Minutes, 0);
                                double result = (span.TotalMinutes / 10);
                                for (int k = 1; k < result; k++)
                                {

                                    DateTime dtim1 = Convert.ToDateTime(newtime).AddMinutes(10);
                                    string dtnw1 = dtim1.ToString("HH:mm");

                                    status = "Disconnected";

                                    model.Add(new GetPingReport
                                    {
                                        Id = id,
                                        SiteCode = sitecode,
                                        SiteName = sitenameobj,
                                        SiteId = sitemasterid,
                                        RegisterUserId = siteuserid,
                                        UserName = username,
                                        Date = date,
                                        Time = dtnw1,
                                        Status = status
                                    });
                                    //  dtstart = Convert.ToDateTime(dtnw1);
                                    newtime = dtnw1;
                                }
                            }
                        }
                        else
                        {
                            //  DateTime currentdate = Convert.ToDateTime(FromDate);
                            string currenttime = "23:50";
                            DateTime dtstart = Convert.ToDateTime(newtime);
                            DateTime dtend = Convert.ToDateTime(currenttime);
                            TimeSpan subttot = dtend.Subtract(dtstart);
                            TimeSpan span = new TimeSpan(subttot.Hours, (subttot.Minutes) + 10, 0);
                            double result = (span.TotalMinutes / 10);

                            for (int k = 0; k < result; k++)
                            {

                                DateTime dtim1 = Convert.ToDateTime(newtime).AddMinutes(10);
                                string dtnw1 = dtim1.ToString("HH:mm");

                                status = "Disconnected";
                                var lis = model.Where(x => x.Time == dtnw1).FirstOrDefault();
                                if (lis == null)
                                {
                                    model.Add(new GetPingReport
                                    {
                                        Id = id,
                                        SiteCode = sitecode,
                                        SiteName = sitenameobj,
                                        SiteId = sitemasterid,
                                        RegisterUserId = siteuserid,
                                        UserName = username,
                                        Date = date,
                                        Time = dtnw1,
                                        Status = status
                                    });
                                }
                                //  dtstart = Convert.ToDateTime(dtnw1);
                                newtime = dtnw1;
                            }

                        }
                        if (FromDate == DateTime.Now.ToString("yyyy-MM-dd"))
                        {
                            newtime = "00:00";
                            var lis = model.Where(x => x.Time == newtime).FirstOrDefault();
                            if (lis == null)
                            {
                                model.Add(new GetPingReport
                                {
                                    Id = id,
                                    SiteCode = sitecode,
                                    SiteName = sitenameobj,
                                    SiteId = sitemasterid,
                                    RegisterUserId = siteuserid,
                                    UserName = username,
                                    Date = date,
                                    Time = newtime,
                                    Status = "Disconnected"
                                });
                            }
                            DateTime dtstart2 = Convert.ToDateTime("00:00");
                            DateTime dtend2 = Convert.ToDateTime(starttime1);
                            TimeSpan subttot2 = dtend2.Subtract(dtstart2);
                            TimeSpan span1 = new TimeSpan(subttot2.Hours, subttot2.Minutes, 0);
                            double result1 = (span1.TotalMinutes / 10);
                            for (int k = 1; k < result1; k++)
                            {

                                DateTime dtim1 = Convert.ToDateTime(newtime).AddMinutes(10);
                                string dtnw1 = dtim1.ToString("HH:mm");

                                status = "Disconnected";
                                var list1 = model.Where(x => x.Time == dtnw1).FirstOrDefault();
                                if (list1 == null)
                                {
                                    model.Add(new GetPingReport
                                    {
                                        Id = id,
                                        SiteCode = sitecode,
                                        SiteName = sitenameobj,
                                        SiteId = sitemasterid,
                                        RegisterUserId = siteuserid,
                                        UserName = username,
                                        Date = date,
                                        Time = dtnw1,
                                        Status = status
                                    });
                                }
                                //  dtstart = Convert.ToDateTime(dtnw1);
                                newtime = dtnw1;
                            }
                        }
                        else
                        {
                            newtime = "00:00";
                            DateTime dtstart2 = Convert.ToDateTime("00:00");
                            DateTime dtend2 = Convert.ToDateTime(starttime1);
                            TimeSpan subttot2 = dtend2.Subtract(dtstart2);
                            TimeSpan span1 = new TimeSpan(subttot2.Hours, subttot2.Minutes, 0);
                            double result1 = (span1.TotalMinutes / 10);
                            for (int k = 1; k < result1; k++)
                            {

                                DateTime dtim1 = Convert.ToDateTime(newtime).AddMinutes(10);
                                string dtnw1 = dtim1.ToString("HH:mm");

                                status = "Disconnected";
                                var list2 = model.Where(x => x.Time == dtnw1).FirstOrDefault();
                                if (list2 == null)
                                {
                                    model.Add(new GetPingReport
                                    {
                                        Id = id,
                                        SiteCode = sitecode,
                                        SiteName = sitenameobj,
                                        SiteId = sitemasterid,
                                        RegisterUserId = siteuserid,
                                        UserName = username,
                                        Date = date,
                                        Time = dtnw1,
                                        Status = status
                                    });
                                }
                                //  dtstart = Convert.ToDateTime(dtnw1);
                                newtime = dtnw1;
                            }
                        }
                    }

                    else if (FromDate != DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        if (SiteId == sites[i].Id)
                        {
                            var siteuseridobj = _dbContext.RegisterUsers.Where(x => x.SiteId == sites[i].Id).FirstOrDefault();
                            if (siteuseridobj != null)
                            {
                                username =_dbContext.RegisterUsers.Where(x=>x.Id== siteuseridobj.Id).FirstOrDefault().FirstName;
                                newtime = "00:00";

                                for (int k = 0; k < 144; k++)
                                {

                                    DateTime dtim1 = Convert.ToDateTime(newtime).AddMinutes(10);
                                    string dtnw1 = dtim1.ToString("HH:mm");

                                    status = "Disconnected";
                                    var list3 = model.Where(x => x.Time == dtnw1).FirstOrDefault();
                                    if (list3 == null)
                                    {
                                        model.Add(new GetPingReport
                                        {
                                            Id = id,
                                            SiteCode = sites[i].ZatparkSitecode,
                                            SiteName = sites[i].SiteName,
                                            SiteId = sites[i].Id,
                                            RegisterUserId = siteuseridobj.Id,
                                            UserName = username,
                                            Date = FromDate,
                                            Time = dtnw1,
                                            Status = status
                                        });
                                    }
                                    //  dtstart = Convert.ToDateTime(dtnw1);
                                    newtime = dtnw1;
                                }
                            }
                        }
                    }
                    else if (FromDate == DateTime.Now.ToString("yyyy-MM-dd"))
                    {
                        if (SiteId == sites[i].Id)
                        {
                            date = FromDate;
                            DateTime currentdate = DateTime.Now.AddHours(Convert.ToInt32(subtracthour));
                            var ukTimeZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
                            DateTime ukTime = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, ukTimeZone);
                            string currenttime = currentdate.ToString("HH:mm");
                            newtime = "00:00";
                            var list5 = model.Where(x => x.Time == newtime).FirstOrDefault();
                            if (list5 == null)
                            {
                                model.Add(new GetPingReport
                                {
                                    Id = id,
                                    SiteCode = sites[i].ZatparkSitecode,
                                    SiteName = sites[i].SiteName,
                                    SiteId = sites[i].Id,
                                    RegisterUserId = siteuserid,
                                    UserName = username,
                                    Date = date,
                                    Time = newtime,
                                    Status = "Disconnected"
                                });
                            }
                            DateTime dtstart = Convert.ToDateTime(newtime);
                            DateTime dtend = Convert.ToDateTime(currenttime);
                            TimeSpan subttot = dtend.Subtract(dtstart);
                            TimeSpan span = new TimeSpan(subttot.Hours, subttot.Minutes, 0);
                            double result = (span.TotalMinutes / 10);
                            var siteuseridobj =_dbContext.RegisterUsers.Where(x => x.SiteId == sites[i].Id).FirstOrDefault();
                            if (siteuseridobj != null)
                            {
                                username = _dbContext.RegisterUsers.Where(x=>x.Id==siteuseridobj.Id).FirstOrDefault().FirstName;
                                siteuserid = _dbContext.RegisterUsers.Where(x => x.Id == siteuseridobj.Id).FirstOrDefault().Id;
                            }
                            for (int k = 1; k < result; k++)
                            {

                                DateTime dtim1 = Convert.ToDateTime(newtime).AddMinutes(10);
                                string dtnw1 = dtim1.ToString("HH:mm");

                                status = "Disconnected";
                                var list4 = model.Where(x => x.Time == dtnw1).FirstOrDefault();
                                if (list4 == null)
                                {
                                    model.Add(new GetPingReport
                                    {
                                        Id = id,
                                        SiteCode = sites[i].ZatparkSitecode,
                                        SiteName = sites[i].SiteName,
                                        SiteId = sites[i].Id,
                                        RegisterUserId = siteuserid,
                                        UserName = username,
                                        Date = date,
                                        Time = dtnw1,
                                        Status = status
                                    });
                                }
                                //  dtstart = Convert.ToDateTime(dtnw1);
                                newtime = dtnw1;
                            }
                        }
                    }
                }
            }
            
            model = model.OrderByDescending(x => x.Time).ToList();
            int totalitems = model.Count();
            double totalpa = (double)totalitems / (double)PageSize;
            double totalpage = Math.Round(totalpa);
            model[0].TotalItem = totalitems;
            model[0].TotalPage = totalpage;
            return model;
            
        }
    }
}

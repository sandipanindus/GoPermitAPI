using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security;
using System.Security.Permissions;
using System.Threading.Tasks;
using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using LabelPad.Repository.PermissionManagement;
using LabelPad.Repository.RegisterManagement;
using LabelPad.Repository.RoleManagement;
using LabelPad.Repository.SiteManagment;
using LabelPad.Repository.UserManagement;
using LabelPad.Repository.VehicleRegistrationManagement;
using LabelPad.Repository.VisitorParkingManagement;
using LabelPadCoreApi.Migrations;
using LabelPadCoreApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using LabelPad.Repository.ReportManagement;
using LabelPad.Repository.TenantManagement;
using Microsoft.CodeAnalysis.Editing;
using LabelPad.Repository.IndustryManagement;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Azure.Core;
using MailKit.Security;
using MimeKit;
using Microsoft.AspNetCore.Http.HttpResults;

namespace LabelPadCoreApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly ISiteRepository _siteRepository;
        private readonly IRegisterRepository _registerRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUserRepository _userRepository;
        private readonly LabelPadDbContext _dbContext;
        private readonly IConfiguration _configuration;

        private readonly IPermissionRepository _permissionRepository;
        private readonly IVehicleRegistrationRepository _vehicleRegistrationRepository;
        private readonly IVisitorParkingRepository _visitorParkingRepository;
        private readonly IReportRepository _reportRepository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IIndustryRepository _industryRepository;
        public AdminController(ITenantRepository tenantRepository, IReportRepository reportRepository, IVisitorParkingRepository visitorParkingRepository, ISiteRepository siteRepository,
        IVehicleRegistrationRepository vehicleRegistrationRepository,
            IPermissionRepository permissionRepository, IRoleRepository roleRepository, IUserRepository userRepository,
        IRegisterRepository registerRepository, LabelPadDbContext dbContext, IConfiguration configuration, IIndustryRepository industryRepository)
        {
            _registerRepository = registerRepository;
            _dbContext = dbContext;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _userRepository = userRepository;
            _siteRepository = siteRepository;
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
            _visitorParkingRepository = visitorParkingRepository;
            _reportRepository = reportRepository;
            _tenantRepository = tenantRepository;
            _permissionRepository = permissionRepository;
            _industryRepository = industryRepository;
        }

        #region GetSupportList
        /// <summary>
        /// method to get the support list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSupportList")]
        public async Task<IActionResult> GetSupportList()
        {
            try
            {
                //AppLogs.InfoLogs("DeleteRole Method was started,Controller:Admin");
                var result = await _reportRepository.GetSupportList();
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the DeleteRole Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSearchUser
        /// <summary>
        /// method to get the search user
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSearchUser")]
        public async Task<IActionResult> GetSearchUser(int PageNo, int PageSize, string FirstName, string LastName, string Email, string SiteName, int LoginId, int RoleId, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("DeleteRole Method was started,Controller:Admin");
                var result = await _userRepository.GetSearchUsers(PageNo, PageSize, FirstName, LastName, Email, SiteName, LoginId, RoleId, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the DeleteRole Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSearchTenant
        /// <summary>
        /// method to get the search tenant
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSearchTenant")]
        public async Task<IActionResult> GetSearchTenant(int PageNo, int PageSize, string FirstName, string LastName, string Email, string MobileNumber, string SiteName, int SiteId, string VRM)
        {
            try
            {
                //AppLogs.InfoLogs("DeleteRole Method was started,Controller:Admin");
                var result = await _userRepository.GetSearchTenants(PageNo, PageSize, FirstName, LastName, Email, MobileNumber, SiteName, SiteId, VRM);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the DeleteRole Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSearchSite
        /// <summary>
        /// method to get the search site
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSearchSite")]
        public async Task<IActionResult> GetSearchSite(int PageNo, int PageSize, string SiteName, string Email, string MobileNumber, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("DeleteRole Method was started,Controller:Admin");
                var result = await _siteRepository.GetSearchSites(PageNo, PageSize, SiteName, Email, MobileNumber, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the DeleteRole Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSearchZatpark
        /// <summary>
        /// method to get the search zatpark
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSearchZatpark")]
        public async Task<IActionResult> GetSearchZatpark(int PageNo, int PageSize, string Tenant, string Sitename, string BayNo, string Fromdate, string Todate, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("DeleteRole Method was started,Controller:Admin");
                var result = await _siteRepository.GetSearchZatpark(PageNo, PageSize, Tenant, Sitename, BayNo, Fromdate, Todate, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the DeleteRole Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSearchAuditReport
        /// <summary>
        /// method to get the search ping report
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetSearchAuditReport")]
        public async Task<IActionResult> GetSearchAuditReport(int PageNo, int PageSize, string Username, string Sitename, string Fromdate, string Todate, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("DeleteRole Method was started,Controller:Admin");
                var result = await _siteRepository.GetSearchAuditReport(PageNo, PageSize, Username, Sitename, Fromdate, Todate, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the DeleteRole Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetVisitorSlot
        /// <summary>
        /// method to get the support list
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetVisitorSlot")]
        public async Task<IActionResult> GetVisitorSlot(string Id)
        {
            try
            {
                //AppLogs.InfoLogs("DeleteRole Method was started,Controller:Admin");
                var result = await _visitorParkingRepository.GetVisitorSlot(Convert.ToInt32(Id));
                if (result == null)
                {
                    var visitorparktemp = _dbContext.VisitorParkingTemps.Where(x => x.Id == Convert.ToInt32(Id) && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = visitorparktemp });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the DeleteRole Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion


        #region UpdateVisitorSlot
        /// <summary>
        /// method to update the visitorslot
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("UpdateVisitorSlot")]
        public async Task<IActionResult> UpdateVisitorSlot([FromBody] UpdateVisitorSlot objdata)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var visitors = await _visitorParkingRepository.UpdateVisitorSlot(objdata);
                //var result = _dbContext.VisitorParkingTemps
                //      .OrderByDescending(v => v.Id)
                //      .ToList();

                var result = await (from v in _dbContext.VisitorParkingTemps
                                    where v.Id == objdata.Id
                                    select v).FirstOrDefaultAsync();

                var visitorParking = await (from v in _dbContext.VisitorBayNos
                                            where v.Id == objdata.visitorbayid
                                            select v).FirstOrDefaultAsync();

                string Firstname = result.Name;
                string Surname = result.Surname;
                string Email = result.Email;
                string Duration = result.Duration;
                string SessionUnit = result.SessionUnit;
                string VRM= result.VRMNumber;
                string StartDate = Convert.ToString(result.StartDate);
                string EndDate = Convert.ToString(result.EndDate); 
                int siteId = result.SiteId;
                int tenantId = result.RegisterUserId;
                string BayName = visitorParking.BayName;

                var siteDetails = await (from u in _dbContext.Sites
                                         where u.Id == siteId
                                         select u).FirstOrDefaultAsync();

                string SiteName = siteDetails.SiteName;

                // Concatenate with commas, skipping empty/null values safely
                string SiteAddress = string.Join(", ", new[] {
                 siteDetails.SiteAddress,
                 siteDetails.City,
                 siteDetails.State,
                 siteDetails.Zipcode
}           .Where(x => !string.IsNullOrWhiteSpace(x)));

                var User = await (from u in _dbContext.RegisterUsers
                                  where u.Id == tenantId
                                  select u).FirstOrDefaultAsync();

                string tenatEmail = User.Email;

                if (visitors != null)
                {

                    var folderName = Path.Combine("EmailHtml", "DriverConfirm.html");
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    string bayid = "";
                    //var baynos = _dbContext.VisitorBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == Convert.ToInt32(objdata.SiteId) && x.Id == objdata.VisitorBayNoId).FirstOrDefault();
                    //if (baynos != null)
                    //{
                    //    bayid = baynos.BayName;
                    //}
                    StreamReader reader = new StreamReader(filePath);
                    //string[] Date = objdata.Date.Split('T');
                    //DateTime newdate = Convert.ToDateTime(Date[0]);
                    string readFile = reader.ReadToEnd();
                    string myString = "";
                    myString = readFile;
                    myString = myString.Replace("%{#{VRM}#}%", VRM);
                    myString = myString.Replace("%{#{SiteName}#}%", SiteName);
                    myString = myString.Replace("%{#{StartTime}#}%", StartDate);
                    myString = myString.Replace("%{#{EndTime}#}%", EndDate);
                    myString = myString.Replace("%{#{SiteAddress}#}%", SiteAddress);
                    myString = myString.Replace("%{#{BayName}#}%", BayName);

                    string tenantemail = "";
                    //var sites = _dbContext.Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == objdata.SiteId).FirstOrDefault();
                    //var user = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == objdata.TenantId).FirstOrDefault();
                    //if (user != null)
                    //{
                    //    tenantemail = user.Email;

                    //}
                    // myString = myString.Replace("%{#{BayNo}#}%", bayid);
                    string body = myString;
                    bool key = await _userRepository.SendEmailAsync(Email, Firstname, "Confirmation Complete ", body, "GOPERMIT_Slot Booking Confirmation");

                    string myString1 = "";
                    
                        var folderName1 = Path.Combine("EmailHtml", "TenantConfirm.html");
                        var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), folderName1);
                        StreamReader reader1 = new StreamReader(filePath1);
                        string readFile1 = reader1.ReadToEnd();


                        myString1 = readFile1;
                        myString1 = myString1.Replace("%{#{Firstname}#}%", Firstname);
                        myString1 = myString1.Replace("%{#{Surname}#}%", Surname);
                        myString1 = myString1.Replace("%{#{Email}#}%", Email);
                        myString1 = myString1.Replace("%{#{Duration}#}%", Duration + " " + SessionUnit);

                        string bodyTenant = myString1;
                        key = await _userRepository.SendEmailAsync(tenatEmail, Firstname, "Confirmation Complete ", bodyTenant, "GOPERMIT_Slot Booking Confirmation");

                    



                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = objdata });
                }


                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion


        #region AddVisitor
        /// <summary>
        /// method to add the visitor
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("AddVisitor")]
        public async Task<IActionResult> AddVisitor([FromBody] AddVisitorParkingAc objdata)
        {
            try
            {
                //AppLogs.InfoLogs("AddSite Method was started,Controller:Admin");
                //if (_visitorParkingRepository.GetExistsVehicleParking(objdata))
                //{
                //    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Already Exists with that email or mobile number", Result = objdata });
                //}
                //else
                //{
                var result = await _visitorParkingRepository.AddVisitorParking(objdata);
                if (result != null)
                {

                    var folderName = Path.Combine("EmailHtml", "VisitorParking.html");
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    string bayid = "";
                    var baynos = _dbContext.VisitorBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == Convert.ToInt32(objdata.SiteId) && x.Id == objdata.VisitorBayNoId).FirstOrDefault();
                    if (baynos != null)
                    {
                        bayid = baynos.BayName;
                    }
                    StreamReader reader = new StreamReader(filePath);
                    string[] Date = objdata.Date.Split('T');
                    DateTime newdate = Convert.ToDateTime(Date[0]);
                    string readFile = reader.ReadToEnd();



                    string myString = "";
                    myString = readFile;
                    string dt = newdate.ToString("dd/MM/yyyy");
                    myString = myString.Replace("%{#{VRM}#}%", objdata.VRM);
                    myString = myString.Replace("%{#{Firstname}#}%", objdata.Name);
                    myString = myString.Replace("%{#{Surname}#}%", objdata.Surname);
                    myString = myString.Replace("%{#{Email}#}%", objdata.Email);
                    myString = myString.Replace("%{#{Contact}#}%", objdata.MobileNumber);
                    myString = myString.Replace("%{#{Date}#}%", dt);
                    myString = myString.Replace("%{#{Duration}#}%", objdata.Duration + " " + objdata.SessionUnit);
                    myString = myString.Replace("%{#{Fromtime}#}%", objdata.StartTime);
                    myString = myString.Replace("%{#{Totime}#}%", objdata.EndTime);
                    string returnlink = _configuration["VisitorConfirm"];
                    var resetLink = returnlink + objdata.Id;
                    myString = myString.Replace("%{#{ConfirmLink}#}%", resetLink);



                    string tenantemail = "";
                    string sitename = "";
                    var sites = _dbContext.Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == objdata.SiteId).FirstOrDefault();
                    if (sites != null)
                    {
                        sitename = sites.SiteName;
                    }

                    myString = myString.Replace("%{#{PropertyName}#}%", sitename);
                    // myString = myString.Replace("%{#{BayNo}#}%", bayid);
                    string body = myString;
                    bool key = await SendEmail(objdata.Email, objdata.Name, "Verify and Confirm Your Booking Details ", body, tenantemail, objdata.cctome, "GOPERMIT_Slot Booking");
                    string myString1 = "";

                    if (objdata.cctome == true)
                    {
                        var folderName1 = Path.Combine("EmailHtml", "VisitorparkingCC.html");
                        var filePath1 = Path.Combine(Directory.GetCurrentDirectory(), folderName1);
                        StreamReader reader1 = new StreamReader(filePath1);
                        string readFile1 = reader1.ReadToEnd();
                        var user = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == objdata.TenantId).FirstOrDefault();
                        if (user != null)
                        {
                            tenantemail = user.Email;

                        }


                        myString1 = readFile1;
                        myString1 = myString1.Replace("%{#{VRM}#}%", objdata.VRM);
                        myString1 = myString1.Replace("%{#{Firstname}#}%", objdata.Name);
                        myString1 = myString1.Replace("%{#{Surname}#}%", objdata.Surname);
                        myString1 = myString1.Replace("%{#{Email}#}%", objdata.Email);
                        myString1 = myString1.Replace("%{#{Contact}#}%", objdata.MobileNumber);
                        myString1 = myString1.Replace("%{#{Date}#}%", dt);
                        myString1 = myString1.Replace("%{#{Duration}#}%", objdata.Duration + " " + objdata.SessionUnit);
                        myString1 = myString1.Replace("%{#{Fromtime}#}%", objdata.StartTime);
                        myString1 = myString1.Replace("%{#{Totime}#}%", objdata.EndTime);
                        myString1 = myString1.Replace("%{#{PropertyName}#}%", sitename);

                        string bodycc = myString1;
                        key = await SendEmail(tenantemail, objdata.Name, "Verify and Confirm Your Booking Details ", bodycc, tenantemail, objdata.cctome, "GOPERMIT_Slot Booking");

                    }



                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = objdata });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = objdata });
                }
                // }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the AddSite Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        #endregion

        #region UpdateVisitorParking
        /// <summary>
        /// method to update the visitor parking
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("UpdateVisitorParking")]
        public async Task<IActionResult> UpdateVisitorParking([FromBody] AddVisitorParkingAc objdata)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var visitors = await _visitorParkingRepository.UpdateVisitorParking(objdata);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetVisitorParkingById
        /// <summary>
        /// method to get the visitorparking by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetVisitorParkingById")]
        public async Task<IActionResult> GetVisitorParkingById(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var visitors = await _visitorParkingRepository.GetVisitorParkingById(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        #endregion

        #region GetVisitorParkingBysiteId
        /// <summary>
        /// method to get the visitorparking by siteid
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetVisitorParkingBysiteId")]
        public async Task<IActionResult> GetVisitorParkingBysiteId(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var visitors = await _visitorParkingRepository.GetVisitorParkingBysiteId(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetVisitordetailsById
        /// <summary>
        /// method to get the visitor details
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetVisitordetailsById")]
        public async Task<IActionResult> GetVisitordetailsById(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var visitors = await _visitorParkingRepository.GetVisitordetailsById(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetVisitorParkingBysiteIdanddate
        /// <summary>
        /// method to get the visitor parking siteid and date
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet("GetVisitorParkingBysiteIdanddate")]
        public async Task<IActionResult> GetVisitorParkingBysiteIddate(int Id, string date)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var visitors = await _visitorParkingRepository.GetVisitorParkingBysiteIddate(Id, date);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetVisitorBayNoTime
        /// <summary>
        /// method to get the visitorbayno time
        /// </summary>
        /// <param name="starttime"></param>
        /// <param name="siteid"></param>
        /// <returns></returns>
        [HttpGet("GetVisitorBayNoTime")]
        public async Task<IActionResult> GetVisitorBayNoTime(string starttime, string siteid, string date)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var visitors = await _siteRepository.GetVisitorBayNo(starttime, siteid, date);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetVisitorBayNoEdit
        /// <summary>
        /// method to get the visitor bayno edit
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet("GetVisitorBayNoEdit")]
        public async Task<IActionResult> GetVisitorBayNoEdit(int SiteId, int UserId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var visitorbaynos = await _visitorParkingRepository.GetVisitorBayNosEdit(SiteId, UserId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitorbaynos });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetVisitorBayNo
        /// <summary>
        /// method to get the visitor bayno
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetVisitorBayNo")]
        public async Task<IActionResult> GetVisitorBayNo(int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var visitorbaynos = await _visitorParkingRepository.GetVisitorBayNos(SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitorbaynos });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetParkingBayNo
        /// <summary>
        /// method to get the parking bayno
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetParkingBayNo")]
        public async Task<IActionResult> GetParkingBayNo(int SiteId, string Date, string EndDate)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var parkingbaynos = await _siteRepository.GetParkingBayNos(SiteId, Date, EndDate);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = parkingbaynos });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetParkingBayNobysiteid
        /// <summary>
        /// method to get the Parkingbayno by siteid
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetParkingBayNobysiteid")]
        public async Task<IActionResult> GetParkingBayNobysiteid(int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var parkingbaynos = await _siteRepository.GetParkingBayNobysiteid(SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = parkingbaynos });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetParkingBayNoEdit
        /// <summary>
        /// method to get the ParkingBayNo edit
        /// </summary>
        /// <param name="SiteId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet("GetParkingBayNoEdit")]
        public async Task<IActionResult> GetParkingBayNoEdit(int SiteId, int UserId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var parkingbaynos = await _siteRepository.GetParkingBayNosEdit(SiteId, UserId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = parkingbaynos });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetVisitorParkings
        /// <summary>
        /// method to get the visitor parkings
        /// </summary>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        [HttpGet("GetVisitorParkings")]
        public async Task<IActionResult> GetVisitorParkings(int TenantId)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var visitors = await _visitorParkingRepository.GetVisitorParkings(TenantId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetTimeSlots
        /// <summary>
        /// method to get the time slots for visitor
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="sessionunit"></param>
        /// <param name="date"></param>
        /// <param name="siteid"></param>
        /// <returns></returns>
        [HttpGet("GetTimeSlots")]
        public async Task<IActionResult> GetTimeSlots(string duration, string sessionunit, DateTime date, string siteid)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var sessions = await _siteRepository.BindTimeSlots(duration, sessionunit, date, siteid);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = sessions });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetVisitorSessions
        /// <summary>
        /// method to get the visitor sessions
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetVisitorSessions")]
        public async Task<IActionResult> GetVisitorSessions(int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var sessions = await _siteRepository.GetVisitorBaySessions(SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = sessions });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetVehicleDetails
        /// <summary>
        /// method to get the vehicle details
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetVehicleDetails")]
        public async Task<IActionResult> GetVehicleDetails()
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var vehicles = await _vehicleRegistrationRepository.GetVehicleDetails();
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = vehicles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetTenantsBySite
        /// <summary>
        /// method to get the tenants by site
        /// </summary>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetTenantsBySite")]
        public async Task<IActionResult> GetTenantsBySite(int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var users = await _vehicleRegistrationRepository.GetTenantsBySite(SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = users });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region AddVehicles
        /// <summary>
        /// method to add the vehicle
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("AddVehicles")]
        public async Task<IActionResult> AddVehicles([FromBody] List<AddVehicleRegistrationAc> objdata)
        {
            try
            {


                var result = await _vehicleRegistrationRepository.AddVehicles(objdata);
                if (result != null)
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = objdata });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = objdata });
                }

            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the AddSite Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region VisitorParkingDelete
        /// <summary>
        /// methdod to delete the visitorparking
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("VisitorParkingDelete")]
        public async Task<IActionResult> VisitorParkingDelete(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("SiteDelete Method was started,Controller:Admin");
                var result = await _visitorParkingRepository.DeleteVisitorParking(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the SiteDelete Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region ManageParkingDelete
        /// <summary>
        /// methdod to delete the manageparking
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("ManageParkingDelete")]
        public async Task<IActionResult> ManageParkingDelete(int Id, int bayno)
        {
            try
            {
                //AppLogs.InfoLogs("SiteDelete Method was started,Controller:Admin");
                var result = await _siteRepository.DeleteManageParking(Id, bayno);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the SiteDelete Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region SiteDelete
        /// <summary>
        /// method to delete the site
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("SiteDelete")]
        public async Task<IActionResult> SiteDelete(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("SiteDelete Method was started,Controller:Admin");
                var result = await _siteRepository.DeleteSite(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the SiteDelete Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSiteById
        /// <summary>
        /// method to get the site by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetSiteById")]
        public async Task<IActionResult> GetSiteById(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetSiteById Method was started,Controller:Admin");
                var sites = await _siteRepository.GetSiteById(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = sites });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSiteById Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion


        #region GetSites
        /// <summary>
        /// method to get the sites
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="LoginId"></param>
        /// <param name="RoleId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetSites")]
        public async Task<IActionResult> GetSites(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var sites = await _siteRepository.GetSites(PageNo, PageSize, LoginId, RoleId, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = sites });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        [HttpGet("GetSitesbyoperatorid")]
        public async Task<IActionResult> GetSitesbyoperatorid(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId, int OperatorId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var sites = await _siteRepository.GetSitesbyoperatorid(PageNo, PageSize, LoginId, RoleId, SiteId, OperatorId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = sites });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }


        #region GetZatparkLogs
        /// <summary>
        /// method to get the zatpark logs
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="LoginId"></param>
        /// <param name="RoleId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetZatparkLogs")]
        public async Task<IActionResult> GetZatparkLogs(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var logs = await _siteRepository.GetZatParkLogs(PageNo, PageSize, LoginId, RoleId, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = logs });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSitesbylogin
        /// <summary>
        /// method to get the sites by login
        /// </summary>
        /// <param name="LoginId"></param>
        /// <returns></returns>
        [HttpGet("GetSitesbylogin")]
        public async Task<IActionResult> GetSiteslogin(int LoginId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var sites = await _siteRepository.GetSiteslogin(LoginId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = sites });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        #endregion


        #region UpdateSite
        /// <summary>
        /// method to update the site
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("UpdateSite")]
        public async Task<IActionResult> UpdateSite([FromBody] AddSiteAc objdata)
        {
            try
            {
                //AppLogs.InfoLogs("UpdateSite Method was started,Controller:Admin");
                var result = await _siteRepository.UpdateSite(objdata);
                if (result != null)
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = objdata });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = objdata });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the UpdateSite Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion


        #region auditlog
        /// <summary>
        /// method to update the site
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("auditlog")]
        public async Task<IActionResult> saveauditlog([FromBody] Auditlog objdata)
        {
            try
            {
                //AppLogs.InfoLogs("UpdateSite Method was started,Controller:Admin");
                var result = await _siteRepository.saveauditlog(objdata);
                if (result != null)
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = objdata });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = objdata });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the UpdateSite Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region saveauditlogfornotification
        /// <summary>
        /// method to update the notification count
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("saveauditlogfornotification")]
        public async Task<IActionResult> saveauditlogfornotification([FromBody] Auditlog objdata)
        {
            try
            {
                //AppLogs.InfoLogs("UpdateSite Method was started,Controller:Admin");
                var result = await _siteRepository.saveauditlogfornotification(objdata);
                if (result != null)
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = objdata });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = objdata });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the UpdateSite Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetAuditLogs
        /// <summary>
        /// method to get the audit logs
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="LoginId"></param>
        /// <param name="RoleId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetAuditLogs")]
        public async Task<IActionResult> GetAuditLogs(int PageNo, int PageSize, int RoleId, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var logs = await _siteRepository.GetAuditLogs(PageNo, PageSize, RoleId, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = logs });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetTenantLogs
        /// <summary>
        /// method to get the audit logs
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="TenantId"></param>AddTenantUseruploads
        /// <returns></returns>
        [HttpGet("GetTenantLogs")]
        public async Task<IActionResult> GetTenantLogs(int PageNo, int PageSize, int SiteId, int TenantId)
        {
            try
            {

                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var NotificationUpdates = await _visitorParkingRepository.UpdateNotification(TenantId);
                //if (NotificationUpdates.Wait(TimeSpan.FromSeconds(10)))
                //    return NotificationUpdates.Result;
                //else
                //    throw new Exception("Timed out");

                //if (NotificationUpdates.AsyncWaitHandle.WaitOne(10000))
                //    Console.WriteLine("Method successful.");
                //else
                //    Console.WriteLine("Method timed out.");



                var logs = await _siteRepository.GetTenantLogs(PageNo, PageSize, SiteId, TenantId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = logs });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetNotificatiosList
        /// <summary>
        /// method to get the notification count
        /// </summary>
        /// <param name="TenantId"></param>
        /// <returns></returns>
        [HttpGet("GetNotificatiosList")]
        public async Task<IActionResult> GetNotificatiosList(int TenantId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSites Method was started,Controller:Admin");
                var logs = await _siteRepository.GetNotificatiosList(TenantId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = logs });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion


        #region AddSite
        /// <summary>
        /// method to add the site
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("AddSite")]
        public async Task<IActionResult> AddSite([FromBody] AddSiteAc objdata)
        {
            try
            {
                //AppLogs.InfoLogs("AddSite Method was started,Controller:Admin");
                if (_siteRepository.GetExistsSite(objdata))
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Already Exists with that site name", Result = objdata });
                }
                else
                {
                    var result = await _siteRepository.AddSite(objdata);
                    if (result != null)
                    {
                        return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = objdata });
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = objdata });
                    }
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the AddSite Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion




        #region UpdateUserProfile
        /// <summary>
        /// method to update the user profile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("UpdateUserProfile")]
        public async Task<IActionResult> UpdateUserProfile([FromForm(Name = "fileupload")] IFormFile file)
        {
            try
            {
                string filefolder = _configuration["ProfileUrl"];
                var folderName = Path.Combine("ProfileImages");
                UpdateRegisterUser objinput = new UpdateRegisterUser();
                if (file == null)
                {
                    objinput.FirstName = Request.Form["FirstName"].ToString();
                    objinput.LastName = Request.Form["LastName"].ToString();
                    objinput.Address1 = Request.Form["Address1"].ToString();
                    objinput.Address2 = Request.Form["Address2"].ToString();
                    objinput.State = Request.Form["State"].ToString();
                    objinput.City = Request.Form["City"].ToString();
                    objinput.Zipcode = Request.Form["Zipcode"].ToString();
                    objinput.MobileNumber = Request.Form["MobileNumber"].ToString();
                    objinput.Email = Request.Form["Email"].ToString();
                    objinput.Id = Convert.ToInt32(Request.Form["Id"]);
                    objinput.ProfilePath = null;
                }
                else
                {


                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }
                    var uniqueFileName = file.FileName;


                    using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    objinput.FirstName = Request.Form["FirstName"].ToString();
                    objinput.LastName = Request.Form["LastName"].ToString();
                    objinput.Address1 = Request.Form["Address1"].ToString();
                    objinput.Address2 = Request.Form["Address2"].ToString();
                    objinput.State = Request.Form["State"].ToString();
                    objinput.City = Request.Form["City"].ToString();
                    objinput.Zipcode = Request.Form["Zipcode"].ToString();
                    objinput.MobileNumber = Request.Form["MobileNumber"].ToString();
                    objinput.Email = Request.Form["Email"].ToString();
                    objinput.Id = Convert.ToInt32(Request.Form["Id"]);
                    objinput.ProfilePath = filefolder + uniqueFileName;
                }



                var result = _userRepository.UpdateUserProfile(objinput);
                if (result != null)
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Error occured", Result = null });
                }

            }
            catch (Exception ex)
            {
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region UpdateProfile
        /// <summary>
        /// method to update the tenant profile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromForm(Name = "fileupload")] List<IFormFile> files)
        {
            try
            {
                UpdateRegisterUserAc objinput = new UpdateRegisterUserAc();
                string filefolder = string.Empty;
                var folderName = string.Empty;
                string filePath = string.Empty;

                var uniqueFileName = "";


                Guid guid;
                //if (files.Count > 0)
                //{
                // for (int i = 0; i < files.Count; i++)
                // {
                // if ()
                // {
                var file = Request.Form.Files["fileupload"];
                if (file != null)
                {
                    filefolder = _configuration["ProfileUrl"];
                    folderName = Path.Combine("ProfileImages");
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    guid = Guid.NewGuid();
                    uniqueFileName = guid + "-" + file.FileName;
                    objinput.ProfilePath = filefolder + uniqueFileName;

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                file = Request.Form.Files["fileupload1"];
                if (file != null)
                {
                    filefolder = _configuration["TenantResidencyPath"];
                    folderName = Path.Combine("TenantResidencyFiles");
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    guid = Guid.NewGuid();
                    uniqueFileName = guid + "-" + file.FileName;
                    objinput.ResidencyProof = filefolder + uniqueFileName;

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                file = Request.Form.Files["fileupload2"];
                if (file != null)
                {
                    filefolder = _configuration["IdentityProofPath"];
                    folderName = Path.Combine("TenantIdentityProofFiles");
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    guid = Guid.NewGuid();
                    uniqueFileName = guid + "-" + file.FileName;
                    objinput.IdentityProof = filefolder + uniqueFileName;

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }






                // }


                // }




                //}


                objinput.FirstName = Request.Form["FirstName"].ToString();
                objinput.LastName = Request.Form["LastName"].ToString();
                objinput.Address = Request.Form["Address"].ToString();
                objinput.State = Request.Form["State"].ToString();
                objinput.City = Request.Form["City"].ToString();
                objinput.Zipcode = Request.Form["Zipcode"].ToString();
                objinput.MobileNumber = Request.Form["MobileNumber"].ToString();
                objinput.Email = Request.Form["Email"].ToString();
                objinput.Id = Convert.ToInt32(Request.Form["Id"]);
                //objinput.ProfilePath = filefolder + uniqueFileName;

                var result = _userRepository.UpdateProfile(objinput);
                if (result != null)
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Error occured", Result = null });
                }

            }
            catch (Exception ex)
            {
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion



        #region SavePermissionsData
        /// <summary>
        /// method to save the previliges
        /// </summary>
        /// <param name="objinput"></param>
        /// <returns></returns>
        [HttpPost("SavePermissionsData")]
        public async Task<IActionResult> SavePermissionsData([FromBody] List<GetModulesAc> objinput)
        {
            try
            {
                //AppLogs.InfoLogs("SavePermissionsData Method was started,Controller:Admin");
                var result = await _permissionRepository.SavePermissionData(objinput);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the SavePermissionsData Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetModulesScreens
        /// <summary>
        /// method to get the module screens
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpGet("GetModulesScreens")]
        public async Task<IActionResult> GetModulesScreens(int RoleId)
        {
            try
            {
                //AppLogs.InfoLogs("GetModulesScreens Method was started,Controller:Admin");
                var result = await _permissionRepository.GetModuleScreens(RoleId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetModulesScreens Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetScreens
        /// <summary>
        /// method to get the screens
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="ClientId"></param>
        /// <param name="LoginId"></param>
        /// <returns></returns>
        [HttpGet("GetScreens")]
        public async Task<IActionResult> GetScreens(int RoleId, int ClientId, int LoginId)
        {
            try
            {
                //AppLogs.InfoLogs("GetScreens Method was started,Controller:Admin");
                var result = await _permissionRepository.GetScreens(RoleId, ClientId, LoginId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetScreens Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region AddUser
        /// <summary>
        /// method to add the user
        /// </summary>
        /// <param name="addUser"></param>
        /// <returns></returns>
        [HttpPost("AddUser")]
        public async Task<IActionResult> AddUser([FromBody] AddUserAc addUser)
        {
            try
            {
                //AppLogs.InfoLogs("AddUser Method was started,Controller:Admin");
                if (_userRepository.GetExistsUser(addUser))
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Already Exists with that email or contact number", Result = addUser });
                }
                else
                {
                    var result = await _userRepository.AddUser(addUser);
                    if (result != null)
                    {
                        string returnlink = _configuration["SetPasswordUrl"];
                        var folderName = Path.Combine("EmailHtml", "SetPassword.html");
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        StreamReader reader = new StreamReader(filePath);
                        var resetLink = returnlink + addUser.EmailCode;
                        string readFile = reader.ReadToEnd();
                        string myString = "";
                        myString = readFile;
                        string dt = DateTime.Now.ToString("dd-MM-yyyy");
                        myString = myString.Replace("%{#{Datetime}#}%", dt);
                        myString = myString.Replace("%{#{Name}#}%", addUser.FirstName );
                        myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                        string body = myString;
                        bool key = await _userRepository.SendEmailAsync(addUser.Email, addUser.FirstName, "Set Password from Go permit", body, "GOPERMIT_Set Password");
                        if (key == true)
                        {
                            return Ok(new ApiServiceResponse() { Status = "200", Message = "Check your mail for reset password link", Result = null });
                        }
                        else
                        {
                            return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
                        }
                        //  return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = addUser });
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = addUser });
                    }
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the AddUser Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }


        [HttpPost("AddOperatorUser")]
        public async Task<IActionResult> AddOperatorUser([FromBody] AddUserAc addUser)
        {
            try
            {
                //AppLogs.InfoLogs("AddUser Method was started,Controller:Admin");
                if (_userRepository.GetExistsUser(addUser))
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Already Exists with that email or contact number", Result = addUser });
                }
                else
                {
                    var result = await _userRepository.AddUser(addUser);
                    if (result != null)
                    {
                        string returnlink = _configuration["SetPasswordUrl"];
                        var folderName = Path.Combine("EmailHtml", "OperatorSetPassword.html");
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        StreamReader reader = new StreamReader(filePath);
                        var resetLink = returnlink + addUser.EmailCode;
                        string readFile = reader.ReadToEnd();
                        string myString = "";
                        myString = readFile;
                        string dt = DateTime.Now.ToString("dd-MM-yyyy");
                        myString = myString.Replace("%{#{Datetime}#}%", dt);
                        myString = myString.Replace("%{#{Name}#}%", addUser.FirstName);
                        myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                        string body = myString;
                        bool key = await _userRepository.SendEmailAsync(addUser.Email, addUser.FirstName, "Set Password from Go permit", body, "GOPERMIT_Set Password");
                        if (key == true)
                        {
                            return Ok(new ApiServiceResponse() { Status = "200", Message = "Check your mail for reset password link", Result = null });
                        }
                        else
                        {
                            return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
                        }
                        //  return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = addUser });
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = addUser });
                    }
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the AddUser Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion
        #region Zatparklog
        [HttpPost("Zatparklog")]
        public async Task<IActionResult> Zatparklog(ZatparkRequest zatpark)
        {
            try
            {
                _userRepository.whitelistvehicle(zatpark.SiteId, zatpark.VRM);
                //_userRepository.Cancelwhitelistvehicle(zatpark.SiteId, zatpark.VRM);

            }
            catch (Exception ex)
            {
                return BadRequest(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = zatpark });
            }

            return Ok(new ApiServiceResponse() { Status = "200", Message = "Check your mail for reset password link", Result = zatpark });
        }
        #endregion

        #region AddTenantUser
        /// <summary>
        /// method to add the tenant user
        /// </summary>
        /// <param name="addUser"></param>
        /// <returns></returns>
        [HttpPost("AddTenantUser")]
        public async Task<IActionResult> AddTenantUser([FromBody] AddTenantUser addUser)
        {
            try
            {
                //AppLogs.InfoLogs("AddUser Method was started,Controller:Admin");
                if (_userRepository.GetExistsTenantUser(addUser))
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Already Exists with that email or contact number", Result = addUser });
                }
                else
                {
                    var result = await _userRepository.AddTenantUser(addUser);
                    if (result != null)
                    {
                        string returnlink = _configuration["TenantSetPasswordUrl"];
                        var folderName = Path.Combine("EmailHtml", "SetPassword.html");
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                        StreamReader reader = new StreamReader(filePath);
                        var resetLink = returnlink + addUser.EmailCode;
                        string readFile = reader.ReadToEnd();
                        string myString = "";
                        myString = readFile;
                        string dt = DateTime.Now.ToString("MM.dd.yyyy");
                        myString = myString.Replace("%{#{Datetime}#}%", dt);
                        myString = myString.Replace("%{#{Name}#}%", addUser.FirstName + " " + addUser.LastName);

                        myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                        string body = myString;
                        bool key = await _userRepository.SendEmailAsync(addUser.Email, addUser.FirstName, "Set Password from Go Permit", body, "GOPERMIT_Set Password");
                        if (key == true)
                        {
                            return Ok(new ApiServiceResponse() { Status = "200", Message = "Check your mail for reset password link", Result = addUser });
                        }
                        else
                        {
                            return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
                        }
                        //  return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = addUser });
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = addUser });
                    }
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the AddUser Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region AddTenantUseruploads
        /// <summary>
        /// method to update the tenant profile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost("AddTenantUseruploads")]
        public async Task<IActionResult> AddTenantUseruploads([FromForm(Name = "fileupload")] List<IFormFile> files)
        {
            try
            {
                UpdateRegisterUserAc objinput = new UpdateRegisterUserAc();
                string filefolder = string.Empty;
                var folderName = string.Empty;
                string filePath = string.Empty;

                var uniqueFileName = "";


                Guid guid;


                var file = Request.Form.Files["fileupload1"];
                if (file != null)
                {
                    filefolder = _configuration["TenantResidencyPath"];
                    folderName = Path.Combine("TenantResidencyFiles");
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    guid = Guid.NewGuid();
                    uniqueFileName = guid + "-" + file.FileName;
                    objinput.ResidencyProof = filefolder + uniqueFileName;

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }
                file = Request.Form.Files["fileupload2"];
                if (file != null)
                {
                    filefolder = _configuration["IdentityProofPath"];
                    folderName = Path.Combine("TenantIdentityProofFiles");
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    guid = Guid.NewGuid();
                    uniqueFileName = guid + "-" + file.FileName;
                    objinput.IdentityProof = filefolder + uniqueFileName;

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }


                    using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }


                objinput.Id = Convert.ToInt32(Request.Form["Id"]);
                //objinput.ProfilePath = filefolder + uniqueFileName;

                var result = await _userRepository.UpdateProfileUploads(objinput);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        [HttpPost("AddOperatorLogo")]
        public async Task<IActionResult> AddOperatorLogo([FromForm(Name = "fileupload")] List<IFormFile> files)
        {
            try
            {
                OperatorLogoRequest objinput = new OperatorLogoRequest();
                string filefolder = string.Empty;
                var folderName = string.Empty;
                string filePath = string.Empty;
                var uniqueFileName = "";
                Guid guid;

                var file = Request.Form.Files["fileupload"];
                if (file != null)
                {
                    filefolder = _configuration["OperatorLogoPath"];
                    folderName = Path.Combine("OperatorLogoFiles");
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    guid = Guid.NewGuid();
                    uniqueFileName = guid + "-" + file.FileName;
                    objinput.OperatorLogo = filefolder + uniqueFileName;

                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                }

                objinput.Id = Convert.ToInt32(Request.Form["Id"]);

                var result = await _userRepository.UpdateOperatorLogoUploads(objinput);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        #endregion
        public class ApproveTenantRequest
        {
            public int Id { get; set; }
            public bool IsApproved { get; set; }
        }
        #region ApproveTenant
        /// <summary>
        /// method to update the tenant status
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("ApproveTenant")]
        public async Task<IActionResult> ApproveTenant(ApproveTenantRequest request)
        {
            try
            {
                var user = _dbContext.RegisterUsers
                                    .Where(x => !x.IsDeleted && x.Id == request.Id)
                                    .FirstOrDefault();
                if (user == null)
                {
                    return Ok(new ApiServiceResponse()
                    {
                        Status = "404",
                        Message = "Failure",
                        Result = "User not found"
                    });
                }
                user.IsApproved = request.IsApproved;
                user.IsActive = true;
                user.UpdatedOn = DateTime.Now;
                _dbContext.RegisterUsers.Update(user);
                await _dbContext.SaveChangesAsync();

                var folderName = Path.Combine("EmailHtml", "ApproveDocuments.html");
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                StreamReader reader = new StreamReader(filePath);
                //var resetLink = returnlink + addUser.EmailCode;
                string readFile = reader.ReadToEnd();
                string myString = "";
                myString = readFile;
                string dt = DateTime.Now.ToString("MM.dd.yyyy");
                myString = myString.Replace("%{#{Datetime}#}%", dt);
                myString = myString.Replace("%{#{Name}#}%", user.FirstName + " " + user.LastName);
                //myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                string body = myString;

                bool emailSent = await _userRepository.SendEmailAsync(
                    user.Email,
                    user.FirstName,
                    "Approved Tenant from Go Permit",
                    body,
                    "GOPERMIT_Approved Tenant"
                );

                string approvalMessage = request.IsApproved
                    ? "Tenant has been approved successfully."
                    : "Tenant approval has been rejected.";

                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = approvalMessage });
            }
            catch (Exception ex)
            {
                return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = ex.Message });
            }
        }

        #endregion

        #region AddBulkTenants
        /// <summary>
        /// method to add the bulk tanants
        /// </summary>
        /// <param name="addUser"></param>
        /// <returns></returns>
        [HttpPost("AddBulkTenants")]
        public async Task<IActionResult> AddBulkTenants([FromBody] List<AddTenantUser> addUser)
        {
            try
            {
                //AppLogs.InfoLogs("AddUser Method was started,Controller:Admin");
                for (int i = 0; i < addUser.Count; i++)
                {
                    if (_userRepository.GetExistsTenantUser(addUser[i]))
                    {
                        // return Ok(new ApiServiceResponse() { Status = "-100", Message = "Already Exists with that email or contact number", Result = addUser });
                    }
                    else
                    {
                        var result = await _userRepository.AddTenantUser(addUser[i]);
                        if (result != null)
                        {
                            string returnlink = _configuration["TenantSetPasswordUrl"];
                            var folderName = Path.Combine("EmailHtml", "SetPassword.html");
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                            StreamReader reader = new StreamReader(filePath);
                            var resetLink = returnlink + addUser[i].EmailCode;
                            string readFile = reader.ReadToEnd();
                            string myString = "";
                            myString = readFile;
                            string dt = DateTime.Now.ToString("MM.dd.yyyy");
                            myString = myString.Replace("%{#{Datetime}#}%", dt);
                            myString = myString.Replace("%{#{Name}#}%", addUser[i].FirstName + " " + addUser[i].LastName);
                            myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                            string body = myString;
                            bool key = await _userRepository.SendEmailAsync(addUser[i].Email, addUser[i].FirstName, "Set Password from Go Permit", body, "GOPERMIT_Set Password");
                            //if (key == true)
                            //{
                            //    return Ok(new ApiServiceResponse() { Status = "200", Message = "Check your mail for reset password link", Result = null });
                            //}
                            //else
                            //{
                            //    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
                            //}

                        }
                        else
                        {
                            return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = addUser });
                        }
                    }
                }

                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = addUser });
                // return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the AddUser Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region AddRole
        /// <summary>
        /// method to add the Role
        /// </summary>
        /// <param name="addRole"></param>
        /// <returns></returns>
        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleAc addRole)
        {
            try
            {
                //AppLogs.InfoLogs("AddRole Method was started,Controller:Admin");
                if (_roleRepository.GetExistsRole(addRole))
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Already Exists with that role name", Result = addRole });
                }
                else
                {
                    var result = await _roleRepository.AddRole(addRole);
                    if (result != null)
                    {
                        return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = addRole });
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = addRole });
                    }
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the AddRole Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region UpdateTenantUser
        /// <summary>
        /// method to update the tenantuser
        /// </summary>
        /// <param name="addUser"></param>
        /// <returns></returns>
        [HttpPost("UpdateTenantUser")]
        public async Task<IActionResult> UpdateTenantUser([FromBody] AddTenantUser addUser)
        {
            try
            {
                //AppLogs.InfoLogs("UpdateUser Method was started,Controller:Admin");
                var user = _dbContext.RegisterUsers.Where(x => x.Id == addUser.Id).FirstOrDefault();
                if (user != null)
                {
                    if (user.SiteId == 0)
                    {
                        var folderNameObj = Path.Combine("EmailHtml", "Welcomemail.html");
                        var filePathObj = Path.Combine(Directory.GetCurrentDirectory(), folderNameObj);

                        StreamReader reader = new StreamReader(filePathObj);

                        string readFile = reader.ReadToEnd();
                        string myString = "";
                        myString = readFile;
                        string dt = DateTime.Now.ToString("MM.dd.yyyy");
                        myString = myString.Replace("%{#{Datetime}#}%", dt);
                        myString = myString.Replace("%{#{Name}#}%", user.FirstName + " " + user.LastName);
                        //  myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                        string body = myString;
                        bool key = await _userRepository.SendEmailAsync(user.Email, user.FirstName, "Welcome ", body, "GOPERMIT_welcome");
                    }
                }
                var result = await _userRepository.UpdateTenantUser_New(addUser);
                if (result != null)
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = addUser });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = addUser });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the UpdateUser Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region UpdateUser
        /// <summary>
        /// method to update the user
        /// </summary>
        /// <param name="addUser"></param>
        /// <returns></returns>
        [HttpPost("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] AddUserAc addUser)
        {
            try
            {
                //AppLogs.InfoLogs("UpdateUser Method was started,Controller:Admin");
                var result = await _userRepository.UpdateUser(addUser);
                if (result != null)
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = addUser });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = addUser });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the UpdateUser Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region UpdateRole
        /// <summary>
        /// method to update the Role
        /// </summary>
        /// <param name="addRole"></param>
        /// <returns></returns>
        [HttpPost("UpdateRole")]
        public async Task<IActionResult> UpdateRole([FromBody] AddRoleAc addRole)
        {
            try
            {
                //AppLogs.InfoLogs("UpdateRole Method was started,Controller:Admin");
                var result = await _roleRepository.UpdateRole(addRole);
                if (result != null)
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = addRole });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = addRole });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the UpdateRole Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetTenantUsers
        /// <summary>
        /// method to get the tenantusers
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="LoginId"></param>
        /// <param name="RoleId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetTenantUsers")]
        public async Task<IActionResult> GetTenantUsers(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId, int OperatorId)
        {
            try
            {
                //AppLogs.InfoLogs("GetUsers Method was started,Controller:Admin");
                var roles = await _userRepository.GetTenantUsers(PageNo, PageSize, LoginId, RoleId, SiteId, OperatorId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = roles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUsers Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetUsers
        /// <summary>
        /// method to get the users
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="LoginId"></param>
        /// <param name="RoleId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetUsers Method was started,Controller:Admin");
                var roles = await _userRepository.GetUsers(PageNo, PageSize, LoginId, RoleId, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = roles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUsers Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        [HttpGet("Getopeartoruser")]
        public async Task<IActionResult> Getopeartoruser(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetUsers Method was started,Controller:Admin");
                var roles = await _userRepository.GetOpeartoruser(PageNo, PageSize, LoginId, RoleId, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = roles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUsers Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        [HttpGet("GetSiteUser")]
        public async Task<IActionResult> GetSiteUser(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetUsers Method was started,Controller:Admin");
                var roles = await _userRepository.GetSiteUser(PageNo, PageSize, LoginId, RoleId, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = roles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUsers Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }


        #region GetRoles
        /// <summary>
        /// method to get the roles
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="LoginId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpGet("GetRoles")]
        public async Task<IActionResult> GetRoles(int PageNo, int PageSize, int LoginId, int RoleId)
        {
            try
            {
                //AppLogs.InfoLogs("GetRoles Method was started,Controller:Admin");
                var roles = await _roleRepository.GetRoles(PageNo, PageSize, LoginId, RoleId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = roles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetRoles Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetCountries
        /// <summary>
        /// method to get the countries
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCountries")]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                //AppLogs.InfoLogs("GetCountries Method was started,Controller:Admin");
                var roles = await _siteRepository.GetCountries();
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = roles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetCountries Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetRoleById
        /// <summary>
        /// method to get the role by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetRoleById")]
        public async Task<IActionResult> GetRoleById(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetRoles Method was started,Controller:Admin");
                var roles = await _roleRepository.GetRoleById(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = roles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetRoles Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetIsReadNotifications
        /// <summary>
        /// method to get the notifications
        /// </summary>
        /// <param name="RoleId"></param>
        /// <param name="LoginId"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetIsReadNotifications")]
        public async Task<IActionResult> GetIsReadNotifications(int RoleId, int LoginId, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetRoles Method was started,Controller:Admin");
                var roles = await _siteRepository.GetIsReadNotifications(RoleId, LoginId, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = roles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetRoles Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetTenantUserById
        /// <summary>
        /// method to get the tenant user by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetTenantUserById")]
        public async Task<IActionResult> GetTenantUserById(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetUserById Method was started,Controller:Admin");
                var roles = await _userRepository.GetTenantUserById(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = roles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUserById Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSearchSupportList
        /// <summary>
        /// method to get the search support list
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="SiteId"></param>
        /// <param name="SiteName"></param>
        /// <param name="Name"></param>
        /// <param name="Email"></param>
        /// <param name="MobileNumber"></param>
        /// <param name="Subject"></param>
        /// <returns></returns>
        [HttpGet("GetSearchSupportlist")]
        public async Task<IActionResult> GetSearchSupportlist(int PageNo, int PageSize, int SiteId, string SiteName, string Name, string Email, string MobileNumber, string Subject)
        {
            try
            {
                //AppLogs.InfoLogs("GetUserById Method was started,Controller:Admin");
                var support = await _siteRepository.GetSearchSupportList(PageNo, PageSize, SiteId, SiteName, Name, Email, MobileNumber, Subject);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = support });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUserById Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSupportListAdmin
        /// <summary>
        /// method to get the support list
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="SiteId"></param>
        /// <returns></returns>
        [HttpGet("GetSupportlistAdmin")]
        public async Task<IActionResult> GetSupportlistAdmin(int PageNo, int PageSize, int SiteId)
        {
            try
            {
                //AppLogs.InfoLogs("GetUserById Method was started,Controller:Admin");
                var support = await _siteRepository.GetSupportListAdmin(PageNo, PageSize, SiteId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = support });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUserById Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region CloseTicket
        /// <summary>
        /// method to close the ticket created by tenant
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("CloseTicket")]
        public async Task<IActionResult> CloseTicket(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetUserById Method was started,Controller:Admin");
                var support = await _siteRepository.CloseTicket(Id);
                var support1 = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Id).FirstOrDefault();
                var userId = support1.RegisterUserId;
                var user = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == userId).FirstOrDefault();
                var folderName = "";
                folderName = Path.Combine("EmailHtml", "CloseTicket.html");
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                StreamReader reader = new StreamReader(filePath);

                //string subject = acountconfirm;
                string readFile = reader.ReadToEnd();
                string myString = "";
                myString = readFile;
                //  string dt = DateTime.Now.ToString("MM.dd.yyyy");
                //  myString = myString.Replace("%{#{Datetime}#}%", dt);
                myString = myString.Replace("%{#{Name}#}%", user.FirstName);
                myString = myString.Replace("%{#{CaseId}#}%", support1.TicketId.ToString());
                myString = myString.Replace("%{#{Subject}#}%", support1.Subject);

                string body = myString;
                bool key = await _userRepository.SendEmailAsync(user.Email, user.FirstName, "Update on Your Support Ticket ", body, "Support Ticket_Status");
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = support });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUserById Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSupportAdminById
        /// <summary>
        /// method to get the Support admin by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetSupportAdminById")]
        public async Task<IActionResult> GetSupportAdminById(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetUserById Method was started,Controller:Admin");
                var support = await _siteRepository.GetSupportAdminById(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = support });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUserById Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSupportById
        /// <summary>
        /// method to get the supportby id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetSupportById")]
        public async Task<IActionResult> GetSupportById(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetUserById Method was started,Controller:Admin");
                var support = await _siteRepository.GetSupportById(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = support });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUserById Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetUserByid
        /// <summary>
        /// method to get the user by id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("GetUserById")]
        public async Task<IActionResult> GetUserById(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetUserById Method was started,Controller:Admin");
                var roles = await _userRepository.GetUserById(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = roles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUserById Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region RoleDelete
        /// <summary>
        /// method to delete the role
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("RoleDelete")]
        public async Task<IActionResult> RoleDelete(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("DeleteRole Method was started,Controller:Admin");
                var result = await _roleRepository.DeleteRole(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the DeleteRole Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region UserDelete
        /// <summary>
        /// method to delete the User
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpGet("UserDelete")]
        public async Task<IActionResult> UserDelete(int Id)
        {
            try
            {
                //AppLogs.InfoLogs("DeleteUser Method was started,Controller:Admin");
                var result = await _userRepository.DeleteUser(Id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the DeleteUser Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        #endregion


        #region SetPassword
        /// <summary>
        /// method to set password
        /// </summary>
        /// <param name="EmailCode"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpGet("SetPassword")]
        public async Task<IActionResult> SetPassword(string EmailCode, string Password)
        {
            try
            {
                //AppLogs.InfoLogs("SetPassword Method was started,Controller:Admin");
                var user = await _dbContext.RegisterUsers.FirstOrDefaultAsync(x => x.IsActive == true && x.EmailCode == EmailCode);
                if (user != null)
                {
                    user.Password = Utilities.Encrypt(Password);
                    user.UpdatedBy = 1;
                    user.UpdatedOn = DateTime.Now;
                    _dbContext.Update(user);
                    await _dbContext.SaveChangesAsync();
                    var folderName = "";
                    if (user.IsAdminCreated)
                    {
                        folderName = Path.Combine("EmailHtml", "Welcomemail.html");
                    }
                    else
                    {
                        if (user.RoleId == 2)
                        {
                            folderName = Path.Combine("EmailHtml", "AlertSiteTenant.html");
                        }
                        else
                        {
                            folderName = Path.Combine("EmailHtml", "Welcomemailuser.html");
                        }
                    }

                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    StreamReader reader = new StreamReader(filePath);

                    //string subject = acountconfirm;
                    string readFile = reader.ReadToEnd();
                    string myString = "";
                    myString = readFile;
                    //  string dt = DateTime.Now.ToString("MM.dd.yyyy");
                    //  myString = myString.Replace("%{#{Datetime}#}%", dt);
                    myString = myString.Replace("%{#{Name}#}%", user.FirstName );

                    string body = myString;
                    bool key = await _userRepository.SendEmailAsync(user.Email, user.FirstName, "Welcome to Go Permit !", body, "GOPERMIT_Welcome");
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Your password was generated", Result = null });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Email code is incorrect", Result = null });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the SetPassword Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region VerifyForgetPassword
        /// <summary>
        /// method to verify the email
        /// </summary>
        /// <param name="EmailCode"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpGet("VerifyForgetPassword")]
        public async Task<IActionResult> VerifyForgetPassword(string EmailCode, string Password)
        {
            try
            {
                //AppLogs.InfoLogs("VerifyForgetPassword Method was started,Controller:Admin");
                var user = await _dbContext.RegisterUsers.FirstOrDefaultAsync(x => x.IsActive == true && x.EmailCode == EmailCode);
                if (user != null)
                {
                    user.Password = Utilities.Encrypt(Password);
                    user.UpdatedBy = 1;
                    user.UpdatedOn = DateTime.Now;
                    _dbContext.Update(user);
                    await _dbContext.SaveChangesAsync();
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Your new password was generated", Result = null });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Email code is incorrect", Result = null });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the VerifyForgetPassword Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        #endregion

        #region ForgetPassword
        /// <summary>
        /// method to get the forgetpassword
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            try
            {
                //AppLogs.InfoLogs("ForgetPassword Method was started,Controller:Admin");
                var user = await _dbContext.RegisterUsers.FirstOrDefaultAsync(x => x.IsActive == true && x.Email == email);
                if (user != null)
                {
                    if (user.IsVerified == true)
                    {
                        string returnlink = string.Empty;
                        if (user.RoleId == 2)
                        {
                            returnlink = _configuration["TenantPasswordVerifyUrl"];
                        }
                        else
                        {
                            returnlink = _configuration["PasswordVerifyUrl"];
                        }

                        var folderName = Path.Combine("EmailHtml", "ForgetPassword.html");
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                        StreamReader reader = new StreamReader(filePath);
                        var resetLink = returnlink + user.EmailCode.Trim();
                        string readFile = reader.ReadToEnd();
                        string myString = "";
                        myString = readFile;
                        //  string dt = DateTime.Now.ToString("MM.dd.yyyy");
                        //   myString = myString.Replace("%{#{Datetime}#}%", dt);
                        myString = myString.Replace("%{#{Name}#}%", user.FirstName);
                        myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                        string body = myString;
                        bool key = await _userRepository.SendEmailAsync(user.Email, user.FirstName, "Password Verification from Go permit", body, "GOPERMIT_Verify Password");
                        if (key == true)
                        {
                            return Ok(new ApiServiceResponse() { Status = "200", Message = "Check your mail for reset password link", Result = null });
                        }
                        else
                        {
                            return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
                        }
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Your mail is not verified", Result = null });
                    }
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Invalid Email", Result = null });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the ForgetPassword Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region UpdateSupport
        /// <summary>
        /// method to update the support
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HttpPost("UpdateSupport")]
        public async Task<IActionResult> UpdateSupportResponse([FromBody] AddSupportAc obj)
        {
            try
            {
                var support = _tenantRepository.ReplySupport(obj);
                if (support != null)
                {

                    var folderName = Path.Combine("EmailHtml", "Response.html");
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    StreamReader reader = new StreamReader(filePath);
                    int ticketid = obj.TicketId;
                    string name = string.Empty;
                    string email = string.Empty;
                    RegisterUser user = new RegisterUser();
                    var visitor = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == ticketid).FirstOrDefault();
                    if (visitor != null)
                    {
                        user = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == visitor.RegisterUserId).FirstOrDefault();
                        if (user != null)
                        {
                            name = user.FirstName + " " + user.LastName;
                            email = user.Email;
                        }
                    }

                    string readFile = reader.ReadToEnd();
                    string myString = "";
                    myString = readFile;
                    myString = myString.Replace("%{#{Name}#}%", name);
                    myString = myString.Replace("%{#{Response}#}%", obj.Issue);
                    string body = myString;
                    bool key = await _userRepository.SendEmailAsync(email, name, obj.Subject, body, "GOPERMIT_Responce");


                    if (key == true)
                    {
                        return Ok(new ApiServiceResponse() { Status = "200", Message = "Response send successfully", Result = null });

                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Mail sending error occured", Result = null });
                    }

                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = null });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region VerifyMail
        /// <summary>
        /// method to verify the mail
        /// </summary>
        /// <param name="EmailCode"></param>
        /// <returns></returns>
        [HttpGet("VerifyMail")]
        public async Task<IActionResult> VerifyMail(string EmailCode)
        {
            try
            {
                //AppLogs.InfoLogs("VerifyMail Method was started,Controller:Admin");
                RegisterUser user = _dbContext.RegisterUsers.FirstOrDefault(x => x.EmailCode == EmailCode);
                if (user != null)
                {
                    user.IsVerified = true;
                    user.UpdatedBy = 1;
                    user.UpdatedOn = DateTime.Now;
                    _dbContext.RegisterUsers.Update(user);
                    await _dbContext.SaveChangesAsync();
                    var folderName = Path.Combine("EmailHtml", "Welcomemail.html");
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    StreamReader reader = new StreamReader(filePath);

                    //string subject = acountconfirm;
                    string readFile = reader.ReadToEnd();
                    string myString = "";
                    myString = readFile;
                    // string dt = DateTime.Now.ToString("MM.dd.yyyy");
                    //   myString = myString.Replace("%{#{Datetime}#}%", dt);
                    string body = myString;
                    bool key = await _userRepository.SendEmailAsync(user.Email, user.FirstName, "Welcome to Go Permit !", body, "GOPERMIT_Welocome");
                    if (key == true)
                    {
                        return Ok(new ApiServiceResponse() { Status = "200", Message = "Your mail activation done", Result = null });
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
                    }

                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Invalid code", Result = null });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the VerifyMail Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region UserLogin
        /// <summary>
        /// method to get the User login
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpPost("UserLogin")]
        public async Task<IActionResult> UserLogin(UserLogin userLogin)//string Email, string Password)
        {

            try
            {
                RegisterUser user = await _dbContext.RegisterUsers.FirstOrDefaultAsync(x => x.IsActive == true && x.IsDeleted == false && x.Email.Trim() == userLogin.Email.Trim());
                if (user != null)
                {
                    if (user.RoleId != 3 && user.RoleId != 2)
                    {
                        if (!string.IsNullOrEmpty(userLogin.Password))
                        {
                            string decryptpass = Utilities.Encrypt(userLogin.Password);
                            if (user.Password == decryptpass)
                            {
                                if (user.IsVerified == true)
                                {
                                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = user });
                                }
                                else
                                {
                                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Your email is not verified", Result = null });
                                }
                            }
                            else
                            {
                                return Ok(new ApiServiceResponse() { Status = "-100", Message = "In correct password", Result = null });
                            }
                        }
                        else
                        {
                            return Ok(new ApiServiceResponse() { Status = "-100", Message = "Password required", Result = null });
                        }
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "You are not authorize", Result = null });
                    }
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "In correct email address", Result = null });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the UserLogin Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion


        #region AddTenant
        [HttpPost("AddTenant")]
        public async Task<IActionResult> AddTenant([FromForm(Name = "fileupload")] List<IFormFile> file)
        {
            try
            {
                UpdateRegisterUserAc model = new UpdateRegisterUserAc();
                model.FirstName = Request.Form["FirstName"].ToString();
                model.LastName = Request.Form["LastName"].ToString();
                model.Address = Request.Form["Address"].ToString();
                model.State = Request.Form["State"].ToString();
                model.City = Request.Form["City"].ToString();
                model.Zipcode = Request.Form["Zipcode"].ToString();
                model.MobileNumber = Request.Form["MobileNumber"].ToString();
                model.Email = Request.Form["Email"].ToString();
                model.EmailCode = Request.Form["EmailCode"].ToString();
                Guid guid;
                if (_userRepository.GetTenant(model))
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Already Exists with email or mobileno", Result = model });
                }
                else
                {

                    var files = Request.Form.Files;
                    string filefolder = string.Empty;

                    var folderName = "";
                    var uniqueFileName = "";
                    var filePath = "";
                    if (files.Count > 0)
                    {
                        for (int i = 0; i < files.Count; i++)
                        {
                            if (files[0] == Request.Form.Files["fileupload"])
                            {
                                if (i == 0)
                                {
                                    filefolder = _configuration["TenantResidencyPath"];
                                    folderName = Path.Combine("TenantResidencyFiles");
                                    filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                                    guid = Guid.NewGuid();
                                    uniqueFileName = guid + "-" + files[i].FileName;
                                    model.ResidencyProof = filefolder + uniqueFileName;
                                }
                                else if (i == 1)
                                {
                                    filefolder = _configuration["IdentityProofPath"];
                                    folderName = Path.Combine("TenantIdentityProofFiles");
                                    filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                                    guid = Guid.NewGuid();
                                    uniqueFileName = guid + "-" + files[i].FileName;
                                    model.IdentityProof = filefolder + uniqueFileName;
                                }


                                if (!Directory.Exists(filePath))
                                {
                                    Directory.CreateDirectory(filePath);
                                }


                                using (var fileStream = new FileStream(Path.Combine(filePath, uniqueFileName), FileMode.Create))
                                {
                                    files[i].CopyTo(fileStream);
                                }




                            }


                        }




                    }
                    var result1 = await _userRepository.AddTenant(model);
                    if (result1 != null)
                    {

                        var folderNameObj = Path.Combine("EmailHtml", "AdminPrompt.html");
                        var filePathObj = Path.Combine(Directory.GetCurrentDirectory(), folderNameObj);

                        StreamReader reader = new StreamReader(filePathObj);

                        string readFile = reader.ReadToEnd();
                        string myString = "";
                        myString = readFile;
                        string dt = DateTime.Now.ToString("MM.dd.yyyy");
                        myString = myString.Replace("%{#{Datetime}#}%", dt);

                        string body = myString;
                        bool key = await _userRepository.SendEmailAdminAsync(_configuration["EmailSettings:AdminMail"], "Go Permit New Customer Application", body, "Go Permit New Customer Application");

                    }
                    // var result = await _userRepository.AddTenant(model);
                    if (true)
                    {
                        string returnlink = _configuration["TenantSetPasswordUrl"];
                        var folderNameObj = Path.Combine("EmailHtml", "SetPassword.html");
                        var filePathObj = Path.Combine(Directory.GetCurrentDirectory(), folderNameObj);

                        StreamReader reader = new StreamReader(filePathObj);
                        var resetLink = returnlink + model.EmailCode;
                        string readFile = reader.ReadToEnd();
                        string myString = "";
                        myString = readFile;
                        string dt = DateTime.Now.ToString("MM.dd.yyyy");
                        myString = myString.Replace("%{#{Datetime}#}%", dt);
                        myString = myString.Replace("%{#{Name}#}%", model.FirstName + " " + model.LastName);
                        myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                        string body = myString;
                        bool key = await _userRepository.SendEmailAsync(model.Email, model.FirstName, "Set Password from Go Permit", body, "GOPERMIT_Set Password");
                        if (key == true)
                        {
                            return Ok(new ApiServiceResponse() { Status = "200", Message = "Check your mail for reset password link", Result = null });
                        }
                        else
                        {
                            return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
                        }
                        //  return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = addUser });
                    }
                    //else
                    //{
                    //    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = model });
                    //}

                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = model });
                    }



                }

            }
            catch (Exception ex)
            {

            }
            return null;
        }
        #endregion

        #region AddRegisterUser
        /// <summary>
        /// method to add the registeruser
        /// </summary>
        /// <param name="addRegister"></param>
        /// <returns></returns>
        [HttpPost("AddRegisterUser")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddRegisterUser([FromBody] AddRegisterUserAc addRegister)
        {
            try
            {
                //AppLogs.InfoLogs("AddRegisterUser Method was started,Controller:Admin");
                if (_registerRepository.GetRegisterUser(addRegister))
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Already Exists with email or mobileno", Result = addRegister });
                }
                else
                {
                    var str = await _registerRepository.AddRegisterUser(addRegister);
                    if (str != null)
                    {
                        RegisterUser user = _dbContext.RegisterUsers.FirstOrDefault(x => x.EmailCode == addRegister.EmailCode);
                        user.IsVerified = true;
                        user.UpdatedBy = 1;
                        user.UpdatedOn = DateTime.Now;
                        _dbContext.RegisterUsers.Update(user);
                        await _dbContext.SaveChangesAsync();
                        if (addRegister.Password == null)
                        {
                            string returnlink = _configuration["SetPasswordUrl"];
                            var folderName = Path.Combine("EmailHtml", "SetPassword.html");
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                            StreamReader reader = new StreamReader(filePath);
                            var resetLink = returnlink + addRegister.EmailCode;
                            string readFile = reader.ReadToEnd();
                            string myString = "";
                            myString = readFile;
                            string dt = DateTime.Now.ToString("MM.dd.yyyy");
                            // myString = myString.Replace("%{#{Datetime}#}%", dt);
                            myString = myString.Replace("%{#{Name}#}%", addRegister.FirstName + " " + addRegister.LastName);
                            myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                            string body = myString;
                            bool key = await _userRepository.SendEmailAsync(addRegister.Email, addRegister.FirstName, "Set Password from Go Permit", body, "GOPERMIT_Set Password");
                        }
                        else
                        {
                            var folderName = Path.Combine("EmailHtml", "Welcomemail.html");
                            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                            StreamReader reader = new StreamReader(filePath);

                            //string subject = acountconfirm;
                            string readFile = reader.ReadToEnd();
                            string myString = "";
                            myString = readFile;
                            // string dt = DateTime.Now.ToString("MM.dd.yyyy");
                            //  myString = myString.Replace("%{#{Datetime}#}%", dt);
                            string body = myString;
                            bool key = await _userRepository.SendEmailAsync(user.Email, user.FirstName, "Welcome to Go Permit!", body, "GOPERMIT_Welcome");
                            if (key == true)
                            {
                                return Ok(new ApiServiceResponse() { Status = "200", Message = "Tenant registered successfully", Result = null });
                            }
                            else
                            {
                                return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
                            }
                        }


                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the AddRegisterUser Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        //#region SendEmail1
        ///// <summary>
        ///// method to send the mail
        ///// </summary>
        ///// <param name="EmailId"></param>
        ///// <param name="User"></param>
        ///// <param name="EmailCode"></param>
        ///// <param name="ActivateLink"></param>
        ///// <returns></returns>
        //bool SendEmail(string EmailId, string User, string Subject, string Body, string Headeraname)
        //{

        //    SmtpClient client = new SmtpClient();
        //    //  client.DeliveryMethod = SmtpDeliveryMethod.Network;
        //    client.EnableSsl = Convert.ToBoolean(_configuration["SSL"]);
        //    client.Host = _configuration["Host"];
        //    client.Port = Convert.ToInt32(_configuration["Port"]);

        //    NetworkCredential credentials = new NetworkCredential();
        //    client.UseDefaultCredentials = false;
        //    credentials.UserName = _configuration["AdminMail"];
        //    credentials.Password = _configuration["Password"];
        //    client.Credentials = credentials;


        //    MailMessage mailMessage = new MailMessage();
        //    mailMessage.From = new MailAddress(_configuration["AdminMail"],
        //       Headeraname);
        //    mailMessage.To.Add(new MailAddress(EmailId));

        //    mailMessage.Subject = Subject;
        //    mailMessage.IsBodyHtml = true;
        //    string Body1 = Body;

        //    mailMessage.Body = Body1;
        //    try
        //    {
        //        //AppLogs.InfoLogs("Start  test email sending AT client.Send(mailMessage) STATEMENT, Login Controller FORM, Method :SendMail");
        //        client.Send(mailMessage);
        //        //AppLogs.InfoLogs("End  test email sending AT client.Send(mailMessage) STATEMENT, Login Controller FORM, Method :SendMail");

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //AppLogs.InfoLogs("Error  Sending Mail , Login FORM, Method :SendMail" + ex.ToString());
        //        return true;
        //    }
        //}
        //#endregion


        #region SendEmail2
        /// <summary>
        /// method to send the mail
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="User"></param>
        /// <param name="EmailCode"></param>
        /// <param name="ActivateLink"></param>
        /// <returns></returns>
        bool SendEmail1(string EmailId, string User, string Subject, string Body, string CC, bool cc, string headername)
        {







            SmtpClient client = new SmtpClient();
            //   client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = Convert.ToBoolean(_configuration["SSL"]);
            client.Host = _configuration["Host"];
            client.Port = Convert.ToInt32(_configuration["Port"]);

            NetworkCredential credentials = new NetworkCredential();
            client.UseDefaultCredentials = false;
            credentials.UserName = _configuration["AdminMail"];
            credentials.Password = _configuration["Password"];
            client.Credentials = credentials;


            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_configuration["AdminMail"],
               headername);
            mailMessage.To.Add(new MailAddress(EmailId));
            if (cc == true)
            {
                mailMessage.CC.Add(new MailAddress(CC));
            }

            mailMessage.Subject = Subject;
            mailMessage.IsBodyHtml = true;
            string Body1 = Body;

            mailMessage.Body = Body1;
            try
            {
                //AppLogs.InfoLogs("Start  test email sending AT client.Send(mailMessage) STATEMENT, Login Controller FORM, Method :SendMail");
                client.Send(mailMessage);
                //AppLogs.InfoLogs("End  test email sending AT client.Send(mailMessage) STATEMENT, Login Controller FORM, Method :SendMail");

                return true;
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error  Sending Mail , Login FORM, Method :SendMail" + ex.ToString());
                return false;
            }
        }
        #endregion

        public async Task<bool> SendEmail(string EmailId, string User, string Subject, string Body, string CC, bool cc, string HeaderName)
        {
            try
            {
                string accessToken = await _userRepository.GetAccessTokenAsync();

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(HeaderName, _configuration["EmailSettings:AdminMail"]));
                message.To.Add(new MailboxAddress("", EmailId));

                if (cc && !string.IsNullOrWhiteSpace(CC))
                {
                    message.Cc.Add(new MailboxAddress("", CC));
                }

                message.Subject = Subject;
                var bodyBuilder = new BodyBuilder { HtmlBody = Body };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(_configuration["EmailSettings:SMTP_Host"], Convert.ToInt32(_configuration["EmailSettings:SMTP_Port"]), SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(new SaslMechanismOAuth2(_configuration["EmailSettings:Username"], accessToken));
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                // Log error (you can replace this with your logging mechanism)
                Console.WriteLine($"Error sending email: {ex.Message}");
                return false;
            }
        }

        #region GetSearchPingFilter
        /// <summary>
        /// method to get the ping reports
        /// </summary>
        /// <param name="PageNo"></param>
        /// <param name="PageSize"></param>
        /// <param name="SiteId"></param>
        /// <param name="UserId"></param>
        /// <param name="FromDate"></param>
        /// <param name="ToDate"></param>
        /// <returns></returns>
        [HttpGet("GetSearchPingFilter")]
        public async Task<IActionResult> GetSearchPingFilter(int PageNo, int PageSize, int SiteId, int UserId, string FromDate, string ToDate)
        {
            try
            {
                //AppLogs.InfoLogs("GetUserById Method was started,Controller:Admin");
                var result = await _permissionRepository.GetPingReports(PageNo, PageSize, SiteId, UserId, FromDate, ToDate);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUserById Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion


        #region TestSendMail
        /// <summary>
        /// method to send test mail
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="User"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="Headername"></param>
        /// <returns></returns>
        [HttpGet("TestSendMail")]
        public async Task<IActionResult> TestSendMail(string EmailId, string User, string Subject, string Body, string Headername)
        {
            try
            {
                //AppLogs.InfoLogs("GetUserById Method was started,Controller:Admin");
                bool result = await _userRepository.SendEmailAsync(EmailId, User, Subject, Body, Headername);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = result });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetUserById Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region
        bool GoDaddySendEmail(string EmailId, string User, string Subject, string Body, string Headeraname)
        {

            SmtpClient client = new SmtpClient();
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //  client.EnableSsl = false;
            client.Host = "smtpout.secureserver.net";
            client.Port = Convert.ToInt32(587);

            NetworkCredential credentials = new NetworkCredential();
            client.UseDefaultCredentials = false;
            credentials.UserName = "info@gopermit.co.uk";
            credentials.Password = "EPS@123";
            client.Credentials = credentials;
            client.EnableSsl = true;

            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("info@gopermit.co.uk",
               Headeraname);
            mailMessage.To.Add(new MailAddress(EmailId));

            mailMessage.Subject = Subject;
            mailMessage.IsBodyHtml = true;
            string Body1 = Body;

            mailMessage.Body = Body1;
            try
            {
                //AppLogs.InfoLogs("Start  test email sending AT client.Send(mailMessage) STATEMENT, Login Controller FORM, Method :SendMail");
                client.Send(mailMessage);
                //AppLogs.InfoLogs("End  test email sending AT client.Send(mailMessage) STATEMENT, Login Controller FORM, Method :SendMail");

                return true;
            }
            catch (Exception ex)
            {
                // Console.WriteLine(ex.Message.ToString());
                // Console.ReadLine();
                //  Console.WriteLine(ex.StackTrace.ToString());
                //  Console.ReadLine();
                AppLogs.InfoLogs("Error  Sending Mail , Login FORM, Method :SendMail" + ex.Message.ToString());
                AppLogs.InfoLogs("Error  Sending Mail , Login FORM, Method :SendMail" + ex.StackTrace.ToString());
                AppLogs.InfoLogs("Error  Sending Mail , Login FORM, Method :SendMail" + ex.InnerException.Message.ToString());
                return false;
            }
        }
        #endregion

        [HttpPost("BulkUploadTenants")]
        public async Task<IActionResult> BulkUploadTenants(IFormFile file)
        {
            AppLogs.InfoLogs("BulkUploadTenants API called at: " + DateTime.UtcNow);

            try
            {
                if (file == null || file.Length == 0)
                {
                    AppLogs.InfoLogs("BulkUploadTenants: Invalid file received.");
                    return BadRequest(new { Status = "-100", Message = "Invalid file" });
                }

                AppLogs.InfoLogs($"Processing file: {file.FileName}, Size: {file.Length} bytes");

                var response = await _userRepository.BulkInsertUsersFromExcel(file);

                AppLogs.InfoLogs($"BulkUploadTenants: Response - {Newtonsoft.Json.JsonConvert.SerializeObject(response)}");
                return Ok(response);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                AppLogs.InfoLogs("BulkUploadTenants: Error occurred - " + errorMessage);
                return StatusCode(500, new { Status = "-500", Message = "Internal server error", Error = errorMessage });
            }
        }
        [HttpGet("GetAllIndustries")]
        public async Task<IActionResult> GetAllIndustries()
        {
            AppLogs.InfoLogs("GetAllIndustries API called at: {Time}");

            var industries = await _industryRepository.GetAllIndustries();

            AppLogs.InfoLogs("GetAllIndustries: Retrieved {Count} records");
            return Ok(industries);
        }

        [HttpGet("GetIndustryById")]
        public async Task<IActionResult> GetIndustryById(int id)
        {
            AppLogs.InfoLogs("GetIndustryById API called for ID: {Id}");

            var industry = await _industryRepository.GetIndustryById(id);
            if (industry == null)
            {
                AppLogs.InfoLogs("GetIndustryById: Industry with ID {Id} not found");
                return null;
            }

            AppLogs.InfoLogs("GetIndustryById: Retrieved industry: {IndustryName}");
            return Ok(industry);
        }

        [HttpPost("InsertIndustry")]
        public async Task<IActionResult> InsertIndustry([FromBody] Industries industry)
        {
            AppLogs.InfoLogs("InsertIndustry API called with data: {@Industry}");

            if (industry == null)
            {
                AppLogs.InfoLogs("InsertIndustry: Received invalid data");
                return BadRequest(new { Message = "Invalid data" });
            }

            var insertedIndustry = await _industryRepository.InsertIndustry(industry);
            AppLogs.InfoLogs("InsertIndustry: Successfully inserted industry with ID: {Id}");

            return Ok(insertedIndustry);
        }

        [HttpPost("UpdateIndustry")]
        public async Task<IActionResult> UpdateIndustry([FromBody] Industries industry)
        {
            AppLogs.InfoLogs("UpdateIndustry API called with data: {@Industry}");

            if (industry == null || industry.Id <= 0)
            {
                AppLogs.InfoLogs("UpdateIndustry: Received invalid data");
                return BadRequest(new { Message = "Invalid data" });
            }

            var updatedIndustry = await _industryRepository.UpdateIndustry(industry);
            if (updatedIndustry == null)
            {
                AppLogs.InfoLogs("UpdateIndustry: Industry with ID {Id} not found");
                return NotFound(new { Message = "Industry not found" });
            }

            AppLogs.InfoLogs("UpdateIndustry: Successfully updated industry with ID: {Id}");
            return Ok(updatedIndustry);
        }

        [HttpPost("DeleteIndustry")]
        public async Task<IActionResult> DeleteIndustry(int id)
        {
            AppLogs.InfoLogs("DeleteIndustry API called for ID: {Id} ");

            var deletedIndustry = await _industryRepository.DeleteIndustry(id);
            if (deletedIndustry == null)
            {
                AppLogs.InfoLogs("DeleteIndustry: Industry with ID: {Id} not found");
                return NotFound(new { Message = "Industry not found" });
            }

            AppLogs.InfoLogs("DeleteIndustry: Successfully deleted industry with ID: {Id}");
            return Ok(deletedIndustry);
        }

    }
}
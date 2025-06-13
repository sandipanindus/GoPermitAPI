using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using LabelPad.Repository.SiteManagment;
using LabelPad.Repository.TenantManagement;
using LabelPad.Repository.UserManagement;
using LabelPad.Repository.VehicleRegistrationManagement;
using LabelPadCoreApi.Models;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;

namespace LabelPadCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly LabelPadDbContext _dbContext;
        private readonly ITenantRepository _tenantRepository;
        private readonly ISiteRepository _siteRepository;
        private readonly IUserRepository _userRepository;

        private readonly IConfiguration _configuration;
        public TenantController(LabelPadDbContext dbContext, ITenantRepository tenantRepository, IConfiguration configuration, ISiteRepository siteRepository, IUserRepository userRepository)
        {
            _dbContext = dbContext;
            _tenantRepository = tenantRepository;
            _configuration = configuration;
            _siteRepository = siteRepository;
            _userRepository = userRepository;
        }

        #region GetVisitorParkings
        /// <summary>
        /// method to get the support list
        /// </summary>
        /// <param name="tenantid"></param>
        /// <returns></returns>
        [HttpGet("GetVisitorParkings")]
        public async Task<IActionResult> GetVisitorParkings(string tenantid)
        {
            try
            {
                //AppLogs.InfoLogs("GetSupportList Method was started,Controller:Admin");
                var visitors = await _siteRepository.GetVisitorParkings(tenantid);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        [HttpGet("GetVisitorParkingsById")]
        public async Task<IActionResult> GetVisitorParkingsById(string tenantid,string id)
        {
            try
            {
                //AppLogs.InfoLogs("GetSupportList Method was started,Controller:Admin");
                var visitors = await _siteRepository.GetVisitorParkingsById(tenantid,id);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion



        #region GetManageParkings
        /// <summary>
        /// method to get the support list
        /// </summary>
        /// <param name="tenantid"></param>
        /// <returns></returns>
        [HttpGet("GetManageParkings")]
        public async Task<IActionResult> GetManageParkings(string tenantid)
        {
            try
            {
                //AppLogs.InfoLogs("GetSupportList Method was started,Controller:Admin");
                var visitors = await _siteRepository.GetManageParkings(tenantid);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = visitors });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSupportList
        /// <summary>
        /// method to get the support list
        /// </summary>
        /// <param name="tenantid"></param>
        /// <returns></returns>
        [HttpGet("GetSupportList")]
        public async Task<IActionResult> GetSupportList(string tenantid)
        {
            try
            {
                //AppLogs.InfoLogs("GetSupportList Method was started,Controller:Admin");
                var support = await _tenantRepository.GetSupportList(Convert.ToInt32(tenantid));
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = support });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region GetSupportById
        /// <summary>
        /// method to get the support by id
        /// </summary>
        /// <param name="tenantid"></param>
        /// <returns></returns>
        [HttpGet("GetSupportById")]
        public async Task<IActionResult> GetSupportById(string id, int TicketId, int TenantId)
        {
            try
            {
                //AppLogs.InfoLogs("GetSupportList Method was started,Controller:Admin");
                var support = await _tenantRepository.GetSupportById(Convert.ToInt32(id), TicketId, TenantId);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = support });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region ChangePassword
        /// <summary>
        /// method to change password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="LoginId"></param>
        /// <returns></returns>
        [HttpGet("ChangePassword")]
        public async Task<IActionResult> ChangePassword(string password, string LoginId)
        {
            try
            {
                RegisterUser user = await _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == Convert.ToInt32(LoginId)).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.Password = Utilities.Encrypt(password);
                    user.UpdatedBy = Convert.ToInt32(LoginId);
                    user.UpdatedOn = DateTime.Now;
                    _dbContext.RegisterUsers.Update(user);
                    await _dbContext.SaveChangesAsync();
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = user });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Invalid user", Result = null });
                }

            }
            catch (Exception ex)
            {
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        private Task<string> GenerateJSONWebToken(UserModel userinfo)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub,userinfo.UserName),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email,userinfo.EmailAddress),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(encodetoken);
        }

        #region TenantLogin
        /// <summary>
        /// method to tenant login
        /// </summary>
        /// <param name="Email"></param>
        /// <param name="Password"></param>
        /// <returns></returns>
        [HttpPost("TenantLogin")]
        public async Task<IActionResult> TenantLogin(UserLogin userLogin)//string Email, string Password)
        {
            try
            {
                RegisterUser user = await _dbContext.RegisterUsers.FirstOrDefaultAsync(x => x.IsActive == true && x.IsDeleted == false && x.Email.Trim() == userLogin.Email.Trim() && x.RoleId == 2);
                if (user != null)
                {
                    
                    if (user.SiteId != 0)
                    {
                        if (!string.IsNullOrEmpty(userLogin.Password))
                        {
                            string decryptpass = Utilities.Encrypt(userLogin.Password);
                            if (user.Password == decryptpass)
                            {
                                if (user.IsVerified == true)
                                {
                                    UserModel login = new UserModel();
                                    login.UserName = "gopermit";
                                    login.Password = userLogin.Password;
                                    login.EmailAddress = userLogin.Email;

                                    var tokenStr = await GenerateJSONWebToken(login);

                                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", token = tokenStr, Result = user });
                                }
                                else
                                {
                                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Your email is not verified", Result = null });
                                }
                            }
                            else
                            {
                                return Ok(new ApiServiceResponse() { Status = "-100", Message = "Incorrect password", Result = null });
                            }
                        }
                        else
                        {
                            return Ok(new ApiServiceResponse() { Status = "-100", Message = "Password required", Result = null });
                        }
                    }
                    else
                    {

                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Please note: "+ userLogin.Email + ": Your account is awaiting for approval. Please try again after 48 hours before contacting the administrator.", Result = null });

                    }

                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Incorrect email address", Result = null });
                }
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the UserLogin Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion


        #region UpdateVehicles
        /// <summary>
        /// method to add vehicle
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("UpdateVehicles")]
        public async Task<IActionResult> UpdateVehicles([FromBody] List<AddVehicleRegistrationAc> objInput)
        {
            if (objInput == null || objInput.Count == 0)
            {
                return Ok(new ApiServiceResponse() { Status = "400", Message = "Input data cannot be empty.", Result = null });
            }

            try
            {
                var result = await _tenantRepository.UpdateVehicle(objInput);
                var resultData = result as dynamic; 
                if (resultData != null && resultData.Messages != null)
                {
                    if (resultData.Messages.Contains("Record updated successfully"))
                    {
                        return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = resultData });
                    }
                    else if (resultData.Messages.Contains("Range Already Exist"))
                    {
                        return Ok(new ApiServiceResponse() { Status = "-200", Message = "Failure", Result = resultData });
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = resultData });
                    }
                }
                return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = result });
            }
            catch (Exception ex)
            {
                return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = ex.Message });
            }
        }
        #endregion

        #region AddVehicles
        /// <summary>
        /// method to add vehicle
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("AddVehicles")]
        public async Task<IActionResult> AddVehicles([FromBody] List<AddVehicleRegistrationAc> objdata)
        {
            try
            {
                var response = await _tenantRepository.AddVehicle_New(objdata);

                if (response.Message == "Range Already Exist")
                {
                    return Ok(new ApiServiceResponse() { Status = "-200", Message = "Failure", Result = response });
                }
                else if (response.Message == "Vehicles updated and new records inserted successfully")
                {
                    return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = response });
                }
                else
                {
                    return Ok(new ApiServiceResponse() { Status = "-100", Message = "Failure", Result = response });
                }
            }
            catch (Exception ex)
            {
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        #endregion

        #region AddVehiclesTimeSlot
        /// <summary>
        /// method to add timeslot
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("AddVehiclesTimeSlot")]
        public async Task<IActionResult> AddVehiclesTimeSlot([FromBody] List<AddVehicleTimeSlotAc> objdata)
        {
            try
            {


                var result = await _tenantRepository.AddVehicleTimeSlot(objdata);
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

        #region AddSupport
        /// <summary>
        /// method to add support
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("AddSupport")]
        public async Task<IActionResult> AddSupport([FromBody] AddSupportAc objdata)
        {
            try
            {


                var result = await _tenantRepository.AddSupport(objdata);
                if (result != null)
                {
                    RegisterUser user = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == objdata.TenantId).FirstOrDefault();
                    var folderName = Path.Combine("EmailHtml", "Contact.html");
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    StreamReader reader = new StreamReader(filePath);

                    //string subject = acountconfirm;
                    string readFile = reader.ReadToEnd();
                    string myString = "";
                    myString = readFile;

                    var Support = _dbContext.Supports.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == result.Id).FirstOrDefault();
                    myString = myString.Replace("%{#{CaseId}#}%", Support.TicketId.ToString());
                    myString = myString.Replace("%{#{Subject}#}%", Support.Subject);
                    myString = myString.Replace("%{#{Name}#}%", user.FirstName + " " + user.LastName);
                    string body = myString;
                    bool key = await SendEmail(user.Email, user.FirstName, "We will Contact Shortly !", body, "GOPERMIT_WE Contact");
                    folderName = Path.Combine("EmailHtml", "SupportAdmin.html");
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    reader = new StreamReader(filePath);

                    //string subject = acountconfirm;
                    readFile = reader.ReadToEnd();
                    myString = "";
                    myString = readFile;
                    var sites = _dbContext.Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == user.SiteId).FirstOrDefault();
                    string sitename = string.Empty;
                    if (sites != null)
                    {
                        sitename = sites.SiteName;
                    }
                    myString = myString.Replace("%{#{Name}#}%", user.FirstName);
                    myString = myString.Replace("%{#{SiteName}#}%", sitename);
                    myString = myString.Replace("%{#{Email}#}%", user.Email);
                    myString = myString.Replace("%{#{Address}#}%", user.Address);
                    myString = myString.Replace("%{#{Issue}#}%", objdata.Issue);
                    body = myString;
                    string email = _configuration["EmailSettings:AdminMail"];
                    key = await SendEmail(email, "Admin", objdata.Subject, body, "GOPERMIT_Support");
                    if (key == true)
                    {
                        return Ok(new ApiServiceResponse() { Status = "200", Message = "send successfully", Result = null });
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
                    }

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

        #region ReplySupport
        /// <summary>
        /// method to add support
        /// </summary>
        /// <param name="objdata"></param>
        /// <returns></returns>
        [HttpPost("ReplySupport")]
        public async Task<IActionResult> ReplySupport([FromBody] AddSupportAc objdata)
        {
            try
            {


                var result = await _tenantRepository.ReplySupport(objdata);
                if (result != null)
                {
                    RegisterUser user = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == objdata.TenantId).FirstOrDefault();
                    var folderName = Path.Combine("EmailHtml", "Contact.html");
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    StreamReader reader = new StreamReader(filePath);

                    //string subject = acountconfirm;
                    string readFile = reader.ReadToEnd();
                    string myString = "";
                    myString = readFile;

                    myString = myString.Replace("%{#{Name}#}%", user.FirstName);
                    myString = myString.Replace("%{#{CaseId}#}%", objdata.TicketId.ToString());
                    myString = myString.Replace("%{#{Subject}#}%", objdata.Subject);
                    string body = myString;
                    bool key = await SendEmail(user.Email, user.FirstName, "We will Contact Shortly !", body, "GOPERMIT_Contact");
                    folderName = Path.Combine("EmailHtml", "SupportAdmin.html");
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    reader = new StreamReader(filePath);

                    //string subject = acountconfirm;
                    readFile = reader.ReadToEnd();
                    myString = "";
                    myString = readFile;
                    var sites = _dbContext.Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == user.SiteId).FirstOrDefault();
                    string sitename = string.Empty;
                    if (sites != null)
                    {
                        sitename = sites.SiteName;
                    }
                    myString = myString.Replace("%{#{Name}#}%", user.FirstName);
                    myString = myString.Replace("%{#{SiteName}#}%", sitename);
                    myString = myString.Replace("%{#{Email}#}%", user.Email);
                    myString = myString.Replace("%{#{Address}#}%", user.Address);
                    myString = myString.Replace("%{#{Issue}#}%", objdata.Issue);
                    myString = myString.Replace("%{#{Ticket no}#}%", objdata.TicketId.ToString());
                    body = myString;
                    string email = _configuration["EmailSettings:AdminMail"];
                    key = await SendEmail(email, "Admin", objdata.Subject, body, "GOPERMIT_Admin");
                    if (key == true)
                    {
                        return Ok(new ApiServiceResponse() { Status = "200", Message = "send successfully", Result = null });
                    }
                    else
                    {
                        return Ok(new ApiServiceResponse() { Status = "-100", Message = "Sending mail error occured", Result = null });
                    }

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
        #region UpdateVisitorParking
        /// <summary>
        /// UpdateVisitorParking
        /// </summary>
        /// <param name="vistorsParkingRequest"></param>
        /// <returns></returns>
        [HttpPost("UpdateVisitorParking")]
        public async Task<IActionResult> UpdateVisitorParking([FromBody] UpdateVistorsParkingRequest vistorsParkingRequest)
        {
            AppLogs.InfoLogs("UpdateIndustry API called with data: {@Industry}");

            if (vistorsParkingRequest == null || vistorsParkingRequest.Id <= 0)
            {
                AppLogs.InfoLogs("vistorsParkings: Received invalid data");
                return BadRequest(new { Message = "Invalid data" });
            }

            var updateparking = await _tenantRepository.UpdateVisitorParking(vistorsParkingRequest);
            if (updateparking == null)
            {
                AppLogs.InfoLogs("vistorsParkings: vistorsParkings with ID {Id} not found");
                return NotFound(new { Message = "vistorsParkings not found" });
            }

            AppLogs.InfoLogs("vistorsParkings: Successfully updated vistorsParkings with ID: {Id}");
            return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = updateparking });

        }
        #endregion
        #region SendEmail
        /// <summary>
        /// method to send the mail
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="User"></param>
        /// <param name="EmailCode"></param>
        /// <param name="ActivateLink"></param>
        /// <returns></returns>
        bool SendEmail1(string EmailId, string User, string Subject, string Body, string headername)
        {

            SmtpClient client = new SmtpClient();
            // client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = Convert.ToBoolean(_configuration["SSL"]);
            client.Host = _configuration["Host"];
            client.Port = Convert.ToInt32(_configuration["Port"]);

            NetworkCredential credentials = new NetworkCredential();
            client.UseDefaultCredentials = false;
            credentials.UserName = _configuration["EmailSettings:AdminMail"]; 
            credentials.Password = _configuration["Password"];
            client.Credentials = credentials;


            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_configuration["EmailSettings:AdminMail"],
               headername);
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
                //AppLogs.InfoLogs("Error  Sending Mail , Login FORM, Method :SendMail" + ex.ToString());
                return false;
            }
        }
        #endregion

        public async Task<bool> SendEmail(string EmailId, string User, string Subject, string Body, string headername)
        {
            try
            {
                string accessToken = await _userRepository.GetAccessTokenAsync(); // Fetch OAuth access token

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(headername, _configuration["EmailSettings:AdminMail"]));
                message.To.Add(new MailboxAddress("", EmailId));
                message.Subject = Subject;

                var bodyBuilder = new BodyBuilder { HtmlBody = Body };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new MailKit.Net.Smtp.SmtpClient();
                await client.ConnectAsync(_configuration["EmailSettings:SMTP_Host"], Convert.ToInt32(_configuration["EmailSettings:SMTP_Port"]), SecureSocketOptions.StartTls);

                // Authenticate using OAuth2
                await client.AuthenticateAsync(new SaslMechanismOAuth2(_configuration["EmailSettings:Username"], accessToken));

                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }

        [HttpGet("GetVehicleDetailsbytenant")]
        public async Task<IActionResult> GetVehicleDetails(string tenantid)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var vehicles = await _tenantRepository.GetVehicleDetails(tenantid);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = vehicles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        [HttpGet("baynobytenant")]
        public async Task<IActionResult> Getbaynobytenant(string tenantid)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var vehicles = await _tenantRepository.Getbaynobytenant(tenantid);
                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = vehicles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        [HttpGet("getvehcilecounts")]
        public async Task<IActionResult> Getvehiclecountsdetails(string tenantid, string bayno)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var vehicles = await _tenantRepository.Getvehiclecountsdetails(tenantid, bayno);

                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = vehicles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        [HttpGet("getvehcilecountsById")]
        public async Task<IActionResult> GetvehiclecountsdetailsById(string tenantid, string bayno, int Id)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var vehicles = await _tenantRepository.GetvehiclecountsdetailsById(tenantid, bayno, Id);

                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = vehicles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }
        

        [HttpGet("getvehcilecountsbydates")]
        public async Task<IActionResult> getvehcilecountsbydates(string tenantid, string bayno, string date)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var vehicles = await _tenantRepository.getvehcilecountsbydates(tenantid, bayno, date);

                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = vehicles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        [HttpGet("getvehcilecountsbydatesvrm")]
        public async Task<IActionResult> getvehcilecountsbydatesvrm(string tenantid, string bayno, string date, string vrm)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var vehicles = await _tenantRepository.getvehcilecountsbydatesvrm(tenantid, bayno, date, vrm);

                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = vehicles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        [HttpGet("getvehcilelistcountsbydates")]
        public async Task<IActionResult> getvehcilelistcountsbydates(string tenantid, string bayno, string date)
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var vehicles = await _tenantRepository.getvehcilelistcountsbydates(tenantid, bayno, date);

                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = vehicles });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

        [HttpGet("GetVersions")]
        public async Task<dynamic> GetVersions()
        {
            try
            {
                //AppLogs.InfoLogs("GetTenantsBySite Method was started,Controller:Admin");
                var versions = await _tenantRepository.getversions();
               

                return Ok(new ApiServiceResponse() { Status = "200", Message = "Success", Result = versions });
            }
            catch (Exception ex)
            {
                //AppLogs.InfoLogs("Error occured in the GetSites Method,Controller:Admin" + ex.ToString());
                return Ok(new ApiServiceResponse() { Status = "-100", Message = ex.ToString(), Result = null });
            }
        }

    }
}

using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Reflection.Emit;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Threading;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace LabelPad.Repository.UserManagement
{

    public class Xml1
    {
        public string @version { get; set; }
        public string @encoding { get; set; }
    }

    public class cancelwhitelist
    {
        public string vrm { get; set; }
        public string reference_no { get; set; }
    }

    public class Xml
    {
        public string error { get; set; }
        public string status_code { get; set; }
        public string message { get; set; }
        public string reference_no { get; set; }
    }

    public class Root
    {
        public Xml1 xml1 { get; set; }
        public Xml xml { get; set; }
    }
    public class UserRepository : IUserRepository
    {
        private readonly LabelPadDbContext _dbContext;
        private readonly IConfiguration _configuration;
        public UserRepository(LabelPadDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async Task<dynamic> AddUser(AddUserAc addUser)
        {
           
                await _dbContext.RegisterUsers.AddAsync(new RegisterUser
                {
                    FirstName = addUser.FirstName,
                    LastName = addUser.LastName,
                    Email = addUser.Email,
                    State = addUser.State,
                    City = addUser.City,
                    ZipCode = addUser.Zipcode,
                    Address = addUser.Address1,
                    MobileNumber = addUser.ContactNumber,
                    CountryId = addUser.CountryId,
                    Address2 = addUser.Address2,
                    IsVerified = true,
                    EmailCode = addUser.EmailCode,
                    ClientId = addUser.LoginId,
                    RoleId = addUser.RoleId,
                    IsActive = addUser.Active,
                    IsDeleted = false,
                    CreatedBy = addUser.LoginId,
                    CreatedOn = DateTime.Now,
                    SiteId = addUser.SiteId,
                    IsSiteUser = addUser.IsSiteUser,
                    OperatorId = addUser.OperatorId,
                    IsMicrosoftAccount = addUser.IsMicrosoftAccount,
                    IsOperator = addUser.IsOperator
                });
                await _dbContext.SaveChangesAsync();
           
            return new { Message = "User saved successfully" };
        }

        public bool GetTenant(UpdateRegisterUserAc addRegister)
        {

            RegisterUser user = _dbContext.RegisterUsers.FirstOrDefault(x => (x.Email == addRegister.Email || x.MobileNumber == addRegister.MobileNumber) && x.IsActive == true && x.IsDeleted == false && x.RoleId == 2);
            return (user != null);

        }
        public async Task<dynamic> AddTenant(UpdateRegisterUserAc model)
        {
           
                RegisterUser user = new RegisterUser();
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Address = model.Address;
                user.City = model.City;
                user.State = model.State;
                user.ZipCode = model.Zipcode;
                user.MobileNumber = model.MobileNumber;
                user.Email = model.Email;
                user.ResidencyProofId = model.ResidencyProof;
                user.IdentityProofId = model.IdentityProof;
                user.IsVerified = true;
                user.IsApproved = false;
                user.SiteId = 0;
                user.EmailCode = model.EmailCode;
                //  user.ClientId = addUser.ParentId;
                user.RoleId = 2;
                user.IsActive = true;
                user.IsDeleted = false;
                user.CreatedBy = 1;
                user.CreatedOn = DateTime.Now;
                _dbContext.RegisterUsers.Add(user);
                _dbContext.SaveChanges();
           

            return new { Message = "User saved successfully" };
        }


        public void whitelistvehicle(int siteId, string vrm)
        {
            string apikey = string.Empty; string apiurl = string.Empty;

            apikey = _configuration["ZatparkApiKey"];
            apiurl = _configuration["ZatparkUrl"];
            //Common.InfoLogs("Config files read");
            string referenceno = string.Empty;
            string referencenew = string.Empty;
            string sitecode = string.Empty;
            string startdate = string.Empty;
            string enddate = string.Empty;
            //Common.InfoLogs("Before fetching sites");
            var sites = _dbContext.Sites.Where(x => x.Id == siteId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault(); //Util.GetSites();
            // Common.InfoLogs("site Fetched");
            if (sites != null)
            {
                bool zatparkhours = sites.Zatparklogs24hrs;
                //ArrayList array = new ArrayList();
                //array.Add(siteId);
                //array.Add(1);
                var vehicles = (from v in _dbContext.VehicleRegistrations
                                join t in _dbContext.VehicleRegistrationTimeSlots on v.Id equals t.VehicleRegistrationId
                                join u in _dbContext.RegisterUsers on v.RegisterUserId equals u.Id
                                join s in _dbContext.Sites on u.SiteId equals s.Id
                                where t.IsActive == true && t.IsDeleted == false && s.Id == siteId && v.VRM == vrm && t.IsSentToZatPark == false
                                //where c.RegisterUserId == Id && c.IsActive == true && c.IsDeleted == false
                                select new
                                {
                                    t.Id,
                                    t.FromDate,
                                    t.ToDate,
                                    VFromDate = v.StartDate,
                                    VToDate = v.EndDate,
                                    v.VRM,
                                    s.ZatparkSitecode,
                                    vehicleregid = v.Id,
                                }
                          ).FirstOrDefault();
                //Util.GetVehicleDetails(array);
                //Guid obj = Guid.NewGuid();
                //referenceno = obj.ToString();
                referenceno = "GP" + Convert.ToString(vehicles.vehicleregid);
                sitecode = vehicles.ZatparkSitecode;
                startdate = Convert.ToDateTime(vehicles.VFromDate).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(vehicles.VFromDate).ToString("HH:mm");
                enddate = Convert.ToDateTime(vehicles.VToDate).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(vehicles.VToDate).ToString("HH:mm");
                vrm = vehicles.VRM;

                string requeststr = "{\"reference_no\":\"" + referenceno + "\",\"site_code\":\"" + sitecode + "\",\"start_date\":\"" + startdate + "\",\"end_date\":\"" + enddate + "\",\"vehicle_details\":{\"vrm\":\"" + vrm + "\"}}";

                RestClient client = new RestClient(apiurl);
                RestRequest request = new RestRequest("add_permit", Method.POST);
                request.AddParameter("permit_data", "{\"reference_no\":\"" + referenceno + "\",\"site_code\":\"" + sitecode + "\",\"start_date\":\"" + startdate + "\",\"end_date\":\"" + enddate + "\",\"vehicle_details\":{\"vrm\":\"" + vrm + "\"}}");
                request.AddHeader("HTTPS_AUTH", apikey);
                var response = client.Execute(request);

                var vehicleslots = (from v in _dbContext.VehicleRegistrations
                                    join t in _dbContext.VehicleRegistrationTimeSlots on v.Id equals t.VehicleRegistrationId
                                    join u in _dbContext.RegisterUsers on v.RegisterUserId equals u.Id
                                    join s in _dbContext.Sites on u.SiteId equals s.Id
                                    where t.IsActive == true && t.IsDeleted == false && s.Id == siteId && v.VRM == vrm && t.IsSentToZatPark == false
                                    //where c.RegisterUserId == Id && c.IsActive == true && c.IsDeleted == false
                                    select new
                                    {
                                        t.Id,
                                        t.FromDate,
                                        t.ToDate,
                                        v.VRM,
                                        s.ZatparkSitecode,
                                        vehicleregid = v.Id,
                                    }
                         ).ToList();

                if (vehicleslots != null && vehicleslots.Count > 0)
                {
                    foreach (var vehicle in vehicleslots)
                    {
                        referenceno = vehicle.Id.ToString();
                        //referencenew = ds.Tables[0].Rows[i]["ReferenceNo"].ToString();
                        sitecode = vehicle.ZatparkSitecode;
                        //sitecode = "eur0011-500";
                        startdate = Convert.ToDateTime(vehicle.FromDate).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(vehicle.FromDate).ToString("HH:mm");
                        enddate = Convert.ToDateTime(vehicle.ToDate).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(vehicle.ToDate).ToString("HH:mm");
                        vrm = vehicle.VRM;
                        // Common.InfoLogs("ZatparkUpdate method was started vrm= " + vrm + " referencenew= " + referenceno + " startdate= " + startdate + " enddate= " + enddate);

                        string requeststr1 = "{\"reference_no\":\"" + referenceno + "\",\"site_code\":\"" + sitecode + "\",\"start_date\":\"" + startdate + "\",\"end_date\":\"" + enddate + "\",\"vehicle_details\":{\"vrm\":\"" + vrm + "\"}}";

                        //RestClient client1 = new RestClient(apiurl);
                        //RestRequest request1 = new RestRequest("add_permit", Method.POST);
                        //request1.AddParameter("permit_data", "{\"reference_no\":\"" + referenceno + "\",\"site_code\":\"" + sitecode + "\",\"start_date\":\"" + startdate + "\",\"end_date\":\"" + enddate + "\",\"vehicle_details\":{\"vrm\":\"" + vrm + "\"}}");
                        //request.AddHeader("HTTPS_AUTH", apikey);
                        ////var response = client.Execute(request);
                        if (response.IsSuccessful)
                        {
                            string xml = response.Content;
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xml);
                            string JsonValue = JsonConvert.SerializeXmlNode(doc);
                            //Common.InfoLogs("Zatparkapi was excuted " + JsonValue);
                            string myJsonResponse = JsonValue.Replace("?xml", "xml1");

                            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
                            if (myDeserializedClass.xml.status_code == "0" && myDeserializedClass.xml.error == "0")
                            {
                                ArrayList list = new ArrayList();
                                list.Add(Convert.ToInt32(referenceno));
                                list.Add(true);
                                list.Add(requeststr);
                                list.Add(JsonValue);
                                list.Add(myDeserializedClass.xml.message);
                                var slots = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.Id == vehicle.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                if (slots != null)
                                {
                                    //slots.ForEach(x =>
                                    //{
                                    slots.IsSentToZatPark = true;
                                    slots.ZatparkResponse = myDeserializedClass.xml.message;
                                    slots.Request = requeststr;
                                    slots.Response = JsonValue;
                                    slots.SentToZatparkDateTime = DateTime.Now;
                                    slots.UpdatedOn = DateTime.Now;
                                    slots.UpdatedBy = 1;
                                    //});

                                    _dbContext.VehicleRegistrationTimeSlots.UpdateRange(slots);
                                    _dbContext.SaveChanges();
                                }
                            }
                            if (myDeserializedClass.xml.status_code == "25" && myDeserializedClass.xml.error == "0")
                            {

                                //DataSet dataset = Util.UpdateVehicleDetails(list);
                                var slots = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.Id == vehicle.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                if (slots != null)
                                {
                                    //slots.ForEach(x =>
                                    //{
                                    slots.IsSentToZatPark = false;
                                    slots.ZatparkResponse = myDeserializedClass.xml.message;
                                    slots.Request = requeststr;
                                    slots.Response = JsonValue;
                                    slots.SentToZatparkDateTime = DateTime.Now;
                                    slots.UpdatedOn = DateTime.Now;
                                    slots.UpdatedBy = 1;
                                    //});

                                    _dbContext.VehicleRegistrationTimeSlots.UpdateRange(slots);
                                    _dbContext.SaveChanges();
                                }
                            }
                            if (myDeserializedClass.xml.status_code == "1" && myDeserializedClass.xml.error == "1")
                            {

                                //DataSet dataset = Util.UpdateVehicleDetails(list);
                                var slots = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.Id == vehicle.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                if (slots != null)
                                {
                                    //slots.ForEach(x =>
                                    //{
                                    slots.IsSentToZatPark = false;
                                    slots.ZatparkResponse = myDeserializedClass.xml.message;
                                    slots.Request = requeststr;
                                    slots.Response = JsonValue;
                                    slots.SentToZatparkDateTime = DateTime.Now;
                                    slots.UpdatedOn = DateTime.Now;
                                    slots.UpdatedBy = 1;
                                    //});

                                    _dbContext.VehicleRegistrationTimeSlots.UpdateRange(slots);
                                    _dbContext.SaveChanges();
                                }

                            }
                        }

                    }



                }
            }
        }

        public void Cancelwhitelistvehicle1(int siteId, string vrm, DateTime startdate, DateTime enddate)
        {

        }
        public void Cancelwhitelistvehicle(int siteId, string vrm)
        {
            string apikey = string.Empty; string apiurl = string.Empty;

            apikey = _configuration["ZatparkApiKey"];
            apiurl = _configuration["ZatparkUrl"];
            //Common.InfoLogs("Config files read");
            string referenceno = string.Empty;
            string referencenew = string.Empty;
            string sitecode = string.Empty;
            string startdate = string.Empty;
            string enddate = string.Empty;
            //Common.InfoLogs("Before fetching sites");
            var sites = _dbContext.Sites.Where(x => x.Id == siteId && x.IsActive == true && x.IsDeleted == false).FirstOrDefault(); //Util.GetSites();
            // Common.InfoLogs("site Fetched");
            if (sites != null)
            {
                bool zatparkhours = sites.Zatparklogs24hrs;
                //ArrayList array = new ArrayList();
                //array.Add(siteId);

                //array.Add(1);
                var vehicles = (from v in _dbContext.VehicleRegistrations
                                join t in _dbContext.VehicleRegistrationTimeSlots on v.Id equals t.VehicleRegistrationId
                                join u in _dbContext.RegisterUsers on v.RegisterUserId equals u.Id
                                join s in _dbContext.Sites on u.SiteId equals s.Id
                                where t.IsActive == true && t.IsDeleted == false && s.Id == siteId && v.VRM == vrm && t.IsSentToZatPark == true
                                //where c.RegisterUserId == Id && c.IsActive == true && c.IsDeleted == false
                                select new
                                {
                                    t.Id,
                                    t.FromDate,
                                    t.ToDate,
                                    VFromDate = v.StartDate,
                                    VToDate = v.EndDate,
                                    v.VRM,
                                    s.ZatparkSitecode,
                                    vehicleregid = v.Id,
                                    t.Response
                                }
                          ).ToList();
                //Util.GetVehicleDetails(array);


                RestClient client = new RestClient(apiurl);
                RestRequest request = new RestRequest("cancel_permit", Method.POST);
                string permitdataarray = string.Empty;
                //permitdataarray = "{";
                List<cancelwhitelist> objcancel = new List<cancelwhitelist>();
                foreach (var vehicle in vehicles)
                {

                    var root = JsonConvert.DeserializeObject<Root>(vehicle.Response);
                    referenceno = root.xml.reference_no;
                    //vrm = vehicle.VRM;
                    //permitdataarray += "[{";
                    ////permitdataarray += "{\"reference_no\":\"" + referenceno + "\",\"vehicle_details\":{\"vrm\":\"" + vrm + "\"}}";

                    //permitdataarray += "\"vrm\":\"" + vrm + ",";
                    //permitdataarray += "\"reference_no\":\"" + referenceno + "";
                    //permitdataarray += "}]";
                    objcancel.Add(new cancelwhitelist
                    {
                        vrm = vehicle.VRM,
                        reference_no = referenceno

                    });


                }
                string requeststr = "{\"reference_no\":\"" + referenceno + "\",\"vehicle_details\":{\"vrm\":\"" + vrm + "\"}}";

                // permitdataarray += "}";
                request.AddParameter("permit_data", JsonConvert.SerializeObject(objcancel.FirstOrDefault()));
                //  request.AddParameter("permit_data", "{\"reference_no\":\"" + referenceno + "\",\"site_code\":\"" + sitecode + "\",\"start_date\":\"" + startdate + "\",\"end_date\":\"" + enddate + "\",\"vehicle_details\":{\"vrm\":\"" + vrm + "\"}}");
                request.AddHeader("HTTPS_AUTH", apikey);
                var response = client.Execute(request);

                var vehicleslots = (from v in _dbContext.VehicleRegistrations
                                    join t in _dbContext.VehicleRegistrationTimeSlots on v.Id equals t.VehicleRegistrationId
                                    join u in _dbContext.RegisterUsers on v.RegisterUserId equals u.Id
                                    join s in _dbContext.Sites on u.SiteId equals s.Id
                                    where t.IsActive == true && t.IsDeleted == false && s.Id == siteId && v.VRM == vrm && t.IsSentToZatPark == true
                                    //where c.RegisterUserId == Id && c.IsActive == true && c.IsDeleted == false
                                    select new
                                    {
                                        t.Id,
                                        t.FromDate,
                                        t.ToDate,
                                        v.VRM,
                                        s.ZatparkSitecode,
                                        vehicleregid = v.Id,
                                    }
                         ).ToList();

                if (vehicleslots != null && vehicleslots.Count > 0)
                {
                    foreach (var vehicle in vehicleslots)
                    {
                        referenceno = vehicle.Id.ToString();
                        //referencenew = ds.Tables[0].Rows[i]["ReferenceNo"].ToString();
                        sitecode = vehicle.ZatparkSitecode;
                        //sitecode = "eur0011-500";
                        startdate = Convert.ToDateTime(vehicle.FromDate).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(vehicle.FromDate).ToString("HH:mm");
                        enddate = Convert.ToDateTime(vehicle.ToDate).ToString("yyyy-MM-dd") + " " + Convert.ToDateTime(vehicle.ToDate).ToString("HH:mm");
                        vrm = vehicle.VRM;
                        // Common.InfoLogs("ZatparkUpdate method was started vrm= " + vrm + " referencenew= " + referenceno + " startdate= " + startdate + " enddate= " + enddate);

                        string requeststr1 = "{\"reference_no\":\"" + referenceno + "\",\"site_code\":\"" + sitecode + "\",\"start_date\":\"" + startdate + "\",\"end_date\":\"" + enddate + "\",\"vehicle_details\":{\"vrm\":\"" + vrm + "\"}}";

                        //RestClient client1 = new RestClient(apiurl);
                        //RestRequest request1 = new RestRequest("add_permit", Method.POST);
                        //request1.AddParameter("permit_data", "{\"reference_no\":\"" + referenceno + "\",\"site_code\":\"" + sitecode + "\",\"start_date\":\"" + startdate + "\",\"end_date\":\"" + enddate + "\",\"vehicle_details\":{\"vrm\":\"" + vrm + "\"}}");
                        //request.AddHeader("HTTPS_AUTH", apikey);
                        ////var response = client.Execute(request);
                        if (response.IsSuccessful)
                        {
                            string xml = response.Content;
                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(xml);
                            string JsonValue = JsonConvert.SerializeXmlNode(doc);
                            //Common.InfoLogs("Zatparkapi was excuted " + JsonValue);
                            string myJsonResponse = JsonValue.Replace("?xml", "xml1");

                            Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
                            if (myDeserializedClass.xml.status_code == "0" && myDeserializedClass.xml.error == "0")
                            {
                                ArrayList list = new ArrayList();
                                list.Add(Convert.ToInt32(referenceno));
                                list.Add(true);
                                list.Add(requeststr);
                                list.Add(JsonValue);
                                list.Add(myDeserializedClass.xml.message);
                                var slots = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.Id == vehicle.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                if (slots != null)
                                {
                                    //slots.ForEach(x =>
                                    //{
                                    slots.IsSentToZatPark = false;
                                    slots.ZatparkResponse = myDeserializedClass.xml.message;
                                    slots.Request = requeststr1;
                                    slots.Response = JsonValue;
                                    slots.SentToZatparkDateTime = DateTime.Now;
                                    slots.UpdatedOn = DateTime.Now;
                                    slots.UpdatedBy = 1;
                                    //});

                                    _dbContext.VehicleRegistrationTimeSlots.UpdateRange(slots);
                                    _dbContext.SaveChanges();
                                }
                            }
                            if (myDeserializedClass.xml.status_code == "25" && myDeserializedClass.xml.error == "0")
                            {

                                //DataSet dataset = Util.UpdateVehicleDetails(list);
                                var slots = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.Id == vehicle.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                if (slots != null)
                                {
                                    //slots.ForEach(x =>
                                    //{
                                    slots.IsSentToZatPark = false;
                                    slots.ZatparkResponse = myDeserializedClass.xml.message;
                                    slots.Request = requeststr;
                                    slots.Response = JsonValue;
                                    slots.SentToZatparkDateTime = DateTime.Now;
                                    slots.UpdatedOn = DateTime.Now;
                                    slots.UpdatedBy = 1;
                                    //});

                                    _dbContext.VehicleRegistrationTimeSlots.UpdateRange(slots);
                                    _dbContext.SaveChanges();
                                }
                            }
                            if (myDeserializedClass.xml.status_code == "1" && myDeserializedClass.xml.error == "1")
                            {

                                //DataSet dataset = Util.UpdateVehicleDetails(list);
                                var slots = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.Id == vehicle.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                if (slots != null)
                                {
                                    //slots.ForEach(x =>
                                    //{
                                    slots.IsSentToZatPark = false;
                                    slots.ZatparkResponse = myDeserializedClass.xml.message;
                                    slots.Request = requeststr;
                                    slots.Response = JsonValue;
                                    slots.SentToZatparkDateTime = DateTime.Now;
                                    slots.UpdatedOn = DateTime.Now;
                                    slots.UpdatedBy = 1;
                                    //});

                                    _dbContext.VehicleRegistrationTimeSlots.UpdateRange(slots);
                                    _dbContext.SaveChanges();
                                }

                            }
                        }

                    }



                }
            }
        }

        public async Task<dynamic> AddTenantUser(AddTenantUser addUser)
        {
            bool iscancelwhitelist = false;
            //bool issentzatpark = false;
           
                int siteid = 0;
                if (addUser.SiteName != null)
                {
                    var sites = _dbContext.Sites.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteName.ToLower().Trim() == addUser.SiteName.ToLower().Trim()).FirstOrDefault();
                    if (sites != null)
                    {
                        siteid = sites.Id;
                    }
                    else
                    {
                        Site objsite = new Site();
                        objsite.SiteName = addUser.SiteName;
                        objsite.IsActive = true;
                        objsite.IsDeleted = false;
                        objsite.CreatedBy = 1;
                        objsite.CreatedOn = DateTime.Now;
                        _dbContext.Sites.Add(objsite);
                        _dbContext.SaveChanges();
                        siteid = objsite.Id;
                    }
                }
                if (addUser.SiteId == null)
                {
                    addUser.SiteId = siteid.ToString();
                }
                // string organisationname = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == addUser.LoginId).Select(x => x.OrganisationName).FirstOrDefault();
                RegisterUser user = new RegisterUser();

                user.FirstName = addUser.FirstName;
                user.LastName = addUser.LastName;
                user.Email = addUser.Email;
                user.State = addUser.State;
                user.City = addUser.City;
                user.ZipCode = addUser.Zipcode;
                user.Address = addUser.Address;
                user.HouseOrFlatNo = addUser.HouseOrFlatNo;
                user.MobileNumber = addUser.MobileNumber;
                user.ParkingBay = addUser.ParkingBay;
                user.SiteId = Convert.ToInt32(addUser.SiteId);
                user.IsVerified = true;
                user.IsApproved = false;
                user.EmailCode = addUser.EmailCode;
                user.ClientId = addUser.ParentId;
                user.UpdateEnddate = addUser.IsUpdateEnddate;
                user.RoleId = 2;
                user.IsActive = true;
                user.IsDeleted = false;
                user.CreatedBy = addUser.ParentId;
                user.CreatedOn = DateTime.Now;
                if (addUser.LoginId == 1)
                {
                    user.IsAdminCreated = true;

                }
                else
                {
                    user.IsAdminCreated = false;
                }

                _dbContext.RegisterUsers.Add(user);
                _dbContext.SaveChanges();
                addUser.Id = user.Id;
                if (addUser.BayConfigs != null)
                {
                    for (int i = 0; i < addUser.BayConfigs.Count; i++)
                    {
                        string startdate1 = addUser.BayConfigs[i].StartDate.ToString("yyyy-MM-dd");
                        string enddate1 = addUser.BayConfigs[i].EndDate.ToString("yyyy-MM-dd");
                        int siteid1 = Convert.ToInt32(addUser.SiteId);
                        var parkingbayno = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.BayName == addUser.BayConfigs[i].bayid && x.SiteId == siteid1).FirstOrDefault();

                        BayConfig bay = new BayConfig();
                        bay.ParkingBayNoId = parkingbayno.Id;//Convert.ToInt32(addUser.BayConfigs[i].bayid);
                        bay.RegisterUserId = user.Id;
                        bay.SiteId = Convert.ToInt32(addUser.SiteId);
                        bay.IsActive = true;
                        bay.IsDeleted = false;
                        bay.CreatedBy = 1;
                        bay.CreatedOn = DateTime.Now;
                        _dbContext.BayConfigs.Add(bay);
                        _dbContext.SaveChanges();
                        // int bayid = addUser.BayConfigs[i].bayid;


                        if (parkingbayno != null)
                        {
                            if (parkingbayno.RegisterUserId != 0)
                            {
                                //string startdate = addUser.BayConfigs[i].StartDate.ToString("yyyy-MM-dd");
                                //string enddate = addUser.BayConfigs[i].EndDate.ToString("yyyy-MM-dd");
                                //ParkingBayNo parkingobj = new ParkingBayNo();
                                //parkingobj.IsActive = true;
                                //parkingobj.IsDeleted = false;
                                //parkingobj.MaxVehiclesPerBay = Convert.ToInt32(addUser.BayConfigs[i].vehiclesperbay);
                                //parkingobj.ParkingBayId = parkingbayno.ParkingBayId;
                                //parkingobj.RegisterUserId = user.Id;
                                //parkingobj.Section = parkingbayno.Section;
                                //parkingobj.SiteId = parkingbayno.SiteId;
                                //parkingobj.StartDate = Convert.ToDateTime(startdate);
                                //parkingobj.EndDate = Convert.ToDateTime(enddate);
                                //parkingobj.BayName = parkingbayno.BayName;
                                //parkingobj.CreatedBy = 1;
                                //parkingobj.CreatedOn = DateTime.Now;
                                //parkingobj.Status = true;
                                //_dbContext.ParkingBayNos.Add(parkingobj);
                                //_dbContext.SaveChanges();
                                var isParking = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == user.Id).FirstOrDefault();
                                if (isParking != null)
                                {
                                    if (addUser.BayConfigs[i].vehiclereg != "")
                                    {
                                        VehicleRegistration vr = new VehicleRegistration();
                                        vr.IsActive = true;
                                        vr.IsDeleted = false;
                                        vr.RegisterUserId = isParking.RegisterUserId;
                                        vr.VRM = addUser.BayConfigs[i].vehiclereg;
                                        vr.CreatedOn = DateTime.Now;
                                        vr.CreatedBy = 1;
                                        vr.StartDate = Convert.ToDateTime(startdate1);
                                        vr.EndDate = Convert.ToDateTime(enddate1);
                                        vr.ParkingBayNo = isParking.Id;
                                        _dbContext.VehicleRegistrations.Add(vr);
                                        _dbContext.SaveChanges();
                                        var Id = vr.Id;
                                        var id = vr.RegisterUserId;
                                        var regvrm = vr.VRM;
                                        var bayno = parkingbayno.Id;
                                        DateTime StartDate = addUser.BayConfigs[i].StartDate;
                                        DateTime EndDate = addUser.BayConfigs[i].EndDate;
                                        if (addUser.BayConfigs[i].dates != "")
                                        {
                                            //savemutliplevehciledates(addUserAc.Id, addUserAc.BayConfigs[i].dates, StartDate, EndDate, Issavecount, id, Convert.ToInt32(bayno));
                                            savemutliplevehciledates(Id, addUser.BayConfigs[i].dates, StartDate, EndDate, 0, id, bayno);

                                        }
                                        else
                                        {
                                            savemutliplevehciletime(Id, StartDate, EndDate, 0);
                                        }
                                        var vrmcompare = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == addUser.Id && x.ParkingBayNo == bayno).FirstOrDefault();
                                        if (vrmcompare != null)
                                        {
                                            if (addUser.BayConfigs[i].StartDate == vrmcompare.StartDate && addUser.BayConfigs[i].EndDate == vrmcompare.EndDate && addUser.BayConfigs[i].vehiclereg == vrmcompare.VRM)
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
                                        if (iscancelwhitelist == true)
                                        {
                                            Cancelwhitelistvehicle(Convert.ToInt32(addUser.SiteId), regvrm);
                                            whitelistvehicle(Convert.ToInt32(addUser.SiteId), regvrm);

                                        }
                                        else
                                        {
                                            whitelistvehicle(Convert.ToInt32(addUser.SiteId), regvrm);
                                        }
                                        // whitelistvehicle(Convert.ToInt32(addUser.SiteId), vr.VRM);

                                    }



                                }


                            }
                            else
                            {
                                string startdate = addUser.BayConfigs[i].StartDate.ToString("yyyy-MM-dd");
                                string enddate = addUser.BayConfigs[i].EndDate.ToString("yyyy-MM-dd");
                                parkingbayno.RegisterUserId = user.Id;
                                parkingbayno.StartDate = Convert.ToDateTime(startdate);
                                parkingbayno.EndDate = Convert.ToDateTime(enddate);
                                parkingbayno.MaxVehiclesPerBay = Convert.ToInt32(addUser.BayConfigs[i].vehiclesperbay);
                                parkingbayno.UpdatedBy = 1;
                                parkingbayno.UpdatedOn = DateTime.Now;
                                _dbContext.ParkingBayNos.Update(parkingbayno);
                                _dbContext.SaveChanges();
                                var isParking = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.BayName == addUser.BayConfigs[i].bayid).FirstOrDefault();
                                VehicleRegistration vr = null;
                                if (isParking != null)
                                {
                                    if (addUser.BayConfigs[i].vehiclereg != "")
                                    {
                                        vr = new VehicleRegistration();

                                        vr.IsActive = true;
                                        vr.IsDeleted = false;
                                        vr.RegisterUserId = isParking.RegisterUserId;
                                        vr.VRM = addUser.BayConfigs[i].vehiclereg;
                                        vr.CreatedOn = DateTime.Now;
                                        vr.CreatedBy = 1;
                                        vr.StartDate = Convert.ToDateTime(startdate1);
                                        vr.EndDate = Convert.ToDateTime(enddate1);
                                        vr.ParkingBayNo = isParking.Id;
                                        _dbContext.VehicleRegistrations.Add(vr);
                                        _dbContext.SaveChanges();
                                        //   var vehicleregistered = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == vr.RegisterUserId && x.VRM == vr.VRM  && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                        //if (vehicleregistered != null)
                                        //{
                                        var Id = vr.Id;
                                        var id = vr.RegisterUserId;
                                        var bayno = parkingbayno.Id;
                                        var siteparkingbayno = vr.ParkingBayNo;
                                        var regvrm = vr.VRM;
                                        DateTime StartDate = addUser.BayConfigs[i].StartDate;
                                        DateTime EndDate = addUser.BayConfigs[i].EndDate;
                                        if (addUser.BayConfigs[i].dates != "")
                                        {
                                            //savemutliplevehciledates(addUserAc.Id, addUserAc.BayConfigs[i].dates, StartDate, EndDate, Issavecount, id, Convert.ToInt32(bayno));
                                            savemutliplevehciledates(Id, addUser.BayConfigs[i].dates, StartDate, EndDate, 0, id, bayno);

                                        }
                                        else
                                        {
                                            savemutliplevehciletime(Id, StartDate, EndDate, 0);
                                        }
                                        var vrmcompare = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == addUser.Id && x.ParkingBayNo == siteparkingbayno).FirstOrDefault();
                                        if (vrmcompare != null)
                                        {
                                            if (addUser.BayConfigs[i].StartDate == vrmcompare.StartDate && addUser.BayConfigs[i].EndDate == vrmcompare.EndDate && addUser.BayConfigs[i].vehiclereg == vrmcompare.VRM)
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
                                        if (iscancelwhitelist == true)
                                        {
                                            Cancelwhitelistvehicle(Convert.ToInt32(addUser.SiteId), regvrm);
                                            whitelistvehicle(Convert.ToInt32(addUser.SiteId), regvrm);

                                        }
                                        else
                                        {
                                            whitelistvehicle(Convert.ToInt32(addUser.SiteId), regvrm);
                                        }
                                    }

                                    // }
                                }



                            }
                        }

                    }
                }
            
            return new { Message = "User saved successfully" };
        }


        public async Task<dynamic> DeleteUser(int Id)
        {
            RegisterUser user = await _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.Id == Id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.IsDeleted = true;
                user.IsActive = false;
                user.UpdatedOn = DateTime.Now;
                _dbContext.RegisterUsers.Update(user);
                await _dbContext.SaveChangesAsync();
                return new { Message = "User deleted successfully" };

            }
            else
            {
                return new { Message = "No data found" };
            }
        }

        public bool GetExistsTenantUser(AddTenantUser addUserAc)
        {
            RegisterUser user = _dbContext.RegisterUsers.FirstOrDefault(x => (x.Email == addUserAc.Email || x.MobileNumber == addUserAc.MobileNumber) && x.IsDeleted == false && x.RoleId == 2);
            return (user != null);

        }
        public bool GetExistsUser(AddUserAc addUserAc)
        {
            RegisterUser user = _dbContext.RegisterUsers.FirstOrDefault(x => (x.Email == addUserAc.Email || x.MobileNumber == addUserAc.ContactNumber) && x.IsDeleted == false && x.RoleId == addUserAc.RoleId);
            return (user != null);

        }
        public async Task<dynamic> GetTenantUserById(int Id)
          {
            int siteId = 0;
            var user1 = _dbContext.RegisterUsers.FirstOrDefault(x => x.Id == Id);
            if (user1 != null)
            {
                siteId = user1.SiteId;
            }

            var user = (from r in _dbContext.RegisterUsers
                        where r.Id == Id
                        select new
                        {
                            r.FirstName,
                            r.HouseOrFlatNo,
                            r.ProfilePath,
                            r.Address,
                            r.Address2,
                            r.City,
                            r.ClientId,
                            r.CountryId,
                            r.CreatedBy,
                            r.CreatedOn,
                            r.Email,
                            r.EmailCode,
                            r.Id,
                            r.IsActive,
                            r.IsDeleted,
                            r.IsVerified,
                            r.LastName,
                            r.MobileNumber,
                            r.ParentId,
                            r.IsApproved,
                            r.ParkingBay,
                            r.RoleId,
                            r.SiteId,
                            r.State,
                            r.ZipCode,
                            r.ResidencyProofId,
                            r.IdentityProofId,
                            r.UpdateEnddate,
                            BaysConfig = (from c in _dbContext.ParkingBayNos
                                          where (c.RegisterUserId == Id || c.UpdatedBy == Id)

                         && c.IsActive == true && c.IsDeleted == false
                                          select new
                                          {
                                              bayconfigid = c.Id,
                                              c.IsActive,
                                              c.IsDeleted,
                                              MaxVehiclesPerBay = c.MaxVehiclesPerBay.ToString(),
                                              bayid = c.Id.ToString(),
                                              status = 1,
                                              c.RegisterUserId,
                                              c.StartDate,
                                              c.EndDate,

                                              // Fetch only vehicles for the corresponding bay
                                              //vehiclereg = (from z in _dbContext.VehicleRegistrations
                                              //              where z.RegisterUserId == Id && z.IsActive == true && z.IsDeleted == false
                                              //              && z.ParkingBayNo == c.Id
                                              //              select new 
                                              //              {
                                              //                  z.VRM,
                                              //              }).ToList(),
                                               vehiclereg = _dbContext.VehicleRegistrations
                                                .Where(z => z.RegisterUserId == Id && z.IsActive && !z.IsDeleted && z.ParkingBayNo == c.Id)
                                                 .Select(z => z.VRM)
                                                 .ToList(),

            // Fetch only bay numbers related to this bayconfig
            baynos = (from b in _dbContext.ParkingBayNos
                                                        where b.IsActive == true && b.IsDeleted == false
                                                        && b.Id == c.Id // Ensure only related bay is included
                                                        select new
                                                        {
                                                            bayName = b.BayName,
                                                            b.ParkingBayId,
                                                            b.SiteId,
                                                            b.Section,
                                                            b.RegisterUserId,
                                                            b.MaxVehiclesPerBay,
                                                            status = false,
                                                            b.StartDate,
                                                            b.EndDate,
                                                            bayConfigs = (object)null, // Placeholder if needed
                                                            b.Id,
                                                            b.IsActive,
                                                            b.IsDeleted,
                                                            b.CreatedOn,
                                                            b.CreatedBy,
                                                            b.UpdatedOn,
                                                            b.UpdatedBy
                                                        }).ToList()
                                          }).ToList()
                        }).FirstOrDefault();

            return user;

        }



        //public async Task<dynamic> GetTenantUserById(int Id)
        //{
        //    int siteId = 0;
        //    var user1 = _dbContext.RegisterUsers.Where(x => x.Id == Id).FirstOrDefault();
        //    if (user1 != null)
        //    {
        //        siteId = user1.SiteId;
        //    }
        //    var user = (from r in _dbContext.RegisterUsers
        //                where r.Id == Id
        //                select new
        //                {
        //                    r.FirstName,
        //                    r.HouseOrFlatNo,
        //                    r.ProfilePath,
        //                    r.Address,
        //                    r.Address2,
        //                    r.City,
        //                    r.ClientId,
        //                    r.CountryId,
        //                    r.CreatedBy,
        //                    r.CreatedOn,
        //                    r.Email,
        //                    r.EmailCode,
        //                    r.Id,
        //                    r.IsActive,
        //                    r.IsDeleted,
        //                    r.IsVerified,
        //                    r.LastName,
        //                    r.MobileNumber,
        //                    r.ParentId,
        //                    r.ParkingBay,
        //                    r.RoleId,
        //                    r.SiteId,
        //                    r.State,
        //                    r.ZipCode,
        //                    r.ResidencyProofId,
        //                    r.IdentityProofId,
        //                    r.UpdateEnddate,
        //                    BaysConfig = (from c in _dbContext.ParkingBayNos
        //                                      //join z in _dbContext.VehicleRegistrations on c.RegisterUserId equals z.RegisterUserId
        //                                  where c.RegisterUserId == Id && c.IsActive == true && c.IsDeleted == false
        //                                  select new
        //                                  {
        //                                      bayconfigid = c.Id,
        //                                      c.IsActive,
        //                                      c.IsDeleted,
        //                                      MaxVehiclesPerBay = c.MaxVehiclesPerBay.ToString(),
        //                                      bayid = c.Id.ToString(),
        //                                      status = 1,//_dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParkingBayNo == c.Id).FirstOrDefault().Id,
        //                                      c.RegisterUserId,
        //                                      c.StartDate,
        //                                      c.EndDate,
        //                                      vehiclereg = (from z in _dbContext.VehicleRegistrations
        //                                                    where z.RegisterUserId == Id && z.IsActive == true && z.IsDeleted == false
        //                                                    select new
        //                                                    {
        //                                                        z.VRM,
        //                                                    }).ToList(),
        //                                      //z.VRM,


        //                                      baynos = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && (x.RegisterUserId == Id || x.RegisterUserId == 0) && x.SiteId == siteId).ToList()
        //                                  }).ToList()
        //                }).FirstOrDefault();
        //    // RegisterUser user = await _dbContext.RegisterUsers.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == Id);
        //    return user;
        //}
        public async Task<RegisterUser> GetUserById(int Id)
        {
            RegisterUser user = await _dbContext.RegisterUsers.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == Id);
            return user;
        }
        public async Task<dynamic> GetTenantUsers(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            if (RoleId == 1)
            {
                if (PageSize == 0)
                {
                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId == 2).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users1 = (from l in _dbContext.RegisterUsers
                                  join r in _dbContext.Roles on l.RoleId equals r.Id
                                  //join v in _dbContext.VehicleRegistrations on l.Id equals v.RegisterUserId
                                  where l.IsDeleted == false && l.RoleId == 2
                                  //&& v.IsDeleted==false
                                  select new
                                  {
                                      pageNo = PageNo,
                                      l.Id,
                                      l.FirstName,
                                     
                                      l.LastName,
                                      l.IsApproved,
                                      l.Email,
                                      l.MobileNumber,
                                      l.RoleId,
                                      RoleName = r.Name,
                                      VRM = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == l.Id && x.IsDeleted == false).FirstOrDefault().VRM == null ? " " : _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == l.Id && x.IsDeleted == false).FirstOrDefault().VRM,
                                      SiteName = _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName == null ? " " : _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName
                                  }).OrderByDescending(x => x.Id).ToList();
                    users1 = users1.Skip(count2).Take(count1).ToList();
                    return users1;
                }
                else
                {
                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId == 2).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id
                                 //join v in _dbContext.VehicleRegistrations on l.Id equals v.RegisterUserId
                                 // join s in _dbContext.Sites on l.SiteId equals s.Id
                                 where l.IsDeleted == false
                                 //&& v.IsActive == true && l.IsDeleted==false
                                 //&& s.IsDeleted == false && s.IsActive == true
                                 && l.RoleId == 2
                                 select new
                                 {
                                     pageNo = PageNo,
                                     l.Id,
                                     l.FirstName,
                                    
                                     l.LastName,
                                     l.IsApproved,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     VRM = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == l.Id && x.IsDeleted == false).FirstOrDefault().VRM == null ? " " : _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == l.Id && x.IsDeleted == false).FirstOrDefault().VRM,
                                     SiteName = _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName == null ? " " : _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName
                                 }).OrderByDescending(x => x.Id).ToList();
                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                }
            }
            else
            {
                int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId == 2 && x.SiteId == SiteId).Count();
                double totalpa = (double)totalitems / (double)PageSize;
                double totalpage = Math.Round(totalpa);
                var users = (from l in _dbContext.RegisterUsers
                             join r in _dbContext.Roles on l.RoleId equals r.Id
                             //join v in _dbContext.VehicleRegistrations on l.Id equals v.RegisterUserId
                             //join s in _dbContext.Sites on l.SiteId equals s.Id
                             where l.IsDeleted == false
                             //&& v.IsDeleted == false
                             //&& s.IsDeleted == false && s.IsActive == true
                             && l.RoleId == 2
                             //&& s.Id == SiteId
                             select new
                             {
                                 pageNo = PageNo,
                                 l.Id,
                                 l.FirstName,
                                
                                 l.LastName,
                                 l.IsApproved,
                                 l.Email,
                                 l.MobileNumber,
                                 l.RoleId,
                                 RoleName = r.Name,
                                 TotalItem = totalitems,
                                 SiteId = l.SiteId,
                                 TotalPage = totalpage + 1,
                                 VRM = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == l.Id && x.IsDeleted == false).FirstOrDefault().VRM == null ? " " : _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == l.Id && x.IsDeleted == false).FirstOrDefault().VRM,
                                 SiteName = _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName == null ? " " : _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName
                             }).OrderByDescending(x => x.Id).ToList();
                users = users.Where(x => x.SiteId == SiteId).ToList();
                users = users.Skip(count2).Take(count1).ToList();
                return users;
            }

        }
        public async Task<dynamic> GetSearchTenants(int PageNo, int PageSize, string FirstName, string LastName, string Email, string MobileNumber, string SiteName, int SiteId, string VRM)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            List<GetSearchTenantCls> list = new List<GetSearchTenantCls>();
            ArrayList array = new ArrayList();
            array.Add(FirstName);
            array.Add(LastName);
            array.Add(Email);
            array.Add(MobileNumber);
            array.Add(SiteName);
            if (SiteId == 0)
            {
                array.Add(null);
            }
            else
            {
                array.Add(SiteId);
            }
            array.Add(VRM);
            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.GetSearchTenants(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int totalitems = ds.Tables[0].Rows.Count;
                double totalpa = (double)totalitems / (double)PageSize;
                double totalpage = Math.Round(totalpa);
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    list.Add(new GetSearchTenantCls
                    {
                        pageNo = PageNo,
                        TotalItem = totalitems,
                        TotalPage = totalpage + 1,
                        Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                        RoleId = Convert.ToInt32(ds.Tables[0].Rows[i]["RoleId"]),
                        RoleName = ds.Tables[0].Rows[i]["RoleName"].ToString(),
                        Email = ds.Tables[0].Rows[i]["Email"].ToString(),
                        FirstName = ds.Tables[0].Rows[i]["FirstName"].ToString(),
                        LastName = ds.Tables[0].Rows[i]["LastName"].ToString(),
                        MobileNumber = ds.Tables[0].Rows[i]["MobileNumber"].ToString(),
                        SiteName = ds.Tables[0].Rows[i]["SiteName"].ToString(),
                        VRM = ds.Tables[0].Rows[i]["VRM"].ToString()
                    });
                }
            }
            list = list.Skip(count2).Take(count1).ToList();
            return list;

        }
        public async Task<dynamic> GetSearchUsers(int PageNo, int PageSize, string FirstName, string LastName, string Email, string SiteName, int LoginId, int RoleId, int SiteId)
        {
            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            List<GetSearchUserCls> list = new List<GetSearchUserCls>();
            ArrayList array = new ArrayList();
            array.Add(FirstName);
            array.Add(LastName);
            array.Add(Email);
            array.Add(SiteName);
            if (RoleId == 1)
            {
                array.Add('1');
                array.Add('1');
            }
            else
            {
                array.Add(RoleId.ToString());
                array.Add(LoginId.ToString());
            }
            array.Add(SiteId.ToString());
            AdminBusiness business = new AdminBusiness();
            string strConnection = _configuration["ConnectionStrings:DefaultConnection"];
            DataSet ds = business.GetSearchUsers(array, strConnection);
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                int totalitems = ds.Tables[0].Rows.Count;
                double totalpa = (double)totalitems / (double)PageSize;
                double totalpage = Math.Round(totalpa);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    list.Add(new GetSearchUserCls
                    {
                        TotalItem = totalitems,
                        TotalPage = totalpage + 1,
                        Id = Convert.ToInt32(ds.Tables[0].Rows[i]["Id"]),
                        RoleId = Convert.ToInt32(ds.Tables[0].Rows[i]["RoleId"]),
                        RoleName = ds.Tables[0].Rows[i]["RoleName"].ToString(),
                        Email = ds.Tables[0].Rows[i]["Email"].ToString(),
                        FirstName = ds.Tables[0].Rows[i]["FirstName"].ToString(),
                        LastName = ds.Tables[0].Rows[i]["LastName"].ToString(),
                        MobileNumber = ds.Tables[0].Rows[i]["MobileNumber"].ToString(),
                        SiteName = ds.Tables[0].Rows[i]["SiteName"].ToString()

                    });
                }
            }

            list = list.Skip(count2).Take(count1).ToList();
            return list;
        }
        public async Task<dynamic> GetUsers(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {

            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            if (RoleId == 1)
            {
                if (SiteId == 0)
                {
                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId != 2).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id

                                 where l.IsDeleted == false && l.RoleId != 2
                                 select new
                                 {
                                     l.Id,
                                     l.FirstName,
                                     l.LastName,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     SiteName = _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName == null ? "NA" : _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName
                                 }).OrderByDescending(x => x.Id).ToList();

                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                }

                else
                {
                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId != 2 && x.SiteId == SiteId).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id
                                 where l.IsDeleted == false && l.RoleId == 2 && l.SiteId == SiteId
                                 select new
                                 {
                                     l.Id,
                                     l.FirstName,
                                     l.IsMicrosoftAccount,
                                     l.IsOperator,
                                     l.OperatorId,
                                     l.LastName,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     l.BayConfigs,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     SiteName = _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName == null ? "NA" : _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName
                                 }).OrderByDescending(x => x.Id).ToList();

                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                }
            

            }
            else
            {
                if (SiteId == 0)
                {
                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId == RoleId).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id
                                 where l.IsDeleted == false && l.RoleId == RoleId && l.RoleId != 2
                                 select new
                                 {
                                     l.Id,
                                     l.FirstName,
                                     l.IsMicrosoftAccount,
                                     l.IsOperator,
                                     l.OperatorId,
                                     l.LastName,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     SiteName = _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName == null ? "NA" : _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName
                                 }).OrderByDescending(x => x.Id).ToList();
                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                }
                else
                {
                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId == RoleId && x.SiteId == SiteId).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id
                                 join s in _dbContext.Sites on l.SiteId equals s.Id
                                 where l.IsDeleted == false && l.RoleId == RoleId && s.Id == SiteId && l.RoleId != 2
                                 select new
                                 {
                                     l.Id,
                                     l.FirstName,
                                     l.IsMicrosoftAccount,
                                     l.IsOperator,
                                     l.OperatorId,
                                     l.LastName,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     s.SiteName
                                 }).OrderByDescending(x => x.Id).ToList();
                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                }
            }
        }


        public async Task<dynamic> GetSiteUser(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {

            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            if (RoleId == 1)
            {
                

                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId != 2 && x.IsSiteUser == true).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id

                                 where l.IsDeleted == false && l.RoleId != 2 && l.IsSiteUser == true 
                                 select new
                                 {
                                     l.Id,
                                     l.FirstName,
                                     l.IsMicrosoftAccount,
                                     l.IsOperator,
                                     l.OperatorId,
                                     l.LastName,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     SiteName = _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName == null ? "NA" : _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName
                                 }).OrderByDescending(x => x.Id).ToList();

                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                

            }
            else
            {
                if (SiteId == 0)
                {
                    var operatordet = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.Id == LoginId).FirstOrDefault();

                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId == RoleId && x.IsSiteUser == true).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id
                                 where l.IsDeleted == false && l.OperatorId == operatordet.OperatorId && l.RoleId != 2 && l.IsSiteUser == true
                                 select new
                                 {
                                     l.Id,
                                     l.FirstName,
                                     l.IsMicrosoftAccount,
                                     l.IsOperator,
                                     l.OperatorId,
                                     l.LastName,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     SiteName = _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName == null ? "NA" : _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName
                                 }).OrderByDescending(x => x.Id).ToList();
                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                }
                else
                {
                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId == RoleId && x.SiteId == SiteId && x.IsSiteUser == true).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id
                                 join s in _dbContext.Sites on l.SiteId equals s.Id
                                 where l.IsDeleted == false && l.RoleId == RoleId && s.Id == SiteId && l.RoleId != 2 && l.IsSiteUser == true
                                 select new
                                 {
                                     l.Id,
                                     l.FirstName,
                                     l.IsMicrosoftAccount,
                                     l.IsOperator,
                                     l.OperatorId,
                                     l.LastName,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     s.SiteName
                                 }).OrderByDescending(x => x.Id).ToList();
                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                }
            }
        }

        public async Task<dynamic> GetOpeartoruser(int PageNo, int PageSize, int LoginId, int RoleId, int SiteId)
        {

            int count1 = PageSize;
            int count2 = count1 * PageNo - count1;
            if (RoleId == 1)
            {
              

                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId != 2 && x.IsOperator == true).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id

                                 where l.IsDeleted == false && l.RoleId != 2 && l.IsOperator==true
                                 select new
                                 {
                                     l.Id,
                                     l.FirstName,
                                     l.IsMicrosoftAccount,
                                     l.IsOperator,
                                     l.OperatorId,
                                     l.LastName,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     SiteName = _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName == null ? "NA" : _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName
                                 }).OrderByDescending(x => x.Id).ToList();

                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                

            }
            else
            {
                if (SiteId == 0)
                {
                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId == RoleId && x.IsOperator == true).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id
                                 where l.IsDeleted == false && l.RoleId == RoleId && l.RoleId != 2 && l.IsOperator == true
                                 select new
                                 {
                                     l.Id,
                                     l.FirstName,
                                     l.IsMicrosoftAccount,
                                     l.IsOperator,
                                     l.OperatorId,
                                     l.LastName,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     SiteName = _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName == null ? "NA" : _dbContext.Sites.Where(x => x.Id == l.SiteId).FirstOrDefault().SiteName
                                 }).OrderByDescending(x => x.Id).ToList();
                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                }
                else
                {
                    int totalitems = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.RoleId == RoleId && x.SiteId == SiteId && x.IsOperator == true).Count();
                    double totalpa = (double)totalitems / (double)PageSize;
                    double totalpage = Math.Round(totalpa);
                    var users = (from l in _dbContext.RegisterUsers
                                 join r in _dbContext.Roles on l.RoleId equals r.Id
                                 join s in _dbContext.Sites on l.SiteId equals s.Id
                                 where l.IsDeleted == false && l.RoleId == RoleId && s.Id == SiteId && l.RoleId != 2 && l.IsOperator == true
                                 select new
                                 {
                                     l.Id,
                                     l.FirstName,
                                     l.IsMicrosoftAccount,
                                     l.IsOperator,
                                     l.OperatorId,
                                     l.LastName,
                                     l.Email,
                                     l.MobileNumber,
                                     l.RoleId,
                                     RoleName = r.Name,
                                     TotalItem = totalitems,
                                     TotalPage = totalpage + 1,
                                     s.SiteName
                                 }).OrderByDescending(x => x.Id).ToList();
                    users = users.Skip(count2).Take(count1).ToList();
                    return users;
                }
            }
        }
        public async Task<dynamic> UpdateUserProfile(UpdateRegisterUser objinput)
        {
            int Id = objinput.Id;
            RegisterUser user = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.Id == Id).FirstOrDefault();
            if (user != null)
            {
                user.FirstName = objinput.FirstName;
                user.LastName = objinput.LastName;
                user.Email = objinput.Email;
                user.MobileNumber = objinput.MobileNumber;
                user.State = objinput.State;
                user.City = objinput.City;
                user.Address = objinput.Address1;
                user.Address2 = objinput.Address2;
                user.CountryId = objinput.CountryId;
                user.ZipCode = objinput.Zipcode;
                user.UpdatedOn = DateTime.Now;
                if (objinput.ProfilePath != null)
                {
                    user.ProfilePath = objinput.ProfilePath;

                }

                _dbContext.RegisterUsers.Update(user);
                _dbContext.SaveChanges();
            }
            return new { Message = "Updated Successfully" };
        }

        public async Task<dynamic> UpdateProfile(UpdateRegisterUserAc objinput)
        {
            int Id = objinput.Id;
            RegisterUser user = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.Id == Id).FirstOrDefault();
            if (user != null)
            {
                user.FirstName = objinput.FirstName;
                user.LastName = objinput.LastName;
                user.Email = objinput.Email;
                user.MobileNumber = objinput.MobileNumber;
                user.State = objinput.State;
                user.City = objinput.City;
                user.Address = objinput.Address;
                user.ZipCode = objinput.Zipcode;
                user.UpdatedOn = DateTime.Now;
                if (objinput.ProfilePath != null)
                {
                    user.ProfilePath = objinput.ProfilePath;
                }
                if (objinput.ResidencyProof != null)
                {
                    user.ResidencyProofId = objinput.ResidencyProof;
                }

                if (objinput.IdentityProof != null)
                {
                    user.IdentityProofId = objinput.IdentityProof;
                }

                user.IsActive = true;
                _dbContext.RegisterUsers.Update(user);
                _dbContext.SaveChanges();
            }
            return new { Message = "Updated Successfully" };
        }
        //public async Task<dynamic> UpdateProfileUploads(UpdateRegisterUserAc objinput)
        //{



        //        int Id = objinput.Id;
        //        RegisterUser user = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.Id == Id).FirstOrDefault();
        //        if (user != null)
        //        {
        //            if (objinput.ResidencyProof != null)
        //            {
        //                user.ResidencyProofId = objinput.ResidencyProof;
        //            }

        //            if (objinput.IdentityProof != null)
        //            {
        //                user.IdentityProofId = objinput.IdentityProof;
        //            }

        //            user.IsActive = true;
        //            _dbContext.RegisterUsers.Update(user);
        //            _dbContext.SaveChanges();

        //        }
        //        return new { Message = "Updated Successfully" };



        //}

        public async Task<dynamic> UpdateProfileUploads(UpdateRegisterUserAc objinput)
        {
            int Id = objinput.Id;
            RegisterUser user = _dbContext.RegisterUsers
                .Where(x => x.IsDeleted == false && x.Id == Id)
                .FirstOrDefault();

            if (user != null)
            {
                if (objinput.ResidencyProof != null)
                {
                    user.ResidencyProofId = objinput.ResidencyProof;
                }

                if (objinput.IdentityProof != null)
                {
                    user.IdentityProofId = objinput.IdentityProof;
                }

                user.IsActive = true;
                _dbContext.RegisterUsers.Update(user);
                _dbContext.SaveChanges();



                var folderNameObj = Path.Combine("EmailHtml", "AdminUploadDocuments.html");
                var filePathObj = Path.Combine(Directory.GetCurrentDirectory(), folderNameObj);

                StreamReader reader = new StreamReader(filePathObj);

                string readFile = reader.ReadToEnd();
                string myString = "";
                myString = readFile;
                string dt = DateTime.Now.ToString("MM.dd.yyyy");
                myString = myString.Replace("%{#{Datetime}#}%", dt);
                myString = myString.Replace("%{#{Name}#}%", user.FirstName + " " + user.LastName);
                string body = myString;

                bool emailSent =await SendEmailAsync(_configuration["AdminTenantMail"], user.FirstName, "Tenant Verification", body, "GOPERMIT_Tenant Verification");

                //bool emailSent = SendEmail(_configuration["AdminTenantMail"], "Go Permit New Customer Application", body, "Go Permit New Customer Application");
               
                if (!emailSent)
                {
                    return new { Message = "Profile updated, but email sending failed" };
                }

                return new { Message = "Updated Successfully and Email Sent" };
            }

            return new { Message = "User Not Found" };
        }

        public async Task<bool> SendEmailAdminAsync(string EmailId, string Subject, string Body, string Headeraname)
        {
            try
            {
                // Get OAuth credentials from appsettings.json
                string clientId = _configuration["OAuthClientId"];
                string clientSecret = _configuration["OAuthClientSecret"];
                string refreshToken = _configuration["OAuthRefreshToken"];

                // Get Access Token using the Refresh Token
                string accessToken = await GetAccessToken(clientId, clientSecret, refreshToken);

                if (string.IsNullOrEmpty(accessToken))
                {
                    throw new Exception("Failed to retrieve access token.");
                }

                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = _configuration["Host"]; // smtp.gmail.com
                    client.Port = Convert.ToInt32(_configuration["Port"]); // 587
                    client.EnableSsl = true;
                    client.UseDefaultCredentials = false;

                    // Use OAuth2 authentication instead of password
                    client.Credentials = new NetworkCredential(_configuration["AdminMail"], accessToken);

                    MailMessage mailMessage = new MailMessage
                    {
                        From = new MailAddress(_configuration["AdminMail"], Headeraname),
                        Subject = Subject,
                        Body = Body,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(new MailAddress(EmailId));

                    client.Send(mailMessage);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending email: " + ex.Message);
                return false;
            }
        }


        //public bool SendEmailAdmin(string EmailId, string Subject, string Body, string Headeraname)
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


        public async Task<dynamic> UpdateUserStatus(int Id)
        {
            
                var user = _dbContext.RegisterUsers
                    .Where(x => !x.IsDeleted && x.Id == Id)
                    .FirstOrDefault();

                if (user == null)
                {
                    return new { Status = "-100", Message = "User not found" };
                }

                user.IsApproved = true;
                user.IsActive = true;

                _dbContext.RegisterUsers.Update(user);
                await _dbContext.SaveChangesAsync();

                return new { Message = "User status updated successfully" };
           
        }

        public async Task<dynamic> UpdateTenantUser_New(AddTenantUser addUserAc)
        {
            RegisterUser user = _dbContext.RegisterUsers
                .Where(x => x.IsDeleted == false && x.Id == addUserAc.Id)
                .FirstOrDefault();

            if (user != null)
            {
                bool bFlag = UpdateUserInTenant(addUserAc, user);
                // Check if a record exists in the VehicleRegistration table for the given user
                var existingVehicle = _dbContext.VehicleRegistrations
                    .Where(v => v.RegisterUserId == user.Id)
                    .ToList();


                if (existingVehicle != null && existingVehicle.Count > 0)
                {
                    foreach (var item in existingVehicle)
                    {
                        item.IsDeleted = true;
                        item.IsActive = false;
                        item.UpdatedOn = DateTime.Now;
                        item.UpdatedBy = user.Id;
                        _dbContext.VehicleRegistrations.Update(item);
                        await _dbContext.SaveChangesAsync();
                    }

                }
                for (int i = 0; i < addUserAc.BayConfigs.Count; i++)
                {
                    var bayConfig = addUserAc.BayConfigs[i];

                    
                    int bayno = Convert.ToInt32(addUserAc.BayConfigs[i].bayid);
                    var newVehicle = new VehicleRegistration();
                    newVehicle.IsActive = true;
                    newVehicle.IsDeleted = false;
                    newVehicle.IsSaveCount = 1;
                    newVehicle.RegisterUserId = user.Id;
                    newVehicle.VRM = addUserAc.BayConfigs[i].vehiclereg;
                    newVehicle.CreatedOn = DateTime.Now;
                    newVehicle.CreatedBy = addUserAc.ParentId;
                    newVehicle.StartDate = Convert.ToDateTime(bayConfig.StartDate);
                    newVehicle.EndDate = Convert.ToDateTime(bayConfig.EndDate);
                    newVehicle.ParkingBayNo = bayno;

                    _dbContext.VehicleRegistrations.Add(newVehicle);
                    await _dbContext.SaveChangesAsync();
                    var parkingBay = _dbContext.ParkingBayNos
               .Where(pb => pb.Id == bayno && pb.IsDeleted == false && pb.IsActive == true)
               .FirstOrDefault();

                    if (parkingBay != null)
                    {
                        parkingBay.Status = true;
                        parkingBay.StartDate = Convert.ToDateTime(addUserAc.BayConfigs[i].StartDate);
                        parkingBay.EndDate = Convert.ToDateTime(addUserAc.BayConfigs[i].EndDate);
                        parkingBay.IsActive = true;
                        parkingBay.IsDeleted = false;
                        parkingBay.UpdatedBy = user.Id;
                        parkingBay.UpdatedOn = DateTime.Now;
                        _dbContext.ParkingBayNos.Update(parkingBay);
                        await _dbContext.SaveChangesAsync();
                    }
                    if (!string.IsNullOrEmpty(bayConfig.dates))
                    {
                        savemutliplevehciledates(newVehicle.Id, bayConfig.dates, newVehicle.StartDate, newVehicle.EndDate, 0, user.Id, bayno);
                    }
                    else
                    {
                        savemutliplevehciletime(newVehicle.Id, newVehicle.StartDate, newVehicle.EndDate, 0);
                    }

                    whitelistvehicle(Convert.ToInt32(addUserAc.SiteId), newVehicle.VRM);


                }

                return new { Message = "Tenant user updated successfully" };
            }

            return new { Message = "No data found" };
        }


        private bool UpdateUserInTenant(AddTenantUser addUserAc, RegisterUser user)
        {

           

                user.FirstName = addUserAc.FirstName;
                user.LastName = addUserAc.LastName;
                user.HouseOrFlatNo = addUserAc.HouseOrFlatNo;
                user.Email = addUserAc.Email;
                user.MobileNumber = addUserAc.MobileNumber;
                user.State = addUserAc.State;
                user.City = addUserAc.City;
                user.Address = addUserAc.Address;
                //user.SiteId = Convert.ToInt32(addUserAc.SiteId);
                //user.ParkingBay = addUserAc.ParkingBay;
                user.ZipCode = addUserAc.Zipcode;
                user.UpdatedOn = DateTime.Now;
                user.ClientId = addUserAc.ParentId;
                user.IsActive = true;
                user.UpdateEnddate = addUserAc.IsUpdateEnddate;
                if (addUserAc.LoginId == 1)
                {
                    user.IsAdminCreated = true;

                }
                else
                {
                    user.IsAdminCreated = false;
                }
                _dbContext.RegisterUsers.Update(user);
                _dbContext.SaveChanges();

                return true;
           

        }

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
        public async Task<dynamic> UpdateUser(AddUserAc addUserAc)
        {
            RegisterUser user = await _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.Id == addUserAc.Id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.FirstName = addUserAc.FirstName;
                user.LastName = addUserAc.LastName;
                user.Email = addUserAc.Email;
                user.MobileNumber = addUserAc.ContactNumber;
                user.State = addUserAc.State;
                user.City = addUserAc.City;
                user.CountryId = addUserAc.CountryId;
                user.Address = addUserAc.Address1;
                user.Address2 = addUserAc.Address2;
                user.RoleId = addUserAc.RoleId;
                user.ZipCode = addUserAc.Zipcode;
                user.UpdatedOn = DateTime.Now;
                user.SiteId = addUserAc.SiteId;
                user.IsActive = addUserAc.Active;
                user.OperatorId = addUserAc.OperatorId;
                user.IsMicrosoftAccount = addUserAc.IsMicrosoftAccount;
                _dbContext.RegisterUsers.Update(user);
                await _dbContext.SaveChangesAsync();
                return new { Message = "User updated successfully" };
            }
            else
            {
                return new { Message = "No data Found" };
            }
        }

        //public async Task<bool> SendEmailAsync(string EmailId, string User, string Subject, string Body, string Headeraname)
        //{
        //    try
        //    {
        //        // Get OAuth credentials from appsettings.json
        //        string clientId = _configuration["OAuthClientId"];
        //        string clientSecret = _configuration["OAuthClientSecret"];
        //        string refreshToken = _configuration["OAuthRefreshToken"];

        //        // Get Access Token using the Refresh Token
        //        string accessToken = await GetAccessToken(clientId, clientSecret, refreshToken);

        //        if (string.IsNullOrEmpty(accessToken))
        //        {
        //            throw new Exception("Failed to retrieve access token.");
        //        }

        //        using (SmtpClient client = new SmtpClient())
        //        {
        //            client.Host = _configuration["Host"]; // smtp.gmail.com
        //            client.Port = Convert.ToInt32(_configuration["Port"]); // 587
        //            client.EnableSsl = true;
        //            client.UseDefaultCredentials = false;

        //            // Use OAuth2 authentication
        //            client.Credentials = new NetworkCredential(_configuration["AdminMail"], accessToken);
        //            client.TargetName = "SMTPSVC"; // Required for OAuth2 with Gmail

        //            MailMessage mailMessage = new MailMessage
        //            {
        //                From = new MailAddress(_configuration["AdminMail"], Headeraname),
        //                Subject = Subject,
        //                Body = Body,
        //                IsBodyHtml = true
        //            };

        //            mailMessage.To.Add(new MailAddress(EmailId));

        //            await client.SendMailAsync(mailMessage); // Use async version of SendMail
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error sending email: " + ex.Message);
        //        return false;
        //    }
        //}

        private async Task<string> GetAccessToken(string clientId, string clientSecret, string refreshToken)
        {
            var tokenRequest = new Google.Apis.Auth.OAuth2.Requests.RefreshTokenRequest
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                RefreshToken = refreshToken
            };

            var tokenResponse = await new Google.Apis.Auth.OAuth2.Flows.GoogleAuthorizationCodeFlow(
                new Google.Apis.Auth.OAuth2.Flows.GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret }
                }).RefreshTokenAsync("user", refreshToken, System.Threading.CancellationToken.None);

            return tokenResponse.AccessToken;
        }

        /// method to send the mail
        /// </summary>
        /// <param name="EmailId"></param>
        /// <param name="User"></param>
        /// <param name="EmailCode"></param>
        /// <param name="ActivateLink"></param>
        /// <returns></returns>
        public async Task<bool> SendEmailAsync(string EmailId, string User, string Subject, string Body, string Headeraname)
        {

            SmtpClient client = new SmtpClient();
            //  client.DeliveryMethod = SmtpDeliveryMethod.Network;
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
                //AppLogs.InfoLogs("Error  Sending Mail , Login FORM, Method :SendMail" + ex.ToString());
                return true;
            }
        }
        public async Task<dynamic> BulkInsertUsersFromExcel(IFormFile file)
        {
           
                if (file == null || file.Length == 0)
                    return new { Status = "-100", Message = "Invalid file" };

                List<RegisterUser> users = new List<RegisterUser>();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets["Tenant Info"];
                        if (worksheet == null)
                            return new { Status = "-100", Message = "Sheet 'Tenant Info' not found" };

                        int rowCount = worksheet.Dimension.Rows;

                        for (int row = 2; row <= rowCount; row++) // Skip header row
                        {
                            string siteName = worksheet.Cells[row, 3].Value?.ToString()?.Trim();
                            var site = _dbContext.Sites.FirstOrDefault(s => s.SiteName == siteName);
                            if (site == null) continue;

                            int.TryParse(worksheet.Cells[row, 13].Value?.ToString(), out int parentId);
                            int.TryParse(worksheet.Cells[row, 14].Value?.ToString(), out int isAdminCreated);
                            bool.TryParse(worksheet.Cells[row, 14].Value?.ToString(), out bool updateEndDate);

                            RegisterUser user = new RegisterUser
                            {
                                FirstName = worksheet.Cells[row, 1].Value?.ToString()?.Trim(),
                                LastName = worksheet.Cells[row, 2].Value?.ToString()?.Trim(),
                                SiteId = site.Id,
                                Address = worksheet.Cells[row, 4].Value?.ToString()?.Trim(),
                                City = worksheet.Cells[row, 5].Value?.ToString()?.Trim(),
                                State = worksheet.Cells[row, 6].Value?.ToString()?.Trim(),
                                ZipCode = worksheet.Cells[row, 7].Value?.ToString()?.Trim(),
                                MobileNumber = worksheet.Cells[row, 8].Value?.ToString()?.Trim(),
                                HouseOrFlatNo = worksheet.Cells[row, 9].Value?.ToString()?.Trim(),
                                Email = worksheet.Cells[row, 10].Value?.ToString()?.Trim(),
                                ParkingBay = worksheet.Cells[row, 11].Value?.ToString()?.Trim(),
                                EmailCode = worksheet.Cells[row, 12].Value?.ToString()?.Trim(),
                                ParentId = parentId,
                                UpdateEnddate = updateEndDate,
                                RoleId = 2,
                                IsVerified = true,
                                IsActive = true,
                                IsDeleted = false,
                                ClientId = parentId,
                                CreatedBy = parentId,
                                CreatedOn = DateTime.Now,
                                IsAdminCreated = Convert.ToInt32(worksheet.Cells[row, 14].Value) == 1
                            };

                            users.Add(user);
                        }
                    }
                }

                if (users.Any())
                {
                    await _dbContext.RegisterUsers.AddRangeAsync(users);
                    await _dbContext.SaveChangesAsync();

                   
                    string returnlink = _configuration["TenantSetPasswordUrl"];
                    var folderName = Path.Combine("EmailHtml", "SetPassword.html");
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                    //if (File.Exists(filePath))
                    //{

                    //    StreamReader reader = new StreamReader(filePath);
                    //    foreach (var user in users)
                    //    {

                    //        var resetLink = returnlink + user.EmailCode;
                    //        //string myString = File.ReadAllText(filePath);
                    //        string myString = readFile;
                    //        // string myString = "";
                    //        myString = readFile;
                    //        string dt = DateTime.Now.ToString("MM.dd.yyyy");
                    //        myString = myString.Replace("%{#{Datetime}#}%", dt);
                    //        myString = myString.Replace("%{#{Name}#}%", user.FirstName + " " + user.LastName);
                    //        myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                    //        string body = myString;
                    //        SendEmail(user.Email, user.FirstName, "Set Password from Go Permit", body, "GOPERMIT_Set Password");
                    //    }
                    //}
                    if (File.Exists(filePath))
                    {
                        string readFile = File.ReadAllText(filePath); 

                        foreach (var user in users)
                        {
                            var resetLink = returnlink + user.EmailCode;
                            string myString = readFile;

                            string dt = DateTime.Now.ToString("MM.dd.yyyy");
                            myString = myString.Replace("%{#{Datetime}#}%", dt);
                            myString = myString.Replace("%{#{Name}#}%", user.FirstName + " " + user.LastName);
                            myString = myString.Replace("%{#{PasswordLink}#}%", resetLink);

                            string body = myString;
                            SendEmailAsync(user.Email, user.FirstName, "Set Password from Go Permit", body, "GOPERMIT_Set Password");
                        }
                    }

                }
                return new { Status = "200", Message = "Users inserted successfully", Result = users.Count };
                

        }

        public async Task<string> GetAccessTokenAsync()
        {
            var tokenResponse = new Google.Apis.Auth.OAuth2.Responses.TokenResponse
            {
                RefreshToken = _configuration["OAuth_Refresh_Token"],
            };
            var credential = new UserCredential(
                new GoogleAuthorizationCodeFlow(new GoogleAuthorizationCodeFlow.Initializer
                {
                    ClientSecrets = new ClientSecrets
                    {
                        ClientId = _configuration["OAuth_Client_Id"],
                        ClientSecret = _configuration["OAuth_Client_Secret"],
                    }
                }),
                "user",
                tokenResponse);

            await credential.RefreshTokenAsync(CancellationToken.None);
            return credential.Token.AccessToken;
        }


    }
}

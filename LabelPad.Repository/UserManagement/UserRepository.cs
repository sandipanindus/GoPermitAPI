using LabelPad.Domain.ApplicationClasses;
using LabelPad.Domain.Data;
using LabelPad.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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
            try
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
                    SiteId = addUser.SiteId
                });
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
            return new { Message = "User saved successfully" };
        }

        public bool GetTenant(UpdateRegisterUserAc addRegister)
        {

            RegisterUser user = _dbContext.RegisterUsers.FirstOrDefault(x => (x.Email == addRegister.Email || x.MobileNumber == addRegister.MobileNumber) && x.IsActive == true && x.IsDeleted == false && x.RoleId == 2);
            return (user != null);

        }
        public async Task<dynamic> AddTenant(UpdateRegisterUserAc model)
        {
            try
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
            }
            catch (Exception ex)
            {

            }

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
                                   VToDate  = v.EndDate,
                                   v.VRM,
                                   s.ZatparkSitecode,
                                   vehicleregid = v.Id,
                               }
                          ).FirstOrDefault();
                //Util.GetVehicleDetails(array);
                //Guid obj = Guid.NewGuid();
                //referenceno = obj.ToString();
                referenceno = "GP"+Convert.ToString(vehicles.vehicleregid);
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

                if (vehicleslots != null && vehicleslots.Count>0)
                {
                    foreach ( var vehicle in vehicleslots)
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
                   referenceno  = root.xml.reference_no;
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
            try
            {
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
                                string startdate = addUser.BayConfigs[i].StartDate.ToString("yyyy-MM-dd");
                                string enddate = addUser.BayConfigs[i].EndDate.ToString("yyyy-MM-dd");
                                ParkingBayNo parkingobj = new ParkingBayNo();
                                parkingobj.IsActive = true;
                                parkingobj.IsDeleted = false;
                                parkingobj.MaxVehiclesPerBay = Convert.ToInt32(addUser.BayConfigs[i].vehiclesperbay);
                                parkingobj.ParkingBayId = parkingbayno.ParkingBayId;
                                parkingobj.RegisterUserId = user.Id;
                                parkingobj.Section = parkingbayno.Section;
                                parkingobj.SiteId = parkingbayno.SiteId;
                                parkingobj.StartDate = Convert.ToDateTime(startdate);
                                parkingobj.EndDate = Convert.ToDateTime(enddate);
                                parkingobj.BayName = parkingbayno.BayName;
                                parkingobj.CreatedBy = 1;
                                parkingobj.CreatedOn = DateTime.Now;
                                parkingobj.Status = true;
                                _dbContext.ParkingBayNos.Add(parkingobj);
                                _dbContext.SaveChanges();
                                var isParking = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == user.Id).FirstOrDefault();
                                if (isParking != null)
                                {
                                    if (addUser.BayConfigs[i].vehiclereg!="")
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
                                var isParking = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == user.Id).FirstOrDefault();
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
            }
            catch (Exception ex)
            {

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
            var user1 = _dbContext.RegisterUsers.Where(x => x.Id == Id).FirstOrDefault();
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
                            r.ParkingBay,
                            r.RoleId,
                            r.SiteId,
                            r.State,
                            r.ZipCode,
                            r.ResidencyProofId,
                            r.IdentityProofId,
                            r.UpdateEnddate,
                            BaysConfig = (from c in _dbContext.ParkingBayNos
                                          //join z in _dbContext.VehicleRegistrations on c.RegisterUserId equals z.RegisterUserId
                                          where c.RegisterUserId == Id && c.IsActive == true && c.IsDeleted == false
                                          select new
                                          {
                                              bayconfigid = c.Id,
                                              c.IsActive,
                                              c.IsDeleted,
                                              MaxVehiclesPerBay = c.MaxVehiclesPerBay.ToString(),
                                              bayid = c.Id.ToString(),
                                              status = 1,//_dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParkingBayNo == c.Id).FirstOrDefault().Id,
                                              c.RegisterUserId,
                                              c.StartDate,
                                              c.EndDate,
                                              vehiclereg = (from z in _dbContext.VehicleRegistrations
                                                            where z.RegisterUserId == Id && z.IsActive == true && z.IsDeleted == false
                                                            select new
                                                            {
                                                                z.VRM,
                                                            }).ToList(),
                                              //z.VRM,


                                              baynos = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && (x.RegisterUserId == Id || x.RegisterUserId == 0) && x.SiteId == siteId).ToList()
                        }).ToList()
                        }).FirstOrDefault();
            // RegisterUser user = await _dbContext.RegisterUsers.FirstOrDefaultAsync(x => x.IsDeleted == false && x.Id == Id);
            return user;
        }
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
                                      pageNo=PageNo,
                                      l.Id,
                                      l.FirstName,
                                      l.LastName,
                                      l.Email,
                                      l.MobileNumber,
                                      l.RoleId,
                                      RoleName = r.Name,
                                      VRM = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == l.Id && x.IsDeleted== false).FirstOrDefault().VRM == null ? " " : _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == l.Id && x.IsDeleted == false).FirstOrDefault().VRM,
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
                        pageNo=PageNo,
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
        public async Task<dynamic> UpdateProfileUploads(UpdateRegisterUserAc objinput)
        {
            int Id = objinput.Id;
            RegisterUser user = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.Id == Id).FirstOrDefault();
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
            }
            return new { Message = "Updated Successfully" };
        }
        public async Task<dynamic> UpdateTenantUser(AddTenantUser addUserAc)
        {
            bool iscancelwhitelist = false;


            RegisterUser user = _dbContext.RegisterUsers.Where(x => x.IsDeleted == false && x.Id == addUserAc.Id).FirstOrDefault();
            if (user != null)
            {
                user.FirstName = addUserAc.FirstName;
                user.LastName = addUserAc.LastName;
                user.HouseOrFlatNo = addUserAc.HouseOrFlatNo;
                user.Email = addUserAc.Email;
                user.MobileNumber = addUserAc.MobileNumber;
                user.State = addUserAc.State;
                user.City = addUserAc.City;
                user.Address = addUserAc.Address;
                user.SiteId = Convert.ToInt32(addUserAc.SiteId);
                user.ParkingBay = addUserAc.ParkingBay;
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
                //var parkingbaynos = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == user.Id).ToList();
                //if (parkingbaynos != null)
                //{
                //    parkingbaynos.ForEach(x =>
                //    {
                //        x.RegisterUserId = 0;
                //        x.UpdatedBy = x.CreatedBy;
                //        x.UpdatedOn = DateTime.Now;
                //    });
                //    _dbContext.ParkingBayNos.UpdateRange(parkingbaynos);
                //    _dbContext.SaveChanges();
                //}
                for (int i = 0; i < addUserAc.BayConfigs.Count; i++)
                {
                    
                    string startdate1 = addUserAc.BayConfigs[i].StartDate.ToString("yyyy-MM-dd");
                    string enddate1 = addUserAc.BayConfigs[i].EndDate.ToString("yyyy-MM-dd");
                    int siteid1 = Convert.ToInt32(addUserAc.SiteId);
                    var parkingbayno1 = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.BayName == addUserAc.BayConfigs[i].bayid && x.SiteId == siteid1).FirstOrDefault();
                    //int bayid = Convert.ToInt32(addUserAc.BayConfigs[i].bayid);
                    int bayconfigid = Convert.ToInt32(addUserAc.BayConfigs[i].bayconfigid);
                    int number = 0;
                    var parkingbay = new ParkingBayNo();
                    
                    
                    if (addUserAc.BayConfigs[i].bayconfigid != 0)
                    {
                        bool isbay = int.TryParse(addUserAc.BayConfigs[i].bayid, out number);
                        if (isbay)
                        {
                            int bayid = Convert.ToInt32(addUserAc.BayConfigs[i].bayid);
                            parkingbay = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == bayid && x.RegisterUserId == user.Id && x.SiteId == siteid1).FirstOrDefault();
                        }
                        else
                        {
                            //changes start
                            var vehicleregistration = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParkingBayNo == bayconfigid).FirstOrDefault();
                            if (vehicleregistration == null)
                            {
                                //changes end
                                parkingbay = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == bayconfigid && x.RegisterUserId == user.Id && x.SiteId == siteid1).FirstOrDefault();
                                if (parkingbay != null)
                                {
                                    parkingbay.RegisterUserId = user.Id;
                                    parkingbay.IsActive = false;
                                    parkingbay.IsDeleted = true;
                                    parkingbay.UpdatedBy = 1;
                                    parkingbay.UpdatedOn = DateTime.Now;
                                    _dbContext.ParkingBayNos.Update(parkingbay);
                                    _dbContext.SaveChanges();
                                }
                                //changes start
                                //parkingbay = null;
                                //changes end

                            }

                        }
                    }
                    else
                    {
                        parkingbay = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.BayName == addUserAc.BayConfigs[i].bayid && x.RegisterUserId == user.Id && x.SiteId == siteid1).FirstOrDefault();
                    }
                    if (parkingbay != null)
                    {
                        // var vehicleregitration = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParkingBayNo == parkingbay.Id ).FirstOrDefault();
                        //changes start
                        var vehicleregitration = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.ParkingBayNo == bayconfigid).FirstOrDefault();
                        //changes End
                        if (vehicleregitration == null)
                        {
                            if (parkingbay.RegisterUserId != 0)
                            {
                                string startdate = addUserAc.BayConfigs[i].StartDate.ToString("yyyy-MM-dd");
                                string enddate =   addUserAc.BayConfigs[i].EndDate.ToString("yyyy-MM-dd");
                                parkingbay.RegisterUserId = user.Id;
                                parkingbay.IsActive = false;
                                parkingbay.IsDeleted = true;
                                parkingbay.UpdatedBy = 1;
                                parkingbay.UpdatedOn = DateTime.Now;
                                _dbContext.ParkingBayNos.Update(parkingbay);
                                _dbContext.SaveChanges();
                                ParkingBayNo bayno = new ParkingBayNo();
                                bayno.IsActive = true;
                                bayno.IsDeleted = false;
                                bayno.ParkingBayId = parkingbay.ParkingBayId;
                                bayno.MaxVehiclesPerBay = Convert.ToInt32(addUserAc.BayConfigs[i].vehiclesperbay);
                                bayno.RegisterUserId = user.Id;
                                bayno.Section = parkingbay.Section;
                                bayno.BayName = parkingbay.BayName;
                                bayno.StartDate = Convert.ToDateTime(startdate);
                                bayno.EndDate = Convert.ToDateTime(enddate);
                                bayno.CreatedBy = 1;
                                bayno.CreatedOn = DateTime.Now;
                                bayno.SiteId = parkingbay.SiteId;
                                _dbContext.ParkingBayNos.Add(bayno);
                                _dbContext.SaveChanges();
                                if (addUserAc.BayConfigs[i].vehiclereg != "")
                                {
                                    var Issavecount = 1;
                                    //var siteparkingbayno = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == user.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault().ParkingBayNo;
                                    //var vrmcompare = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == user.Id && x.ParkingBayNo == siteparkingbayno).FirstOrDefault();
                                    //if (vrmcompare != null)
                                    //{
                                    //    if (addUserAc.BayConfigs[i].StartDate == vrmcompare.StartDate && addUserAc.BayConfigs[i].EndDate == vrmcompare.EndDate && addUserAc.BayConfigs[i].vehiclereg == vrmcompare.VRM)
                                    //    {
                                    //        iscancelwhitelist = false;
                                    //    }
                                    //    else
                                    //    {
                                    //        iscancelwhitelist = true;
                                    //    }

                                    //}
                                    //else
                                    //{
                                    //    iscancelwhitelist = false;
                                    //}
                                    var isParking = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == user.Id).FirstOrDefault();
                                    if (isParking != null)
                                    {

                                        VehicleRegistration vr = new VehicleRegistration();
                                        vr.IsActive = true;
                                        vr.IsDeleted = false;
                                        vr.RegisterUserId = isParking.RegisterUserId;
                                        vr.VRM = addUserAc.BayConfigs[i].vehiclereg;
                                        vr.CreatedOn = DateTime.Now;
                                        vr.CreatedBy = 1;
                                        vr.StartDate = Convert.ToDateTime(startdate1);
                                        vr.EndDate = Convert.ToDateTime(enddate1);
                                        vr.ParkingBayNo = isParking.Id;
                                        _dbContext.VehicleRegistrations.Add(vr);
                                        _dbContext.SaveChanges();
                                    }
                                    var vehicleregistered = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == user.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                    if (vehicleregistered != null)
                                    {
                                        var Id = vehicleregistered.Id;
                                        var id = vehicleregistered.RegisterUserId;
                                        var bayno1 = isParking.Id;
                                       // var siteparkingbayno = vehicleregistered.ParkingBayNo;
                                        DateTime StartDate = addUserAc.BayConfigs[i].StartDate;
                                        DateTime EndDate = addUserAc.BayConfigs[i].EndDate;
                                        //bool issentzatpark = false;
                                        if (addUserAc.BayConfigs[i].dates != "")
                                        {
                                            //savemutliplevehciledates(addUserAc.Id, addUserAc.BayConfigs[i].dates, StartDate, EndDate, Issavecount, id, Convert.ToInt32(bayno));
                                            savemutliplevehciledates(Id, addUserAc.BayConfigs[i].dates, StartDate, EndDate, 0, id, bayno1);

                                        }
                                        else
                                        {
                                            savemutliplevehciletime(Id, StartDate, EndDate, 0);
                                        }
                                        
                                        //if (iscancelwhitelist==true)
                                        //{
                                        //    Cancelwhitelistvehicle(Convert.ToInt32(addUserAc.SiteId), vehicleregistered.VRM);
                                        //    whitelistvehicle(Convert.ToInt32(addUserAc.SiteId), vehicleregistered.VRM);

                                        //}
                                        //else
                                        //{
                                            whitelistvehicle(Convert.ToInt32(addUserAc.SiteId), vehicleregistered.VRM);
                                        //}


                                    }

                                }


                            }
                            else
                            {

                            }
                        }
                        else
                        {
                            //changes
                            //parkingbay = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.BayName == addUserAc.BayConfigs[i].bayid && x.RegisterUserId == user.Id && x.SiteId == siteid1).FirstOrDefault();
                            //if (parkingbay!=null) 
                            //{ 
                            //if (vehicleregitration.EndDate.Value.Date > DateTime.Now && vehicleregitration.EndDate.Value.Date > parkingbay.EndDate)
                            ////changes
                            //    {
                            //        if (parkingbay.RegisterUserId != 0)
                            //    {
                            //        string startdate = addUserAc.BayConfigs[i].StartDate.ToString("yyyy-MM-dd");
                            //        string enddate = addUserAc.BayConfigs[i].EndDate.ToString("yyyy-MM-dd");
                            //        parkingbay.RegisterUserId = user.Id;  
                            //        parkingbay.IsActive = false;
                            //        parkingbay.IsDeleted = true;
                            //        parkingbay.UpdatedBy = 1;
                            //        parkingbay.UpdatedOn = DateTime.Now;
                            //        _dbContext.ParkingBayNos.Update(parkingbay);
                            //        _dbContext.SaveChanges();
                            //        ParkingBayNo bayno = new ParkingBayNo();
                            //        bayno.IsActive = true;
                            //        bayno.IsDeleted = false;
                            //        bayno.ParkingBayId = parkingbay.ParkingBayId;
                            //        bayno.MaxVehiclesPerBay = Convert.ToInt32(addUserAc.BayConfigs[i].vehiclesperbay);
                            //        bayno.RegisterUserId = user.Id;
                            //        bayno.Section = parkingbay.Section;
                            //        bayno.BayName = parkingbay.BayName;
                            //        bayno.StartDate = Convert.ToDateTime(startdate);
                            //        bayno.EndDate = Convert.ToDateTime(enddate);
                            //        bayno.CreatedBy = 1;
                            //        bayno.CreatedOn = DateTime.Now;
                            //        bayno.SiteId = parkingbay.SiteId;
                            //        _dbContext.ParkingBayNos.Add(bayno);
                            //        _dbContext.SaveChanges();
                            //    }
                            //}
                            //// changes 
                            //else
                            // {

                            //changes start
                            var Issavecount = 1;
                            int id = addUserAc.Id;
                            bool updatevehicle = false;
                            int bayno = Convert.ToInt32(addUserAc.BayConfigs[i].bayconfigid);
                            var vehicleregistration = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == id && x.ParkingBayNo == bayno && x.IsDeleted == false && x.IsActive == true).OrderByDescending(x => x.Id).FirstOrDefault();
                            if (vehicleregistration != null)
                            {
                                if (vehicleregistration.RegisterUserId == id)
                                {
                                    updatevehicle = true;
                                }
                                else
                                {

                                    if (vehicleregistration.RegisterUserId == 0)
                                    {
                                        updatevehicle = true;
                                    }
                                    else
                                    {
                                        updatevehicle = false;

                                        return new { Message = "Can not be updated as the the bayname is already booked for respective dates" };
                                    }

                                }
                                if (updatevehicle == true)
                                {
                                    //var issentzatpark = _dbContext.VehicleRegistrationTimeSlots.Where(x => x.RegisterUserId == user.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault().ParkingBayNo;
                                    var siteparkingbayno = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == user.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault().ParkingBayNo;
                                    var vrmcompare = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == id && x.ParkingBayNo == siteparkingbayno).FirstOrDefault();
                                    var oldvrm = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == id && x.ParkingBayNo == siteparkingbayno).FirstOrDefault().VRM;

                                    if (vrmcompare != null)
                                    {
                                        if (addUserAc.BayConfigs[i].StartDate == vrmcompare.StartDate && addUserAc.BayConfigs[i].EndDate == vrmcompare.EndDate && addUserAc.BayConfigs[i].vehiclereg == vrmcompare.VRM)
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
                                    int parkingbayno = vehicleregistration.ParkingBayNo;
                                    var parkingbaynos = _dbContext.ParkingBayNos.Where(x => x.Id == parkingbayno).FirstOrDefault();
                                    if (parkingbaynos != null)
                                    {
                                        int maxvehicle = parkingbaynos.MaxVehiclesPerBay;
                                        if (maxvehicle == 1)
                                        {
                                            if(iscancelwhitelist==true) {
                                                Cancelwhitelistvehicle(Convert.ToInt32(addUserAc.SiteId), oldvrm);
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
                                                //changes started
                                                if (vehicleregistration == null)
                                                {
                                                    //changes Ends
                                                    vehicleregistration.IsActive = false;
                                                    vehicleregistration.IsDeleted = true;
                                                    vehicleregistration.UpdatedBy = 1;
                                                    vehicleregistration.UpdatedOn = DateTime.Now;
                                                    _dbContext.VehicleRegistrations.Update(vehicleregistration);
                                                    _dbContext.SaveChanges();

                                                }


                                            }
                                        }
                                    }
                                    var parkingbay1 = _dbContext.ParkingBayNos.Where(x => x.RegisterUserId == id && x.Id == bayno && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                    if (parkingbay != null)
                                    {
                                      //parkingbay1.StartDate = Convert.ToDateTime(addUserAc.BayConfigs[i].StartDate.ToString("yyyy-MM-dd"));
                                      //parkingbay1.EndDate = Convert.ToDateTime(addUserAc.BayConfigs[i].EndDate.ToString("yyyy-MM-dd"));
                                        parkingbay1.StartDate = Convert.ToDateTime(addUserAc.BayConfigs[i].StartDate);
                                        parkingbay1.EndDate = Convert.ToDateTime(addUserAc.BayConfigs[i].EndDate);
                                        parkingbay1.UpdatedBy = 1;
                                        parkingbay1.UpdatedOn = DateTime.Now;
                                        _dbContext.ParkingBayNos.Update(parkingbay1);
                                        _dbContext.SaveChanges();

                                    }
                                    if (addUserAc.BayConfigs[i].dates != "")
                                    {
                                        string[] values = addUserAc.BayConfigs[i].dates.Split(',');
                                        //DateTime modifieddate = Convert.ToDateTime(date1.Year + "-" + date1.Month + "-" + date1.Day + " " + date2.Hour + ":" + date2.Minute);
                                        for (int j = 0; j < values.Length; j++)
                                        {
                                            values[j] = values[j].Trim();
                                            string testdate = values[i].Substring(0, 16);
                                            DateTime testdate1 = Convert.ToDateTime(testdate);


                                            var responce = (from b in _dbContext.VehicleRegistrations
                                                            join pb in _dbContext.VehicleRegistrationTimeSlots on b.Id equals pb.VehicleRegistrationId
                                                            where b.RegisterUserId == id && b.ParkingBayNo == Convert.ToInt32(bayno)
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


                                        //if (objinput[0].Issavecount == 1)
                                        //{
                                        DateTime todaydate = DateTime.Now;
                                        var vehilce = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == id && x.ParkingBayNo == bayno).ToList();
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
                                        

                                        DateTime StartDate = addUserAc.BayConfigs[i].StartDate;
                                        DateTime EndDate = addUserAc.BayConfigs[i].EndDate;
                                        var vehicleregistered = _dbContext.VehicleRegistrations.Where(x => x.ParkingBayNo == bayconfigid && x.RegisterUserId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                        if (vehicleregistered != null)
                                        {
                                            vehicleregistered.UpdatedOn = DateTime.Now;
                                            vehicleregistered.UpdatedBy = 1;
                                            vehicleregistered.StartDate = StartDate;
                                            vehicleregistered.EndDate = EndDate;
                                            vehicleregistered.VRM = addUserAc.BayConfigs[i].vehiclereg;
                                            vehicleregistered.IsActive = true;
                                            vehicleregistered.IsDeleted = false;
                                            vehicleregistered.RegisterUserId = parkingbay1.RegisterUserId;
                                            vehicleregistered.ParkingBayNo = parkingbay1.Id;
                                            vehicleregistered.IsSaveCount = Issavecount;
                                            _dbContext.VehicleRegistrations.Update(vehicleregistered);
                                            _dbContext.SaveChanges();
                                        }
                                        var vehicleregisteredup = _dbContext.VehicleRegistrations.Where(x => x.ParkingBayNo == bayconfigid && x.RegisterUserId == id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();

                                        var Id = vehicleregisteredup.Id;
                                        if (addUserAc.BayConfigs[i].dates != "")
                                        {
                                            //savemutliplevehciledates(addUserAc.Id, addUserAc.BayConfigs[i].dates, StartDate, EndDate, Issavecount, id, Convert.ToInt32(bayno));
                                            savemutliplevehciledates(Id, addUserAc.BayConfigs[i].dates, StartDate, EndDate, Issavecount, id, Convert.ToInt32(bayno));

                                        }
                                        else
                                        {
                                            savemutliplevehciletime(Id, StartDate, EndDate, Issavecount);
                                        }
                                        
                                        if (iscancelwhitelist == true)
                                        {
                                            //Cancelwhitelistvehicle(Convert.ToInt32(addUserAc.SiteId), oldvrm);
                                            whitelistvehicle(Convert.ToInt32(addUserAc.SiteId), vehicleregistered.VRM);

                                        }
                                        //else
                                        //{
                                        //    whitelistvehicle(Convert.ToInt32(addUserAc.SiteId), vehicleregistered.VRM);
                                        //}
                                        //}

                                    }
                                    //else
                                    //{

                                    //}
                                }
                                else
                                {
                                    return new { Message = "Can not be updated as the the bayname is already booked for respective dates" };

                                }
                                //changes end

                            }

                            //return new { Message = "Cannot be updated vehicle is registered" };
                            // }
                            //}


                        }
                        //return new { Message = "User updated successfully" };

                    }
                    else
                    {
                        var parkingbayno = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.BayName == addUserAc.BayConfigs[i].bayid && x.SiteId == siteid1).FirstOrDefault();
                        if (parkingbayno != null)
                        {
                            //if (parkingbayno.RegisterUserId != 0)
                            //{
                            //    string startdate1 = addUserAc.BayConfigs[i].StartDate.ToString("yyyy-MM-dd");
                            //    string enddate1 = addUserAc.BayConfigs[i].EndDate.ToString("yyyy-MM-dd");
                            //    ParkingBayNo parkingobj = new ParkingBayNo();
                            //    parkingobj.IsActive = true;
                            //    parkingobj.IsDeleted = false;
                            //    parkingobj.MaxVehiclesPerBay = Convert.ToInt32(addUserAc.BayConfigs[i].vehiclesperbay);
                            //    parkingobj.ParkingBayId = parkingbayno.ParkingBayId;
                            //    parkingobj.RegisterUserId = user.Id;
                            //    parkingobj.Section = parkingbayno.Section;
                            //    parkingobj.SiteId = parkingbayno.SiteId;
                            //    parkingobj.StartDate = Convert.ToDateTime(startdate1);
                            //    parkingobj.EndDate = Convert.ToDateTime(enddate1);
                            //    parkingobj.BayName = parkingbayno.BayName; 
                            //    parkingobj.CreatedBy = 1;
                            //    parkingobj.CreatedOn = DateTime.Now;
                            //    parkingobj.Status = true;
                            //    _dbContext.ParkingBayNos.Add(parkingobj);
                            //    _dbContext.SaveChanges();
                            //}

                            string startdate = addUserAc.BayConfigs[i].StartDate.ToString("yyyy-MM-dd");
                            string enddate = addUserAc.BayConfigs[i].EndDate.ToString("yyyy-MM-dd");
                            //parkingbay.RegisterUserId = addUserAc.Id;
                            //parkingbay.IsActive = false;
                            //parkingbay.IsDeleted = true;
                            //parkingbay.UpdatedBy = 1;
                            //parkingbay.UpdatedOn = DateTime.Now;
                            //_dbContext.ParkingBayNos.Update(parkingbay);
                            //_dbContext.SaveChanges();
                            var parkbay = _dbContext.ParkingBays.Where(x => x.IsActive == true && x.IsDeleted == false && x.SiteId == Convert.ToInt32( addUserAc.SiteId)).FirstOrDefault();
                            ParkingBayNo bayno = new ParkingBayNo();
                            bayno.IsActive = true;
                            bayno.IsDeleted = false;
                            bayno.ParkingBayId = parkbay.Id;
                            bayno.MaxVehiclesPerBay = Convert.ToInt32(addUserAc.BayConfigs[i].vehiclesperbay);
                            bayno.RegisterUserId = addUserAc.Id;
                            bayno.Section = Convert.ToString(parkbay.Section);
                            bayno.BayName = addUserAc.BayConfigs[i].bayid;
                            bayno.StartDate = Convert.ToDateTime(startdate);
                            bayno.EndDate = Convert.ToDateTime(enddate);
                            bayno.CreatedBy = 1;
                            bayno.CreatedOn = DateTime.Now;
                            bayno.SiteId = Convert.ToInt32( addUserAc.SiteId);
                            _dbContext.ParkingBayNos.Add(bayno);
                            _dbContext.SaveChanges();

                            //var Reguser = _dbContext.RegisterUsers.Where(x => x.IsActive == true && x.IsDeleted == false && x.Email==user.Email && x.MobileNumber == user.MobileNumber).FirstOrDefault();
                            //string startdate = addUserAc.BayConfigs[i].StartDate.ToString("yyyy-MM-dd");
                            //string enddate = addUserAc.BayConfigs[i].EndDate.ToString("yyyy-MM-dd");
                            //parkingbayno.RegisterUserId = addUserAc.Id;
                            //parkingbayno.StartDate = Convert.ToDateTime(startdate);
                            //parkingbayno.EndDate = Convert.ToDateTime(enddate);
                            //parkingbayno.MaxVehiclesPerBay = Convert.ToInt32(addUserAc.BayConfigs[i].vehiclesperbay);
                            //parkingbayno.UpdatedBy = 1;
                            //parkingbayno.UpdatedOn = DateTime.Now;
                            //_dbContext.ParkingBayNos.Update(parkingbayno);
                            //_dbContext.SaveChanges();
                            if (addUserAc.BayConfigs[i].vehiclereg!="")
                            {
                                var siteparkingbayno = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == user.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault().ParkingBayNo;
                                var vrmcompare = _dbContext.VehicleRegistrations.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == addUserAc.Id && x.ParkingBayNo == siteparkingbayno).FirstOrDefault();
                                if (vrmcompare != null)
                                {
                                    if (addUserAc.BayConfigs[i].StartDate == vrmcompare.StartDate && addUserAc.BayConfigs[i].EndDate == vrmcompare.EndDate && addUserAc.BayConfigs[i].vehiclereg == vrmcompare.VRM)
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
                                var Issavecount = 1;
                                var isParking = _dbContext.ParkingBayNos.Where(x => x.IsActive == true && x.IsDeleted == false && x.RegisterUserId == user.Id).FirstOrDefault();
                                if (isParking != null)
                                {

                                    VehicleRegistration vr = new VehicleRegistration();
                                    vr.IsActive = true;
                                    vr.IsDeleted = false;
                                    vr.RegisterUserId = isParking.RegisterUserId;
                                    vr.VRM = addUserAc.BayConfigs[i].vehiclereg;
                                    vr.CreatedOn = DateTime.Now;
                                    vr.CreatedBy = 1;
                                    vr.StartDate = Convert.ToDateTime(startdate1);
                                    vr.EndDate = Convert.ToDateTime(enddate1);
                                    vr.ParkingBayNo = isParking.Id;
                                    _dbContext.VehicleRegistrations.Add(vr);
                                    _dbContext.SaveChanges();
                                }
                                var vehicleregistered = _dbContext.VehicleRegistrations.Where(x => x.RegisterUserId == user.Id && x.IsActive == true && x.IsDeleted == false).FirstOrDefault();
                                if (vehicleregistered != null)
                                {
                                    var Id = vehicleregistered.Id;
                                    var id = vehicleregistered.RegisterUserId;
                                    var bayno1 = parkingbayno1.Id;
                                    DateTime StartDate = addUserAc.BayConfigs[i].StartDate;
                                    DateTime EndDate = addUserAc.BayConfigs[i].EndDate;
                                    //bool issentzatpark = false;
                                    if (addUserAc.BayConfigs[i].dates != "")
                                    {
                                        //savemutliplevehciledates(addUserAc.Id, addUserAc.BayConfigs[i].dates, StartDate, EndDate, Issavecount, id, Convert.ToInt32(bayno));
                                        savemutliplevehciledates(Id, addUserAc.BayConfigs[i].dates, StartDate, EndDate, 0, id, bayno1);

                                    }
                                    else
                                    {
                                        savemutliplevehciletime(Id, StartDate, EndDate, 0);
                                    }
                                    if (iscancelwhitelist == true)
                                    {
                                        Cancelwhitelistvehicle(Convert.ToInt32(addUserAc.SiteId), vehicleregistered.VRM);
                                        whitelistvehicle(Convert.ToInt32(addUserAc.SiteId), vehicleregistered.VRM);

                                    }
                                    else
                                    {
                                        whitelistvehicle(Convert.ToInt32(addUserAc.SiteId), vehicleregistered.VRM);
                                    }
                                }

                            }
                            
                        }
                    }
                    //  int configid =(int)addUserAc.BayConfigs[i].bayconfigid;
                    //BayConfig config = _dbContext.BayConfigs.Where(x => x.IsActive == true && x.IsDeleted == false && x.Id == configid).FirstOrDefault();
                    //if (config != null)
                    //{
                    //    config.ParkingBayNoId =Convert.ToInt32(addUserAc.BayConfigs[i].bayid);
                    //    config.SiteId = Convert.ToInt32(addUserAc.SiteId);
                    //    config.UpdatedBy = 1;
                    //    config.UpdatedOn = DateTime.Now;
                    //    _dbContext.BayConfigs.Update(config);
                    //    _dbContext.SaveChanges();
                    //}
                    //else
                    //{
                    //    BayConfig bay = new BayConfig();
                    //    bay.ParkingBayNoId = Convert.ToInt32(addUserAc.BayConfigs[i].bayid);
                    //    bay.RegisterUserId = user.Id;
                    //    bay.SiteId = Convert.ToInt32(addUserAc.SiteId);
                    //    bay.IsActive = true;
                    //    bay.IsDeleted = false;
                    //    bay.CreatedBy = 1;
                    //    bay.CreatedOn = DateTime.Now;
                    //    _dbContext.BayConfigs.Add(bay);
                    //    _dbContext.SaveChanges();
                    //}
                }
                return new { Message = "User updated successfully" };
            }
            else
            {
                return new { Message = "No data Found" };
            }
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
                _dbContext.RegisterUsers.Update(user);
                await _dbContext.SaveChangesAsync();
                return new { Message = "User updated successfully" };
            }
            else
            {
                return new { Message = "No data Found" };
            }
        }
    }
}

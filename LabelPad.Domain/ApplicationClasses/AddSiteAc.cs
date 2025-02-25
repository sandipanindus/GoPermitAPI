using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
    public class AddSiteAc
    {
        public int LoginId { get; set; }
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string SiteAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string ContactPersonName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string MobileNumber { get; set; }
        public bool Active { get; set; }
        public bool ManageParkingAvailble { get; set; }
        public bool VisitorParkingAvailble { get; set; }
        public bool Zatparklogs24hrs { get; set; }

        public string TenantParkingBays { get; set; }
        public string VisitorParkingBays { get; set; }
        public string Section { get; set; }
        public string Seperator { get; set; }
        public string VSection { get; set; }
        public string VSeperator { get; set; }
        public int Total { get; set; }
        public int VehiclesPerBay { get; set; }

        public int OperatorId { get; set; }
        public string maxparkingsession { get; set; }
        public string TimeUnit { get; set; }
        public string SiteCode { get; set; }
        public List<ParkingBays> ParkingBays { get; set; }
        public List<VisitorBays> VisitorBays { get; set; }
        public List<VisitorSession> VisitorSessions { get; set; }
    }
    public class ParkingBays
    {
        public int Id { get; set; }
        public string prefix { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string count { get; set; }
    }
    public class VisitorBays
    {
        public int Id { get; set; }
        public string prefix { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public string count { get; set; }
      
    }
    public class VisitorSession
    {
        public int Id { get; set; }
        public string duration { get; set; }
        public string sessionunit { get; set; }
    }
    public class TimeSlotCls
    {
        public int Id { get; set; }
        public string Time { get; set; }
    }


    public class Auditlog
    {
        public int RegisterUserId { get; set; }
        public string RoleId { get; set; }
        public string Function { get; set; }
        public string IP { get; set; }
        public string Agent { get; set; }
        public string Operation { get; set; }
        public int TenantId { get; set; }
    }
}

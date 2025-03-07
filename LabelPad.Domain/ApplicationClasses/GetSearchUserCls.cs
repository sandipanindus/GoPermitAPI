using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
   public class GetSearchUserCls
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int TotalItem { get; set; }
        public double TotalPage { get; set; }
        public string SiteName { get; set; }
    }
    public class GetSearchTenantCls
    {
        public int pageNo { get; set; }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int TotalItem { get; set; }
        public double TotalPage { get; set; }
        public string SiteName { get; set; }

        public string VRM { get; set; }
    }
    public class GetSearchSiteCls
    {
        public int pageNo { get; set; }
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int TotalItem { get; set; }
        public double TotalPage { get; set; }
    }
    public class GetSearchZatpark
    {
        public int Sno { get; set; }
        public int Id { get; set; }
        public int RegistrationTimeSlotId { get; set; }
        public string SiteName { get; set; }
        public int SiteId { get; set; }
        public string Tenant { get; set; }
        public string Request { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int ParkingBayNoId { get; set; }
        public string BayName { get; set; }
        public string ZatparkResponse { get; set; }
        public int TotalItem { get; set; }
        public double TotalPage { get; set; }
        public string vrm { get; set; }
    }
    public class GetSearchAuditLogs
    {
        public int pageNo { get; set; }
        public int Sno { get; set; }
        public int Id { get; set; }
        public string SiteName { get; set; }
        public string Operation { get; set; }
        public string Function { get; set; }
        public string Agent { get; set; }
        public string Date { get; set; }
        public int TotalItem { get; set; }
        public double TotalPage { get; set; }
        public string Username { get; set; }
        public int SiteId { get; set; }
    }
}

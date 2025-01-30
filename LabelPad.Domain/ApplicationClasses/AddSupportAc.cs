using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
    public class AddSupportAc
    {
        public string Issue { get; set; }
        public int TenantId { get; set; }
        public string Subject { get; set; }
        public int Id { get; set; }
        public int TicketId { get; set; }
    }
    public class GetSupportList
    {
        public int pageNo { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public string Subject { get; set; }
        public bool IsRead { get; set; }
        public int TicketId { get; set; }
        public string Status { get; set; }
        public int TotalItem { get; set; }
        public double TotalPage { get; set; }
        public string SiteName { get; set; }
    }
    public class GetSearchSupport
    {
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }
        public int SiteId { get; set; }
        public bool IsRead { get; set; }
        public string SiteName { get; set; }
        public string Subject { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
   public class GetPingReport
    {
        public int SiteId { get; set; }
        public int RegisterUserId { get; set; }
        public string SiteCode { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        public string SiteName { get; set; }
        public string UserName { get; set; }
        public string Date { get; set; }
        public int Id { get; set; }
        public string sno { get; set; }
        public string LastPingDate { get; set; }
        public string LastPingTime { get; set; }
        public string LastVRM { get; set; }
        public string LastVRMDate { get; set; }
        public string LastVRMTime { get; set; }
        public int TotalItem { get; set; }
        public double TotalPage { get; set; }
    }
}

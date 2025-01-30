using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
   public class AddVisitorParkingAc
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public int SiteId { get; set; }
        public string Name { get; set; }
        public string Make { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Model { get; set; }
        public string VRM { get; set; }
        public string SessionUnit { get; set; }
        public string Duration { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int VisitorBayNoId { get; set; }
        public bool cctome { get; set; }
        public string Surname { get; set; }
        public string Date { get; set; }
    }
    public  class vehiclelists
    {
        public int id { get; set; }
        public string bayid { get; set; }
        public string make { get; set; }
        public string model { get; set; }
        public string vrm { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public int vehicledetailid { get; set; }
    }
    public class UpdateVisitorSlot
    {
        public int Id { get; set; }
        public string vrm { get; set; }
        public int visitorbayid { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string contact { get; set; }
    }
}

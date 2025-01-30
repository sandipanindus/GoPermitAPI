using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
    public class AddVehicleRegistrationAc
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string vrm { get; set; }
        public string bayno { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string dates { get; set; }
        public int Issavecount { get; set; }
        public int TenantId { get; set; }
        //in save we are passing iteration values
        public int Id { get; set; }
        public int LoginId { get; set; }
        public bool Update { get; set; }
    }
}

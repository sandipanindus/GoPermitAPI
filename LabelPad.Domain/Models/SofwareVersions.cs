using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class SoftwareVersion 
    {
        public int Id { get; set; }
        public string Versions { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
    }
}

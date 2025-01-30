using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabelPad.Domain.Models
{
   public class Country:BaseModel
    {
        [StringLength(100)]
        public string CountryCode { get; set; }
        [StringLength(200)]
        public string CountryName { get; set; }
        [StringLength(100)]
        public string TimeZone { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabelPad.Domain.Models
{
   public class Role:BaseModel
    {
        [StringLength(200)]
        public string Name { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string Description { get; set; }
    }
}

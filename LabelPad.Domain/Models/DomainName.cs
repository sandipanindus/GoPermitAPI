using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabelPad.Domain.Models
{
   public class DomainName:BaseModel
    {
        [StringLength(200)]
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class Team:BaseModel
    {
        [StringLength(200)]
        public string Code { get; set; }
        [StringLength(200)]
        public string Name { get; set; }
       
    }
}

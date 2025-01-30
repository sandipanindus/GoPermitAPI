using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabelPad.Domain.Models
{
   public class DataSource:BaseModel
    {
        [Required]
        [StringLength(300)]
        public string Name { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
   public class LabelClass:BaseModel
    {
        [StringLength(200)]
        public string Name { get; set; }
        public int LoginId { get; set; }
        [ForeignKey("Project")]
        public  int ProjectId { get; set; }
    }
}

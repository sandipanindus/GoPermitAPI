using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class Module : BaseModel
    {
        [StringLength(200)]
        public string ModuleName { get; set; }
        [StringLength(200)]
        public string Icon { get; set; }
        public int SequenceNo { get; set; }
        public string Label { get; set; }
        public string To { get; set; }
    }
}

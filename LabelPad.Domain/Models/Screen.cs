using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
   public class Screen:BaseModel
    {
        [ForeignKey("Module")]
        public int ModuleId { get; set; }
        [StringLength(200)]
        public string ScreenName { get; set; }
        [StringLength(200)]
        public string Icon { get; set; }
        [StringLength(200)]
        public int SequenceNo { get; set; }
        [StringLength(200)]
        public string ScreenTableName { get; set; }
        [StringLength(200)]
        public string ScreenChildTableName { get; set; }
        [StringLength(200)]
        public string Label { get; set; }
        public string To { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class DatasetFiles:BaseModel
    {
        [ForeignKey("Dataset")]
        public int DatasetId { get; set; }
        [StringLength(500)]
        public string FilePath { get; set; }
        public string Status { get; set; }
        [StringLength(100)]
        public string FileName { get; set; }
    }
}

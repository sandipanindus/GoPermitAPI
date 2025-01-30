using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class Project:BaseModel
    {
        [StringLength(200)]
        public string Name { get; set; }
        [StringLength(200)]
        public string Code { get; set; }
        [ForeignKey("DataSource")]
        public int DataSourceId { get; set; }
        [ForeignKey("DataType")]
        public int DataTypeId { get; set; }
        public int Status { get; set; }
        [ForeignKey("Team")]
        public int TeamId { get; set; }

    }
}

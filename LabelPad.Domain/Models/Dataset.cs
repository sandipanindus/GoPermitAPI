using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
   public class Dataset:BaseModel
    {
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        [StringLength(200)]
        public string DatasetName { get; set; }
        [ForeignKey("DataSource")]
        public int DatasourceId { get; set; }
        [ForeignKey("DataType")]
        public int DatatypeId { get; set; }

    }
}

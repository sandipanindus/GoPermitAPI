using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class ProjectAnnotationType:BaseModel
    {
        [ForeignKey("Project")]
        public int ProjectId { get; set; }
        [ForeignKey("AnnotationType")]
        public int AnnotationTypeId { get; set; }
    }
}

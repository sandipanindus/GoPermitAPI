using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
    public class AddProjectAc
    {
        public int DataTypeId { get; set; }
        public int DataSourceId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int Id { get; set; }
        public List<ProjectAnnotation> AnnotationTypeModel { get; set; }
        public int LoginId { get; set; }
        public int StatusId { get; set; }
        public int TeamId { get; set; }
    }
    public class ProjectAnnotation
    {
        public int AnnotationTypeId { get; set; }
       
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class AuditLogs:BaseModel
    {
        public int RegisterUserId { get; set; }
        public int RoleId { get; set; }
        public int TenantId { get; set; }
        public DateTime Date { get; set; }
        public string Function { get; set; }
        public string IP { get; set; }
        public string Agent { get; set; }
        public string Operation { get; set; }
        public bool Isread { get; set; }
    }
}

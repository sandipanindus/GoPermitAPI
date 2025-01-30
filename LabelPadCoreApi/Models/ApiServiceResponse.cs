using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LabelPadCoreApi.Models
{
    public class ApiServiceResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string type { get; set; }
        public string token { get; set; }
        public object Result { get; set; }
    }
}

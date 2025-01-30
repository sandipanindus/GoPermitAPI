using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
   public class AddRoleAc
    {
        public int Id { get; set; }
        public int LoginId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}

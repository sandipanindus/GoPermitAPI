using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
    public class GetTeamRegisterUsers
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public bool chkteam { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
    }
}

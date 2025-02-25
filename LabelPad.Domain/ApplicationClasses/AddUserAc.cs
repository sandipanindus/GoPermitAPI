using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
   public class AddUserAc
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string ContactNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CountryId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public int RoleId { get; set; }
        public int LoginId { get; set; }
        public string EmailCode { get; set; }
        public bool Active { get; set; }
        public string Zipcode { get; set; }
        public int SiteId { get; set; }
        public bool IsOperator { get; set; }
        public bool IsSiteUser { get; set; }


        public int OperatorId { get; set; }
        public bool IsMicrosoftAccount { get; set; }
    }
}

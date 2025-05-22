using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
    public class UpdateRegisterUserAc
    {
        public string ResidencyProof { get; set; }
        public string IdentityProof { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int Id { get; set; }
        public string ProfilePath { get; set; }
        public string EmailCode { get; set; }
    }
    public class UpdateRegisterUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PostCode { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public int Id { get; set; }
        public string ProfilePath { get; set; }
        public int CountryId { get; set; }
        public int RoleId { get; set; }
        public int LoginId { get; set; }
        public bool Active { get; set; }
        public int SiteId { get; set; }
    }

    public class OperatorLogoRequest
    {
        public int Id { get; set; }

        public string OperatorLogo { get; set; }
    }
}

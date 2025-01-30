using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
   public class AddRegisterUserAc
    {
        [Required]
        [StringLength(300)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(300)]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string SiteId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string ParkingBay { get; set; }

        public string EmailCode { get; set; }

        public int ParentId { get; set; }
        public int Id { get; set; }
        public string SiteName { get; set; }
    }

    public class ZatparkRequest
    {
        public int SiteId { get; set; }
        public string VRM { get; set; }
    }
    public class AddTenantUser
    {
        [Required]
        [StringLength(300)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(300)]
        public string LastName { get; set; }
        public string Address { get; set; }
        public string SiteId { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zipcode { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string Password { get; set; }
        public string ParkingBay { get; set; }

        public string EmailCode { get; set; }
        public bool IsUpdateEnddate { get; set; }

        public int ParentId { get; set; }
        public int Id { get; set; }
        public int? LoginId { get; set; }
        public string SiteName { get; set; }
        public string HouseOrFlatNo { get; set; }
        public List<BayConfigs> BayConfigs { get; set; }
    }
    public class BayConfigs
    {
        public int id { get; set; }
        public int? bayconfigid { get; set; }
        public string bayid { get; set; }
        public string vehiclesperbay { get; set; }
        public string dates { get; set; }
        public string vehiclereg { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
 
}

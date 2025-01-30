using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class RegisterUser:BaseModel
    {
        [Required]
        [StringLength(300)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(300)]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [StringLength(200)]
        public string State { get; set; }
        [StringLength(200)]
        public string City { get; set; }
        [StringLength(20)]
        public string ZipCode { get; set; }
        [StringLength(20)]
        public string MobileNumber { get; set; }
        [Required]
        [StringLength(100)]
        public string Email { get; set; }
        
        [StringLength(100)]
        public string Password { get; set; }
        public string EmailCode { get; set; }

        public int ParentId { get; set; }
        public bool IsVerified { get; set; }
        public int ClientId { get; set; }
        public int RoleId { get; set; }
        public int SiteId { get; set; }
        public string ParkingBay { get; set; }
        public int CountryId { get; set; }
        [StringLength(50)]
        public string HouseOrFlatNo { get; set; }

        public string ResidencyProofId { get; set; }
        public string IdentityProofId { get; set; }
        public string Address2 { get; set; }
        [StringLength(200)]
        public string ProfilePath { get; set; }
        public bool IsAdminCreated { get; set; }
        public bool UpdateEnddate { get; set; }
        public ICollection<VehicleRegistration> VehicleRegistrations { get; set; }
        public ICollection<Support> Supports { get; set; }
        public ICollection<BayConfig> BayConfigs { get; set; }
        public ICollection<VisitorParking> VisitorParkings { get; set; }
        public virtual ICollection<Ping> Pings { get; set; }
    }
}

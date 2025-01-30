using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace LabelPad.Domain.Models
{
    public class User:BaseModel
    {
        [StringLength(200)]
        public string FirstName { get; set; }
        [StringLength(200)]
        public string LastName { get; set; }
        [StringLength(200)]
        public string Email { get; set; }
        [StringLength(20)]
        public string ContactNumber { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public int CountryId { get; set; }
        [StringLength(200)]
        public string State { get; set; }
        [StringLength(200)]
        public string City { get; set; }
        public string ZipCode { get; set; }
        public int RoleId { get; set; }
        public string ConcurrencyStamp { get; set; }
        [StringLength(100)]
        public string Password { get; set; }
        [StringLength(20)]
        public string EmailCode { get; set; }
        public int ClientId { get; set; }
    }
}

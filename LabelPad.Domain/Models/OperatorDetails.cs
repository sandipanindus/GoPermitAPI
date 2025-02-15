using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LabelPad.Domain.Models
{
    public class OperatorDetails
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string OperatorName { get; set; }
        public string Email { get; set; }
        public string ContactctNumber { get; set; }
        public string RegisteredAddress { get; set; }
        public string TradingAddress { get; set; }
        public string RegisteredCity { get; set; }
        public string TradingCity { get; set; }
        public string RegisteredCounty { get; set; }
        public string TradingCounty { get; set; }
        public int RegisteredCountryId { get; set; }
        public int TradingCountryId { get; set; }
        public string RegisteredZipCode { get; set; }
        public string TradingZipCode { get; set; }
        public bool VatRegistered { get; set; }
        public string VatNumber { get; set; }
        public DateTime Date { get; set; }
        public int RoleId { get; set; }
        public string Notes { get; set; }
        public string Profile { get; set; }
        public string HelpImage { get; set; }
        public string Content { get; set; }
        public string Heading { get; set; }
        public bool? IsMicrosoftAccount { get; set; }
    }


}

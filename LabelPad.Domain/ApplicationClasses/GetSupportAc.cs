using System;
using System.Collections.Generic;
using System.Text;

namespace LabelPad.Domain.ApplicationClasses
{
    public class GetSupportAc
    {
        public string Date { get; set; }
        public int Id { get; set; }
        public string Subject { get; set; }
        public bool IsRead { get; set; }
        public int TicketId { get; set; }
        public string Status { get; set; }
        public string LastComment { get; set; }
    }
   public class GetSupportCls
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PropertyName { get; set; }
        public string Issue { get; set; }
        public string Subject { get; set; }
        public int Id { get; set; }
        public int RegisterUserId { get; set; }
        public string Response { get; set; }
    }
    public class GetPastQuery
    {
        public string Issue { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }
        public string Date { get; set; }
        public string Class { get; set; }
        public int CreatedBy { get; set; }
        public string Subject { get; set; }
        public string Status { get; set; }
        public string ProfilePath { get; set; }
        public int RoleId { get; set; }

    }
    public class Baycls
    {
        public int BayId { get; set; }
        public string BayName { get; set; }
    }
}

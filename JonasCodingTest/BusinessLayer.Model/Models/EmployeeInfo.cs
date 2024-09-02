using System;
using System.Collections.Generic;

namespace BusinessLayer.Model.Models
{
    public class EmployeeInfo : BaseInfo
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string CompanyName { get; set; }
        public string OccupationName { get; set; }
        public string EmployeeStatus { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string LastModifiedDateTime { get; set; }
        public List<ArSubledgerInfo> ArSubledgers { get; set; }
    }
}

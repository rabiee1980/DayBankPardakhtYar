using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Response
{
    public class GetClientInfoListResposneVM
    {
        public string customerNumber { get; set; }
        public string name { get; set; }
        public string birthDate { get; set; }
        public string birthCertificateId { get; set; }
        public string nationalCode { get; set; }
        public string address { get; set; }
        public int customerType { get; set; }
        public string shahabCode { get; set; }
        public object errorCode { get; set; }
        public string errorDesc { get; set; }
        public List<string> errorDetails { get; set; }
    }
}

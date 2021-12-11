using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Response
{
    public class GetAccountNumberResponseVM
    {
        public string accountNumber { get; set; }
        public string accountDescription { get; set; }
        public string errorCode { get; set; }
        public string errorDesc { get; set; }
        public List<string> errorDetails { get; set; }
    }
}

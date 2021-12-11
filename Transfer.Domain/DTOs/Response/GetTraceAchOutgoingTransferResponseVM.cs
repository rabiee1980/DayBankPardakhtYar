using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Response
{
    public class GetTraceAchOutgoingTransferResponseVM
    {
        public string transactionId { get; set; }
        public string status { get; set; }
        public string errorCode { get; set; }
        public string errorDesc { get; set; }
        public List<string> errorDetails { get; set; }
    }
}

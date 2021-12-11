using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Response
{
    public class GetSatnaOutgoingTransferValuesResponseVM
    {
        public string errorCode { get; set; }
        public string errorDesc { get; set; }
        public List<string> errorDetails { get; set; }
        public string traceId { get; set; }
        public string transactionId { get; set; }
    }
}

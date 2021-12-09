using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Request
{
    public class KarafarinGetTraceAchOutgoingTransferRequestVM
    {
        public string clientNo { get; set; }
        public string originalServiceCode { get; set; }
        public string traceId { get; set; }
        public string transactionDate { get; set; }
    }
}

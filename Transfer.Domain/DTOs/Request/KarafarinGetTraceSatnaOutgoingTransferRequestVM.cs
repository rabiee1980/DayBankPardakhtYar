using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Request
{
    public class KarafarinGetTraceSatnaOutgoingTransferRequestVM
    {
        public string clientNo { get; set; }
        public string paymentIdSender { get; set; }
        public string traceId { get; set; }
    }
}

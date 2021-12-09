using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Request
{
    public class KarafarinGetSatnaOutgoingTransferValuesRequestVM
    {
        public string traceId { get; set; }
        public string transactionDate { get; set; }
        public string sourceAccount { get; set; }
        public string destinationIban { get; set; }
        public string destinationBank { get; set; }
        public string amount { get; set; }
        public string sourcePaymentId { get; set; }
        public string destinationPaymentId { get; set; }
        public string reason { get; set; }
        public string destinationName { get; set; }
        public string destinationLastName { get; set; }
        public string transactionDescription { get; set; }
        public string branchCode { get; set; }
        public string clientNo { get; set; }
        public string originalServiceCode { get; set; }
        public List<string> signedBy { get; set; }
    }
}

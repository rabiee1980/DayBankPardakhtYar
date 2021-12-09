using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Request
{
    public class KarafarinGetLocalFundTransferRequestVM
    {
        public string transactionType { get; set; }
        public string traceId { get; set; }
        public string branchCode { get; set; }
        public string amount { get; set; }
        public string sourceAccount { get; set; }
        public string destinationAccount { get; set; }
        public string sourceTransactionDescription { get; set; }
        public string transactionDate { get; set; }
        public string sourcePaymentId { get; set; }
        public string destinationPaymentId { get; set; }
        public string clientNo { get; set; }
        public string destinationTransactionDescription { get; set; }
        public List<string> signedBy { get; set; }
    }
}

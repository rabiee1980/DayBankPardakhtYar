using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Response
{
    public class GetStatementByCountResponseVM
    {
        public string transactionDate { get; set; }
        public int amount { get; set; }
        public string systemDate { get; set; }
        public string creditDebit { get; set; }
        public object previousBalance { get; set; }
        public object actualBalance { get; set; }
        public string branchCode { get; set; }
        public string branchCodeDescription { get; set; }
        public string transactionDescription { get; set; }
        public string narrative { get; set; }
        public string paymentId { get; set; }
        public string creditDebitDescription { get; set; }
        public string errorCode { get; set; }
        public string errorDesc { get; set; }
        public List<string> errorDetails { get; set; }
    }
}

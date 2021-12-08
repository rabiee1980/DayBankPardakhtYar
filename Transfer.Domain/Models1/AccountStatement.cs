using System;
using System.Collections.Generic;

namespace Transfer.Domain.Models
{
    public partial class AccountStatement
    {
        public long Id { get; set; }
        public string Iban { get; set; }
        public string StatementId { get; set; }
        public DateTime? Date { get; set; }
        public bool? DateSpecified { get; set; }
        public int? TurnoverTypeId { get; set; }
        public decimal? TransferAmount { get; set; }
        public bool? TransferAmountSpecified { get; set; }
        public decimal? Balance { get; set; }
        public bool? BalanceSpecified { get; set; }
        public string Description { get; set; }
        public string PaymentId { get; set; }
        public string ReferenceNumber { get; set; }
        public long? RegistrationNumber { get; set; }
        public bool? RegistrationNumberSpecified { get; set; }
        public long? Serial { get; set; }
        public bool? SerialSpecified { get; set; }
        public string SerialNumber { get; set; }
        public string BranchCode { get; set; }
        public string BranchName { get; set; }
        public string AgentBranchCode { get; set; }
        public string AgentBranchName { get; set; }
    }
}

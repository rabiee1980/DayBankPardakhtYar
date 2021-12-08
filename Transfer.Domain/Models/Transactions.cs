using System;
using System.Collections.Generic;

namespace Transfer.Domain.Models
{
    public partial class Transactions
    {
        public long Id { get; set; }
        public DateTime CreationDateTime { get; set; }
        public string CreationJalaliDateTime { get; set; }
        public long RegistrationDate { get; set; }
        public string SourceIban { get; set; }
        public int TransferTypeId { get; set; }
        public long TransactionDate { get; set; }
        public string TrackingNumber { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public string DestinationIban { get; set; }
        public string OwnerName { get; set; }
        public string OwnerNationalId { get; set; }
        public string Description { get; set; }
        public int TransferStatusId { get; set; }
        public int? ErrorId { get; set; }
        public string Currency { get; set; }
        public string ReferenceId { get; set; }
        public string TransactionStatus { get; set; }
        public string TransferDescription { get; set; }
        public string TransferStatus { get; set; }
        public bool? Acceptable { get; set; }
        public bool? Cancelable { get; set; }
        public bool? Resumeable { get; set; }
        public bool? Suspendable { get; set; }
        public long? LastUpdate { get; set; }

        public virtual Errors Error { get; set; }
        public virtual TransferStatus TransferStatusNavigation { get; set; }
        public virtual TransferType TransferType { get; set; }
    }
}

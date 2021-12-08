using System;
using System.Collections.Generic;

namespace Transfer.Domain.Models
{
    public partial class TransferStatus
    {
        public TransferStatus()
        {
            Transactions = new HashSet<Transactions>();
            TransferStatusWs = new HashSet<TransferStatusWs>();
        }

        public int Id { get; set; }
        public int Value { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Transactions> Transactions { get; set; }
        public virtual ICollection<TransferStatusWs> TransferStatusWs { get; set; }
    }
}

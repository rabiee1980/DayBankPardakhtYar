using System;
using System.Collections.Generic;

namespace Transfer.Domain.Models
{
    public partial class TransferType
    {
        public TransferType()
        {
            Transactions = new HashSet<Transactions>();
        }

        public int Id { get; set; }
        public int Value { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}

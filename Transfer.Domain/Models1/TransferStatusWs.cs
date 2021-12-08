using System;
using System.Collections.Generic;

namespace Transfer.Domain.Models
{
    public partial class TransferStatusWs
    {
        public int Id { get; set; }
        public int TransferStatusId { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }

        public virtual TransferStatus TransferStatus { get; set; }
    }
}

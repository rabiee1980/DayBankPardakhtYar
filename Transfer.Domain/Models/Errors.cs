using System;
using System.Collections.Generic;

namespace Transfer.Domain.Models
{
    public partial class Errors
    {
        public Errors()
        {
            Transactions = new HashSet<Transactions>();
        }

        public int Id { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string ExtraData { get; set; }

        public virtual ErrorCodes ErrorCodeNavigation { get; set; }
        public virtual ICollection<Transactions> Transactions { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Transfer.Domain.DTOs
{
    public class sp_UsersReport
    {
        [Key]
        public long RowNumber { get; set; }

        public string UserName { get; set; }

        public int CountTransactions { get; set; }
        public decimal SumTransactions { get; set; }

        public int CountBillPayment { get; set; }
        public decimal SumBillPayment { get; set; }

        public int CountCharge { get; set; }
        public decimal SumCharge { get; set; }

        public int CountOrder { get; set; }
        public decimal SumOrder { get; set; }

        public int CountPurchase { get; set; }
        public decimal SumPurchase { get; set; }
        public int Marketing { get; set; }
        public decimal Score { get; set; }
        public int All { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Request
{
    public class KarafarinGetStatementByDateRequestVM
    {
        public string accountNumber { get; set; }
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }
}

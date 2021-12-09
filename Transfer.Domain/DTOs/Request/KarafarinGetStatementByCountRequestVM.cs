using System;
using System.Collections.Generic;
using System.Text;

namespace Transfer.Domain.DTOs.Request
{
    public class KarafarinGetStatementByCountRequestVM
    {
        public string accountNumber { get; set; }
        public string count { get; set; }

    }
}

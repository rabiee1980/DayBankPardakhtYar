using System;
using System.Collections.Generic;
using System.Text;
using Transfer.Domain.Models;

namespace Transfer.Services.Contracts
{
    public interface ITransferStatusService
    {
        TransferStatus Find(int? value);

        bool IsExists(int? value);
    }
}

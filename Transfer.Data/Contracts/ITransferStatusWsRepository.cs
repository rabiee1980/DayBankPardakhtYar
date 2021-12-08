using System;
using System.Collections.Generic;
using System.Text;
using Transfer.Domain.Models;

namespace Transfer.Data.Contracts
{
    public interface ITransferStatusWsRepository
    {
        TransferStatusWS Find(string value);

        bool IsExists(string value);
    }
}

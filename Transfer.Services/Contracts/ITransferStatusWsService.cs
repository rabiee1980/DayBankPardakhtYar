using System;
using System.Collections.Generic;
using System.Text;
using Transfer.Domain.Models;

namespace Transfer.Services.Contracts
{
    public interface ITransferStatusWsService
    {
        TransferStatusWS Find(string value);

        bool IsExists(string value);
    }
}

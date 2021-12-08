using System;
using System.Collections.Generic;
using System.Text;
using Transfer.Domain.Models;

namespace Transfer.Services.Contracts
{
    public interface ITransferTypeService
    {
        TransferType Find(int? value);

        bool IsExists(int? value);
    }
}

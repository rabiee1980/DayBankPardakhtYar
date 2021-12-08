using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Transfer.Core.Model.Base;
using Transfer.Domain.Models;

namespace Transfer.Data.Contracts
{
    public interface ITransactionsRepository
    {
        bool Find(string iban);

        Transactions CreateTransaction(Transactions  transactions);

        List<Transactions> TransferInquiry(ConvertedInquiryParams  convertedInquiryParams);

        List<Transactions> GetTransactionsForTransfer();

        List<Transactions> GetTranForUpdateTransferStatus();

        bool UpdateTransferValue(long id, string currency, string referenceId, string transactionStatus, string transferDescription, string transferStatus,int transferStatusId);

        bool UpdateTransferStatus(long id, string referenceId, int transferStatusId, string transferStatus, bool acceptable, bool cancelable, bool resumeable, bool suspendable);
        
    }
}

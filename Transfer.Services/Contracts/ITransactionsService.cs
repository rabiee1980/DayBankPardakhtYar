using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer.Core.Model.Base;
using Transfer.Domain.Models;

namespace Transfer.Services.Contracts
{
    public interface ITransactionsService
    {
        bool Find(string iban);

        Transactions CreateTransaction(Transactions transactions);
        
        List<Transactions> GetTransactionsForTransfer();

        List<Transactions> GetTranForUpdateTransferStatus();

        List<Transactions> TransferInquiry(ConvertedInquiryParams convertedInquiryParams);

        Token GenToken(string userName, string password);
        
        bool UpdateTransferValue(long id, string currency, string referenceId, string transactionStatus, string transferDescription, string transferStatus, int transferStatusId);

        bool UpdateTransferStatus(long id, string referenceId, int transferStatusId, string transferStatus, bool acceptable, bool cancelable, bool resumeable, bool suspendable);

    }
}

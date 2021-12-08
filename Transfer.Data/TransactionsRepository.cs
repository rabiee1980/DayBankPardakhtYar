using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Transfer.Core.Helpers;
using Transfer.Core.Model.Base;
using Transfer.Data.Base;
using Transfer.Data.Contracts;
using Transfer.Domain.DTOs;
using Transfer.Domain.Models;

namespace Transfer.Data
{
    public class TransactionsRepository : BaseRepository<Transactions>, ITransactionsRepository
    {
        private TSS_DBContext _repositoryContext;

        public TransactionsRepository(TSS_DBContext repositoryContext) : base(repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public bool Find(string iban)
        {
            return FindByCondition(a => a.SourceIban == iban).Any();
        }

        public Transactions CreateTransaction(Transactions transactions)
        {
            Create(transactions);
            Save();
            return transactions;
        }

        public List<Transactions> GetTransactionsForTransfer()
        {
            return FindByCondition(a => a.TransferStatusId == 0).OrderBy(a => a.CreationDateTime).ToList();
        }

        public List<Transactions> GetTranForUpdateTransferStatus()
        {
            return FindByCondition(a => a.TransferStatusId == 1).OrderBy(a => a.CreationDateTime).ToList();
        }

        public bool UpdateTransferValue(long id, string currency, string referenceId, string transactionStatus, string transferDescription, string transferStatus, int transferStatusId)
        {
            Transactions dbTransactions = FindByCondition(item => item.Id == id).SingleOrDefault();
            if (dbTransactions == null) return false;
            dbTransactions.MapProccess(currency, referenceId, transactionStatus, transferDescription, transferStatus, transferStatusId);
            Update(dbTransactions);
            Save();
            _repositoryContext.Entry(dbTransactions).Reload();
            return true;
        }

        public bool UpdateTransferStatus(long id, string referenceId, int transferStatusId, string transferStatus, bool acceptable, bool cancelable, bool resumeable, bool suspendable)
        {
            Transactions dbTransactions = FindByCondition(item => item.Id == id && item.ReferenceId == referenceId).SingleOrDefault();
            if (dbTransactions == null) return false;
            dbTransactions.MapTransferStatus(transferStatusId, transferStatus, acceptable, cancelable, resumeable, suspendable);
            Update(dbTransactions);
            Save();
            _repositoryContext.Entry(dbTransactions).Reload();
            return true;
        }

        [Obsolete]
        public List<Transactions> TransferInquiry(ConvertedInquiryParams convertedInquiryParams)
        {
            List<Transactions> transactions = _repositoryContext.Transactions.FromSql("sp_TransferInquiry @sourceIban={0} , @transferTypeId={1} , @ftransactionDate={2} ,@ltransactionDate={3} , @fregistrationDate={4} ,@lregistrationDate={5} , @trackingNumber={6} , @referenceNumber={7} , @id={8} , @transferStatusId={9} , @famount={10} ,@lamount={11} , @destinationIban={12}  ", convertedInquiryParams.sourceIbans, convertedInquiryParams.transferType, convertedInquiryParams.ftransferDate, convertedInquiryParams.ltransferDate, convertedInquiryParams.fregistrationDate, convertedInquiryParams.lregistrationDate, convertedInquiryParams.trackingNumbers, convertedInquiryParams.referenceNumbers, convertedInquiryParams.transactionIds, convertedInquiryParams.status, convertedInquiryParams.famount, convertedInquiryParams.lamount, convertedInquiryParams.destinationIbans).AsEnumerable().OrderBy(a => a.RegistrationDate).ToList();
            return transactions;
        }
    }

    static class TransactionsExtends
    {
        public static Transactions MapProccess(this Transactions dbTransactions, string currency, string referenceId, string transactionStatus, string transferDescription, string transferStatus, int transferStatusId)
        {
            if (dbTransactions == null) return null;
            dbTransactions.Currency = currency;
            dbTransactions.ReferenceId = referenceId;
            dbTransactions.TransactionStatus = transactionStatus;
            dbTransactions.TransferDescription = transferDescription;
            dbTransactions.TransferStatus = transferStatus;
            dbTransactions.TransferStatusId = transferStatusId;
            dbTransactions.LastUpdate = DateHelpers.DateTimeToTimestamp(DateTime.Now);
            return dbTransactions;
        }

        public static Transactions MapTransferStatus(this Transactions dbTransactions, int transferStatusId, string transferStatus, bool acceptable, bool cancelable, bool resumeable, bool suspendable)
        {
            if (dbTransactions == null) return null;

            if (dbTransactions.TransferStatusId != transferStatusId)
            {
                dbTransactions.LastUpdate = DateHelpers.DateTimeToTimestamp(DateTime.Now);
            }

            dbTransactions.TransferStatusId = transferStatusId;
            dbTransactions.TransferStatus = transferStatus;
            dbTransactions.Acceptable = acceptable;
            dbTransactions.Cancelable = cancelable;
            dbTransactions.Resumeable = resumeable;
            dbTransactions.Suspendable = suspendable;

            return dbTransactions;
        }
    }
}

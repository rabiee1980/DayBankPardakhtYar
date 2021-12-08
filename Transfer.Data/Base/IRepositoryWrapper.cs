using Transfer.Data;
using Transfer.Data.Contracts;

namespace Transfer.Data.Base
{
    public interface IRepositoryWrapper
    {
        ITransactionsRepository Transactions { get; }

        ITransferTypeRepository  TransferType { get; }

        ITransferStatusRepository TransferStatus { get; }

        ITransferStatusWsRepository TransferStatusWs { get; }

        IErrorsRepository Errors { get; }

        IErrorCodesRepository ErrorCodes { get; } 

        IAccountStatementRepository AccountStatement { get; }

    }
}

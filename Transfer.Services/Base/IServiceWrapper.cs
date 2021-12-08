using Transfer.Services.Contracts;

namespace Transfer.Services.Base
{
    public interface IServiceWrapper
    {
        ITransactionsService  Transactions { get; }    
        
        ITransferTypeService TransferType { get; }

        ITransferStatusService TransferStatus { get; }

        ITransferStatusWsService TransferStatusWs { get; }

        IErrorsService Errors { get; } 

        IErrorCodesService ErrorCodes { get; }

        IAccountStatementService AccountStatement { get; }
    }
}

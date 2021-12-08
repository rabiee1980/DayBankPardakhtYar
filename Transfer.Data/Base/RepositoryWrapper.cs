using Transfer.Data;
using Transfer.Data.Contracts;
using Transfer.Domain.Models;

namespace Transfer.Data.Base
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private TSS_DBContext _repoContext;
       
        public ITransactionsRepository Transactions => new TransactionsRepository(_repoContext);

        public ITransferTypeRepository TransferType => new TransferTypeRepository(_repoContext);

        public ITransferStatusRepository TransferStatus => new TransferStatusRepository(_repoContext);

        public ITransferStatusWsRepository TransferStatusWs => new TransferStatusWsRepository(_repoContext);

        public IErrorsRepository Errors => new ErrorsRepository(_repoContext);

        public IErrorCodesRepository ErrorCodes => new ErrorCodesRepository(_repoContext);

        public IAccountStatementRepository AccountStatement => new AccountStatementRepository(_repoContext);

        public RepositoryWrapper(TSS_DBContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }
    }
}

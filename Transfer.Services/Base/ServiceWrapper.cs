using Transfer.Core.Model.Base;
using Transfer.Services.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Transfer.Data.Base;
using Transfer.RemoteServices.Base;
using Transfer.Domain.Models;

namespace Transfer.Services.Base
{
    public class ServiceWrapper : IServiceWrapper
    {
        private readonly IOptions<AppSettings> _appSettings;
        private IRepositoryWrapper _repository;

        public ITransactionsService  Transactions => new TransactionsService(_repository,_appSettings);

        public ITransferTypeService TransferType => new TransferTypeService(_repository,_appSettings);

        public ITransferStatusService TransferStatus => new TransferStatusService(_repository, _appSettings);

        public ITransferStatusWsService TransferStatusWs => new TransferStatusWsService(_repository, _appSettings);

        public IErrorsService Errors => new ErrorsService(_repository, _appSettings);

        public IErrorCodesService ErrorCodes => new ErrorCodesService(_repository,_appSettings);

        public IAccountStatementService AccountStatement => new AccountStatementService(_repository,_appSettings);

        public ServiceWrapper(IRepositoryWrapper repositoryWrapper, IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings;
            _repository = repositoryWrapper;
        }
    }
}


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Transfer.Core.Model.Base;
using Transfer.Data.Base;
using Transfer.Domain.Models;
using Transfer.RemoteServices.Base;
using Transfer.Services.Contracts;

namespace Transfer.Services
{
    public class TransferTypeService : ITransferTypeService
    {
        private readonly AppSettings _appSettings;
        private IRepositoryWrapper _repository;

        public TransferTypeService(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _appSettings = appSettings.Value;
        }
        public TransferType Find(int? value)
        {
            return _repository.TransferType.Find(value);
        }

        public bool IsExists(int? value)
        {
            return _repository.TransferType.IsExists(value);
        }
    }
}

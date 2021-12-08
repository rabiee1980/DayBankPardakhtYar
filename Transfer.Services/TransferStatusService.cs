using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Transfer.Core.Model.Base;
using Transfer.Data.Base;
using Transfer.Domain.Models;
using Transfer.Services.Contracts;

namespace Transfer.Services
{
    public class TransferStatusService : ITransferStatusService
    {
        private readonly AppSettings _appSettings;
        private IRepositoryWrapper _repository;

        public TransferStatusService(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _appSettings = appSettings.Value;
        }
        public TransferStatus Find(int? value)
        {
            return _repository.TransferStatus.Find(value);
        }

        public bool IsExists(int? value)
        {
            return _repository.TransferStatus.IsExists(value);
        }
    }
}

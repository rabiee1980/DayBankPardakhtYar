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
    public class TransferStatusWsService : ITransferStatusWsService
    {
        private readonly AppSettings _appSettings;
        private IRepositoryWrapper _repository;

        public TransferStatusWsService(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _appSettings = appSettings.Value;
        }
        public TransferStatusWS Find(string value)
        {
            return _repository.TransferStatusWs.Find(value);
        }

        public bool IsExists(string value)
        {
            return _repository.TransferStatusWs.IsExists(value);
        }
    }
}

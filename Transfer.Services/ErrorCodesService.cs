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
    public class ErrorCodesService : IErrorCodesService
    {
        private readonly AppSettings _appSettings;
        private IRepositoryWrapper _repository;

        public ErrorCodesService(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _appSettings = appSettings.Value;
        }
        public ErrorCodes Find(int? id)
        {
            return _repository.ErrorCodes.Find(id);
        }
    }
}

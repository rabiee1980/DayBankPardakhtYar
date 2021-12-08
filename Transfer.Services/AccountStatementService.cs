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
    public class AccountStatementService : IAccountStatementService
    {
        private readonly AppSettings _appSettings;
        IRepositoryWrapper _repository;
        public AccountStatementService(IRepositoryWrapper repository, IOptions<AppSettings> appSettings)
        {
            _repository = repository;
            _appSettings = appSettings.Value;
        }
        public AccountStatement CreateAccountStatement(AccountStatement accountStatement)
        {
            return _repository.AccountStatement.CreateAccountStatement(accountStatement);
        }

        public DateTime? GetLastDate(string iban)
        {
            return _repository.AccountStatement.GetLastDate(iban);
        }

        public List<AccountStatement> StatementInquiry(ConvertedStatementInquiryParams convertedStatementInquiryParams)
        {
            return _repository.AccountStatement.StatementInquiry(convertedStatementInquiryParams);
        }
    }
}

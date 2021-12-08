using System;
using System.Collections.Generic;
using System.Text;
using Transfer.Core.Model.Base;
using Transfer.Domain.Models;

namespace Transfer.Services.Contracts
{
    public interface IAccountStatementService
    {
        AccountStatement CreateAccountStatement(AccountStatement accountStatement);

        DateTime? GetLastDate(string iban);

        List<AccountStatement> StatementInquiry(ConvertedStatementInquiryParams convertedStatementInquiryParams);
    }
}

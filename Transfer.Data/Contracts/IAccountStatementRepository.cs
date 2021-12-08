using System;
using System.Collections.Generic;
using System.Text;
using Transfer.Core.Model.Base;
using Transfer.Domain.Models;

namespace Transfer.Data.Contracts
{
    public interface IAccountStatementRepository
    {
        AccountStatement CreateAccountStatement(AccountStatement accountStatement);

        DateTime? GetLastDate(string iban);

        List<AccountStatement> StatementInquiry(ConvertedStatementInquiryParams convertedStatementInquiryParams);
    }
}

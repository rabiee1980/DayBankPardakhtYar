using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer.Core.Model.Base;
using Transfer.Data.Base;
using Transfer.Data.Contracts;
using Transfer.Domain.Models;

namespace Transfer.Data
{
    public class AccountStatementRepository : BaseRepository<AccountStatement>, IAccountStatementRepository
    {
        TSS_DBContext _context;
        public AccountStatementRepository(TSS_DBContext context) : base(context)
        {
            _context = context;
        }

        public AccountStatement CreateAccountStatement(AccountStatement accountStatement)
        {
            Create(accountStatement);
            Save();
            return accountStatement;
        }

        public DateTime? GetLastDate(string iban)
        {
            if (FindByCondition(a => a.Iban == iban).Any())
            {
                return FindByCondition(a => a.Iban == iban).OrderByDescending(a => a.Date).First().Date;
            }
            else
            {
                return null;
            }
        }


        [Obsolete]
        public List<AccountStatement> StatementInquiry(ConvertedStatementInquiryParams convertedStatementInquiryParams)
        {
            List<AccountStatement> accountStatement = _context.AccountStatement.FromSql("sp_StatementInquiry @iban={0} , @fdate={1} , @ldate={2} , @turnoverType={3} , @amount={4} , @voucherDescription={5} , @articleDescription={6} , @referenceNumbers={7} , @offset={8} , @pageSize={9} ", convertedStatementInquiryParams.iban, convertedStatementInquiryParams.fdate, convertedStatementInquiryParams.ldate, convertedStatementInquiryParams.turnoverType, convertedStatementInquiryParams.amount, convertedStatementInquiryParams.voucherDescription, convertedStatementInquiryParams.articleDescription, convertedStatementInquiryParams.referenceNumbers, convertedStatementInquiryParams.offset, convertedStatementInquiryParams.pageSize).ToList();
            return accountStatement;
        }
    }
}

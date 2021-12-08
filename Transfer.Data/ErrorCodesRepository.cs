using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer.Data.Base;
using Transfer.Data.Contracts;
using Transfer.Domain.Models;

namespace Transfer.Data
{
    public class ErrorCodesRepository : BaseRepository<ErrorCodes>, IErrorCodesRepository
    {
        public ErrorCodesRepository(TSS_DBContext context):base(context)
        { 
        
        }
        public ErrorCodes Find(int? id)
        {
            return FindByCondition(a=>a.Id==id).SingleOrDefault();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer.Data.Base;
using Transfer.Data.Contracts;
using Transfer.Domain.Models;

namespace Transfer.Data
{
    public class ErrorsRepository :BaseRepository<Errors>, IErrorsRepository
    {
        public ErrorsRepository(TSS_DBContext context):base(context)
        { 
        
        }
        public Errors Find(int? id)
        {
            return FindByCondition(a=>a.Id== id).SingleOrDefault();
        }
    }
}

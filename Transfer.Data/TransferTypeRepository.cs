using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transfer.Data.Base;
using Transfer.Data.Contracts;
using Transfer.Domain.Models;

namespace Transfer.Data
{
    public class TransferTypeRepository : BaseRepository<TransferType>, ITransferTypeRepository
    {
        public TransferTypeRepository(TSS_DBContext context) : base(context)
        {

        }
        public TransferType Find(int? value)
        {
            return FindByCondition(a => a.Value == value).SingleOrDefault();
        }

        public bool IsExists(int? value)
        {
            return FindByCondition(a => a.Value == value).Any();
        }
    }
}

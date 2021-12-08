using System.Linq;
using Transfer.Data.Base;
using Transfer.Data.Contracts;
using Transfer.Domain.Models;

namespace Transfer.Data
{
    public class TransferStatusRepository:BaseRepository<TransferStatus>, ITransferStatusRepository
    {
        public TransferStatusRepository(TSS_DBContext context):base(context)
        { 
        
        }

        public TransferStatus Find(int? value)
        {
            return FindByCondition(a=>a.Value==value).SingleOrDefault();
        }

        public bool IsExists(int? value)
        {
            return FindByCondition(a => a.Value == value).Any();
        }
    }
}

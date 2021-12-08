using System.Linq;
using Transfer.Data.Base;
using Transfer.Data.Contracts;
using Transfer.Domain.Models;

namespace Transfer.Data
{
    public class TransferStatusWsRepository : BaseRepository<TransferStatusWS>, ITransferStatusWsRepository
    {
        public TransferStatusWsRepository(TSS_DBContext context):base(context)
        { 
        
        }

        public TransferStatusWS Find(string value)
        {
            return FindByCondition(a=>a.Name==value).SingleOrDefault();
        }

        public bool IsExists(string value)
        {
            bool res= FindByCondition(a => a.Name == value).Any();
            return res;
        }
    }
}

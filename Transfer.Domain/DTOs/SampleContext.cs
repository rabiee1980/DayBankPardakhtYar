using Microsoft.EntityFrameworkCore;

namespace Transfer.Domain.DTOs
{
    public partial class SampleContext : DbContext
    {
        public SampleContext()
        {
        }

        public SampleContext(DbContextOptions<SampleContext> options)
            : base(options)
        {
        }
       
        public virtual DbSet<sp_UsersReport> sp_UsersReport { get; set; }        
    }
}

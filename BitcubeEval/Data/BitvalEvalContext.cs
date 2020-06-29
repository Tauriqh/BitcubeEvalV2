using Microsoft.EntityFrameworkCore;

namespace BitcubeEval.Data
{
    public class BitvalEvalContext : DbContext
    {
        public BitvalEvalContext (DbContextOptions<BitvalEvalContext> options)
            : base(options)
        {
        }

        public DbSet<BitcubeEval.Models.ApplicationUser> ApplicationUser { get; set; }
    }
}

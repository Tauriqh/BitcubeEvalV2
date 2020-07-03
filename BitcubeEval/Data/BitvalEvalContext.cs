using BitcubeEval.Models;
using Microsoft.EntityFrameworkCore;

namespace BitcubeEval.Data
{
    public class BitvalEvalContext : DbContext
    {
        public BitvalEvalContext (DbContextOptions<BitvalEvalContext> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Friend> Friend { get; set; }
    }
}

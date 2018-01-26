using System.Data.Entity;

namespace Test.DAL
{
    public class Context:DbContext
    {
        public DbSet<Game> Games { get; set; }
    }
}

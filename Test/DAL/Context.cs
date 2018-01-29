using System.Data.Entity;

namespace Test.DAL
{
    public class Context:DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Play> Plays { get; set; }
    }
}

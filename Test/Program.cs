using Test.DAL;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AppStart.InitDataBasePath();
            LinkGenerator linkGen = new LinkGenerator(2017, 350, 2);
            Context db = new Context();
            JsonParser.ParseScoringPlays(db, linkGen);
            db.SaveChanges();
            db.Dispose();
        }
    }
}

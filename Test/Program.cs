using Test.DAL;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            AppStart.InitDataBasePath();
            LinkGenerator linkGen = new LinkGenerator(2017, 360, 2);
            Context db = new Context();

            int failedCount = 0;
            foreach (string link in linkGen)
            {
                JsonParser parser = new JsonParser(link);
                if (parser.PageNotFound)
                {
                    failedCount += 1;
                    if (failedCount == 2)
                    {
                        break;
                    }
                    continue;
                }
                JsonParser.ParseScoringPlays(db);
            }
            db.SaveChanges();
            db.Dispose();
        }
    }
}

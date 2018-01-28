using System.Collections;
using System.Linq;
using Test.DAL;

namespace Test
{
    /// <summary>
    ///       Generate links for NHL API for each game in (int year, int numOfMatches, int gameType) 
    ///       year is the starting year of the season, 
    ///       numOfMatches is as many matches the links will be generated starting from 1
    ///       gameType is either 1 for pre-season, 2 for regular season, 3 for playoffs
    /// </summary>
    public class LinkGenerator : IEnumerable
    {
        public int NumOfMatches;
        public int Year;
        public int GameType;
        public int Starting;

        /// <summary>
        ///       Generate links for NHL API for each game in (int year, int numOfMatches, int gameType) 
        ///       year is the starting year of the season, 
        ///       numOfMatches is as many matches the links will be generated starting from 1
        ///       gameType is either 1 for pre-season, 2 for regular season, 3 for playoffs
        /// </summary>
        public LinkGenerator(int year, int numOfMatches, int gameType)
        {
            NumOfMatches = numOfMatches;
            Year = year;
            GameType = gameType;

            Context db = new Context();
            var season = db.Games.Select(game => game.NHLID).Where(id => id.Substring(0, 4) == Year.ToString());
            var starting = "";

            if (season.Count() == 0)
            {
                starting = "0000";
            }
            else
            {
                starting = season.Max().Substring(6, 4);
            }
            int startingfield;

            int.TryParse(starting, out startingfield);
            Starting = startingfield + 1;

            db.Dispose();
        }

        public IEnumerator GetEnumerator()
        {
            string GameTypePadded = GameType.ToString().PadLeft(2, '0');
            for (int GameID = Starting; GameID <= NumOfMatches; GameID++)
            {
                string GameIdPadded = GameID.ToString().PadLeft(4, '0');
                yield return "http://statsapi.web.nhl.com/api/v1/game/" + Year.ToString() + GameTypePadded + GameIdPadded + "/feed/live";
            }
        }
    }
}

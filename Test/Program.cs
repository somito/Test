using System;
using System.Collections;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using System.Dynamic;
using Newtonsoft.Json.Converters;
using Test.DAL;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            string executable = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string path = (System.IO.Path.GetDirectoryName(executable));
            AppDomain.CurrentDomain.SetData("DataDirectory", path);

            Season reg = new Season(2017, 2);
            Context db = new Context();

            //db.Database.Delete();
            int failedCount = 0;

            foreach (string link in reg)
            {
                WebClient client = new WebClient();
                if (failedCount == 2) { break; }
                try
                {
                    String htmlCode = client.DownloadString(link);
                    dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(htmlCode, new ExpandoObjectConverter());

                    Game game = new Game();
                    game.NHLID = json.gamePk.ToString();
                    string IDs = "";
                    foreach (var playID in json.liveData.plays.scoringPlays)
                    {
                        IDs = IDs + playID.ToString() + ";";
                    }
                    game.ScoringPlays = IDs;
                    db.Games.Add(game);
                }
                catch (WebException)
                {
                    failedCount += 1;
                }
                
            }
            db.SaveChanges();
            db.Dispose();
        }
    }

    public class Season : IEnumerable
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
        public Season(int year, int gameType)
        {
            NumOfMatches = 1280;
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

            int.TryParse(starting, out int startingfield);
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

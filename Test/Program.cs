using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
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
            
            Season reg = new Season(2017, 100, 2);
            Context db = new Context();

            db.Database.Delete();

            foreach (string link in reg)
            {
                WebClient client = new WebClient();
                String htmlCode = client.DownloadString(link);
                dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(htmlCode, new ExpandoObjectConverter());

                string IDs = "";
                foreach (var playID in json.liveData.plays.scoringPlays)
                {
                    IDs = IDs + playID.ToString() + ";";
                }

                

                Game game = new Game();

                game.ScoringPlays = IDs;

                db.Games.Add(game);

                

                
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

        /// <summary>
        ///       Generate links for NHL API for each game in (int year, int numOfMatches, int gameType) 
        ///       year is the starting year of the season, 
        ///       numOfMatches is as many matches the links will be generated starting from 1
        ///       gameType is either 1 for pre-season, 2 for regular season, 3 for playoffs
        /// </summary>
        public Season(int year, int numOfMatches, int gameType)
        {
            NumOfMatches = numOfMatches;
            Year = year;
            GameType = gameType;
        }

        public IEnumerator GetEnumerator()
        {
            string GameTypePadded = GameType.ToString().PadLeft(2, '0');
            for (int GameID = 1; GameID <= NumOfMatches; GameID++)
            {
                string GameIdPadded = GameID.ToString().PadLeft(4, '0');
                yield return "http://statsapi.web.nhl.com/api/v1/game/" + Year.ToString() + GameTypePadded + GameIdPadded + "/feed/live";
            }
        }
    }
}

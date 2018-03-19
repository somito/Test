using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Test.DAL;

namespace Test
{
    /// <summary>
    ///       A collection of methods to parse data from the NHL Json API
    /// </summary>
    public class JsonParser
    {
        private dynamic Json; //the Json object created from each link
        public bool PageNotFound = false;

        /// <summary>
        ///       Initialize the Json object from the link, and check if the link could be found
        /// </summary>
        public JsonParser(string link)
        {
            WebClient client = new WebClient();
            try
            {
                String htmlCode = client.DownloadString(link);
                dynamic json = JsonConvert.DeserializeObject<ExpandoObject>(htmlCode, new ExpandoObjectConverter());
                Json = json;
            }
            catch (WebException)
            {
                PageNotFound = true;
            }
        }
        /// <summary>
        ///       Gets a sequence of IDs of scoring plays from the Json object and adds it to the DB.Context
        /// </summary>
        public void ParseScoringPlays(Context db)
        {
            Game game = new Game();
            game.NHLID = Json.gamePk.ToString();
            string IDs = "";
            foreach (var playID in Json.liveData.plays.scoringPlays)
            {
                IDs = IDs + playID.ToString() + ";";
            }
            game.ScoringPlays = IDs;
            db.Games.Add(game);
            db.SaveChanges();

            String pattern = @"[;]";
            string[] scoringPlays = Regex.Split(game.ScoringPlays, pattern);
            foreach (var playNo in scoringPlays)
            {
                if (playNo != "")
                {
                    Play playToAdd = new Play();
                    playToAdd.GameID = game.ID;
                    playToAdd.PlayNo = playNo;
                    var coordinates = Json.liveData.plays.allPlays[int.Parse(playNo)].coordinates;
                    playToAdd.Coordinates = coordinates.x + "," + coordinates.y;

                    var playerList = Json.liveData.plays.allPlays[int.Parse(playNo)].players;

                    foreach (var player in playerList)
                    {
                        if (player.playerType == "Scorer")
                        {
                            var scorerID = player.player.id;
                            playToAdd.ScorerID = scorerID.ToString();
                        }

                        if (player.playerType == "Goalie")
                        {
                            var golieID = player.player.id;
                            playToAdd.GolieID = golieID.ToString();
                        }
                    }
                    //var scorerID = Json.liveData.plays.allPlays[int.Parse(playNo)].players[0].player.id;
                    //var golieID = Json.liveData.plays.allPlays[int.Parse(playNo)].players[lastItemIndex - 1].player.id;

                    //playToAdd.ScorerID = scorerID.ToString();
                    //playToAdd.GolieID = golieID.ToString();
                    db.Plays.Add(playToAdd);
                }
                db.SaveChanges();
            }
        }
    }
}

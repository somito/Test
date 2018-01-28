﻿using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using Test.DAL;

namespace Test
{
    public class JsonParser
    {
        public static void ParseScoringPlays(Context db, LinkGenerator linkGen)
        {
            int failedCount = 0;
            foreach (string link in linkGen)
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

        }
    }
}

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
    /// <summary>
    ///       A collection of methods to parse data from the NHL Json API
    /// </summary>
    public class JsonParser
    {
        private static dynamic Json; //the Json object created from each link
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
        public static void ParseScoringPlays(Context db)
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
        }
    }
}

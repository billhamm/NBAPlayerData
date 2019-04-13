using System;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NBACrawler.DAL;

namespace NBACrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient wc = new WebClient();

            for (int i = 201000; i <= 300000; i++)
            {
                string x = "http://stats.nba.com/stats/commonplayerinfo?LeagueID=00&PlayerID=";
                string z = "&SeasonType=Regular+Season";
                string url = x + i + z;

                try
                {
                    var json = (JObject) JsonConvert.DeserializeObject(wc.DownloadString(url));

                    var results = json.First.Next.Next.Last;
                    var playerDetails = results.First.Last.First;

                    foreach (var detail in playerDetails)
                    {
                        Console.WriteLine(detail[3]);

                        DateTime? birthDate = null;

                        if (detail[6].HasValues)
                        {
                            birthDate = Convert.ToDateTime(detail[6]);
                        }
                        if (!detail[6].HasValues)
                        {
                            birthDate = null;
                        }

                        var context = new NBADataEntities();

                        var t = new Player()
                        {
                            PERSON_ID = Convert.ToInt32(detail[0]),
                            FIRST_NAME = !string.IsNullOrEmpty(detail[1].ToString()) ? detail[1].ToString() : null,
                            LAST_NAME = !string.IsNullOrEmpty(detail[2].ToString()) ? detail[2].ToString() : null,
                            DISPLAY_FIRST_LAST = !string.IsNullOrEmpty(detail[3].ToString()) ? detail[3].ToString() : null,
                            DISPLAY_LAST_COMMA_FIRST = !string.IsNullOrEmpty(detail[4].ToString()) ? detail[4].ToString() : null,
                            DISPLAY_FI_LAST = !string.IsNullOrEmpty(detail[5].ToString()) ? detail[5].ToString() : null,
                            BIRTHDATE = birthDate,
                            SCHOOL = !string.IsNullOrEmpty(detail[7].ToString()) ? detail[7].ToString() : null,
                            COUNTRY = !string.IsNullOrEmpty(detail[8].ToString()) ? detail[8].ToString() : null,
                            LAST_AFFILIATION = !string.IsNullOrEmpty(detail[9].ToString()) ? detail[9].ToString() : null,
                            HEIGHT = !string.IsNullOrEmpty(detail[10].ToString()) ? detail[10].ToString() : null,
                            WEIGHT = !string.IsNullOrEmpty(detail[11].ToString()) ? detail[11].ToString() : null,
                            SEASON_EXP = !string.IsNullOrEmpty(detail[12].ToString()) ? detail[12].ToString() : null,
                            JERSEY = !string.IsNullOrEmpty(detail[13].ToString()) ? detail[13].ToString() : null,
                            POSITION = !string.IsNullOrEmpty(detail[14].ToString()) ? detail[14].ToString() : null,
                            ROSTERSTATUS = !string.IsNullOrEmpty(detail[15].ToString()) ? detail[15].ToString() : null,
                            TEAM_ID = !string.IsNullOrEmpty(detail[16].ToString()) ? detail[16].ToString() : null,
                            TEAM_NAME = !string.IsNullOrEmpty(detail[17].ToString()) ? detail[17].ToString() : null,
                            TEAM_ABBREVIATION = !string.IsNullOrEmpty(detail[18].ToString()) ? detail[18].ToString() : null,
                            TEAM_CODE = !string.IsNullOrEmpty(detail[19].ToString()) ? detail[19].ToString() : null,
                            TEAM_CITY = !string.IsNullOrEmpty(detail[20].ToString()) ? detail[20].ToString() : null,
                            PLAYERCODE = !string.IsNullOrEmpty(detail[21].ToString()) ? detail[21].ToString() : null,
                            FROM_YEAR = !string.IsNullOrEmpty(detail[22].ToString()) ? detail[22].ToString() : null,
                            TO_YEAR = !string.IsNullOrEmpty(detail[23].ToString()) ? detail[23].ToString() : null,
                            DLEAGUE_FLAG = !string.IsNullOrEmpty(detail[24].ToString()) ? detail[24].ToString() : null,
                            GAMES_PLAYED_FLAG = !string.IsNullOrEmpty(detail[25].ToString()) ? detail[25].ToString() : null,
                            DRAFT_YEAR = !string.IsNullOrEmpty(detail[26].ToString()) ? detail[26].ToString() : null,
                            DRAFT_ROUND = !string.IsNullOrEmpty(detail[27].ToString()) ? detail[27].ToString() : null,
                            DRAFT_NUMBER = !string.IsNullOrEmpty(detail[28].ToString()) ? detail[28].ToString() : null

                        };

                        if (context.Players.Any(k => k.PERSON_ID == t.PERSON_ID))
                        {
                            continue;
                        }
                        else
                        {
                            context.Players.Add(t);
                            context.SaveChanges();
                        }


                    }
                }
                catch(Exception ex)

                {
                    continue;
                }
            }
        }
    }
}

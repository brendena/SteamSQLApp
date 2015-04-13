using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Data;
using PortableSteam;
using PortableSteam.Fluent;
using PortableSteam.Infrastructure;
using PortableSteam.Interfaces;
using System.Data.SqlClient;

namespace PortableSteam
{
    class Program
    {
        static void Main(string[] args)
        {


            SqlConnection conn = new SqlConnection("Server=BRENDEN_PC\\SQLEXPRESS;Database=steamDb;Integrated Security=true");
            conn.Open();

            long[] steamID = new long[] { 76561198065588383, 76561198047886273 };
            SteamWebAPI.SetGlobalKey("6373F8810E9FA6C9781491E5F32D753F");
            /*
            var mySteamLevel = SteamWebAPI.General()
                                    .IPlayerService()
                                    .GetSteamLevel(SteamIdentity.FromSteamID(76561198047886273))
                                    .GetResponse();

            //var response = SteamWebAPI.General().ISteamApps().GetAppList().GetResponseString();

            //Console.WriteLine(response);
            /*
            var hello = SteamWebAPI.General().ISteamUser().GetFriendList(SteamIdentity.FromSteamID(76561198047886273), RelationshipType.All).GetResponse();
            foreach (var friend in hello.Data.Friends)
            {
                Console.WriteLine(friend.Identity.SteamID);
            }
            var steamProfiles = SteamWebAPI.General().ISteamUser().GetPlayerSummaries(SteamIdentity.FromSteamID(76561198065588383)).GetResponse();
            foreach (var player in steamProfiles.Data.Players){
                Console.WriteLine(player.PersonaName);
                Console.WriteLine(player.ProfileUrl);
                Console.WriteLine(player.Identity);  //not your steam id
                Console.WriteLine(player.LastLogOff);
            }
            */
            string stmt = "INSERT INTO game(gameId, name) VALUES(@GameID, @Name)";

            SqlCommand cmd = new SqlCommand(stmt, conn);
            cmd.Parameters.Add("@GameIDd", SqlDbType.Int);
            cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50);

                

            var gameInfo = SteamWebAPI.General().ISteamApps().GetAppList().GetResponse();

            foreach (var game in gameInfo.Data.Apps){
                Console.WriteLine(game.Name);
                Console.WriteLine(game.AppID);
                cmd.Parameters["@GameID"].Value = game.AppID;
                cmd.Parameters["@Name"].Value = game.Name;

                cmd.ExecuteNonQuery();
                if (game.AppID > 300){
                    
                    break;
                }
            }
            
           
            conn.Close();
            /*
            var ownedGames = SteamWebAPI.General().IPlayerService().GetOwnedGames(SteamIdentity.FromSteamID(76561198047886273)).GetResponse();
            foreach (var game in ownedGames.Data.Games) {
                Console.WriteLine(game.PlayTime2Weeks);
                Console.WriteLine(game.PlayTimeTotal);
            }
            var gameSchema = SteamWebAPI.General().ISteamUserStats().GetSchemaForGame(220).GetResponse();  //half-life 2
            if (gameSchema.Response.ErrorCode != 500)
            { //if th server has issue 
                if (gameSchema.Data.AvailableGameStats != null)
                {
                    foreach (var achievement in gameSchema.Data.AvailableGameStats.Achievements)
                    {
                        Console.WriteLine(achievement.Name);
                    }
                }
            }
            var playersAchievementsGame = SteamWebAPI.General().ISteamUserStats().GetPlayerAchievements(220, SteamIdentity.FromSteamID(76561198047886273)).GetResponse();
            if (playersAchievementsGame.Response.ErrorCode != 500)
            {
                foreach (var achivement in playersAchievementsGame.Data.Achievements)
                {
                    Console.WriteLine(achivement.APIName);
                    Console.WriteLine(achivement.Achieved);
                    Console.WriteLine(achivement.Name);
                }
            }
            Console.WriteLine("hello");
            var identity = SteamIdentity.FromSteamID(steamID[1]);
            Console.WriteLine(identity.AccountID);
            */
            Console.WriteLine("asdf");
            Console.ReadLine();

        }
    }
}

/*
https://wiki.teamfortress.com/wiki/WebAPI/GetPlayerSummaries
ISteamUser-
GetPlayerSummaries - takes array of steamID
    -steamid
    -personaname 
    -profileurl
    -lastlogOff
https://wiki.teamfortress.com/wiki/WebAPI/GetAppList /*get every app on steam from by lowest index value
ISteamApps-
GetAppList
    -appid
    -name
https://wiki.teamfortress.com/wiki/WebAPI/GetOwnedGames
IPlayerService
GetOwnedGames
    -games - array of games
        -appid
        -playtime_2weeks - possible null value
        -playtime_forever -possible null value
    
//achivments
https://wiki.teamfortress.com/wiki/WebAPI/GetSchemaForGame
ISteamUserStats
GetSchemaForGame
    -achievements
https://wiki.teamfortress.com/wiki/WebAPI/GetPlayerAchievements
ISteamUserStats
GetPlayerAchievements
    -
*/




/*
SqlConnection conn = new SqlConnection("Server=BRENDEN_PC\\SQLEXPRESS;Database=shipsDB;Integrated Security=true");
conn.Open();

SqlCommand cmd = new SqlCommand("select name from Battles", conn);

SqlDataReader reader = cmd.ExecuteReader();

while (reader.Read()) {
Console.WriteLine("a");
Console.WriteLine("{0}", reader.GetString(0));
}
reader.Close();
conn.Close();
*/


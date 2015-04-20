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
//OPEN CONNECTION AND DEFINE TARGET PLAYER
            //open the sql connection by finding the server on the currently used computer
            SqlConnection conn = new SqlConnection( "user id=CHAMPLAIN\\colin.reilly" +
                                                    "password = coljack" +
                                                    "MININT-G4VR32G//SQLEXPRESS;" +
                                                    "Database=steamDb;" +
                                                    "Integrated Security=true");
            conn.Open();

            //define target player cridentials 
            //long[] steamID = new long[] { 76561198065588383, 76561198047886273 };
            SteamWebAPI.SetGlobalKey("00E30769A6BA27CB7804374A82DBD737");

            //create steam identity
            var colin = SteamIdentity.FromSteamID(76561198047886273);
            var brenden = SteamIdentity.FromSteamID(76561198065588383);
/*
//POPULATE GAME TABLE
            //define sql command
            string gameStatement = "INSERT INTO game(gameId, name) VALUES(@GameID, @Name)";

            SqlCommand gameCommand = new SqlCommand(gameStatement, conn);
            gameCommand.Parameters.Add("@GameID", SqlDbType.Int);
            gameCommand.Parameters.Add("@Name", SqlDbType.VarChar, 50);

            //get the game info on the currently defined player
            var gameInfo = SteamWebAPI.General().ISteamApps().GetAppList().GetResponse();

            //cycle through the returned data and execute each command
            foreach (var game in gameInfo.Data.Apps){
                Console.WriteLine(game.Name);
                Console.WriteLine(game.AppID);
                gameCommand.Parameters["@GameID"].Value = game.AppID;
                gameCommand.Parameters["@Name"].Value = game.Name;

                gameCommand.ExecuteNonQuery();
                if (game.AppID > 300)
                    break;
            }*/
//POPULATE PLAYER TABLE
            //define sql command
            string playerStatement = "INSERT INTO player(steamId, personName, profileURL, lastLogOff) VALUES(@SteamId, @PersonName, @ProfileURL, @LastLogOff)";

            SqlCommand playerCommand = new SqlCommand(playerStatement, conn);
            playerCommand.Parameters.Add("@SteamId", SqlDbType.Int);
            playerCommand.Parameters.Add("@PersonName", SqlDbType.VarChar, 30);
            playerCommand.Parameters.Add("@ProfileURL", SqlDbType.VarChar, 75);
            playerCommand.Parameters.Add("@LastLogOff", SqlDbType.Int);

            //get the game info on the currently defined player
            var playerInfo = SteamWebAPI.General().ISteamUser().GetPlayerSummaries(colin).GetResponse();

            Console.WriteLine(sizeof(int)); // Output: 4

            /*//cycle through the returned data and execute each command
            foreach (var player in playerInfo.Data.Players)
            {
                Console.WriteLine(colin.SteamID);
                Console.WriteLine(player.PersonaName);
                Console.WriteLine(player.ProfileUrl);
                Console.WriteLine(player.LastLogOff);
                playerCommand.Parameters["@SteamId"].Value = colin.SteamID;
                playerCommand.Parameters["@PersonName"].Value = player.PersonaName;
                playerCommand.Parameters["@ProfileURL"].Value = player.ProfileUrl;
                playerCommand.Parameters["@LastLogOff"].Value = player.LastLogOff;
                
                //playerCommand.ExecuteNonQuery();
            }*/

            //close sql connection and exit
            conn.Close();
            Console.WriteLine("press enter to exit");
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
    
//achievments
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
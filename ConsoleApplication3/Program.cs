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
           SqlConnection conn = new SqlConnection("Server=COLINREILLY\\SQLEXPRESS;" +
                                                   "Database=steamDb;" +
                                                   "Integrated Security=true");
           conn.Open();
//feature request: check if connection was opened properly
           
           SqlDataReader reader;
           reader = null;

           string value = getInputFor("go through the options\n" +
                                        "1 profile\n" +
                                        "2 game\n" +
                                        "3 achivment\n" +
                                        "4 refresh data\n");

            switch (Convert.ToInt32(value))
            {
                case 1: // players
                    value = getInputFor("go through the options\n" +
                                        "1 get Info by id\n" +
                                        "2 get Info by name\n" +
                                        "3 output all name\n");

                    switch (Convert.ToInt32(value))
                    {
                        case 1:
                            value = getInputFor("Id");
                            reader = executeSQLStatmentWithReturn(conn, "SELECT * FROM Player where steamId=" + value);
                            break;
                        case 2:
                            value = getInputFor("players name");
                            reader = executeSQLStatmentWithReturn(conn, "SELECT * FROM Player where personName='" + value + "'");
                            break;
                        case 3:
                            reader = executeSQLStatmentWithReturn(conn, "SELECT * FROM Player");
                            break;
                        default:
                            Console.WriteLine("none of your things matched");
                            break;
                    }

                    if (reader != null)
                    {
                        if (reader.Read())
                            do
                            {
                                Console.WriteLine("{0}", reader.GetInt64(0));
                                Console.WriteLine("{0}", reader.GetString(1));
                                Console.WriteLine("{0}", reader.GetString(2));
                                Console.WriteLine("{0}", reader.GetDateTime(3));
                            } while (reader.Read());
                        else
                        { //if you can't read anything from reader that means you didn't get any rows of data
                            Console.WriteLine("cound't find anything");
                        }
                    }

                    reader.Close();
                    break;
                case 2: //achievements
                    value = getInputFor("go through the options\n" +
                                        "1 get Info by id\n" +
                                        "2 get Info by name\n" +
                                        "3 get games from a range");

                    switch (Convert.ToInt32(value))
                    {
                        case 1: //get info by id
                            value = getInputFor("Id");
                            reader = executeSQLStatmentWithReturn(conn, "SELECT * FROM Game where gameId=" + value);
                            break;
                        case 2: //get info by name
                            value = getInputFor("Name");
                            reader = executeSQLStatmentWithReturn(conn, "SELECT * FROM Game where name='" + value + "'");
                            break;
                        case 3:
                            reader = executeSQLStatmentWithReturn(conn, "SELECT * FROM Game " +
                                                                        "where gameId > " + getInputFor("greter Then") +
                                                                         " and gameId < " + getInputFor("less then"));
                            break;
                        default:
                            break;
                    }
                    if (reader != null)
                    {

                        if (reader.Read())
                            do
                            {
                                Console.WriteLine("{0}", reader.GetInt32(0));
                                Console.WriteLine("{0}", reader.GetString(1));
                            } while (reader.Read());
                        else
                        { //if you can't read anything from reader that mean you didn't get any rows of data
                            Console.WriteLine("cound't find anything");
                        }
                    }
                    break;
                case 3: //game info
                    value = getInputFor("go through the options\n" +
                                        "1 get achivments for game\n");

                    switch (Convert.ToInt32(value))
                    {
                        case 1: //get info by id
                            value = getInputFor("Id");
                            reader = executeSQLStatmentWithReturn(conn, "SELECT * FROM Achievement where gameId=" + value);
                            break;
                        default:
                            break;
                    }
                    if (reader != null)
                    {
                        if (reader.Read())
                            do
                            {
                                Console.WriteLine("{0}", reader.GetString(0));
                                Console.WriteLine("{0}", reader.GetInt32(1));
                                Console.WriteLine("{0}", reader.GetInt32(2));
                            } while (reader.Read());
                        else
                        { //if you can't read anything from reader that mean you didn't get any rows of data
                            Console.WriteLine("cound't find anything");
                        }
                    }
                    break;
                case 4: //rebuild everthing
                    //dropAllTables(conn);
                    //createAllTables(conn);
                    getAllData(conn);
                    break;
                default:
                    Console.WriteLine("you pick wrong");
                    Console.WriteLine(value);
                    break;
            }
            Console.ReadLine();
         }

         static void getAllData(SqlConnection conn) { 
//define target player cridentials 
            //long[] steamID = new long[] { 76561198065588383, 76561198047886273 };
            SteamWebAPI.SetGlobalKey("00E30769A6BA27CB7804374A82DBD737");

            //create steam identity
            var colin = SteamIdentity.FromSteamID(76561198047886273);
            var brenden = SteamIdentity.FromSteamID(76561198065588383);

//POPULATE PLAYER TABLE
            //define sql command
            string playerStatement = "INSERT INTO Player(steamId, personName, profileURL, lastLogOff) VALUES(@SteamId, @PersonName, @ProfileURL, @LastLogOff)";

            SqlCommand playerCommand = new SqlCommand(playerStatement, conn);
            playerCommand.Parameters.Add("@SteamId", SqlDbType.BigInt);
            playerCommand.Parameters.Add("@PersonName", SqlDbType.VarChar, 30);
            playerCommand.Parameters.Add("@ProfileURL", SqlDbType.VarChar, 75);
            playerCommand.Parameters.Add("@LastLogOff", SqlDbType.DateTime);

            //get the game info on the currently defined player
            var playerInfo = SteamWebAPI.General().ISteamUser().GetPlayerSummaries(colin).GetResponse();

            //cycle through the returned data and execute each command
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

                playerCommand.ExecuteNonQuery();
            }

//POPULATE ACHIEVEMENT TABLE
            //define sql command
            string achievementStatement = "INSERT INTO Achievement(Name, gameId) VALUES(@Name, @GameId)";

            SqlCommand achievementCommand = new SqlCommand(achievementStatement, conn);
            achievementCommand.Parameters.Add("@Name", SqlDbType.VarChar, 50);
            achievementCommand.Parameters.Add("@GameId", SqlDbType.Int);

            //get the game info on the currently defined player
            var achievementInfo = SteamWebAPI.General().ISteamUserStats().GetPlayerAchievements(440, colin).GetResponse();

            //cycle through the returned data and execute each command
            foreach (var achievement in achievementInfo.Data.Achievements)
            {
                Console.WriteLine(achievement.APIName);
                Console.WriteLine(440);
                achievementCommand.Parameters["@Name"].Value = achievement.APIName;
                achievementCommand.Parameters["@GameId"].Value = 440;

                achievementCommand.ExecuteNonQuery();
            }

//POPULATE GAME TABLE
            //define sql command
            string gameStatement = "INSERT INTO Game(gameId, name) VALUES(@GameID, @Name)";

            SqlCommand gameCommand = new SqlCommand(gameStatement, conn);
            gameCommand.Parameters.Add("@GameID", SqlDbType.Int);
            gameCommand.Parameters.Add("@Name", SqlDbType.VarChar, 50);

            //get the game info on the currently defined player
            var gameInfo = SteamWebAPI.General().ISteamApps().GetAppList().GetResponse();

            //cycle through the returned data and execute each command
            foreach (var game in gameInfo.Data.Apps)
            {
                Console.WriteLine(game.Name);
                Console.WriteLine(game.AppID);
                gameCommand.Parameters["@GameID"].Value = game.AppID;
                gameCommand.Parameters["@Name"].Value = game.Name;

                gameCommand.ExecuteNonQuery();
                if (game.AppID > 300)
                    break;
            }

            //close sql connection and exit
            conn.Close();
            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }//end of getAllData

         static void dropAllTables(SqlConnection conn)
         {    
             executeSQLStatment(conn,"drop table Player;\n" + 
                                     "drop table Game;\n" + 
                                     "drop table Achievement;\n" + 
                                     "drop table GameOwned;\n" + 
                                     "drop table AchievementOwned;\n");
         }

         static void createAllTables(SqlConnection conn)
         {
             executeSQLStatment(conn, "create table Player( steamId int, personName varchar(30), profileURL varchar(75), lastLogOff int);" +
                                      "create table Game( gameId int, name varchar(50));" +
                                      "create table Achievement( name varchar(50), gameId int);" +
                                      "create table GameOwned( steamId int, gameId int, playTimeTwoWeek int, playTimeForever int);" +
                                      "create table AchievementOwned( gameId int, playerId int, achievementId int, completed binary(1));" );
         }

         static void executeSQLStatment(SqlConnection conn, string statment)
         {
             SqlCommand cmd = new SqlCommand();
             cmd.CommandText = statment;
             cmd.CommandType = CommandType.Text;
             cmd.Connection = conn;
             cmd.ExecuteReader();
         }

         static SqlDataReader executeSQLStatmentWithReturn(SqlConnection conn, string statment)
         {
             try
             {
                 SqlDataReader reader;
                 SqlCommand cmd = new SqlCommand();
                 cmd.CommandText = statment;
                 cmd.CommandType = CommandType.Text;
                 cmd.Connection = conn;
                 reader = cmd.ExecuteReader();
                 Console.WriteLine("\n\n");
                 return reader;
             }
             catch (Exception e)
             {
                 Console.WriteLine(String.Concat(e.Message, e.StackTrace));
                 return null;
             }

             
         }
         static string getInputFor(string request)
         {
             Console.WriteLine(request);
             return Console.ReadLine();
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
*/

/*
 * how reader works
 http://idealprogrammer.com/net-languages/code-samples/sqldatareader-source-code/
 */

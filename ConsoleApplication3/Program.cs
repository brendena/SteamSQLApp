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
            SqlConnection conn = new SqlConnection("Server=BRENDEN_PC\\SQLEXPRESS;" +
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
                                         "4 refresh data\n" +
                                         "5 do fancy SQL Queries");

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
                    truncAllTables(conn);
                    getAllData(conn);
                    break;
                    case 5://do fancy queries
                    string user, game;

                    value = getInputFor("go through the options\n" +
                                        "1 what games does a user play?\n" +
                                        "2 who are the friends of this user who are also in the Player table?\n" +
                                        "3 what are they achievements accomplished for a certain game by a certain user? \n" +
                                        "4 what are the achievements not accomplished for a certain game by a certian user?");

                    switch(Convert.ToInt32(value))
                    {
                        case 1:
                            value = getInputFor("user");
                            executeSQLStatmentWithReturn(conn, "SELECT personName, playTimeTwoWeek, gameId FROM GameOwned, Player WHERE playTimeTwoWeek > 0 and GameOwned.steamId = Player.steamId and Player.steamId = " + 76561198018133285 + ";");
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
                                { //if you can't read anything from reader that means you didn't get any rows of data
                                    Console.WriteLine("cound't find anything");
                                }
                            }
                            break;
                        case 2:
                            value = getInputFor("user");
                            executeSQLStatmentWithReturn(conn, "select FriendHave.friendOne, Player.personName from FriendHave, Player where friendOne = " + value + " and Player.steamId = FriendHave.friendTwo;");
                            if (reader != null)
                            {
                                if (reader.Read())
                                    do
                                    {
                                        Console.WriteLine("{0}", reader.GetInt64(0));
                                        Console.WriteLine("{0}", reader.GetString(1));
                                    } while (reader.Read());
                                else
                                { //if you can't read anything from reader that means you didn't get any rows of data
                                    Console.WriteLine("cound't find anything");
                                }
                            }
                            break;
                        case 3:
                            user = getInputFor("user");
                            game = getInputFor("game");
                            executeSQLStatmentWithReturn(conn, "select gameId, name from AchievementOwned where playerId = " + user + " and gameId = " + game + ";");
                            if (reader != null)
                            {
                                if (reader.Read())
                                    do
                                    {
                                        Console.WriteLine("{0}", reader.GetInt32(0));
                                        Console.WriteLine("{0}", reader.GetString(1));
                                    } while (reader.Read());
                                else
                                { //if you can't read anything from reader that means you didn't get any rows of data
                                    Console.WriteLine("cound't find anything");
                                }
                            }
                            break;
                        case 4:
                            user = getInputFor("user");
                            game = getInputFor("game");
                            executeSQLStatmentWithReturn(conn, "(SELECT name, gameId FROM Achievement where gameId = " + game + ") EXCEPT (SELECT name, gameId FROM AchievementOwned WHERE playerId = " + user + " and gameId = " + game + ");");
                            if (reader != null)
                            {
                                if (reader.Read())
                                    do
                                    {
                                        Console.WriteLine("{0}", reader.GetString(0));
                                        Console.WriteLine("{0}", reader.GetInt32(1));
                                    } while (reader.Read());
                                else
                                { //if you can't read anything from reader that means you didn't get any rows of data
                                    Console.WriteLine("cound't find anything");
                                }
                            }
                            break;
                default:
                    Console.WriteLine("you pick wrong");
                    Console.WriteLine(value);
                    break;
            }
            Console.ReadLine();
        }

        static void getAllData(SqlConnection conn)
        {
            SteamWebAPI.SetGlobalKey("00E30769A6BA27CB7804374A82DBD737");

            //create steam identity
            SteamIdentity[] steamID = new SteamIdentity[] { SteamIdentity.FromSteamID(76561198047886273), //colin
                                                            SteamIdentity.FromSteamID(76561198065588383),  //brenden
                                                            SteamIdentity.FromSteamID(76561198018133285), //john
                                                            SteamIdentity.FromSteamID(76561197983072534),
                                                            SteamIdentity.FromSteamID(76561197996591065),
                                                            SteamIdentity.FromSteamID(76561197999979429),
                                                            SteamIdentity.FromSteamID(76561198009844144)};
            populateGameTable(conn);
            foreach(var player in steamID){
                populatePlayerTable(conn, player);
                populateGameOwnedTable(conn, player);
                populateAchievementTable(conn, player);
                populateAchievementOwnedTable(conn, player);
                populateFriendsHaveTable(conn, player);
            }

            //close sql connection and exit
            conn.Close();
            Console.WriteLine("press enter to exit");
            Console.ReadLine();
        }//end of getAllData
        static void populatePlayerTable(SqlConnection conn,SteamIdentity person)
        {
            //POPULATE PLAYER TABLE
            //define sql command
            string playerStatement = "INSERT INTO Player(steamId, personName, profileURL, lastLogOff) VALUES(@SteamId, @PersonName, @ProfileURL, @LastLogOff)";

            SqlCommand playerCommand = new SqlCommand(playerStatement, conn);
            playerCommand.Parameters.Add("@SteamId", SqlDbType.BigInt);
            playerCommand.Parameters.Add("@PersonName", SqlDbType.VarChar, 30);
            playerCommand.Parameters.Add("@ProfileURL", SqlDbType.VarChar, 75);
            playerCommand.Parameters.Add("@LastLogOff", SqlDbType.DateTime);

            //get the game info on the currently defined player
            var playerInfo = SteamWebAPI.General().ISteamUser().GetPlayerSummaries(person).GetResponse();

            //cycle through the returned data and execute each command
            foreach (var player in playerInfo.Data.Players)
            {
                playerCommand.Parameters["@SteamId"].Value = person.SteamID;
                playerCommand.Parameters["@PersonName"].Value = player.PersonaName;
                playerCommand.Parameters["@ProfileURL"].Value = player.ProfileUrl;
                playerCommand.Parameters["@LastLogOff"].Value = player.LastLogOff;

                playerCommand.ExecuteNonQuery();
            }
        }
        static void populateGameTable(SqlConnection conn)
        {
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
                gameCommand.Parameters["@GameID"].Value = game.AppID;
                gameCommand.Parameters["@Name"].Value = game.Name;

                gameCommand.ExecuteNonQuery();
            }
        }
        static void populateGameOwnedTable(SqlConnection conn, SteamIdentity person)
        {
            //POPULATE GameOwned TABLE
            //define sql command

            string gameOwnedStatement = "INSERT INTO GameOwned(steamId, gameId, playTimeTwoWeek, playTimeForever) VALUES(@SteamId, @GameId, @PlayTimeTwoWeek, @PlayTimeForever)";

            SqlCommand gameOwnedCommand = new SqlCommand(gameOwnedStatement, conn);
            gameOwnedCommand.Parameters.Add("@SteamId", SqlDbType.BigInt);
            gameOwnedCommand.Parameters.Add("@GameId", SqlDbType.Int);
            gameOwnedCommand.Parameters.Add("@PlayTimeTwoWeek", SqlDbType.Int);
            gameOwnedCommand.Parameters.Add("@PlayTimeForever", SqlDbType.Int);

            //get all the games owned
            var gameOwnedInfo = SteamWebAPI.General().IPlayerService().GetOwnedGames(person).GetResponse();

            //cycle through the returned data and execute each command
            foreach (var gameOwned in gameOwnedInfo.Data.Games)
            {
                gameOwnedCommand.Parameters["@SteamId"].Value = person.SteamID;
                gameOwnedCommand.Parameters["@GameId"].Value = gameOwned.AppID;
                gameOwnedCommand.Parameters["@PlayTimeTwoWeek"].Value = gameOwned.PlayTime2Weeks.TotalMinutes;
                gameOwnedCommand.Parameters["@PlayTimeForever"].Value = gameOwned.PlayTimeTotal.TotalMinutes;

                gameOwnedCommand.ExecuteNonQuery();
            }
        }
        static void populateFriendsHaveTable(SqlConnection conn, SteamIdentity person)
        {
            string friendStatement = "INSERT INTO FriendHave(friendOne, friendTwo, friendSince) VALUES(@FriendOne, @FriendTwo, @FriendSince)";

            SqlCommand friendCommand = new SqlCommand(friendStatement, conn);
            friendCommand.Parameters.Add("@FriendOne", SqlDbType.BigInt);
            friendCommand.Parameters.Add("@FriendTwo", SqlDbType.BigInt);
            friendCommand.Parameters.Add("@FriendSince", SqlDbType.DateTime);

            //get all the games owned
            var friendInfo = SteamWebAPI.General().ISteamUser().GetFriendList(person,RelationshipType.Friend).GetResponse();

            //cycle through the returned data and execute each command
            foreach (var friend in friendInfo.Data.Friends)
            {
                friendCommand.Parameters["@FriendOne"].Value = person.SteamID;
                friendCommand.Parameters["@FriendTwo"].Value = friend.Identity.SteamID;
                friendCommand.Parameters["@FriendSince"].Value = friend.FriendSince;

                friendCommand.ExecuteNonQuery();
            }
        }
        static void populateAchievementOwnedTable(SqlConnection conn, SteamIdentity person) {
            //POPULATE ACHIEVEMENT OWNED TABLE
            //define sql command
            string achievementStatement = "INSERT INTO AchievementOwned(playerId, gameId, name) VALUES(@PlayerId, @GameId, @Name)";

            SqlCommand achievementCommand = new SqlCommand(achievementStatement, conn);
            achievementCommand.Parameters.Add("@PlayerId", SqlDbType.BigInt);
            achievementCommand.Parameters.Add("@GameId", SqlDbType.Int);
            achievementCommand.Parameters.Add("@Name", SqlDbType.VarChar, 50);
            //get all the games owned 
            var gameOwnedInfo = SteamWebAPI.General().IPlayerService().GetOwnedGames(person).GetResponse();

            //cycle through the returned data and execute each command
            foreach (var gameOwned in gameOwnedInfo.Data.Games)
            {
                //get the achievement info on the currently defined player
                var achievementInfo = SteamWebAPI.General().ISteamUserStats().GetPlayerAchievements(gameOwned.AppID, person).GetResponse();

                //the try for see if there Achievements
                try
                {
                    //cycle through the returned data and execute each command
                    foreach (var achievement in achievementInfo.Data.Achievements)
                    {
                        achievementCommand.Parameters["@PlayerId"].Value = person.SteamID;
                        achievementCommand.Parameters["@GameId"].Value = gameOwned.AppID;
                        achievementCommand.Parameters["@Name"].Value = achievement.APIName;


                        achievementCommand.ExecuteNonQuery();
                    }
                }
                catch {
                    //Console.WriteLine("can't");
                }
            }
        }

        static void populateAchievementTable(SqlConnection conn, SteamIdentity person) {
            //POPULATE ACHIEVEMENT TABLE
            //define sql command
            string achievementStatement = "INSERT INTO Achievement(name, gameId) VALUES(@Name, @GameId)";

            SqlCommand achievementCommand = new SqlCommand(achievementStatement, conn);
            achievementCommand.Parameters.Add("@Name", SqlDbType.VarChar, 50);
            achievementCommand.Parameters.Add("@GameId", SqlDbType.Int);

            //get all the games owned 
            var gameOwnedInfo = SteamWebAPI.General().IPlayerService().GetOwnedGames(person).GetResponse();

            //cycle through the returned data and execute each command
            foreach (var gameOwned in gameOwnedInfo.Data.Games)
            {
                //Colins nightmare
                //Steam put a fake game into there data base and its to big to take in
                //This probablem is only on this one game and the error likes in avaible games states
                //Location
                //http://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2/?key=00E30769A6BA27CB7804374A82DBD737&appid=240
                //http://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2/?key=00E30769A6BA27CB7804374A82DBD737&appid=730
                bool bad = false;
                int[] badIds = new int[] { 240, 730, 223220, 221910, 35450, 236830, 317950, 104320, 224780 };
                foreach(var badId in badIds){if(badId == gameOwned.AppID){bad = true;}}
                if(bad == true){continue;}


                //get the achievement info on a game
                try
                {
                    var achievementInfo = SteamWebAPI.General().ISteamUserStats().GetSchemaForGame(gameOwned.AppID).GetResponse();

                    //the try for see if there Achievements
                    try
                    {
                        //if there already in the table
                        try
                        {   //cycle through the returned data and execute each command
                            foreach (var achievement in achievementInfo.Data.AvailableGameStats.Achievements)
                            {
                                achievementCommand.Parameters["@Name"].Value = achievement.Name;
                                achievementCommand.Parameters["@GameId"].Value = gameOwned.AppID;

                                achievementCommand.ExecuteNonQuery();
                            }
                        }
                        catch { 
                        
                       }
                    }
                    catch
                    {
                        //example of a game that has no schema
                        //http://api.steampowered.com/ISteamUserStats/GetSchemaForGame/v2/?key=00E30769A6BA27CB7804374A82DBD737&appid=3910
                        //Console.WriteLine(gameOwned.AppID);
                        //Console.WriteLine("could't get the value");
                    }
                }
                catch {
                    Console.WriteLine(gameOwned.AppID);
                }
            }
        }
        static void truncAllTables(SqlConnection conn)
        {
            executeSQLStatment(conn, "truncate table Player;\n" +
                                    "truncate table Game;\n" +
                                    "truncate table Achievement;\n" +
                                    "truncate table GameOwned;\n" +
                                    "truncate table AchievementOwned;\n"  +
                                    "truncate table FriendHave;\n");
        }
        static void dropAllTables(SqlConnection conn)
        {
            executeSQLStatment(conn, "drop table Player;\n" +
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
                                     "create table AchievementOwned( gameId int, playerId int, achievementId int, completed binary(1));");
        }

        static void executeSQLStatment(SqlConnection conn, string statment)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = statment;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = conn;
            cmd.ExecuteNonQuery();
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using PortableSteam;
using PortableSteam.Fluent;
using PortableSteam.Infrastructure;
using PortableSteam.Interfaces;

namespace PortableSteam
{
    class Program
    {
        static void Main(string[] args)
        {
            long [] steamID = new long [] {76561198065588383, 76561198047886273 };
            SteamWebAPI.SetGlobalKey("6373F8810E9FA6C9781491E5F32D753F");
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
             */
            /*
            var steamProfiles = SteamWebAPI.General().ISteamUser().GetPlayerSummaries(SteamIdentity.FromSteamID(76561198065588383)).GetResponse();
            foreach (var player in steamProfiles.Data.Players){
                Console.WriteLine(player.PersonaName);
                Console.WriteLine(player.ProfileUrl);
                Console.WriteLine(player.Identity);  //not your steam id
                Console.WriteLine(player.LastLogOff);
            }
            */
            /*
            var gameInfo = SteamWebAPI.General().ISteamApps().GetAppList().GetResponse();
            
            foreach (var game in gameInfo.Data.Apps){
                Console.WriteLine(game.Name);
                Console.WriteLine(game.AppID);
                if (game.AppID != 240) //some reason it breaks evertime with 240
                {
                    var gameSchema = SteamWebAPI.General().ISteamUserStats().GetSchemaForGame(game.AppID).GetResponse();
                    if (gameSchema.Response.ErrorCode != 500) { //if th server has issue 
                        if (gameSchema.Data.AvailableGameStats != null)
                        {
                            foreach (var achievement in gameSchema.Data.AvailableGameStats.Achievements)
                            {
                                Console.WriteLine(achievement.Name);
                            }
                        }
                    }
                }
                if(game.AppID > 300){
                    break;
                }
            }
            
            /*
            var ownedGames = SteamWebAPI.General().IPlayerService().GetOwnedGames(SteamIdentity.FromSteamID(76561198047886273)).GetResponse();

            foreach (var game in ownedGames.Data.Games) {
                Console.WriteLine(game.PlayTime2Weeks);
                Console.WriteLine(game.PlayTimeTotal);
            }
            */
            /*
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
            */

            var playersAchievementsGame = SteamWebAPI.General().ISteamUserStats().GetPlayerAchievements(220,SteamIdentity.FromSteamID(76561198047886273)).GetResponse();
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

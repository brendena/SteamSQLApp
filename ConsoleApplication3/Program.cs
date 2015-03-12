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

            var hello = SteamWebAPI.General().ISteamUser().GetFriendList(SteamIdentity.FromSteamID(76561198047886273), RelationshipType.All).GetResponse();

            foreach (var friend in hello.Data.Friends)
            {
                Console.WriteLine(friend.Identity.SteamID);
            }
            Console.WriteLine(hello);
            var identity = SteamIdentity.FromSteamID(steamID[1]);
            Console.WriteLine(identity.AccountID);
            Console.ReadLine();

        }
    }
}

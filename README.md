shittySteamSQLApp
-----------------

Portable Steam doc's
https://portablesteamwebapi.codeplex.com/documentation

All the aviable functions
https://wiki.teamfortress.com/wiki/WebAPI


Good reads for c#
http://andymcm.com/csharpfaq.htm
http://www.dotnetperls.com/debugging
http://csharp.net-informations.com/collection/csharp-array.htm
https://msdn.microsoft.com/en-us/magazine/cc301520.aspx


SQL code with visual studio
https://www.youtube.com/watch?v=EiUSK5-sv4Q

http://www.codeproject.com/Articles/4416/Beginners-guide-to-accessing-SQL-Server-through-C

http://stackoverflow.com/questions/8218867/c-sharp-sql-insert-command


Things to remember

In the current code your namespace is PortableSteam.
So SteamWebAPI and SteamIdentity or ReleastionshipType are all inherited from PortableSteam.
There's a differance from acount.id and steam.id

SQL Schema
----------

Player(
	steamId BigInt,
	personName varchar(30),
	profileURL varchar(75),
	lastLogOff DateTime,
	Primary Key(steamId))

Game(
	gameId int,
	name varchar(50),
	Primary Key(gameId))

Achievement(
	name varchar(50),
	gameId int,
	Primary Key(name,gameId))

GameOwned(
	steamId BigInt,
	gameId int, 
	playTimeTwoWeek int,
	playTimeForever int,
	Primary Key(steamId,gameId))

AchievementOwned(
	playerId BigInt,
	gameId int,
	achievementId int,
	Primary Key(gameId,playerId,achievementId))

FriendHave(
	playerOne BigInt,
	playerTwo BigInt,
	friendSince DateTime,
	Primary Key(playerOne,playerTwo)
)



SQL starter Code
----------------
use steamDb;

drop table Player;
drop table Game;
drop table Achievement;
drop table GameOwned;
drop table AchievementOwned;
drop table FriendHave;

create table Player( steamId Bigint , personName varchar(30), profileURL varchar(75), lastLogOff DateTime, primary key(steamId))

create table Game( gameId int , name varchar(50))

create table Achievement( name varchar(50), gameId int)

create table GameOwned( steamId Bigint, gameId int, playTimeTwoWeek int, playTimeForever int)

create table  AchievementOwned(  playerId BigInt, gameId int, name varchar(50))

create table FriendHave(friendOne BigInt, friendTwo BigInt, friendSince DateTime)



CODING STANDARDS
----------------
1) If comments are capatalized they should also have no indent
2) feature request: means feature request

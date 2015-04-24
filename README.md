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
	steamId int,
	personName varchar(30),
	profileURL varchar(75),
	lastLogOff int)

Game(
	gameId int,
	name varchar(50))

Achievement(
	name varchar(50),
	gameId int)

GameOwned(
	steamId int,
	gameId int, 
	playTimeTwoWeek int,
	playTimeForever int)

AchievementOwned(
	gameId int,
	playerId int,
	achievementId int,
	completed binary(1))


CODING STANDARDS
----------------
1) If comments are capatalized they should also have no indent
2) feature request: means feature request
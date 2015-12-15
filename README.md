# WimP
WimP is a personnal PathFinding project based on the A* (a-star) algorithm improved to find the shortest path in a maze with multiple possibilites.
/!\ Comments and Debug file are in French because it's my native language. /!\

---------------------------------------------------------------------------------

Developped on :
- Windows 7 Pro 64x
- Visual Studio 2013 Ultimate
- Microsoft .NET 4.5

---------------------------------------------------------------------------------

MINIMAL CONFIGURATION:
- RAM: 512 Mo
- Windows 7 (32 or 64 bits)
- Without Service pack -> .Net v4
- With Service pack 1  -> .Net v4 or greater

---------------------------------------------------------------------------------

WHAT YOU MUST BE ATTENTIVE TO:
- X and Y are like this : X goes down from the top, Y goes right from the left. because of the use of Arrays in the programm.
- Your maze has to be smaller than 255x255, because it's coded in byte.
- To use the library you have to use a maze without blanks squares greater than 2x2 or the algo won't be able to find a path.

---------------------------------------------------------------------------------

If you enter a position similar to a wall or out of the map, the programm will throw an exception telling you what you've done wrong.

Before the algorithm try to find a path, it will cleans the map by blocking dead end and a case surrounded by 2 walls in a block 2x2 to prevent "cube" in the path solution.

FindPath.cs is the main class, Carrefour.cs and Chemin.cs are the children of FindPath.
To use these classes you have to instantiate an FindPath object then use .Find(Map,PosX,PosY,DestX,DestY).

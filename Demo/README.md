/!\ YOU HAVE TO PUT FindPath.cs, Chemin.cs AND Carrefour.cs from WIMP in the directory of this app /!\

Create a folder and put each file and the 3 classes into, then open Demo PathFinder.csproj with Visual Studio.
Build the project and you should be able to run it.

Read the WIMP's README.md to know how the pathfinder works.

Informations:

- Buttons:
    - Choisir la map | Choose the map
    - Dessiner la map | Draw the map
    - Précision double | Wimp will search a path in both directions to really choose the shortest path. (OFF by default)
    - Afficher le debug | Show you what Wimp have done
    - Quitter | Exit
    - Debug | Only usefull if you are debugging from Visual Studio, it will write step by step each information in the console.

- Labels:
    - Etat Actuel : Shows if Debug for VS is ON or OFF, it slows the app.
    - Précision : Tells you the state of "Précision double" <normal> is OFF | <double> is ON. slightly increases the exec time.
    - Temps requis : Shows the execution time.

"Saisir X puis Y de l'objet" : Writes the X and Y position of the "hunter" or "mobile".

"Saisir X puis Y de la destination" : Writes the X and Y position of the "prey or "destination".

BE CAREFULL WITH X & Y. They are NOT logical.


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

BE CAREFULL WITH X & Y. They are NOT logical.

"Saisir X puis Y de l'objet" : Writes the X and Y position of the "hunter" or "mobile".<br />
"Saisir X puis Y de la destination" : Writes the X and Y position of the "prey or "destination".


HOW TO ADD MAPS:


initialize an Int[,] array with a name, MyArray for example and put the values in.

    - (it has to be rectangular)
    - 1 is a wall
    - 0 is a possible path
    
Then in Form1_Load, add your MyArray in the List like this : <<-- ListeDesMaps.Add(MyArray); -->>

Example:

    private int[,] tmap2 = 
        {
           
            {1,0,0,0,0,0,1,0,1},
            {0,0,1,0,1,0,0,0,0},
            {1,0,0,0,0,0,1,1,0},
            {0,0,1,0,1,0,0,0,0},
            {0,0,1,0,1,0,1,0,1},
            {1,1,0,0,0,0,1,0,0},
            {0,0,0,1,0,0,0,0,0},
            {0,1,1,1,0,1,0,1,0},
            {0,0,0,0,0,1,0,0,0}
        };
        private int[,] Path;
        int[] MapDim = new int[2];
        // ----- GRAPHICS ----- //
        private const int Echelle = 25;
        private void Form1_Load(object sender, EventArgs e)
        {
            // Charge les maps
            if (!Loaded)
            {
                ListeDesMaps.Add(tmap0); 
                
                ...
                
                
You can add as many maps as you wish by adding them directly in the code of Form1.cs like this.

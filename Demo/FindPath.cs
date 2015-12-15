/***************************************************************************
 * Crée le 25.08.2015
 * Par Salomon Segan
 * segan.salomon@rpn.ch
 * Ce programme est un PathFinder, son but est donc de trouver le chemin le
 * plus court parmis tout ceux qui serait possible en retournant un tableau
 * de byte qui contient ce chemin. Dans ce tableau le chemin sera noté avec
 * des nombres qui sont en lien avec la somme des déplacements requis pour 
 * arriver à la destination souhaitée.
 ***************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace PathFinder
{
               
    class FindPath
    {
        #region Attributs
        private TimeSpan TSPATH;
        private DateTime dtPATH;

        private bool debug = false;
        private const int TABSIZE = 100;

        private byte EntityX;
        private byte EntityY;
        private byte EntityXTemp;
        private byte EntityYTemp;

        private byte DestinationX;
        private byte DestinationY;

        private int[,] ArrayPath;
        private int[,] ArrayMap; 
        private int[,] ArrayMapTemp;
        private int[] MapDimension;
        
        private int[,] ArraySUREPATH; // Contient le chemin le plus court

        private int[,] ArraySurePathTemp;
        private bool ArraySurePathTempSaved = false;

        private bool wallup;
        private bool walldown;
        private bool wallleft;
        private bool wallright;
        private int wallnumber;

        private bool shortestpathfound;// sera true lorsque l'entité sera sur la destination

        private int iNbCheminsValides;
        
        private bool NoeudAjouter;
        private bool firsttime;
        private bool FourWallsEncounter;
        private bool FirstCrossEncounter;

        private List<string> strDebugLog = new List<string>();
        // --- https://msdn.microsoft.com/fr-fr/library/8bh11f1k.aspx --- //
        // ---------------------------------------

        private List<Chemin> Paths;
        private List<Carrefour> EquiCross;

        #endregion

        #region Paramètres
        /// <summary>
        /// X value of the object
        /// </summary>
        public byte EntX
        {
            get { return EntityX; }
            set{
                if (value < 0 || value >= MapDimension[0])
                    throw new Exception("Valeur trop grande ou trop petite pour la position X du 'mobile'");
                else
                    EntityX = value;
            }
        }
        /// <summary>
        /// Y value of the object
        /// </summary>
        public byte EntY
        {
            get { return EntityY; }
            set
            {
                if (value < 0 || value >= MapDimension[1])
                    throw new Exception("Valeur trop grande ou trop petite pour la position Y du 'mobile'");
                else
                    EntityY = value;
            }
        }
        /// <summary>
        /// X value of the destination
        /// </summary>
        public byte DestX
        {
            get { return DestinationX; }
            set
            {
                if (value < 0 || value >= MapDimension[0])
                    throw new Exception("Valeur trop grande ou trop petite pour la position X de la destination");
                else
                    DestinationX = value;
            }
        }
        /// <summary>
        /// Y value of the destination
        /// </summary>
        public byte DestY
        {
            get { return DestinationY; }
            set
            {
                if (value < 0 || value >= MapDimension[1])
                    throw new Exception("Valeur trop grande ou trop petite pour la position Y de la destination");
                else
                    DestinationY = value;
            }
        }
        /// <summary>
        /// The map where the object shall find his way
        /// </summary>
        public int[,] AMap
        {
            set
            {
                ArrayMap = value;
            }
        }
        /// <summary>
        /// Define if debug message shows up
        /// </summary>
        public bool DEBUG
        {
            get { return debug; }
            set 
            {
                debug = value;
            }
        }

        #endregion 
                
        #region Méthodes
        /// <summary>
        /// This class returns an array numbred from 1 to X according to the number of moves needed to reach destination
        /// </summary>
        /// <param name="Map">Array of byte representing a Map, 0 = floor, 1 = wall</param>
        /// <param name="EntiX">Position X : Entity</param>
        /// <param name="EntiY">Position Y : Entity</param>
        /// <param name="DestiX">Destination X : Entity</param>
        /// <param name="DestiY">Destination Y : Entity</param>
        /// <param name="accurate">Will perform the test Ent->Dest and Dest->Ent to compare which path is the shortest</param>
        /// <returns>An array with the path in</returns>
        public int[,] Find(int[,] Map, byte EntiX, byte EntiY, byte DestiX, byte DestiY, bool accurate)
        {
            // Appelle le constructeur de la class
            InitializeValues(Map, EntiX, EntiY, DestiX, DestiY);
            strDebugLog.Add("\r\n\r\n--------------------------------------\r\n---- Initialisation réussie !! -----\r\n--------------------------------------\r\n\r\n");
            if (debug)
            {
                Debug.Write("\r\n\r\n--------------------------------------\r\n----- Initialisation réussie !! -----\r\n--------------------------------------\r\n\r\n");
            }
            // Nettoye la map
            RetireCubeAMap(ref Map);
            strDebugLog.Add("\r\n\r\n------------------------------------\r\n--- Retrait des cubes réussi ! ---\r\n------------------------------------\r\n\r\n");
            if (debug)
            {
                Debug.Write("\r\n\r\n------------------------------------\r\n--- Retrait des cubes réussi ! ---\r\n------------------------------------\r\n\r\n");
            }
            BloqueQdeSac(ref Map);
            strDebugLog.Add("\r\n\r\n----------------------------------\r\n--- Retrait des cul-de-sac réussi ! ---\r\n----------------------------------\r\n\r\n");
            if (debug)
            {
                Debug.Write("\r\n\r\n----------------------------------\r\n--- Retrait des cul-de-sac réussi ! ---\r\n----------------------------------\r\n\r\n");                
            }
            dtPATH = DateTime.Now;
        ReverseSearch:
            Array.Copy(Map, ArrayMap, MapDimension[0] * MapDimension[1]);
            Array.Copy(ArrayMap, ArrayMapTemp, MapDimension[0] * MapDimension[1]);
            dtPATH = DateTime.Now;
            while (!shortestpathfound)
            {
                strDebugLog.Add("\r\nRecherche d'un nouveau chemin\r\n");
                if (debug)
                {
                    Debug.WriteLine("\r\nRecherche d'un nouveau chemin\r\n");
                }
                #region Reconstruction du début de chemin avant le noeud
                // On vérifie qu'un chemin à déjà était trouvé une fois
                // afin de recéer le chemin parcouru jusqu'au dernier carrefour
                if (!firsttime)
                {
                    Paths[iNbCheminsValides].BuildPath(ArrayPath);
                }
                #endregion
                /// ------------------------------------------------------------------------------------
                /// Cette boucle cherche un chemin entier jusqu'à trouver la destination ou être bloquée
                /// ------------------------------------------------------------------------------------
                #region Recherche d'un chemin complet, valide ou non
                while (!Paths[iNbCheminsValides].PathValid)
                {
                    // --- EVITE LE BLOQUAGE --- //
                    TSPATH = DateTime.Now - dtPATH;
                    if (TSPATH.Seconds > 2 && !DEBUG || DEBUG && TSPATH.Seconds > 20)
                    {
                        EcritDansLogs(strDebugLog);
                        throw new Exception("Le programme ne trouve pas de chemin, une erreur s'est produite.");
                    }
                    strDebugLog.Add("Temps : " + TSPATH.Seconds + " secondes " + TSPATH.Milliseconds + " millisecondes");
                    if (debug)
                    {
                        Debug.WriteLine("Temps : " + TSPATH.Seconds + " secondes " + TSPATH.Milliseconds + " millisecondes");
                    }

                    NoeudAjouter = false;
                    int[] ArrayCurrentDistance = new int[4]; // cargo the value of the distance between Entity and Destination
                    // ------ RECHERCHE DE MURS ------ //
                    strDebugLog.Add("\r\nRecherche des murs autour de [" + EntityXTemp.ToString() + "," + EntityYTemp.ToString() + "]");
                    if (debug)
                    {
                        Debug.WriteLine("\r\nRecherche des murs autour de [" + EntityXTemp.ToString() + "," + EntityYTemp.ToString() + "]");
                    }
                    wallnumber = FindWall(EntityXTemp, EntityYTemp, false);
                    if (wallnumber < 4)
                    {
                        #region DIVERS TEST POUR ASSURER LES NOEUDS
                        /// ------------------------------------
                        /// EntityX va de haut en bas !!!
                        /// EntityY va de gauche a droite !!!
                        /// ---///---
                        /// MapDimension[] -1 car les tableaux commence à 0, la valeur max est prise de MapDimension où X et Y sont 1 plus grands
                        /// car si il y a 6 lignes la valeur MAX de X sera 5 et non pas 6
                        /// ------------------------------------
                        // ------ AU MOINS DEUX CHEMINS DOIVENT ETRE VALABES --- RAISON POUR LAQUELLE CES if SONT GRANDS ------ //
                        //CONTRE MUR HAUT
                        ArrayPath[EntityX, EntityY] = byte.MaxValue;
                        if (EntityXTemp == 0 && EntityYTemp < MapDimension[1] - 1 && EntityYTemp > 0) 
                        {
                            if (wallnumber < 4 &&
                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0 ||

                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0 ||

                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0
                                )
                                AddNoeud(wallnumber);
                                
                        }
                        //CONTRE MUR BAS
                        if (EntityXTemp == MapDimension[0] - 1 && EntityYTemp < MapDimension[1] - 1 && EntityYTemp > 0)
                        {
                            if (wallnumber < 4 &&
                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0 ||

                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0 ||
                                
                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0
                                )
                                AddNoeud(wallnumber);
                        }
                        //CONTRE MUR GAUCHE
                        if (EntityYTemp == 0 && EntityXTemp < MapDimension[0] - 1 && EntityXTemp > 0)
                        {
                            if (wallnumber < 4 &&
                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 ||

                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0 ||

                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0
                                )
                                AddNoeud(wallnumber);
                        }
                        //CONTRE MUR DROIT
                        if (EntityYTemp == MapDimension[1] - 1 && EntityXTemp < MapDimension[0] - 1 && EntityXTemp > 0)
                        {
                            if (wallnumber < 4 &&
                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 ||

                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0 ||
                                
                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0
                                )
                                AddNoeud(wallnumber);
                        }
                        //COIN SUPERIEUR GAUCHE
                        if (EntityXTemp == 0 && EntityYTemp == 0)
                        {
                            if (wallnumber < 4 &&
                                    ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 &&
                                    ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0
                                    )
                                AddNoeud(wallnumber);
                        }
                        //COIN INFERIEUR GAUCHE
                        if (EntityXTemp == MapDimension[0] - 1 && EntityYTemp == 0)
                        {
                            if (wallnumber < 4 &&
                                    ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                    ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0
                                    )
                                AddNoeud(wallnumber);
                        }
                        //COIN SUPERIEUR DROIT
                        if (EntityXTemp == 0 && EntityYTemp == MapDimension[1] - 1)
                        {
                            if (wallnumber < 4 &&
                                    ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 &&
                                    ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0
                                    )
                                AddNoeud(wallnumber);
                        }
                        //COIN INFERIEUR DROIT
                        if (EntityXTemp == MapDimension[0] - 1 && EntityYTemp == MapDimension[1] - 1)
                        {
                            if (wallnumber < 4 &&
                                    ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                    ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0
                                    )
                                AddNoeud(wallnumber);
                        }
                        //A L'INTERIEUR DES TERRES
                        if (EntityXTemp > 0 && EntityXTemp < MapDimension[0] - 1 &&
                            EntityYTemp > 0 && EntityYTemp < MapDimension[1] - 1)
                        {
                            if (wallnumber < 4 &&
                                // --- ArrayPath[EntityXBuff + 1, EntityYBuff] == 0 && ArrayMapTemp[EntityXBuff + 1, EntityYBuff] == 0 && //
                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 ||

                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0 ||

                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0 ||
                                // --- ArrayPath[EntityXBuff - 1, EntityYBuff] == 0 && ArrayMapTemp[EntityXBuff - 1, EntityYBuff] == 0 && //
                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 ||

                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0 ||

                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0 ||
                                // --- ArrayPath[EntityXBuff, EntityYBuff - 1] == 0 && ArrayMapTemp[EntityXBuff, EntityYBuff - 1] == 0 && //
                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0 &&
                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 ||

                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0 &&
                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 ||

                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0 ||
                                // --- ArrayPath[EntityXBuff, EntityYBuff + 1] == 0 && ArrayMapTemp[EntityXBuff, EntityYBuff + 1] == 0 && //
                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0 &&
                                ArrayPath[EntityXTemp + 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp + 1, EntityYTemp] == 0 ||

                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0 &&
                                ArrayPath[EntityXTemp - 1, EntityYTemp] == 0 && ArrayMapTemp[EntityXTemp - 1, EntityYTemp] == 0 ||

                                ArrayPath[EntityXTemp, EntityYTemp + 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp + 1] == 0 &&
                                ArrayPath[EntityXTemp, EntityYTemp - 1] == 0 && ArrayMapTemp[EntityXTemp, EntityYTemp - 1] == 0
                                )
                                AddNoeud(wallnumber);
                        }
                        ArrayPath[EntityX, EntityY] = 0;
                        #endregion
                    }
                    else
                    {
                        if (wallnumber == 4)
                        {
                            if (iNbCheminsValides > 0)
                            {
                                BuildFinalPath();
                                // Ecriture dans le fichiers logs
                                EcritDansLogs(strDebugLog);
                                PurgeMemory();
                                return ArraySUREPATH;
                            }
                            else
                            {
                                // Ecriture dans le fichiers logs
                                EcritDansLogs(strDebugLog);
                                PurgeMemory();
                                throw new Exception("Aucun chemin disponible | No Path Available");
                            }
                        }
                    }
                    //---------------------------------//

                    #region RECHERCHE DES DISTANCES
                    // Info Debug //
                    if (debug)
                        Debug.Write("\r\n\r\nMurs : \r\n");
                    strDebugLog.Add("\r\n\r\nMurs : \r\n");
                    if (wallup)
                    {
                        if (debug)
                            Debug.Write("- haut\r\n");
                        strDebugLog.Add("- haut\r\n");
                    }
                    if (walldown)
                    {
                        if (debug)
                            Debug.Write("- bas\r\n");
                        strDebugLog.Add("- bas\r\n");
                    }
                    if (wallleft)
                    {
                        if (debug)
                            Debug.Write("- gauche\r\n");
                        strDebugLog.Add("- gauche\r\n");
                    }
                    if (wallright)
                    {
                        if (debug)
                            Debug.Write("- droite\r\n");
                        strDebugLog.Add("- droite\r\n");
                    }
                    // --- RECHERCHE DES DISTANCES --- //
                    if (!wallup && EntityXTemp >= 0)
                    {
                        ArrayCurrentDistance[0] = Finddistance(EntityXTemp - 1, EntityYTemp, DestinationX, DestinationY, "0");
                    }
                    else
                    {
                        ArrayCurrentDistance[0] = int.MaxValue;
                    }
                    if (!walldown && EntityXTemp <= MapDimension[0])
                    {
                        ArrayCurrentDistance[1] = Finddistance(EntityXTemp + 1, EntityYTemp, DestinationX, DestinationY, "1");
                    }
                    else
                    {
                        ArrayCurrentDistance[1] = int.MaxValue;
                    }
                    if (!wallleft && EntityYTemp >= 0)
                    {
                        ArrayCurrentDistance[2] = Finddistance(EntityXTemp, EntityYTemp - 1, DestinationX, DestinationY, "2");
                    }
                    else
                    {
                        ArrayCurrentDistance[2] = int.MaxValue;
                    }
                    if (!wallright && EntityYTemp <= MapDimension[1])
                    {
                        ArrayCurrentDistance[3] = Finddistance(EntityXTemp, EntityYTemp + 1, DestinationX, DestinationY, "3");
                    }
                    else
                    {
                        ArrayCurrentDistance[3] = int.MaxValue;
                    }
                    // Info Debug //
                    strDebugLog.Add("\r\n\r\nDistance entre Objet et Destination : \r\n" +
                                    "- haut   : " + ArrayCurrentDistance[0] + "\r\n" +
                                    "- bas    : " + ArrayCurrentDistance[1] + "\r\n" +
                                    "- gauche : " + ArrayCurrentDistance[2] + "\r\n" +
                                    "- droite : " + ArrayCurrentDistance[3] + "\r\n");
                    if (debug)
                    {
                        Debug.Write("\r\nDistance entre Objet et Destination : \r\n");
                        Debug.Write("- haut   : " + ArrayCurrentDistance[0] + "\r\n");
                        Debug.Write("- bas    : " + ArrayCurrentDistance[1] + "\r\n");
                        Debug.Write("- gauche : " + ArrayCurrentDistance[2] + "\r\n");
                        Debug.Write("- droite : " + ArrayCurrentDistance[3] + "\r\n");
                    }
                    

                // --------------------------------------------------------------------- //
                // à l'aide du goto la boucle sera parcouru plusieurs fois afin de 
                // trouver la valeur la plus petite sans déplacer les valeurs
                // --------------------------------------------------------------------- //
                SearchAgain:
                    //---------------------------------//
                    int smallerDist = int.MaxValue;
                    int iPosSmaller = 10;
                Search:
                    if (ArrayCurrentDistance[0] == int.MaxValue &&
                        ArrayCurrentDistance[1] == int.MaxValue &&
                        ArrayCurrentDistance[2] == int.MaxValue &&
                        ArrayCurrentDistance[3] == int.MaxValue)
                    {
                        FourWallsEncounter = true;
                        strDebugLog.Add("\r\nEntité bloquée\r\n");
                        if (debug)
                        {
                            Debug.Write("\r\nEntité bloquée\r\n");
                        }
                        // Au cas ou deux chemins sont possibles dès le départ
                        // et qu'un des chemins n'as pas de noeuds pour retourner ...
                        if (Paths[iNbCheminsValides].Crosses.Count == -1)
                        {
                            EntityXTemp = EntityX;
                            EntityYTemp = EntityY;
                            ArrayMapTemp[Paths[iNbCheminsValides].PathX[0], Paths[iNbCheminsValides].PathY[0]] = 1;
                            Paths[iNbCheminsValides] = new Chemin(MapDimension[0] * 10, MapDimension[1] * 10);
                        }
                        break;
                    }
                    for (int i = 0; i < 4; i++)
                    {
                        if (ArrayCurrentDistance[i] < smallerDist)
                        {
                            smallerDist = ArrayCurrentDistance[i];
                            iPosSmaller = i;
                            goto Search;
                        }
                    }
                    #endregion

                    //
                    // RECHERCHE D'UN CARREFOUR EQUIDISTANT
                    //

                    // ------------------------------------
                    #region SWITCH DEPLACEMENT
                    /*
                     * Les conditions ci-dessous servent à savoir si la case à déjà était atteinte une fois 
                     */
                    //string s = "");
                    switch (iPosSmaller) // 0 = up | 1 = down | 2 = left | 3 = right
                    {
                        case 0:
                            //s = IsEquiCross(EntityXTemp - 1, EntityYTemp);
                            //if (string.IsNullOrEmpty(s))
                            //{
                            if (AlreadyPassed(EntityXTemp - 1, EntityYTemp))
                            {
                                TabCleaner(ArrayCurrentDistance, iPosSmaller);
                                strDebugLog.Add("\r\nRecherche d'une nouvelle distance\r\n");
                                if (debug)
                                {
                                    Debug.WriteLine("\r\nRecherche d'une nouvelle distance");
                                }
                                goto SearchAgain;
                            }
                            else
                            {
                                EntityXTemp--;
                            }
                            //}
                            //else
                            //    MoveEntity(s);
                            break;
                        case 1:
                            //s = IsEquiCross(EntityXTemp + 1, EntityYTemp);
                            //if (string.IsNullOrEmpty(s))
                            //{
                            if (AlreadyPassed(EntityXTemp + 1, EntityYTemp))
                            {
                                TabCleaner(ArrayCurrentDistance, iPosSmaller);
                                strDebugLog.Add("\r\nRecherche d'une nouvelle distance\r\n\r\n");
                                if (debug)
                                {
                                    Debug.WriteLine("\r\nRecherche d'une nouvelle distance\r\n");
                                }
                                goto SearchAgain;
                            }
                            else
                            {
                                EntityXTemp++;
                            }
                            //}
                            //else
                            //    MoveEntity(s);
                            break;
                        case 2:
                            //s = IsEquiCross(EntityXTemp, EntityYTemp - 1);
                            //if (string.IsNullOrEmpty(s))
                            //{

                            if (AlreadyPassed(EntityXTemp, EntityYTemp - 1))
                            {
                                TabCleaner(ArrayCurrentDistance, iPosSmaller);
                                strDebugLog.Add("\r\nRecherche d'une nouvelle distance\r\n\r\n");
                                if (debug)
                                {
                                    Debug.WriteLine("\r\nRecherche d'une nouvelle distance\r\n");
                                }
                                goto SearchAgain;
                            }
                            else
                            {
                                EntityYTemp--;
                            }
                            //}
                            //else
                            //    MoveEntity(s);
                            break;
                        case 3:
                            //s = IsEquiCross(EntityXTemp, EntityYTemp + 1);
                            //if (string.IsNullOrEmpty(s))
                            //{
                            if (AlreadyPassed(EntityXTemp, EntityYTemp + 1))
                            {
                                TabCleaner(ArrayCurrentDistance, iPosSmaller);
                                strDebugLog.Add("\r\nRecherche d'une nouvelle distance\r\n\r\n");
                                if (debug)
                                {
                                    Debug.WriteLine("\r\nRecherche d'une nouvelle distance\r\n");
                                }
                                goto SearchAgain;
                            }
                            else
                            {
                                EntityYTemp++;
                            }
                            //}
                            //else
                            //    MoveEntity(s);
                            break;
                    }
                    #endregion
                    // -------------------------------- //
                    Paths[iNbCheminsValides].PathLenght = Paths[iNbCheminsValides].nbMovementPath; // on récupère le nombre de mouvement fait pour un chemin
                    UpdatePath();
                    // -------------------------------- //
                    if (NoeudAjouter) // on ajoute la localisation du mobile seulement si un noeud a été trouvé
                    {
                        if (!FirstCrossEncounter) //sauvegarde le chemin jusqu'au premier carrefour du chemin en cours
                        {
                            FirstCrossEncounter = true;
                        }
                        Paths[iNbCheminsValides].Crosses[Paths[iNbCheminsValides].Crosses.Count - 1].PosAfterCrossX = EntityXTemp;
                        Paths[iNbCheminsValides].Crosses[Paths[iNbCheminsValides].Crosses.Count - 1].PosAfterCrossY = EntityYTemp;
                        // Permet de mettre à true Equidistance dans le carrefour si deux valeurs sont semblables
                        // utilisé par la recherche des autres chemins
                        if (ArrayCurrentDistance[0] == ArrayCurrentDistance[1] && ArrayCurrentDistance[0] != int.MaxValue ||
                            ArrayCurrentDistance[0] == ArrayCurrentDistance[2] && ArrayCurrentDistance[0] != int.MaxValue ||
                            ArrayCurrentDistance[0] == ArrayCurrentDistance[3] && ArrayCurrentDistance[0] != int.MaxValue ||
                            ArrayCurrentDistance[1] == ArrayCurrentDistance[2] && ArrayCurrentDistance[1] != int.MaxValue ||
                            ArrayCurrentDistance[1] == ArrayCurrentDistance[3] && ArrayCurrentDistance[1] != int.MaxValue ||
                            ArrayCurrentDistance[2] == ArrayCurrentDistance[3] && ArrayCurrentDistance[2] != int.MaxValue)
                        {
                            Paths[iNbCheminsValides].Crosses[Paths[iNbCheminsValides].Crosses.Count - 1].Equidistance = true;
                            strDebugLog.Add("Carrefour en [" + Paths[iNbCheminsValides].Crosses[Paths[iNbCheminsValides].Crosses.Count - 1].CrossX.ToString() + "," + Paths[iNbCheminsValides].Crosses[Paths[iNbCheminsValides].Crosses.Count - 1].CrossY.ToString() + "] est équidistant\r\n\r\n");
                            if (debug)
                            {
                                Debug.Write("Carrefour en [" + Paths[iNbCheminsValides].Crosses[Paths[iNbCheminsValides].Crosses.Count - 1].CrossX.ToString() + "," + Paths[iNbCheminsValides].Crosses[Paths[iNbCheminsValides].Crosses.Count - 1].CrossY.ToString() + "] est équidistant\r\n\r\n");
                            }
                        }
                        NoeudAjouter = false;
                    }
                    if (!FourWallsEncounter && EntityXTemp == DestinationX && EntityYTemp == DestinationY)
                    {
                        Paths[iNbCheminsValides].PathValid = true;
                    }
                    if (FourWallsEncounter)
                        break;
                    // Cette condition permet de ne pas perdre de temps si le chemin emprunter
                    // après un carrefour situé sur l'axe X ou Y de la destination est mauvais
                    // FIN DU WHILE //
                }
                #endregion

                #region Validation du chemin
                if (!FourWallsEncounter && EntityXTemp == DestinationX && EntityYTemp == DestinationY)
                {
                    strDebugLog.Add("\r\nChemin Valide\r\n");
                    if (debug)
                    {
                        Debug.Write("\r\nChemin Valide\r\n");
                    }
                    Paths[iNbCheminsValides].PathValid = true;
                    // Validera le chemin
                    ValidPath(); 
                }
                else
                {
                    strDebugLog.Add("\r\nChemin invalide\r\n");
                    if (debug)
                    {
                        Debug.Write("\r\nChemin invalide\r\n");
                    }
                    // Mettera en mur la direction prise après le dernier croisement
                    CloseDirectionAfterCross();
                }
                #endregion
                FourWallsEncounter = false;
                firsttime = false;
                
            }

            BuildFinalPath();

            if (accurate && !ArraySurePathTempSaved) // Recherche dans l'autre sens
            {
                ArraySurePathTempSaved = true;
                Array.Copy(ArraySUREPATH, ArraySurePathTemp, MapDimension[0] * MapDimension[1]);
                InitializeValues(Map, DestiX, DestiY, EntiX, EntiY);
                shortestpathfound = false;
                goto ReverseSearch;

            }
            else
            {
                if (ArraySurePathTempSaved)
                {
                    int a = 0; int b = 0;
                    foreach (var i in ArraySurePathTemp)
                    {
                        if (i > 0)
                            a++;
                    }
                    foreach (var i in ArraySUREPATH)
                    {
                        if (i > 0)
                            b++;
                    }
                    ArraySurePathTempSaved = false;
                    EcritDansLogs(strDebugLog);
                    if (a < b) // A représente le premier chemin trouvé ( Ent -> Dest ) || B représente le deuxième chemin trouvé ( Dest -> Ent )
                    {
                        PurgeMemory();
                        return ArraySurePathTemp;
                    }
                }
                ArraySurePathTempSaved = false;
                // Ecriture dans le fichiers logs
                EcritDansLogs(strDebugLog);
                PurgeMemory();
                return ArraySUREPATH;
            }
        }
        #region Création de mur / repositionnement du mobile

        private void MoveEntity(string strDir)
        {
            switch (strDir)
            { 
                case "up":
                    EntityXTemp--;
                    break;
                case "down":
                    EntityXTemp++;
                    break;
                case "left":
                    EntityYTemp--;
                    break;
                case "right":
                    EntityYTemp++;
                    break;
            }
        }

        /// <summary>
        /// Pose un mur sur la position prise après le noeud.
        /// Replace l'entité sur la position du noeud.
        /// 
        /// Réagit différemment si l'entité à été bloquée 0-2 ou 3+ de fois
        /// </summary>
        private void CloseDirectionAfterCross()
        {
            // si plus que 1 bloquage
            // FAIRE UN CODE DYNAMIQUE QUI PERMET DE REVENIR AU PREMIER CARREFOUR
            // DU CHEMIN EN FONCTION DE (NbBloquage)
            // -----------------------------------------------
            // si il n'y a plus de carrefour mais que le chemin est invalide, il faut alors 
            // vérifier si un autre chemin à déjà été trouvé au paravent ! Sinon il n'y a pas de solution
            if (Paths[iNbCheminsValides].Crosses.Count > 0)
            {
                int TempNbCross = Paths[iNbCheminsValides].Crosses.Count - 1;

                // --- Placement du mur --- //
                ArrayMapTemp[Paths[iNbCheminsValides].Crosses[TempNbCross].PosAfterCrossX, Paths[iNbCheminsValides].Crosses[TempNbCross].PosAfterCrossY] = 1;
                // --- debug --- //
                strDebugLog.Add("Mur créé en [" + Paths[iNbCheminsValides].Crosses[TempNbCross].PosAfterCrossX + "," + Paths[iNbCheminsValides].Crosses[TempNbCross].PosAfterCrossY + "] \t<---------\r\n");
                if (debug)
                {
                    Debug.WriteLine("Wall created at [" + Paths[iNbCheminsValides].Crosses[TempNbCross].PosAfterCrossX + "," + Paths[iNbCheminsValides].Crosses[TempNbCross].PosAfterCrossY + "] \t<---------");                    
                }
                // --- Replacement de l'objet sur le carrefour --- //
                EntityXTemp = (byte)Paths[iNbCheminsValides].Crosses[TempNbCross].CrossX;
                EntityYTemp = (byte)Paths[iNbCheminsValides].Crosses[TempNbCross].CrossY;

                // +1 Car dans le code chaque nouvelle position est enregistrée avec nbmouvement-1, car après ces opérations
                // on fait nbMouvement++ peut importe si on trouve une nouvelle case ou non
                Paths[iNbCheminsValides].nbMovementPath = Paths[iNbCheminsValides].Crosses[TempNbCross].CrossLenght + 1;

                Paths[iNbCheminsValides].NbMoveSinceLastNode = 0;
                // --- Effacement des derniers enregistrements dans PathX/Y  jusqu'au dernier croisement --- //
                Paths[iNbCheminsValides].PurgePathXY(Paths[iNbCheminsValides].Crosses[TempNbCross].CrossLenght);
                if (TempNbCross - 1 >= 0)
                    Paths[iNbCheminsValides].Crosses[TempNbCross - 1].NbBloquage = Paths[iNbCheminsValides].NbBloquage;
                // --- 
                Paths[iNbCheminsValides].RemoveCross(TempNbCross);
                Paths[iNbCheminsValides].NbBloquage++;

                strDebugLog.Add("\r\n --- Position du mobile [" + EntityXTemp.ToString() + "," + EntityYTemp.ToString() + "]\r\n");
                if (debug)
                {                    
                    Debug.Write("\r\n --- Position du mobile [" + EntityXTemp.ToString() + "," + EntityYTemp.ToString() + "]\r\n");
                }

                ArrayPath = new int[MapDimension[0], MapDimension[1]]; // rechargement de la map vierge
            }
            else
            {
                strDebugLog.Add("\r\n--- Validation du chemin ---\r\n");
                if (debug)
                {  
                    Debug.Write("\r\n--- Validation du chemin ---\r\n");
                }
                ValidPath();
            }   // si le premier carrefour est la position de départ
        }

        private string IsEquiCross(int X, int Y)
        {
            string strGo = "";
            Carrefour temp = new Carrefour();

            foreach (Carrefour c in EquiCross)
            {
                if (EntityXTemp == c.CrossX && EntityYTemp== c.CrossY && iNbCheminsValides > 0)
                {
                    temp = c;
                    break;
                }
            }
            if (temp.CrossX > temp.PosAfterCrossX)
                strGo = "up";
            if (temp.CrossX < temp.PosAfterCrossX)
                strGo = "down";
            if (temp.CrossY > temp.PosAfterCrossY)
                strGo = "left";
            if (temp.CrossY > temp.PosAfterCrossY)
                strGo = "right";
            return strGo;
        }
        // ------------------------------------------------------- //
        // --- Chemin Valide --- Recherche d'un nouveau chemin --- //
        // ------------------------------------------------------- //
        private void ValidPath()
        {
            bool HasEqui = false;
            foreach (Carrefour c in Paths[iNbCheminsValides].Crosses)
            {
                if (c.Equidistance == true)
                    HasEqui = true;
            }
            if (HasEqui)
            {
                string strDebug = "";
                if (iNbCheminsValides == 0)
                {
                    //Paths.Add(Paths[0]); // Le chemin qui sera à la case 0 servira de base pour les opérations
                    iNbCheminsValides++;
                    strDebug = NewPath(true);
                }
                else
                {
                    // Sauvegarde des chemins valides
                    if (Paths[iNbCheminsValides].nbMovementPath <= Paths[iNbCheminsValides - 1].nbMovementPath && Paths[iNbCheminsValides].PathValid)
                    {
                        iNbCheminsValides++;
                        strDebug = NewPath(true);
                    }
                    else
                    {
                        strDebug = NewPath(false);
                    }
                }
                if (debug)
                    Debug.WriteLine(strDebug);
            }
            else
            {
                shortestpathfound = true; // à la palce de BuildFinalpath
            }
        }
        /// <summary>
        /// Permet de construire le chemin final
        /// </summary>
        private void BuildFinalPath()
        {
            shortestpathfound = true;
            // CAR CE CHEMIN ETAIT EN CONSTRUCTION
            int[] ElementsToRemove = new int[Paths.Count];
            int nbElements = 0;
            foreach (Chemin c in Paths)
            {
                if (!c.PathValid)
                {
                    ElementsToRemove[nbElements] = nbElements;
                }
                nbElements++;
            }
            // Suppresion de tout les mauvais chemins
            foreach (int x in ElementsToRemove)
            {
                if (x != 0)
                    Paths.RemoveAt(x);
            }
            // Construit le chemin le plus court parmis tout ceux trouvé
            int shortestDist = 0;
            int i = 0; 
            int j = 0; 
            int[] TabDist = new int[Paths.Count];
            // Remplissage du tableau
            foreach (Chemin c in Paths)
            {
                TabDist[i] = c.nbMovementPath;
                i++;
            }
            //Recherche de la position du plus petit chemin
            shortestDist = TabDist.Min();
            i = 0;
            int Shortestpath = 0;
            //
            foreach (Chemin c in Paths)
            {
                if (c.nbMovementPath == shortestDist)
                {
                    break;
                }
                Shortestpath++;
            }

            // Construction du chemin
            int nbmove = 1; // 1 car pour atteindre la première case il y a eu 1 déplacement
            while (Paths[Shortestpath].PathX[j] != byte.MaxValue)
            {
                ArraySUREPATH[Paths[Shortestpath].PathX[j], Paths[Shortestpath].PathY[j]] = nbmove;
                j++; nbmove++;
            }
            int ii = 0;
            strDebugLog.Add("\r\nChemin le plus court : " + Shortestpath + "\r\n\r\n");
            strDebugLog.Add("Nombre total de chemin : " + Paths.Count +  "\r\n\r\n");
            foreach (int disti in TabDist)
            { 
                strDebugLog.Add("Chemin N°" + ii.ToString() + " nb mouvements : " + disti.ToString() + "\r\n\r\n");
                ii++;
            }
            strDebugLog.Add(" --- FIN --- ");
        }
        #endregion

        #region Vérification de tableau
        /// <summary>
        /// Place la valeur max dans le tableau passé en référence
        /// </summary>
        /// <param name="CurrentDist">tableau référencer, correspond à ArrayDistance</param>
        /// <param name="celltodelete">détermine la cellule à "supprimer"</param>
        private void TabCleaner(int[] CurrentDist, int celltodelete)
        {
            CurrentDist[celltodelete] = int.MaxValue;
        }
        /// <summary>
        /// Permet de déterminer si l'entité est déjà passer par la case passée en paramètre
        /// </summary>
        /// <param name="X">Valeur X de la case</param>
        /// <param name="Y">Valeur Y de la case</param>
        /// <returns></returns>
        private bool AlreadyPassed(int X, int Y)
        {
            // Car on se trouve dans un coin qui n'est pas la destination, autant le bloquer, évite un bug
            if (FindWall(X,Y,false) > 3)
                ArrayMap[Paths[iNbCheminsValides].PathX[0], Paths[iNbCheminsValides].PathY[0]] = 1;
            if (ArrayPath[X, Y] != 0 && ArrayMapTemp[X, Y] != 1 || X == EntityX && Y == EntityY && ArrayMapTemp[X, Y] != 1) // ce qui suit le || est la correction //focntion pas !!!!!!!!!!
                return true;
            else
                return false;
        }
        #endregion

        #region Mise à jour d'information (chemin/noeuds)

        private string NewPath( bool Pathvalid)
        {
            if (!Pathvalid)
                Paths.RemoveAt(Paths.Count - 1);
            string strDebug = "";
            
            Paths.Add(new Chemin(MapDimension[0] * 10, MapDimension[1] * 10));

            Paths[iNbCheminsValides].PathValid = false; // Au cas ou inbCheminsValides n'a pas été incrémenté
            Chemin previousPath = Paths[iNbCheminsValides - 1];

            // Préparation du nouveau Chemin
            int i = previousPath.Crosses.Count - 1;
            int iNbDelnode = 1;
            while (previousPath.Crosses[i].Equidistance == false)
            {
                i--;
                iNbDelnode++;
            }

            int k = 0;
            int X = new int(); int Y = new int();
            previousPath.PathX.CopyTo(Paths[iNbCheminsValides].PathX, 0);
            previousPath.PathY.CopyTo(Paths[iNbCheminsValides].PathY, 0);

            //EquiCross.RemoveRange(0,EquiCross.Count);

            foreach (Carrefour C in previousPath.Crosses)
            {
                Paths[iNbCheminsValides].Crosses.Insert(k,C);
                k++;
            }


            // AJOUT DES CARREFOURS EQUIDISTANTS
            Carrefour Cx = (from item in previousPath.Crosses
                            where item.Equidistance == true
                            select item).Last();
            Carrefour iooo = FindCross(Cx, EquiCross);

            if (iooo != null)
            {
                if (ArrayMapTemp[Cx.PosAfterCrossX, Cx.PosAfterCrossY] != 1)
                {
                    // TROUVER UN MOYEN DE COMPARER EQUICROSS:X:Y AVEC N?IMPORTE LEQUEL QUI EXISTERAIT REPONDANT A LA CONDITION
                    // LE CARREFOUR EXISTE DANS EQUICROSS ?
                    if (iooo.Possibilites > 0 && !matchCross(Cx, EquiCross))
                    {
                        EquiCross.Add(new Carrefour(Cx.CrossX, Cx.CrossY, Cx.PosAfterCrossX, Cx.PosAfterCrossY, Cx.Possibilites, Cx.CrossLenght));
                        X = EquiCross[EquiCross.Count - 1].PosAfterCrossX;
                        Y = EquiCross[EquiCross.Count - 1].PosAfterCrossY;
                    }
                    else
                    {
                        // sinon il reste qu'une seule possiblite, il est donc plus nécessaire d'y revenir
                        if (iooo.Possibilites > 1)
                        {
                            X = Cx.PosAfterCrossX;
                            Y = Cx.PosAfterCrossY;
                        }
                        else
                        {
                            X = iooo.PosAfterCrossX;
                            Y = iooo.PosAfterCrossY;
                        }
                    }
                }
                //
                else
                {
                    // Recupère l'avant dernier carrefour équidistant
                SearchNewEquiCross:
                    Carrefour Cxx = (from item in previousPath.Crosses
                                    where item.Equidistance == true && item != Cx
                                    select item).LastOrDefault();
                    if (Cxx != null)
                    {
                        if (!matchCross(Cxx, EquiCross))
                        {
                            EquiCross.Add(new Carrefour(Cxx.CrossX, Cxx.CrossY, Cxx.PosAfterCrossX, Cxx.PosAfterCrossY, Cxx.Possibilites, Cxx.CrossLenght));
                            X = Cxx.PosAfterCrossX;
                            Y = Cxx.PosAfterCrossY;
                        }
                        else
                        {
                            Cxx.Equidistance = false;
                            goto SearchNewEquiCross;
                        }
                    }
                    else
                    {
                        short iii = 0;
                        foreach (Carrefour c in EquiCross)
                        {
                            if (iii == 0)
                            { 
                                Array.Copy(ArrayMap, ArrayMapTemp, MapDimension[0] * MapDimension[1]);
                            }
                            // Permet de ne pas prendre la derniere
                            if (iii != EquiCross.Count - 1)
                            {
                                if (c.NbBloquage != byte.MaxValue)
                                {
                                    c.Equidistance = true;
                                    c.Possibilites++;
                                    iii++;
                                }
                            }
                            else
                            {
                                // inbcheminsvalides -- pour éviter une double incrementation
                                if (c.NbBloquage == byte.MaxValue)
                                { shortestpathfound = true; iNbCheminsValides--; return "end"; }
                                else
                                    c.NbBloquage = byte.MaxValue;
                                
                                X = c.PosAfterCrossX;
                                Y = c.PosAfterCrossY;
                                EntityXTemp = (byte)c.CrossX;
                                EntityYTemp = (byte)c.CrossY;
                                c.Possibilites--;
                            }
                            
                        }
                    }
                }
            }
            else
            {
                EquiCross.Add(new Carrefour(Cx.CrossX, Cx.CrossY, Cx.PosAfterCrossX, Cx.PosAfterCrossY, Cx.Possibilites, Cx.CrossLenght));
                X = EquiCross[EquiCross.Count - 1].PosAfterCrossX;
                Y = EquiCross[EquiCross.Count - 1].PosAfterCrossY;
            }
            Chemin ui = new Chemin(0, 0);
            Chemin iu = new Chemin(0, 0);
            if (iNbCheminsValides > 1)
            {
                ui = Paths[iNbCheminsValides - 1];
                iu = Paths[iNbCheminsValides - 2];
            }
            if (iNbCheminsValides == 1 || ui.Crosses.Count != iu.Crosses.Count || iu.PathX != ui.PathX)
            {
                // --- EN TEST --- //
                Array.Copy(ArrayMap, ArrayMapTemp, MapDimension[0] * MapDimension[1]);
                // --- --- --- --- //
                Carrefour TempNode = EquiCross[EquiCross.Count - 1];
                Paths[iNbCheminsValides].Crosses.RemoveRange(i + 1, iNbDelnode - 1);
                // INFO DEBUG //
                strDebug += "\r\n\r\nDonnées de PathX :";
                foreach (byte b in Paths[iNbCheminsValides].PathX)
                {
                    if (b == byte.MaxValue)
                    {
                        strDebug += "";
                    }
                    else
                    {
                        strDebug += b.ToString() + ",";
                    }
                }
                strDebug += "\r\nDonnées de PathY :";
                foreach (byte b in Paths[iNbCheminsValides].PathY)
                {
                    if (b == byte.MaxValue)
                    {
                        strDebug += "";
                    }
                    else
                    {
                        strDebug += b.ToString() + ",";
                    }
                }
                strDebug += "\r\n\r\n";
                // --- //
                Paths[iNbCheminsValides].PurgePathXY(TempNode.CrossLenght);
                Paths[iNbCheminsValides].nbMovementPath = TempNode.CrossLenght + 1;

                EntityXTemp = (byte)TempNode.CrossX;
                EntityYTemp = (byte)TempNode.CrossY;

                ArrayMapTemp[TempNode.PosAfterCrossX, TempNode.PosAfterCrossY] = 1;

                ArrayPath = new int[MapDimension[0], MapDimension[1]];

                strDebug += "\r\nSauvegarde du chemin N°" + iNbCheminsValides.ToString() + " effectuée \r\n";
                strDebug += "\r\nRapport du traitement des données pour le nouveau chemin : \r\n";
                strDebug += "Effacement de " + iNbDelnode.ToString() + " valeurs dans PathX/Y \r\n";
                strDebug += "Nombre de mouvement du chemin en cours : " + Paths[iNbCheminsValides].nbMovementPath.ToString() + "\r\n";
                strDebug += "Nombre de croisements : " + Paths[iNbCheminsValides].Crosses.Count + "\r\n";
                strDebug += "Coordonnées de l'entité : [" + EntityXTemp.ToString() + "," + EntityYTemp.ToString() + "]\r\n";
                strDebug += "Coordonnées du mur créé : [" + TempNode.PosAfterCrossX.ToString() + "," + TempNode.PosAfterCrossY.ToString() + "]\r\n\r\n";

                strDebugLog.Add(strDebug);
                // EFFACE SEULEMENT SI LE DERNIER CARREFOUR EQUIDISTANT EST DIFFERENT DE L?ANCIEN
                Carrefour ioo = (from item in EquiCross
                                 where 
                                 item.CrossX == Paths[iNbCheminsValides].Crosses[i].CrossX && 
                                 item.CrossY == Paths[iNbCheminsValides].Crosses[i].CrossY 
                                select item).LastOrDefault();
                if (ioo == null)
                {
                    Array.Copy(ArrayMap, ArrayMapTemp, MapDimension[0] * MapDimension[1]);
                }
                else
                {
                    // PLACEMENT DU MUR APRES PRISE D UN NOUVEAU CHEMIN
                    ArrayMapTemp[X, Y] = 1;

                    // http://stackoverflow.com/questions/1175645/find-an-item-in-list-by-linq
                    Carrefour io = (from item in EquiCross
                                    where item.PosAfterCrossX == X && item.PosAfterCrossY == Y
                                    select item).FirstOrDefault();
                    if (io != null)
                    {
                        EquiCross[EquiCross.IndexOf(io)].Possibilites--;
                        // --- EN TEST --- //
                        if (iNbCheminsValides > 1)
                        {
                            if (iu.nbMovementPath > ui.nbMovementPath)
                            {
                                ArrayMap[X, Y] = 1;
                            }
                        }
                        // --- --- --- --- //
                    }
                    else
                    {
                        shortestpathfound = true;
                    }
                        // le code doit se finir ici !!!
                }
            }
            else
            {
                shortestpathfound = true;
            }
            return strDebug;
        }
        /// <summary>
        /// Permet de retourner vrai ou faux si le carrefour recherché existe.
        /// Les éléments comparés sont CrossX, CrossY, PosAfterCrossX, PosAfterCrossY
        /// </summary>
        /// <param name="CrossToCompare">Le carrefour a comparé</param>
        /// <param name="TabCross">List contenant la totalité des carrefours équidistants découverts</param>
        /// <returns>RETOURNE FAUX SI LE CARREFOUR N'EXISTE PAS DANS LA LISTE DES CARREFOURS EQUIDISTANTS </returns>
        private bool matchCross(Carrefour CrossToCompare, List<Carrefour> TabCross)
        {
            if (TabCross.Count > 0)
            {
                if (FindCross(CrossToCompare,TabCross) != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false; // le carrefour ne peut pas exister puisque la list est vide.
            }
            
        
        }
        // BUG ICI LORS DE L?APPEL AVANT LES TESTS DES CARREFOURS DANS NEW PATH
        private Carrefour FindCross(Carrefour ToFind, List<Carrefour> ListCross)
        {
            if (ToFind != null && ListCross.Count != 0)
            {
                Carrefour comp = (from item in ListCross
                                  where item.CrossX == ToFind.CrossX
                                  && item.CrossY == ToFind.CrossY
                                  && item.PosAfterCrossX == ToFind.PosAfterCrossX
                                  && item.PosAfterCrossY == ToFind.PosAfterCrossY
                                  select item).FirstOrDefault();
                return comp;
            }
            else
                return null;
        
        }
        /// <summary>
        /// Permet de mettre à jour ArrayPath
        /// Sauvegarde le dépalcement (Position X/Y) dans PathX et PathY
        /// </summary>
        private void UpdatePath()
        {
            int TemPos = Paths[iNbCheminsValides].nbMovementPath - 1;
            // -1 lors de PathX et PathY car les tableaux commencent à 0 et nbmovementPath à 1
            ArrayPath[EntityXTemp, EntityYTemp] = Paths[iNbCheminsValides].nbMovementPath;
            Paths[iNbCheminsValides].PathX[TemPos] = EntityXTemp; //sauvegarde du chemin en cours
            Paths[iNbCheminsValides].PathY[TemPos] = EntityYTemp; //sauvegarde du chemin en cours
            Paths[iNbCheminsValides].nbMovementPath++;
            //if (Paths[iNbCheminsValides].NbCrossFoundForThisPath > 0)
            Paths[iNbCheminsValides].NbMoveSinceLastNode++;
            // INFO DEBUG //
            strDebugLog.Add("\r\n\r\nDéplacement en : [" + EntityXTemp + "," + EntityYTemp + "]\r\n\r\n");
            if (debug)
            {
                Debug.Write("\r\n\r\nDéplacement en : [" + EntityXTemp + "," + EntityYTemp + "]\r\n\r\n");                
            }
            if (Paths[iNbCheminsValides].NbMoveSinceLastNode > 2)
            {
                Paths[iNbCheminsValides].NbBloquage = 0;
            }

        }
        /// <summary>
        /// Permet l'ajout de Noeud (Carrefour)
        /// </summary>
        private void AddNoeud(int walls)
        {
            int Possiblities = 3 - walls;
            Paths[iNbCheminsValides].AddCross(EntityXTemp, EntityYTemp, Paths[iNbCheminsValides].nbMovementPath - 1, Paths[iNbCheminsValides].NbBloquage, Possiblities);
            NoeudAjouter = true;
            Paths[iNbCheminsValides].NbMoveSinceLastNode = 0;
        }

        #region Prénettoyage de la map (retrait des "cubes")
        private int[,] RetireCubeAMap(ref int[,] amap)
        {
            List<List<byte>> ListCube = new List<List<byte>>();

            for (byte X = 0; X < amap.GetLength(0); X++)
            {
                for (byte Y = 0; Y < amap.GetLength(1); Y++)
                {
                    //Déclaration du cube
                    List<byte> Cube = new List<byte>(8);

                    if (X > 0 && Y > 0)
                    {
                        //1er cas
                        if (
                            amap[X - 1, Y - 1] == 0 &&
                            amap[X, Y - 1] == 0 &&
                            amap[X, Y] == 0 &&
                            amap[X - 1, Y] == 0
                            )
                        {
                            Cube = RemplirCube(X - 1, Y - 1, X - 1, Y, X, Y - 1, X, Y);
                        }
                    }
                    else
                    {
                        //2ème cas
                        if (X > 0 && Y < amap.GetLength(1) - 1)
                        {
                            if (
                                amap[X, Y] == 0 &&
                                amap[X - 1, Y] == 0 &&
                                amap[X, Y + 1] == 0 &&
                                amap[X - 1, Y + 1] == 0)
                            {
                                Cube = RemplirCube(X - 1, Y + 1, X - 1, Y, X, Y + 1, X, Y);
                            }
                        }
                        else
                        {
                            //3ème cas
                            if (X < amap.GetLength(0) - 1 && Y < amap.GetLength(1) - 1)
                            {
                                if (
                                    amap[X, Y] == 0 &&
                                    amap[X + 1, Y] == 0 &&
                                    amap[X, Y + 1] == 0 &&
                                    amap[X + 1, Y + 1] == 0
                                    )
                                {
                                    Cube = RemplirCube(X + 1, Y + 1, X + 1, Y, X, Y + 1, X, Y);
                                }
                            }
                            else
                            {
                                // 4ème cas
                                if (X < amap.GetLength(0) - 1 && Y > 0)
                                {
                                    if (
                                        amap[X, Y] == 0 &&
                                        amap[X + 1, Y] == 0 &&
                                        amap[X, Y - 1] == 0 &&
                                        amap[X + 1, Y - 1] == 0
                                        )
                                    {
                                        Cube = RemplirCube(X + 1, Y - 1, X + 1, Y, X, Y - 1, X, Y);
                                    }

                                }
                            }
                        }
                    }
                    // On ajoute le cube une fois fini

                    if (Cube.Count > 1)
                    {
                        
                        List<byte> TempCube = new List<byte>();
                        bool SameCubeFound = false;
                        //Copie de la list
                        TempCube.InsertRange(0, Cube);
                        TempCube.Sort();

                        UInt64 iCube =
                                    (UInt64)(TempCube[0]) << 56 |
                                    (UInt64)(TempCube[1]) << 48 |
                                    (UInt64)(TempCube[2]) << 40 |
                                    (UInt64)(TempCube[3]) << 32 |
                                    (UInt64)(TempCube[4]) << 24 |
                                    (UInt64)(TempCube[5]) << 16 |
                                    (UInt64)(TempCube[6]) << 8 |
                                    (UInt64)TempCube[7];

                        // test de comparaison
                        foreach (List<byte> CubeDeListe in ListCube)
                        {
                            List<byte> TempCube2 = new List<byte>();
                            TempCube2.InsertRange(0, CubeDeListe);
                            TempCube2.Sort();
                            UInt64 iCubedeListe =
                                    (UInt64)(TempCube2[0]) << 56 |
                                    (UInt64)(TempCube2[1]) << 48 |
                                    (UInt64)(TempCube2[2]) << 40 |
                                    (UInt64)(TempCube2[3]) << 32 |
                                    (UInt64)(TempCube2[4]) << 24 |
                                    (UInt64)(TempCube2[5]) << 16 |
                                    (UInt64)(TempCube2[6]) << 8  |
                                    (UInt64)(TempCube2[7]);
                            
                            if (iCubedeListe == iCube)
                            {
                                SameCubeFound = true;
                            }
                            else
                            { 
                                SameCubeFound = false; 
                            }
                        }
                        // Ajout du cube
                        if (!SameCubeFound)
                            ListCube.Add(Cube);
                    }
                }
            }
            // Tout les cubes trouvés
            // APPLICATION D'UN MUR SUR LA CASE QUI A DEUX MURS AUTOUR
            int i = 0;
            int OldCaseX = byte.MaxValue;
            int OldCaseY = byte.MaxValue;
            int[] CasesX = new int[ListCube.Count * 4];
            int[] CasesY = new int[ListCube.Count * 4];
            int[] Murs = new int[ListCube.Count * 4];
            
            // Permet l'utilisation de FindWall
            foreach (List<byte> OneCube in ListCube)
            {
                int nbCube = 0;
                int nbCase = 0;
                int Y = 0;
                int X = 0;
                // Checker les murs
                foreach (int Case in OneCube) // Case => position ||| Exemple -> 0 = PosX du la 1ère cellule, 1 = PosY de la 2ème cellule
                {
                    if (nbCase % 2 == 0)
                    {
                        X = Case;
                    }
                    else
                    {
                        Y = Case;
                        strDebugLog.Add("\r\n\r\nRecherche de mur pour le nettoyage --> Cube : " + nbCube.ToString() + " | case : " + nbCase.ToString() + " X: " + nbCube.ToString() + " Y: " + Case.ToString() + "\r\n");
                        if (debug)
                        {                            
                            Debug.WriteLine("\r\n\r\nRecherche de mur pour le nettoyage --> Cube : " + nbCube.ToString() + " | case : " + nbCase.ToString() + " X: " + nbCube.ToString() + " Y: " + Case.ToString());
                        }

                        if (FindWall(X, Y, false) > 1)
                        {
                            if (OldCaseX != byte.MaxValue)
                            {
                                if (
                                    X + 1 == OldCaseX && Y + 1 == OldCaseY ||
                                    X + 1 == OldCaseX && Y == OldCaseY ||
                                    X + 1 == OldCaseX && Y - 1 == OldCaseY ||
                                    X == OldCaseX     && Y + 1 == OldCaseY ||
                                    X == OldCaseX     && Y == OldCaseY ||
                                    X == OldCaseX     && Y - 1 == OldCaseY ||
                                    X - 1 == OldCaseX && Y + 1 == OldCaseY ||
                                    X - 1 == OldCaseX && Y == OldCaseY ||
                                    X - 1 == OldCaseX && Y - 1 == OldCaseY ||
                                    X == OldCaseX     && Y == OldCaseY ||
                                    X == OldCaseX     && Y == OldCaseY ||
                                    X == OldCaseX     && Y == OldCaseY
                                    )
                                {
                                    if (debug)
                                        Debug.Write("\r\n Mur non placé car cube déjà traité\r\n");
                                    strDebugLog.Add("\r\n Mur non placé car cube déjà traité\r\n");
                                }
                                else
                                {
                                    CasesX[i] = X;
                                    CasesY[i] = Y;
                                    i++;
                                    OldCaseX = X;
                                    OldCaseY = Y;
                                }

                            }
                            else
                            {
                                CasesX[i] = X;
                                CasesY[i] = Y;
                                i++;
                                OldCaseX = X;
                                OldCaseY = Y;
                            }
                        }
                    }
                    nbCase++;
                }
                nbCube++;
            }
            // Application des murs sur les cases entourées de 2 murs

            for (int NbCasesTraitees = 0; NbCasesTraitees < CasesX.Count(); NbCasesTraitees++ )
            {
                if (CasesX[NbCasesTraitees] != 0)
                {
                    amap[CasesX[NbCasesTraitees], CasesY[NbCasesTraitees]] = 1;
                    strDebugLog.Add("\r\nApplication d'un mur de correction de cube en [" + CasesX[NbCasesTraitees].ToString() + "," + CasesY[NbCasesTraitees].ToString() + "]\r\n");
                    if (debug)
                        Debug.Write("\r\nApplication d'un mur de correction de cube en [" + CasesX[NbCasesTraitees].ToString() + "," + CasesY[NbCasesTraitees].ToString() + "]\r\n");
                }
                else
                { break; }
            }

                // fin
                // Nettoyage effectué
                Array.Copy(amap, ArrayMap, amap.GetLength(0) * amap.GetLength(1));
                Array.Copy(amap, ArrayMapTemp, amap.GetLength(0) * amap.GetLength(1));
                return amap;
        }

        private int[,] BloqueQdeSac(ref int[,] amap)
        {
            ///représente le nombre total de cases dans la map
            int nbCasesMap = amap.GetLength(0) * amap.GetLength(1);
            ///Passe à TRUE lorsqu'on place un mur supplémentaire
            bool NewWallCreated = false;

            do
            {
                NewWallCreated = false;
                for (int x = 0; x < amap.GetLength(1); x++)
                {
                    for (int y = 0; y < amap.GetLength(0); y++)
                    {
                        if (FindWall(x, y, true) == 3 && amap[x, y] == 0 )
                        {
                            // Si position == Position de l'entité || Si position == Position de la destination
                            if (x == EntityX && y == EntityY || x == DestinationX && y == DestinationY)
                            {
                                if (debug)
                                    Debug.Write("\r\n\r\nPosition semblable au départ ou à l'arrivée\r\n\r\n");
                                strDebugLog.Add("\r\n\r\nPosition semblable au départ ou à l'arrivée\r\n\r\n");
                            }
                            else
                            {
                                amap[x, y] = 1;
                                strDebugLog.Add("\r\nMur de nettoyage créé en [" + x.ToString() + "," + y.ToString() + "]\r\n");
                                if (debug)
                                    Debug.Write("\r\nMur de nettoyage créé en [" + x.ToString() + "," + y.ToString() + "]\r\n");
                                Array.Copy(amap, ArrayMapTemp, amap.GetLength(0) * amap.GetLength(1));
                                NewWallCreated = true;
                            }
                        }
                    }
                }
                strDebugLog.Add("\r\n --- Fin d'une itération de recherche --- \r\n");
                if (debug)
                    Debug.Write("\r\n --- Fin d'une itération de recherche --- \r\n");
            }
            while (NewWallCreated == true);
            Array.Copy(amap, ArrayMap, amap.GetLength(0) * amap.GetLength(1));
            Array.Copy(amap, ArrayMapTemp, amap.GetLength(0) * amap.GetLength(1));
            return amap;
        }

        private List<byte> RemplirCube(int Val1x, int Val1y, int Val2x, int Val2y, int Val3x, int Val3y, int Val4x, int Val4y)
        {
            return new List<byte>(new byte[8] { (byte)Val1x, (byte)Val1y, (byte)Val2x, (byte)Val2y, (byte)Val3x, (byte)Val3y, (byte)Val4x, (byte)Val4y });
        }

        #endregion

        #endregion

        #region Recherche d'éléments (méthodes Find...)
        /// <summary>
        /// Recherche la distance entre l'entité et la destination
        /// </summary>
        /// <param name="EntXBuff">Position X de l'entité</param>
        /// <param name="EntYBuff">Position Y de l'entité</param>
        /// <param name="DestinationX">Position X de la destination</param>
        /// <param name="DestinationY">Position Y de la destination</param>
        /// <param name="Direction">1 = haut | 2 = bas | 3 = gauche | 4 = droite</param>
        /// <returns> return the value between Entity and Destination</returns>
        private int Finddistance(int EntXBuff, int EntYBuff, int DestinationX, int DestinationY, string strDirection)
        {
            int DiffVertical;
            int DiffLateral;
            int DiffAjustement = 0;
            int i = 0;
            int j = 0;

            if (DestinationX < EntXBuff)
                DiffVertical = EntXBuff - DestinationX;
            else
                DiffVertical = DestinationX - EntXBuff;
            if (DestinationY < EntYBuff)
                DiffLateral = EntYBuff - DestinationY;
            else
                DiffLateral = DestinationY - EntYBuff;

            // --- Différences à zéro --- //
            int X = EntityXTemp;
            int Y = EntityYTemp;
            //-------------------------------------------------------------------------------------------//
            // Permet d'anticiper si la prochaine case que le mobile pourrait prendre est un mur ou non. //
            // ---
            // Peut probablement encore etre améliorer
            // ---
            //-------------------------------------------------------------------------------------------//
            // MapDimension[n] - 1 car la dimension de la map est dans la logique que le premier chiffre 
            // est 1 alors que les tableaux commencent à 0
            if (X - DestinationX == 0 || DestinationX - X == 0)
            {
                #region DestinationY > EntityYBuff
                if (DestinationY > Y && wallright && Y < MapDimension[1] - 1)
                {
                    // la destination est à droite, un mur sépare les 2 modules
                    // ----------------------------------------------------------------
                    while (ArrayMapTemp[X + i, Y + 1] == 1) // vers le bas
                    {
                        if (X + i < MapDimension[0] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X - j, Y + 1] == 1) // vers le haut
                    {
                        if (X - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                if (DestinationY > Y && wallup && X < MapDimension[0] - 1)
                {
                    while (ArrayMapTemp[X + 1, Y + i] == 1) // vers la droite
                    {
                        if (Y + i < MapDimension[1] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X + 1, Y - j] == 1) // vers la gauche
                    {
                        if (Y - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                if (DestinationY > Y && walldown && X > 0)
                {
                    while (ArrayMapTemp[X - 1, Y + i] == 1) // vers la droite
                    {
                        if (Y + i < MapDimension[1] - 1)
                            i++;
                        else
                            break;
                    }

                    while (ArrayMapTemp[X - 1, Y - j] == 1) // vers la gauche
                    {
                        if (Y - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                #endregion
                // ----------------------------------------------------------------------------
                #region DestinationY < EntityYBuff
                if (DestinationY < Y && wallleft && Y > 0)
                {
                    // la destination est à gauche, un mur sépare les 2 modules
                    while (ArrayMapTemp[X + i, Y - 1] == 1) // vers le bas
                    {
                        if (X + i < MapDimension[0] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X - j, Y - 1] == 1) // vers le haut
                    {
                        if (X - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                if (DestinationY < Y && wallup && X > 0)
                {
                    // la destination est à gauche, un mur sépare les 2 modules
                    while (ArrayMapTemp[X - 1, Y + i] == 1) // vers la droite
                    {
                        if (Y + i < MapDimension[1] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X - 1, Y - j] == 1) // vers la gauche
                    {
                        if (Y - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                if (DestinationY < Y && walldown && X < MapDimension[0] - 1)
                {
                    // la destination est à gauche, un mur sépare les 2 modules
                    while (ArrayMapTemp[X + 1, Y + i] == 1) // vers la droite
                    {
                        if (Y + i < MapDimension[1] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X + 1, Y - j] == 1) // vers la gauche
                    {
                        if (Y - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                #endregion
            }
            if (Y - DestinationY == 0 || DestinationY - Y == 0)
            {
                #region DestinationX > EntityXBuff
                if (DestinationX > X && walldown && X < MapDimension[0] - 1)
                {
                    // la destination est en bas, un mur sépare les 2 modules
                    while (ArrayMapTemp[X + 1, Y + i] == 1) // vers la droite
                    {
                        if (Y + i < MapDimension[1] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X + 1, Y - j] == 1) // vers la gauche
                    {
                        if (Y - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                if (DestinationX > X && wallleft && Y > 0)
                {
                    // la destination est en bas, un mur sépare les 2 modules
                    while (ArrayMapTemp[X + i, Y - 1] == 1) // vers la droite
                    {
                        if (X + i < MapDimension[0] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X - j, Y - 1] == 1) // vers la gauche
                    {
                        if (X - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                if (DestinationX > X && wallright && Y < MapDimension[1] - 1)
                {
                    // la destination est en bas, un mur sépare les 2 modules
                    while (ArrayMapTemp[X + i, Y + 1] == 1) // vers la droite
                    {
                        if (X + i < MapDimension[0] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X - j, Y + 1] == 1) // vers la gauche
                    {
                        if (X - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                #endregion
                // ----------------------------------------------------------------------------
                #region DestinationX < EntityXBuff

                if (DestinationX < X && wallup && X > 0)
                {
                    // la destination est en haut, un mur sépare les 2 modules
                    while (ArrayMapTemp[X - 1, Y + i] == 1) // vers la droite
                    {
                        if (Y + i < MapDimension[1] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X - 1, Y - j] == 1) // vers la gauche
                    {
                        if (Y - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                if (DestinationX < X && wallleft && Y > 0)
                {
                    // la destination est en haut, un mur sépare les 2 modules
                    while (ArrayMapTemp[X + i, Y - 1] == 1) // vers la droite
                    {
                        if (X + i < MapDimension[0] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X - j, Y - 1] == 1) // vers la gauche
                    {
                        if (X - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                if (DestinationX < X && wallright && Y < MapDimension[1] - 1)
                {
                    // la destination est en haut, un mur sépare les 2 modules
                    while (ArrayMapTemp[X + i, Y + 1] == 1) // vers la droite
                    {
                        if (X + i < MapDimension[0] - 1)
                            i++;
                        else
                            break;
                    }
                    while (ArrayMapTemp[X - j, Y + 1] == 1) // vers la gauche
                    {
                        if (X - j > 0)
                            j++;
                        else
                            break;
                    }
                    if (j > i)
                        DiffAjustement += j;
                    else
                        DiffAjustement += i;
                    i = j = 0;
                }
                #endregion
            }

            // 0 = up | 1 = down | 2 = left | 3 = right
            switch (strDirection)
            { 
                case "0":
                    strDirection = "en haut de la position";
                    break;
                case "1":
                    strDirection = "en bas de la position";
                    break;
                case "2":
                    strDirection = "à gauche de la position";
                    break;
                case "3":
                    strDirection = "à droite de la position";
                    break;
            }

            
            // permet de "simuler" le fait que l'objet contourne sa position
            // de départ ce qui résulte à faire +2 au nombre total de déplacements
            // si l'objet se trouve par exemple à gauche de sa position passée en
            // paramètre et que la destination est à droite, l'ajustement se ferra
            if (EntXBuff + 1 == EntityXTemp && DestinationX > EntityXTemp && DiffVertical > DiffLateral ||
                EntXBuff - 1 == EntityXTemp && DestinationX < EntityXTemp && DiffVertical > DiffLateral ||
                EntYBuff + 1 == EntityYTemp && DestinationY > EntityYTemp && DiffVertical < DiffLateral ||
                EntYBuff - 1 == EntityYTemp && DestinationY < EntityYTemp && DiffVertical < DiffLateral)
            {
                DiffAjustement += 2;
                strDebugLog.Add("\r\nApplication de l'ajustement (+" + DiffAjustement.ToString() + ") sur la distance entre : \r\nla case recherchée " + strDirection + " [" + EntityXTemp + "," + EntityYTemp + "]\r\n");
                if (debug)
                {
                    Debug.Write("\r\nApplication de l'ajustement (+" + DiffAjustement.ToString() + ") sur la distance entre : \r\nla case recherchée " + strDirection + " [" + EntityXTemp + "," + EntityYTemp + "]\r\n");
                }
            }
            return DiffLateral + DiffVertical + DiffAjustement;
        }
        /// <summary>
        /// Permet de trouver le nombre de mur qui se situent autour de la position donnée en paramètre
        /// </summary>
        /// <returns>Le nombre de mur qui entour l'entité</returns>
        /// <param name="X">La valeur de position X</param>
        /// <param name="Y">La valeur de position Y</param>
        /// <param name="silence">Affiche les infos dans le DEBUG de visual studio si SILENCE == TRUE</param>
        private int FindWall(int X, int Y, bool silence)
        {
            // --- remise à zéro du nombre de mur -------------- //
            wallnumber = 0;
            // ----------------- MUR A GAUCHE ? ---------------- //
            // --- vérifications des murs autour de l'entité --- //
            if (!silence)
                strDebugLog.Add("\r\nRecherche d'un mur : gauche");
            if (debug && !silence)
            {
                Debug.Write("\r\nSearching a wall : left");
            }

            if (Y > 0)
                if (ArrayMapTemp[X, Y - 1] != 0)
                {
                    wallleft = true; wallnumber++;
                    if (!silence)
                        strDebugLog.Add("\r\nPrésent");
                    if (debug && !silence)
                    {
                        Debug.Write("\r\nFound !");
                    }
                }
                else
                {
                    wallleft = false;
                    if (!silence)
                        strDebugLog.Add("\r\nAbsent");
                    if (debug && !silence)
                    {
                        Debug.Write("\r\nNot found !");
                    }
                }
            else
            {
                wallleft = true;
                wallnumber++;
                if (!silence)
                    strDebugLog.Add("\r\nRecherche en dehors de la map\r\n");
                if (debug && !silence)
                {
                    Debug.Write("\r\nOut of Map");
                }

                // out of map
            }
            //---------------- MUR A DROITE ? -------------- //
            if (!silence)
                strDebugLog.Add("\r\nRecherche d'un mur : droite");
            if (debug && !silence)
            {
                Debug.Write("\r\nSearching a wall : right");
            }

            if (Y < MapDimension[1] - 1)
                if (ArrayMapTemp[X, Y + 1] != 0)
                {
                    wallright = true;
                    wallnumber++;
                    if (!silence)
                        strDebugLog.Add("\r\nPrésent");
                    if (debug && !silence)
                    {
                        Debug.Write("\r\nFound !");
                    }
                }
                else
                {
                    wallright = false;
                    if (!silence)
                        strDebugLog.Add("\r\nAbsent");
                    if (debug && !silence)
                    {
                        Debug.Write("\r\nNot found !");
                    }
                }
            else
            {
                wallright = true;
                wallnumber++;
                if (!silence)
                    strDebugLog.Add("\r\nRecherche en dehors de la map\r\n");
                if (debug && !silence)
                {
                    Debug.Write("\r\nOut of Map");
                    // out of map
                }

            }
            //---------------- MUR EN HAUT ? -------------- //
            if (!silence)
                strDebugLog.Add("\r\nRecherche d'un mur : haut");
            if (debug && !silence)
            {
                Debug.Write("\r\nSearching a wall : top");
            }

            if (X > 0)
                if (ArrayMapTemp[X - 1, Y] != 0)
                {
                    wallup = true;
                    wallnumber++;
                    if (!silence)
                        strDebugLog.Add("\r\nPrésent");
                    if (debug && !silence)
                    {
                        Debug.Write("\r\nFound !");
                    }
                }
                else
                {
                    wallup = false;
                    if (!silence)
                        strDebugLog.Add("\r\nAbsent");
                    if (debug && !silence)
                    {
                        Debug.Write("\r\nNot found !");
                    }
                }
            else
            {
                wallup = true;
                wallnumber++;
                if (!silence)
                    strDebugLog.Add("\r\nRecherche en dehors de la map\r\n");
                if (debug && !silence)
                {
                    Debug.Write("\r\nOut of Map");
                }

                // out of map
            }
            //---------------- MUR EN BAS ? -------------- //
            if (!silence)
                strDebugLog.Add("\r\nRecherche d'un mur : bas");
            if (debug && !silence)
            {
                Debug.Write("\r\nSearching a wall : bottom");
            }

            if (X < MapDimension[0] - 1)
                if (ArrayMapTemp[X + 1, Y] != 0)
                {
                    walldown = true;
                    wallnumber++;
                    if (!silence)
                        strDebugLog.Add("\r\nPrésent");
                    if (debug && !silence)
                    {
                        Debug.Write("\r\nFound !");
                    }
                }
                else
                {
                    walldown = false;
                    if (!silence)
                        strDebugLog.Add("\r\nAbsent");
                    if (debug && !silence)
                    {
                        Debug.Write("\r\nNot found !");
                    }
                }
            else
            {
                walldown = true;
                wallnumber++;
                if (!silence)
                    strDebugLog.Add("\r\nRecherche en dehors de la map\r\n");
                if (debug && !silence)
                {
                    Debug.Write("\r\nOut of Map");
                    // out of map
                }

            }
            return wallnumber;
        }
        #endregion

        #endregion

        private void EcritDansLogs(List<string> Logs)
        {
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@".\WiMP_Logs.txt"))
            foreach (string s in Logs)
            {
                file.Write(s);
            }
        }
        /// <summary>
        /// Vide au mieux le cache de l'application
        /// </summary>
        private void PurgeMemory()
        {
            MapDimension = null;
            ArraySurePathTemp = null;
            // Map | labyrinthe
            AMap = null;
            // Map | provisoire
            ArrayMapTemp = null;
            ArrayPath = null;
            // Ajout d'un noeud
            Paths = null;
            EquiCross = null;
        }

        #region Constructeur
        /// <summary>
        /// Assignation of all values used in this code
        /// </summary>
        /// <param name="AMap">Array containing the map</param>
        /// <param name="EntX">Entity's Position X</param>
        /// <param name="EntY">Entity's Position Y</param>
        /// <param name="DestX">Destination's Position X</param>
        /// <param name="DestY">Destination's Position Y</param>
        private void InitializeValues(int[,] AMap, byte EntX, byte EntY, byte DestX, byte DestY)
        {
            if (EntX == DestX && EntY == DestY)
                throw new Exception("Erreur ! Destination et Objet semblable");

            MapDimension = new int[2];
            MapDimension[0] = AMap.GetLength(0);
            MapDimension[1] = AMap.GetLength(1);

            for (int i = 0; i < MapDimension[0]; i++)
            {
                for (int j = 0; j < MapDimension[1]; j++)
                {
                    // --- Dessin
                    if (AMap[i, j] == 1 && EntX == i && EntY == j ||
                        AMap[i, j] == 1 && DestX == i && DestY == j)
                    {
                        throw new Exception("Une position de départ ou d'arrivée ne peut pas être un mur !");
                    }
                }
            }

            //Paths
            if (!ArraySurePathTempSaved)
                ArraySurePathTemp = new int[MapDimension[0], MapDimension[1]];
            // position de départ
            this.EntX = EntX;
            this.EntY = EntY;
            EntityXTemp = EntityX;
            EntityYTemp = EntityY;
            // point d'arrivée
            this.DestX = DestX;
            this.DestY = DestY;
            // Map | labyrinthe
            this.AMap = AMap;
            // Map | provisoire
            ArrayMapTemp = new int[MapDimension[0], MapDimension[1]];
            Array.Copy(AMap, ArrayMapTemp, MapDimension[0] * MapDimension[1]);
            // tableau CHEMIN
            ArrayPath             = new int[MapDimension[0], MapDimension[1]];
            ArraySUREPATH         = new int[MapDimension[0], MapDimension[1]];
            // MURS
            wallup = false;
            walldown = false;
            wallleft = false;
            wallright = false;
            wallnumber = 0;
            FourWallsEncounter = false;
            // Première exécution de qq'ch
            firsttime = true;
            FirstCrossEncounter = false;
            // Validation Chemin
            shortestpathfound = false;
            // Ajout d'un noeud
            NoeudAjouter = false;
            iNbCheminsValides = 0;

            Paths = new List<Chemin>();
            EquiCross = new List<Carrefour>();
            Paths.Add(new Chemin(MapDimension[0] * 10, MapDimension[1] * 10));
        }
        #endregion
    }
}

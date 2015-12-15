using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.Generic;

namespace PathFinder
{
    class Chemin
    {
        #region Attributs

        private int iNbMoveSinceLastNode;
        public int  NbBloquage;

        public DateTime dtPATH;

        // contient le chemin possible (n'est pas définitif) 
        public byte[] PathX;
        public byte[] PathY;

        public int PathLenght;
        public bool PathValid;
        /// <summary>
        /// Correspond au nombre de mouvement effectué depuis le début du chemin en cours
        /// </summary>
        public int nbMovementPath;

        public int NbMoveSinceLastNode
        {
            get { return iNbMoveSinceLastNode; }
            set
            {
                iNbMoveSinceLastNode = value;
            }
        }
        public List<Carrefour> Crosses;
        #endregion

        #region Méthodes
        public void BuildPath(int[,] ArrayPath)
        {
            byte loop = 0;
            byte nbmove = 1;
            while (PathX[loop] < byte.MaxValue || PathY[loop] < byte.MaxValue)
            {
                ArrayPath[PathX[loop], PathY[loop]] = nbmove;
                nbmove++;
                loop++;
            }
        }
        /// <summary>
        /// Permet d'effacer du contenu dans PathX et PathY en partant d'un point donné, pour une certaine limite.
        /// </summary>
        /// <param name="Vassigner">Définit le point de départ</param>
        /// <param name="Vlimite">Définit le nombre de cellules qui devront être effacées</param>
        public void PurgePathXY(int Vassigner, int Vlimite)
        {
            for (int i = Vassigner; i < Vlimite; i++)
            {
                PathX[i] = byte.MaxValue;
                PathY[i] = byte.MaxValue;
            }
        }
        /// <summary>
        /// Permet d'effacer toutes les cellules dans PathX et PathY depuis l'index donné.  
        /// </summary>
        /// <param name="Vassigner">Définit le point de départ</param>
        public void PurgePathXY(int Vassigner)
        {
            for (int i = Vassigner; PathX[i] != 255; i++)
            {
                PathX[i] = byte.MaxValue;
                PathY[i] = byte.MaxValue;
            }
        }
        public void AddCross(int CrossX, int CrossY, int CrossLenght, int iNbBloquage, int Possibilites)
        {
            Crosses.Add(new Carrefour(CrossX, CrossY, CrossLenght, iNbBloquage, Possibilites));
        }
        public void RemoveCross(int Indice)
        {
            Crosses.RemoveAt(Indice);
        }
        #endregion

        #region Constructeur
        /// <summary>
        /// Initialise un chemin
        /// </summary>
        /// <param name="SizeX">Largeur de la Map</param>
        /// <param name="SizeY">Hauteur de la map</param>
        public Chemin(int SizeX, int SizeY)
        {
            Crosses = new List<Carrefour>();

            PathX = new byte[SizeX];
            PathY = new byte[SizeY];
            for (int i = PathX.GetLength(0) - 1; i > 0; i--)
                PathX[i] = byte.MaxValue;
            for (int i = PathY.GetLength(0) - 1; i > 0; i--)
                PathY[i] = byte.MaxValue;
            
            nbMovementPath = 1;
            PathLenght = 0;
            
            NbBloquage = 0;
            
            PathValid = false;

            dtPATH = DateTime.Now;
        }
        #endregion
    }
}

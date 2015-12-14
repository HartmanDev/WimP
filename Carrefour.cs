using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinder
{
    class Carrefour
    {
        #region Attributs

        private TimeSpan timePath;
        private DateTime dateCurrentTime;
        
        private int iCrossX;
        private int iCrossY;
        private int iCrossLenght;
        private int iPosAfterCrossX;
        private int iPosAfterCrossY;

        private int iNbBloquage;
        private bool equidistance;


        #endregion

        #region Paramètres

        public TimeSpan TSPATH
        { get { return timePath; } }
        public DateTime dtPATH
        { get { return dateCurrentTime; }
            set {
                dateCurrentTime = value;
            }
        }

        public int CrossX
        {
            get { return iCrossX; }
            set {
                iCrossX = value;
            }
        }
        public int CrossY
        {
            get { return iCrossY; }
            set
            {
                iCrossY = value;
            }
        }
        public int CrossLenght
        {
            get { return iCrossLenght; }
            set
            {
                iCrossLenght = value;
            }
        }
        public int PosAfterCrossX
        {
            get { return iPosAfterCrossX; }
            set
            {
                iPosAfterCrossX = value;
            }
        }
        public int PosAfterCrossY
        {
            get { return iPosAfterCrossY; }
            set
            {
                iPosAfterCrossY = value;
            }
        }
        public int NbBloquage
        {
            get { return iNbBloquage; }
            set
            {
                iNbBloquage = value;
            }
        }
        public bool Equidistance
        {
            get { return equidistance; }
            set
            {
                equidistance = value;
            }
        }
        public int Possibilites;
        #endregion
        /// <summary>
        /// /!\ Initialise rien /!\
        /// </summary>
        public Carrefour()
        { }
        /// <summary>
        /// Permet l'instanciation des carrefours équidistants
        /// </summary>
        /// <param name="CrossX">Position X du carrefour</param>
        /// <param name="CrossY">Position Y du carrefour</param>
        /// <param name="PosAfterCrossX">Position X prise après le carrefour</param>
        /// <param name="PosAfterCrossY">Position Y prise après le carrefour</param>
        public Carrefour(int CrossX, int CrossY, int PosAfterCrossX, int PosAfterCrossY, int Possibilites, int CrossLenght)
        {
            this.dtPATH = DateTime.Now;
            this.CrossX = CrossX;
            this.CrossY = CrossY;
            this.CrossLenght = CrossLenght;
            this.NbBloquage = 0;
            this.iPosAfterCrossX = PosAfterCrossX;
            this.iPosAfterCrossY = PosAfterCrossY;
            equidistance = false;
            this.Possibilites = Possibilites;
        }
        /// <summary>
        /// Permet de créer un carrefour
        /// </summary>
        public Carrefour(int CrossX, int CrossY, int CrossLenght, int NbBloquage, int Possibilites)
        {
            this.dtPATH = DateTime.Now;
            this.CrossX = CrossX;
            this.CrossY = CrossY;
            this.CrossLenght = CrossLenght;
            this.NbBloquage = NbBloquage;
            iPosAfterCrossX = 0;
            iPosAfterCrossY = 0;
            equidistance = false;
            this.Possibilites = Possibilites;
        }
    }
}

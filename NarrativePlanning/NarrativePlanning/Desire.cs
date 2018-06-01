using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarrativePlanning
{
    class Desire
    {


        public String character
        {
            get;
            set;
        }

        /// <summary>
        /// The Character basically stores the name and ONE literal 
        /// in either the B+, B- or Unsure.
        /// </summary>
        public Character goals
        {
            get;
            set;
        }

        /// <summary>
        /// Motivations are also the literals that spur the desire.
        /// </summary>
        public Character motivations
        {
            get;
            set;
        }

        public Desire()
        {
            goals = new Character();
            motivations = new Character();
            character = "";
        }
    }
}

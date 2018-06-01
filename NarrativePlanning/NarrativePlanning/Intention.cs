using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarrativePlanning
{
    class Intention
    {

        /// <summary>
        /// Must store the character name and the single literal
        /// which can be either B plus, B minues, or Unsure
        /// </summary>
        Character character
        {
            get;
            set;
        }

        /// <summary>
        /// WorldState where this intention was adopted
        /// </summary>
        WorldState state
        {
            get;
            set;
        }

        List<String> motivations
        {
            get;
            set;
        }

        /// <summary>
        /// The plan that the character can come up with 
        /// to achieve the goal.
        /// </summary>
        Plan plan
        {
            get;
            set;
        }

    }
}

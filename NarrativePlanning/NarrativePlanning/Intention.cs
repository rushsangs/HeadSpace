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
        Character goals
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

        Character motivations
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

        public override bool Equals(object obj)
        {
            Intention i = obj as Intention;
            bool a = this.goals.Equals(i.goals);
            bool b = this.state.Equals(i.state);
            bool c = this.motivations.Equals(i.motivations);
            return a && b && c;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}

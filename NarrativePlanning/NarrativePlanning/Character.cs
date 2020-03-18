using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NarrativePlanning
{
    /// <summary>
    /// While this class is used as representration for a character,
    /// it is also used in multiple places to represent belief states
    /// in motivations and desires, etc. Basically, it's used to 
	/// represent a tuple of B+,B= and U.
    /// </summary>
    [Serializable]
    public class Character
    {
        public String name;
        public Hashtable bPlus
        {
            get;
            set;
        }
        public Hashtable bMinus
        {
            get;
            set;
        }
        public Hashtable unsure
        {
            get;
            set;
        }
        

        //public BeliefState bs;

        public Character()
        {
            //bs = new BeliefState();
            bPlus = new Hashtable();
            bMinus = new Hashtable();
            unsure = new Hashtable();
        }

        public Character(Hashtable bPlus, Hashtable bMinus, Hashtable unsure)
        {
            this.bPlus = bPlus;
            this.bMinus = bMinus;
            this.unsure = unsure;
        }

        /// <summary>
        /// Creates the characters next relaxed state using
        /// the belief update effects of the operators. Used
        /// by the FF algorithm.
        /// </summary>
        /// <param name="current">Current character</param>
        /// <param name="ground">Grounded operator</param>
        /// <returns>The relaxed beliefs of the character</returns>
        public static Character getNextRelaxedState(Character current, Operator ground)
        {
            Character newState = current.clone();
            foreach (String lit in ground.effBPlus.Keys)
            {
                //if (newState.fWorld.Contains(lit))
                //newState.fWorld.Remove(lit);
                if (!newState.bPlus.Contains(lit))
					newState.bPlus.Add(lit, 1);
            }
            foreach (String lit in ground.effBMinus.Keys)
            {
                //if (newState.tWorld.Contains(lit))
                //newState.tWorld.Remove(lit);
                if (!newState.bMinus.Contains(lit))
					newState.bMinus.Add(lit, 1);
            }
            foreach (String lit in ground.effUnsure.Keys)
            {
                //if (newState.tWorld.Contains(lit))
                //newState.tWorld.Remove(lit);
                if (!newState.unsure.Contains(lit))
					newState.unsure.Add(lit, 1);
            }
            return newState;
        }

        /// <summary>
        /// Checks whether a character thinks if an action
        /// is executable.
        /// </summary>
        /// <param name="gop">The grounded operator</param>
        /// <param name="c">The character object</param>
        /// <returns>True if apparently executable</returns>
        public static bool isApparentlyExecutable(Operator gop, Character c)
        {
            foreach (String gl in gop.preBPlus.Keys)
            {
                if (!c.bPlus.Contains(gl))
                    return false;
            }
            foreach (String gl in gop.preBMinus.Keys)
            {
                if (!c.bMinus.Contains(gl))
                    return false;
            }
            foreach (String gl in gop.preUnsure.Keys)
            {
                if (!c.unsure.Contains(gl))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Checks if a character believes whether they
        /// have achieved their goal conditions
        /// </summary>
        /// <param name="goal">The character object with goal beliefs</param>
        /// <returns>True if goal achieved</returns>
        public bool isGoalState(Character goal)
        {
            foreach (String l in goal.bPlus.Keys)
            {
                if (!this.bPlus.Contains(l))
                    return false;
            }
            foreach (String l in goal.bMinus.Keys)
            {
                if (!this.bMinus.Contains(l))
                    return false;
            }
            foreach (String l in goal.unsure.Keys)
            {
                if (!this.unsure.Contains(l))
                    return false;
            }
            return true;
        }

        public bool hasMotivations(List<Character> motivationslist)
        {
            bool flag = true;
            foreach (Character motivations in motivationslist)
            {
                foreach (String l in motivations.bPlus.Keys)
                {
                    if (!this.bPlus.Contains(l))
                        flag = false;
                }
                foreach (String l in motivations.bMinus.Keys)
                {
                    if (!this.bMinus.Contains(l))
                        flag = false;
                }
                foreach (String l in motivations.unsure.Keys)
                {
                    if (!this.unsure.Contains(l))
                        flag = false;
                }
                if (flag)
                    return true;
            }
            return false ;
        }

        public static WorldState createCharacterGoal(Character goal, String name)
        {
            WorldState res = new WorldState(new Hashtable(), new Hashtable(), new List<Character>());
            if (goal.bPlus.Count > 0)
            {
                foreach(String lit in goal.bPlus.Keys)
                {
                    res.tWorld.Add(lit, 1);
                }
            }
            if (goal.bMinus.Count > 0)
            {
                foreach (String lit in goal.bMinus.Keys)
                {
                    res.fWorld.Add(lit, 1);
                }
            }

            Character c = goal.clone();
            c.name = name;
            res.characters.Add(c);
            return res;
        }


        public static String getGoalBeliefState(Character goals)
        {
            if (goals.bPlus.Count > 0)
                return "bplus";
            else if (goals.bMinus.Count > 0)
                return "bminus";
            return "unsure";

        }

        public Character clone(){
            Character res = new Character();
            res.name = this.name;
			res.bPlus = Operator.DeepClone<Hashtable>(this.bPlus);
			res.bMinus = Operator.DeepClone<Hashtable>(this.bMinus);
			res.unsure = Operator.DeepClone<Hashtable>(this.unsure);
            //res.intentions = this.intentions.C
            return res;
        }

        /// <summary>
        /// Determines whether the specified <see cref="object"/> is equal to the current <see cref="T:NarrativePlanning.Character"/>.
		/// Note: i dont think it checks equality for observability parts of the belief effect stuff
		///  </summary>
        /// <param name="obj">The <see cref="object"/> to compare with the current <see cref="T:NarrativePlanning.Character"/>.</param>
        /// <returns><c>true</c> if the specified <see cref="object"/> is equal to the current
        /// <see cref="T:NarrativePlanning.Character"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            Character c = obj as Character;
            bool a = this.bPlus.Cast<DictionaryEntry>().Union(c.bPlus.Cast<DictionaryEntry>()).Count() == this.bPlus.Count && this.bPlus.Count == c.bPlus.Count;
            bool b = this.bMinus.Cast<DictionaryEntry>().Union(c.bMinus.Cast<DictionaryEntry>()).Count() == this.bMinus.Count && this.bMinus.Count == c.bMinus.Count;
            bool d = this.unsure.Cast<DictionaryEntry>().Union(c.unsure.Cast<DictionaryEntry>()).Count() == this.unsure.Count && this.unsure.Count == c.unsure.Count;
            bool e = (this.name == c.name) || this.name.Equals(c.name);
            return a && b && d && e;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}

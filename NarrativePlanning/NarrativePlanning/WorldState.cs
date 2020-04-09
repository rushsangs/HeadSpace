using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Reflection;

namespace NarrativePlanning
{
    [Serializable]
    public class WorldState
    {
        /// <summary>
        /// set of literals in String form which hold true
        /// </summary>
        public Hashtable tWorld
        {
            get;
            set;
        }

        /// <summary>
        /// set of literals in string form which are false in the world
        /// </summary>
        public Hashtable fWorld
        {
            get;
            set;
        }

        //public List<Literal> tWorld;
        //public List<Literal> fWorld;
        /// <summary>
        /// list of characters
        /// </summary>
        public List<Character> characters
        {
            get;
            set;
        }

        public List<Intention> intentions
        {
            get;
            set;
        }

        public WorldState(Hashtable tWorld, Hashtable fWorld, List<Character> characters)
        {
            
            this.tWorld = tWorld;
            this.fWorld = fWorld;
            this.characters = characters;
            this.intentions = new List<Intention>();
        }

        public WorldState(Hashtable tWorld, Hashtable fWorld, List<Character> characters, List<Intention> intentions1) : this(tWorld, fWorld, characters)
        {
            this.intentions = intentions1;
        }

        /// <summary>
        /// Returns the character object for a queried character name.
        /// </summary>
        /// <param name="name">name of character</param>
        /// <returns>The character object.</returns>
        public Character getCharacter(String name)
        {
            foreach(Character c in this.characters)
            {
                if (c.name.Equals(name))
                    return c;
            }
            return null;
        }

        /// <summary>
        /// Returns a list of possible next world states given a list of operators.
        /// This function isn't used that much because it doesn't provide the operation
        /// performed.
        /// </summary>
        /// <param name="operators">List of grounded operators</param>
        /// <returns>A list of possible world states.</returns>
        public List<WorldState> getPossibleNextStates(List<Operator> operators){
            List<WorldState> possibleNextStates = new List<WorldState>();
            foreach(Operator gop in operators){
                if (isExecutable(gop, this))
                    possibleNextStates.Add(getNextState(this, gop));
            }
            return possibleNextStates;
        }

        /// <summary>
        /// Returns a list of tuples of possible next states APPARENT to the character.
        /// This means that it looks plainly at the character's beliefs and returns the set of tuples
        /// of possible things characters can do.
        /// </summary>
        /// <param name="operators">The list of grounded operators</param>
        /// <returns>A list of tuples. Each tuple contains the operation text and the resulting state.</returns>
        public List<Tuple<String, WorldState>> getPossibleApparentNextStatesTuples(List<Operator> operators)
        {
            List<Tuple<String, WorldState>> possibleNextStateTuples = new List<Tuple<string, WorldState>>();
            foreach (Operator gop in operators)
            {
                 
                if (Character.isApparentlyExecutable(gop, this.getCharacter(gop.character)))
                    possibleNextStateTuples.Add(new Tuple<String, WorldState>(gop.text, getNextState(this, gop)));
            }
            return possibleNextStateTuples;
        }

        /// <summary>
        /// Checks whether an operation is executable at 
        /// a given world state.
        /// </summary>
        /// <param name="gop">The grounded operator</param>
        /// <param name="w">The world state</param>
        /// <returns>True if executable.</returns>
        public static bool isExecutable(Operator gop, WorldState w)
        {
            foreach (String gl in gop.preT.Keys)
            {
                if (!w.tWorld.Contains(gl))
                    return false;
            }
            foreach (String gl in gop.preF.Keys)
            {
                if (!w.fWorld.Contains(gl))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the resultant state when an action is applied on 
        /// a world state
        /// </summary>
        /// <param name="current">The current world state</param>
        /// <param name="ground">Grounded operator to be applied</param>
        /// <returns>Resultant world state.</returns>
        public static WorldState getNextState(WorldState current, Operator ground)
		{
			WorldState newState = current.clone();
			foreach (String lit in ground.effT.Keys)
			{
				if (newState.fWorld.Contains(lit))
					newState.fWorld.Remove(lit);
				if (!newState.tWorld.Contains(lit))
					newState.tWorld.Add(lit, 1);
			}
			foreach (String lit in ground.effF.Keys)
			{
				if (newState.tWorld.Contains(lit))
					newState.tWorld.Remove(lit);
				if (!newState.fWorld.Contains(lit))
					newState.fWorld.Add(lit, 1);
			}
			Character c = newState.getCharacter(ground.character);

			foreach (String lit in ground.effBPlus.Keys)
			{
				List<String> chars = getCharactersThatObserveThis(current, ground, ground.effBPlus[lit] as EffectTuple);
                foreach (String ch in chars)
                {
					Character character = newState.getCharacter(ch);
					if (character.bMinus.Contains(lit))
						character.bMinus.Remove(lit);
                    if (character.unsure.Contains(lit))
                        character.unsure.Remove(lit);
					if (!character.bPlus.Contains(lit))
                        character.bPlus.Add(lit, 1);
                }
				if (c.bMinus.Contains(lit))
					c.bMinus.Remove(lit);
				if (c.unsure.Contains(lit))
					c.unsure.Remove(lit);
				if (!c.bPlus.ContainsKey(lit))
					c.bPlus.Add(lit, 1);
			}
			foreach (String lit in ground.effBMinus.Keys)
			{
				List<String> chars = getCharactersThatObserveThis(current, ground, ground.effBMinus[lit] as EffectTuple);
                foreach (String ch in chars)
                {
                    Character character = newState.getCharacter(ch);
                    if (character.bPlus.Contains(lit))
                        character.bPlus.Remove(lit);
                    if (character.unsure.Contains(lit))
                        character.unsure.Remove(lit);
                    if (!character.bMinus.Contains(lit))
                        character.bMinus.Add(lit, 1);
                }
				if (c.bPlus.Contains(lit))
					c.bPlus.Remove(lit);
				if (c.unsure.Contains(lit))
					c.unsure.Remove(lit);
				if (!c.bMinus.ContainsKey(lit))
					c.bMinus.Add(lit, 1);
			}
			foreach (String lit in ground.effUnsure.Keys)
			{
				List<String> chars = getCharactersThatObserveThis(current, ground, ground.effUnsure[lit] as EffectTuple);
                foreach (String ch in chars)
                {
                    Character character = newState.getCharacter(ch);
                    if (character.bMinus.Contains(lit))
                        character.bMinus.Remove(lit);
					if (character.bPlus.Contains(lit))
						character.bPlus.Remove(lit);
                    if (!character.unsure.Contains(lit))
                        character.unsure.Add(lit, 1);
                }
				if (c.bMinus.Contains(lit))
					c.bMinus.Remove(lit);
				if (c.bPlus.Contains(lit))
					c.bPlus.Remove(lit);
				if (!c.unsure.ContainsKey(lit))
					c.unsure.Add(lit, 1);
			}

            // old observabilility stuff

           
			//foreach(String literal in c.bPlus.Keys)
			//{
			//    if(literal.Trim().StartsWith("at ") && literal.Contains(c.name) && literal.Contains(ground.location))
			//    {
			//        //character was in same location, apply effects.
			//        foreach(String lit in ground.effBPlus.Keys)
			//        {
			//            if (!ground.privateEffects.ContainsKey(lit))
			//            {
			//                if (c.bMinus.Contains(lit))
			//                    c.bMinus.Remove(lit);
			//                if (c.unsure.Contains(lit))
			//                    c.unsure.Remove(lit);
			//                if (!c.bPlus.ContainsKey(lit))
			//                    c.bPlus.Add(lit, 1);
			//            }
			//        }
			//        foreach (String lit in ground.effBMinus.Keys)
			//        {
			//            if (!ground.privateEffects.ContainsKey(lit))
			//            {
			//                if (c.bPlus.Contains(lit))
			//                    c.bPlus.Remove(lit);
			//                if (c.unsure.Contains(lit))
			//                    c.unsure.Remove(lit);
			//                if (!c.bMinus.ContainsKey(lit))
			//                    c.bMinus.Add(lit, 1);
			//            }
			//        }
			//        foreach (String lit in ground.effUnsure.Keys)
			//        {
			//            if (!ground.privateEffects.ContainsKey(lit))
			//            {
			//                if (c.bMinus.Contains(lit))
			//                    c.bMinus.Remove(lit);
			//                if (c.bPlus.Contains(lit))
			//                    c.bPlus.Remove(lit);
			//                if (!c.unsure.ContainsKey(lit))
			//                    c.unsure.Add(lit, 1);
			//            }
			//        }
			//        break;
			//    } 
			//}

			return newState;
          }

		/// <summary>
		/// Returns the relaxed next state, i.e. the WorldState when only the
		/// add effects are applied and not delete effects.
		/// </summary>
		/// <param name="current">Current world state</param>
		/// <param name="ground">Grounded operator to be applied</param>
		/// <returns>Resulting relaxed world state.</returns>
		public static WorldState getNextRelaxedState(WorldState current, Operator ground) 
		{
			WorldState newState = current.clone();
			foreach (String lit in ground.effT.Keys)
			{
				if (!newState.tWorld.Contains(lit))
					newState.tWorld.Add(lit, 1);
			}
			foreach (String lit in ground.effF.Keys)
			{
				if (!newState.fWorld.Contains(lit))
					newState.fWorld.Add(lit, 1);
			}
			Character ch = newState.getCharacter(ground.character);
			foreach (String lit in ground.effBPlus.Keys)
			{
				List<String> chars = getCharactersThatObserveThis(current, ground, ground.effBPlus[lit] as EffectTuple);
				foreach(String c in chars){
					Character character = newState.getCharacter(c);
					if (!character.bPlus.Contains(lit))
                        character.bPlus.Add(lit, 1);
				}
				if (!ch.bPlus.Contains(lit))
					ch.bPlus.Add(lit, 1);
				
			}
			foreach (String lit in ground.effBMinus.Keys)
			{
				List<String> chars = getCharactersThatObserveThis(current, ground, ground.effBMinus[lit] as EffectTuple);
                foreach (String c in chars)
                {
					Character character = newState.getCharacter(c);
					if (!character.bMinus.Contains(lit))
						character.bMinus.Add(lit, 1);
                }
				if (!ch.bMinus.Contains(lit))
					ch.bMinus.Add(lit, 1);
			}
			foreach (String lit in ground.effUnsure.Keys)
			{
				List<String> chars = getCharactersThatObserveThis(current, ground, ground.effUnsure[lit] as EffectTuple);
                foreach (String c in chars)
                {
                    Character character = newState.characters.Find(x => x.name.Equals(c));
					if (!character.unsure.Contains(lit))
						character.unsure.Add(lit, 1);
                }
				if (!ch.unsure.Contains(lit))
					ch.unsure.Add(lit, 1);
			}
          
			/// old observability, not used anymore
			//foreach ( Character c in newState.characters)
			//{
			//    if (c.name.Equals(ground.character))
			//        continue;
			//    foreach (String literal in c.bPlus.Keys)
			//    {
			//        if (literal.Trim().StartsWith("at ") && literal.Contains(c.name) && literal.Contains(ground.location))
			//        {
			//            //character was in same location, apply effects.
			//            foreach (String lit in ground.effBPlus.Keys)
			//            {
			//                if (!ground.privateEffects.ContainsKey(lit))
			//                {
			//                    if (!c.bPlus.ContainsKey(lit))
			//                        c.bPlus.Add(lit, 1);
			//                }
			//            }
			//            foreach (String lit in ground.effBMinus.Keys)
			//            {
			//                if (!ground.privateEffects.ContainsKey(lit))
			//                {
			//                    if (!c.bMinus.ContainsKey(lit))
			//                        c.bMinus.Add(lit, 1);
			//                }
			//            }
			//            foreach (String lit in ground.effUnsure.Keys)
			//            {
			//                if (!ground.privateEffects.ContainsKey(lit))
			//                {
			//                    if (!c.unsure.ContainsKey(lit))
			//                        c.unsure.Add(lit, 1);
			//                }
			//            }
			//            break;
			//        }
			//    }
			//}

			return newState;
		}

		private static List<string> getCharactersThatObserveThis(WorldState world, Operator ground, EffectTuple effect)
		{
			//run the function specified in the effect tuple observability and send the correct args
            

			HashSet<String> characters = new HashSet<string>();
			foreach(ObservabilityRule rule in effect.observabilityrules)
			{
				String fname = rule.fName;
				List<string> args = rule.args;


				Type type = typeof(Observabilities);
				Observabilities  o= new Observabilities();
				MethodInfo theMethod = type.GetMethod(fname);
				Object[] a = { world ,args };
				List<Character> res = theMethod.Invoke(o, a) as List<Character>;
				foreach(Character c in res){
					characters.Add(c.name);
				} 
			}
			return characters.ToList<String>();
		}

		/// <summary>
		/// Checks whether the world state satisfies the provided goal conditions.
		/// </summary>
		/// <param name="goal">Goal, a world state which is a subset of literals
		/// which should be the goal.</param>
		/// <returns>True if world state meets goal conditions.</returns>
		public bool isGoalState(WorldState goal){
            foreach(String l in goal.tWorld.Keys){
                if (!this.tWorld.Contains(l))
                    return false;
            }
            foreach (String l in goal.fWorld.Keys)
            {
                if (!this.fWorld.Contains(l))
                    return false;
            }
            foreach(Character cgoals in goal.characters){
                Character ccurrent = this.characters.Find(x => x.name.Equals(cgoals.name));
                if (ccurrent == null)
                    return false;
                foreach(String l in cgoals.bPlus.Keys)
                {
                    if (!ccurrent.bPlus.Contains(l))
                        return false;
                }
                foreach (String l in cgoals.bMinus.Keys)
                {
                    if (!ccurrent.bMinus.Contains(l))
                        return false;
                }
                foreach (String l in cgoals.unsure.Keys)
                {
                    if (!ccurrent.unsure.Contains(l))
                        return false;
                }
            }
            return true;
        }

        public List<Intention> extractArisingIntentions(List<Desire> desires)
        {
            List<Intention> intentions = new List<Intention>();
            //UnityEngine.Debug.Break();
            foreach(Desire d in desires)
            {
                bool flag = true;
                Character character = this.getCharacter(d.character);
                foreach (String lit in d.motivations.bPlus.Keys)
                {
                    if (!character.bPlus.ContainsKey(lit))
                    {
                        flag = false;
                        break;
                    }
                }
                foreach (String lit in d.motivations.bMinus.Keys)
                {
                    if (!character.bMinus.ContainsKey(lit))
                    {
                        flag = false;
                        break;
                    }
                }
                foreach (String lit in d.motivations.unsure.Keys)
                {
                    if (!character.unsure.ContainsKey(lit))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    //motivations have been satisfied, create intention frame but first check if one exists!
                    foreach(Intention intention in intentions)
                    {
                        if (intention.hasGoal(d.goals))
                        {
                            //add the motivations to this intention
                            intention.motivations.Add(d.motivations);
                            flag = false;
                        }
                    }

                    if (flag)
                    {
                        //create new intention frame 
                        Intention i = new Intention();
                        i.character = character.name;
                        i.goals = d.goals;
                        i.motivations.Add(d.motivations);
                        i.plan = null;
                        i.state = this;
                        intentions.Add(i);
                    }
                }
            }
            return intentions;
        }
        
        /// <summary>
        /// Returns a deep copied clone for a world state.
        /// This has been handwritten because C# doesn't have
        /// a deepcopy by default.
        /// </summary>
        /// <returns>A cloned world state instance</returns>
        public WorldState clone(){
            Hashtable t = this.tWorld.Clone() as Hashtable;
            Hashtable f = this.fWorld.Clone() as Hashtable;
            List<Character> cs = new List<Character>();
            List<Intention> intentions = new List<Intention>();
            foreach(Character c in this.characters){
                cs.Add(c.clone());
            }
            foreach(Intention i in this.intentions)
            {
                intentions.Add(i.clone());
            }
            return new WorldState(t, f, cs, intentions);
        }

        public override bool Equals(object obj)
        {
            WorldState w = obj as WorldState;
            bool a = this.tWorld.Cast<DictionaryEntry>().Union(w.tWorld.Cast<DictionaryEntry>()).Count() == this.tWorld.Count && this.tWorld.Count == w.tWorld.Count;
            bool b = this.fWorld.Cast<DictionaryEntry>().Union(w.fWorld.Cast<DictionaryEntry>()).Count() == this.fWorld.Count && this.fWorld.Count == w.fWorld.Count;
            bool c = this.characters.Count() == w.characters.Count();
            bool d = this.intentions.Count() == w.intentions.Count();
            for (int i = 0; i < this.characters.Count(); ++i) {
                c = c && this.characters[i].Equals(w.characters[i]);
            }
            for (int i = 0; i < this.intentions.Count(); ++i)
            {
                d = d && this.intentions[i].Equals(w.intentions[i]);
            }
            return a && b && c && d;
        }

        public bool HasChangedFrom(object obj)
        {
            WorldState w = obj as WorldState;
            bool a = this.tWorld.Cast<DictionaryEntry>().Union(w.tWorld.Cast<DictionaryEntry>()).Count() == this.tWorld.Count && this.tWorld.Count == w.tWorld.Count;
            bool b = this.fWorld.Cast<DictionaryEntry>().Union(w.fWorld.Cast<DictionaryEntry>()).Count() == this.fWorld.Count && this.fWorld.Count == w.fWorld.Count;
            bool c = this.characters.Count() == w.characters.Count();
            bool d = this.intentions.Count() == w.intentions.Count();
            for (int i = 0; i < this.characters.Count(); ++i)
            {
                c = c && this.characters[i].Equals(w.characters[i]);
            }
            //for (int i = 0; i < this.intentions.Count(); ++i)
            //{
            //    d = d && this.intentions[i].Equals(w.intentions[i]);
            //}
            return a && b && c && d;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;

namespace NarrativePlanning
{
    [Serializable]
    public class WorldState
    {

        public Hashtable tWorld
        {
            get;
            set;
        }
        public Hashtable fWorld
        {
            get;
            set;
        }
        //public List<Literal> tWorld;
        //public List<Literal> fWorld;
        public List<Character> characters
        {
            get;
            set;
        }

        public WorldState(Hashtable tWorld, Hashtable fWorld, List<Character> characters)
        {
            
            this.tWorld = tWorld;
            this.fWorld = fWorld;
            this.characters = characters;
        }

        public List<WorldState> getPossibleNextStates(List<Operator> operators){
            List<WorldState> possibleNextStates = new List<WorldState>();
            foreach(Operator gop in operators){
                if (isExecutable(gop, this))
                    possibleNextStates.Add(getNextState(this, gop));
            }
            return possibleNextStates;
        }

        public List<Tuple<String, WorldState>> getPossibleNextStatesTuples(List<Operator> operators)
        {
            List<Tuple<String, WorldState>> possibleNextStateTuples = new List<Tuple<string, WorldState>>();
            foreach (Operator gop in operators)
            {
                if (isExecutable(gop, this))
                    possibleNextStateTuples.Add(new Tuple<String, WorldState>(gop.text, getNextState(this, gop)));
            }
            return possibleNextStateTuples;
        }

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

        public static WorldState getNextState(WorldState current, Operator ground){
            WorldState newState = current.clone();
            foreach(String lit in ground.effT.Keys){
                if (newState.fWorld.Contains(lit))
                    newState.fWorld.Remove(lit);
                newState.tWorld.Add(lit, 1);
            }
            foreach (String lit in ground.effF.Keys)
            {
                if (newState.tWorld.Contains(lit))
                    newState.tWorld.Remove(lit);
                newState.fWorld.Add(lit, 1);
            }
            foreach(Character c in newState.characters){
                if(c.name.Equals(ground.character)){
                    foreach(String lit in ground.effBPlus.Keys){
                        if (c.bMinus.Contains(lit))
                            c.bMinus.Remove(lit);
                        if (c.unsure.Contains(lit))
                            c.unsure.Remove(lit);
                        if(!c.bPlus.ContainsKey(lit))
                            c.bPlus.Add(lit, 1);
                    }
                    foreach (String lit in ground.effBMinus.Keys)
                    {
                        if (c.bPlus.Contains(lit))
                            c.bPlus.Remove(lit);
                        if (c.unsure.Contains(lit))
                            c.unsure.Remove(lit);
                        if (!c.bMinus.ContainsKey(lit))
                            c.bMinus.Add(lit, 1);
                    }
                    foreach (String lit in ground.effUnsure.Keys)
                    {
                        if (c.bMinus.Contains(lit))
                            c.bMinus.Remove(lit);
                        if (c.bPlus.Contains(lit))
                            c.bPlus.Remove(lit);
                        if (!c.unsure.ContainsKey(lit))
                            c.unsure.Add(lit, 1);
                    }
                    break;
                }
            }
            return newState;
        }

        public static WorldState getNextRelaxedState(WorldState current, Operator ground)
        {
            WorldState newState = current.clone();
            foreach (String lit in ground.effT.Keys)
            {
                //if (newState.fWorld.Contains(lit))
                    //newState.fWorld.Remove(lit);
                if(!newState.tWorld.Contains(lit))
                    newState.tWorld.Add(lit, 1);
            }
            foreach (String lit in ground.effF.Keys)
            {
                //if (newState.tWorld.Contains(lit))
                    //newState.tWorld.Remove(lit);
                if (!newState.fWorld.Contains(lit))
                newState.fWorld.Add(lit, 1);
            }
            Character c = newState.characters.Find(x => x.name.Equals(ground.character));
            foreach (String lit in ground.effBPlus.Keys)
            {
                //if (c.bMinus.Contains(lit))
                //    c.bMinus.Remove(lit);
                //if (c.unsure.Contains(lit))
                //c.unsure.Remove(lit);
                if (!c.bPlus.Contains(lit))
                    c.bPlus.Add(lit, 1);
            }
            foreach (String lit in ground.effBMinus.Keys)
            {
                //if (c.bPlus.Contains(lit))
                //    c.bPlus.Remove(lit);
                //if (c.unsure.Contains(lit))
                //c.unsure.Remove(lit);
                if (!c.bMinus.Contains(lit))
                    c.bMinus.Add(lit, 1);
            }
            foreach (String lit in ground.effUnsure.Keys)
            {
                //if (c.bMinus.Contains(lit))
                //    c.bMinus.Remove(lit);
                //if (c.bPlus.Contains(lit))
                //c.bPlus.Remove(lit);
                if (!c.unsure.Contains(lit))
                    c.unsure.Add(lit, 1);
            }
            return newState;
        }

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
        //public static T DeepCopy<T>(T other)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(ms, other);
        //        ms.Position = 0;
        //        return (T)formatter.Deserialize(ms);
        //    }
        //}
        public WorldState clone(){
            Hashtable t = this.tWorld.Clone() as Hashtable;
            Hashtable f = this.fWorld.Clone() as Hashtable;
            List<Character> cs = new List<Character>();
            foreach(Character c in this.characters){
                cs.Add(c.clone());
            }
            return new WorldState(t, f, cs);
        }

        public override bool Equals(object obj)
        {
            WorldState w = obj as WorldState;
            bool a = this.tWorld.Cast<DictionaryEntry>().Union(w.tWorld.Cast<DictionaryEntry>()).Count() == this.tWorld.Count && this.tWorld.Count == w.tWorld.Count;
            bool b = this.fWorld.Cast<DictionaryEntry>().Union(w.fWorld.Cast<DictionaryEntry>()).Count() == this.fWorld.Count && this.fWorld.Count == w.fWorld.Count;
            bool c = this.characters.Count() == w.characters.Count();
            for (int i = 0; i < this.characters.Count(); ++i){
                c = c && this.characters[i].Equals(w.characters[i]);
            }
            return a && b && c;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}

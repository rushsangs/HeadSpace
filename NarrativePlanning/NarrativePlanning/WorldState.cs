using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NarrativePlanning
{
    [Serializable]
    public class WorldState
    {
        public List<Literal> tWorld;
        public List<Literal> fWorld;
        public List<Character> characters;

        public WorldState(List<Literal> tWorld, List<Literal> fWorld, List<Character> characters)
        {
            this.tWorld = tWorld;
            this.fWorld = fWorld;
            this.characters = characters;
        }

        public List<WorldState> getPossibleNextStates(List<Operator> operators, List<String> groundOperators){
            List<WorldState> possibleNextStates = new List<WorldState>();
            foreach(String ground in groundOperators){
                Operator gop = Operator.getOperator(operators, ground);
                if (isExecutable(gop, this))
                    possibleNextStates.Add(getNextState(this, gop));
            }
            return possibleNextStates;
        }

        public List<Tuple<String, WorldState>> getPossibleNextStatesTuples(List<Operator> operators, List<String> groundOperators)
        {
            List<Tuple<String, WorldState>> possibleNextStateTuples = new List<Tuple<string, WorldState>>();
            foreach (String ground in groundOperators)
            {
                Operator gop = Operator.getOperator(operators, ground);
                if (isExecutable(gop, this))
                    possibleNextStateTuples.Add(new Tuple<String, WorldState>(ground, getNextState(this, gop)));
            }
            return possibleNextStateTuples;
        }

        public static bool isExecutable(Operator gop, WorldState w)
        {
            foreach (Literal gl in gop.preT)
            {
                if (!w.tWorld.Contains(gl))
                    return false;
            }
            foreach (Literal gl in gop.preF)
            {
                if (!w.fWorld.Contains(gl))
                    return false;
            }
            return true;
        }

        public static WorldState getNextState(WorldState current, Operator ground){
            WorldState newState = DeepCopy<WorldState>(current);
            foreach(Literal lit in ground.effT){
                if (newState.fWorld.Contains(lit))
                    newState.fWorld.Remove(lit);
                newState.tWorld.Add(lit);
            }
            foreach (Literal lit in ground.effF)
            {
                if (newState.tWorld.Contains(lit))
                    newState.tWorld.Remove(lit);
                newState.fWorld.Add(lit);
            }
            foreach(Character c in newState.characters){
                if(c.name.Equals(ground.character)){
                    foreach(Literal lit in ground.effBPlus){
                        if (c.bs.bMinus.Contains(lit))
                            c.bs.bMinus.Remove(lit);
                        if (c.bs.unsure.Contains(lit))
                            c.bs.unsure.Remove(lit);
                        c.bs.bPlus.Add(lit);
                    }
                    foreach (Literal lit in ground.effBMinus)
                    {
                        if (c.bs.bPlus.Contains(lit))
                            c.bs.bPlus.Remove(lit);
                        if (c.bs.unsure.Contains(lit))
                            c.bs.unsure.Remove(lit);
                        c.bs.bMinus.Add(lit);
                    }
                    foreach (Literal lit in ground.effUnsure)
                    {
                        if (c.bs.bMinus.Contains(lit))
                            c.bs.bMinus.Remove(lit);
                        if (c.bs.bPlus.Contains(lit))
                            c.bs.bPlus.Remove(lit);
                        c.bs.unsure.Add(lit);
                    }
                    break;
                }
            }
            return newState;
        }

        public bool isGoalState(WorldState goal){
            foreach(Literal l in goal.tWorld){
                if (!this.tWorld.Contains(l))
                    return false;
            }
            foreach (Literal l in goal.fWorld)
            {
                if (!this.fWorld.Contains(l))
                    return false;
            }
            foreach(Character cgoals in goal.characters){
                Character ccurrent = this.characters.Find(x => x.name.Equals(cgoals.name));
                if (ccurrent == null)
                    return false;
                foreach(Literal l in cgoals.bs.bPlus)
                {
                    if (!ccurrent.bs.bPlus.Contains(l))
                        return false;
                }
                foreach (Literal l in cgoals.bs.bMinus)
                {
                    if (!ccurrent.bs.bMinus.Contains(l))
                        return false;
                }
                foreach (Literal l in cgoals.bs.unsure)
                {
                    if (!ccurrent.bs.unsure.Contains(l))
                        return false;
                }
            }
            return true;
        }
        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}

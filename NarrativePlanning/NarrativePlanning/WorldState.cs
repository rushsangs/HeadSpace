using System;
using System.Collections;
using System.Collections.Generic;

namespace NarrativePlanning
{
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

        public bool isExecutable(Operator gop, WorldState w)
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

        public WorldState getNextState(WorldState current, Operator ground){
            WorldState newState = current;
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
    }
}

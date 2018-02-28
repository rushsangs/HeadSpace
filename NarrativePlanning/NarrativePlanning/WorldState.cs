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

        public WorldState(WorldFrame wf )
        {
            characters = wf.characters;
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
            return newState;
            for(current.bsCharacters.)
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class WorldState
    {
        public List<GroundLiteral> tWorld;
        public List<GroundLiteral> fWorld;
        public List<BeliefState> bsCharacters;

        public WorldState(WorldFrame wf)
        {
            bsCharacters = new List<BeliefState>(wf.characters.Count);
            foreach(Character c in wf.characters){
                bsCharacters.Add(c.bs);
            }
        }
    }
}

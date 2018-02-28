using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class MasterGoal
    {
        List<Literal> tWorld;
        List<Literal> fWorld;
        List<EpistemicGoal> egCharacters;

        public MasterGoal(WorldState initial)
        {
            egCharacters = new List<EpistemicGoal>(initial.characters.Count);
            foreach (Character c in initial.characters)
            {
                egCharacters.Add(c.eg);
            }
        }
    }
}

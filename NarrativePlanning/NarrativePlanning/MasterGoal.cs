using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class MasterGoal
    {
        List<GroundLiteral> tWorld;
        List<GroundLiteral> fWorld;
        List<EpistemicGoal> egCharacters;

        public MasterGoal(WorldFrame wf)
        {
            egCharacters = new List<EpistemicGoal>(wf.characters.Count);
            foreach (Character c in wf.characters)
            {
                egCharacters.Add(c.eg);
            }
        }
    }
}

using System;
using System.Collections.Generic;

namespace NarrativePlanning
{

    //DO NOT USE, USE WORLDSTATE INSTEAD
    public class MasterGoal
    {
        List<Literal> tWorld;
        List<Literal> fWorld;
        List<Character> egCharacters;

        public MasterGoal(WorldState initial)
        {
            egCharacters = new List<Character>(initial.characters.Count);
            foreach (Character c in initial.characters)
            {
                egCharacters.Add(c);
            }
        }
    }
}

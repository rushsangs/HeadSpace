using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    //DO NOT USE
    public class EpistemicGoal
    {
        List<Literal> bPlus;
        List<Literal> bMinus;
        List<Literal> unsure;

        public EpistemicGoal()
        {
            bPlus = new List<Literal>();
            bMinus = new List<Literal>();
            unsure = new List<Literal>();
        }
    }
}

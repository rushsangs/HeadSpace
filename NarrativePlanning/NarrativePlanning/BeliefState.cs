using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    [Serializable]
    public class BeliefState
    {
        //all three should make up the GL in the world.
        public List<Literal> bPlus;
        public List<Literal> bMinus;
        public List<Literal> unsure;

        public BeliefState()
        {
            bPlus = new List<Literal>();
            bMinus = new List<Literal>();
            unsure = new List<Literal>();
        }
    }
}

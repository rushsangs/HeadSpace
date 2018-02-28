using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class BeliefState
    {
        //all three should make up the GL in the world.
        public List<Literal> bPlus;
        public List<Literal> bMinus;
        public List<Literal> unsure;

        public BeliefState()
        {
        }
    }
}

using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class BeliefState
    {
        //all three should make up the GL in the world.
        public List<GroundLiteral> bPlus;
        public List<GroundLiteral> bMinus;
        public List<GroundLiteral> unsure;

        public BeliefState()
        {
        }
    }
}

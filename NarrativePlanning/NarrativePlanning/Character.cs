using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class Character : Object
    {
        List<Operator> actions;
        public BeliefState bs;
        public EpistemicGoal eg;

        public Character()
        {
        }

        public bool isExecutable(GroundOperator gop, WorldState w){
            foreach(GroundLiteral gl in gop.preT){
                if (!w.tWorld.Contains(gl))
                    return false;
            }
            foreach (GroundLiteral gl in gop.preF)
            {
                if (!w.fWorld.Contains(gl))
                    return false;
            }
            return true;
        }

        public bool isApparentlyExecutable(GroundOperator gop, WorldState w)
        {
            foreach (GroundLiteral gl in gop.preBPlus)
            {
                if (!this.bs.bPlus.Contains(gl))
                    return false;
            }
            foreach (GroundLiteral gl in gop.preBMinus)
            {
                if (!this.bs.bMinus.Contains(gl))
                    return false;
            }
            return true;
        }
    }
}

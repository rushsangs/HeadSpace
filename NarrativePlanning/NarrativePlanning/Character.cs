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

        public bool isExecutable(Operator gop, WorldState w){
            foreach(Literal gl in gop.preT){
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

        public bool isApparentlyExecutable(Operator gop, WorldState w)
        {
            foreach (Literal gl in gop.preBPlus)
            {
                if (!this.bs.bPlus.Contains(gl))
                    return false;
            }
            foreach (Literal gl in gop.preBMinus)
            {
                if (!this.bs.bMinus.Contains(gl))
                    return false;
            }
            return true;
        }
    }
}

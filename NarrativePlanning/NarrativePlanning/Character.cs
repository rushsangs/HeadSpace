using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    [Serializable]
    public class Character
    {
        public String name;
        //List<Operator> actions;
        public BeliefState bs;
        //public EpistemicGoal eg;

        public Character()
        {
            //actions = new List<Operator>();
            bs = new BeliefState();
            //eg = new EpistemicGoal();
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

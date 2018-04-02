using System;
using System.Collections;
using System.Collections.Generic;

namespace NarrativePlanning
{
    [Serializable]
    public class Character
    {
        public String name;
        public Hashtable bPlus
        {
            get;
            set;
        }
        public Hashtable bMinus
        {
            get;
            set;
        }
        public Hashtable unsure
        {
            get;
            set;
        }
        //public BeliefState bs;

        public Character()
        {
            //bs = new BeliefState();
            bPlus = new Hashtable();
            bMinus = new Hashtable();
            unsure = new Hashtable();
        }

        //public bool isExecutable(Operator gop, WorldState w){
        //    foreach(Literal gl in gop.preT){
        //        if (!w.tWorld.Contains(gl))
        //            return false;
        //    }
        //    foreach (Literal gl in gop.preF)
        //    {
        //        if (!w.fWorld.Contains(gl))
        //            return false;
        //    }
        //    return true;
        //}

        public bool isApparentlyExecutable(Operator gop, WorldState w)
        {
            foreach (String gl in gop.preBPlus.Keys)
            {
                if (!this.bPlus.Contains(gl))
                    return false;
            }
            foreach (String gl in gop.preBMinus.Keys)
            {
                if (!this.bMinus.Contains(gl))
                    return false;
            }
            return true;
        }

        public Character clone(){
            Character res = new Character();
            res.name = this.name;
            res.bPlus = this.bPlus.Clone() as Hashtable;
            res.bMinus = this.bMinus.Clone() as Hashtable;
            res.unsure = this.unsure.Clone() as Hashtable;
            return res;
        }
    }
}

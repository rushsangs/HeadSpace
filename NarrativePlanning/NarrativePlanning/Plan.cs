using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    [Serializable]
    public class Plan
    {
        public PlanningProblem pp;
        public List<Tuple<String, WorldState>> steps;
        public Plan(PlanningProblem pp)
        {
            this.pp = pp;
            steps = new List<Tuple<String, WorldState>>();
            steps.Add(new Tuple<String, WorldState>("null", pp.w0));
        }

        public String toString(){
            String s = "\n";
            steps.ForEach(step=>s=s+step.Item1+"\n");
            return s;
        }

        public Plan clone(){
            Plan p = new Plan(this.pp.clone());
            p.steps = new List<Tuple<string, WorldState>>();
            foreach(Tuple<String, WorldState> t in this.steps){
                p.steps.Add(t);
            }
            return p;
        }
    }
}

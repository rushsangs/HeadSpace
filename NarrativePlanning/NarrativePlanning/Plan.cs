using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    /// <summary>
    /// The data structure that stores a plan.
    /// </summary>
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

    public class Microplan
    {
        public List<String> steps
        {
            get;
            set;
        }

        public List<bool> executed;
        public List<CausalLink> links;


        public Microplan()
        {
            this.steps = new List<String>();
            this.steps.Add("null");
            executed = new List<bool>();
            executed.Add(true);
            links = new List<CausalLink>();
        }

        public Microplan(Plan p)
        {
            this.steps = getStringSteps(p.steps);
            executed = new List<bool>();
            executed.Add(true);
            links = new List<CausalLink>();
        }

        public Microplan(List<String> steps)
        {
            this.steps = steps;
            executed = new List<bool>();
            executed.Add(true);
            links = new List<CausalLink>();
        }

        public List<String> getStringSteps(List<Tuple<String, WorldState>> steps)
        {
            List<String> res = new List<string>();

            foreach(Tuple<String, WorldState> t in steps)
            {
                res.Add(t.Item1);
            }
            return res;
        }

        public void computeCLinks(List<Operator> grounds)
        {
            this.links = CausalLink.findLinks(this, grounds);
        }

        public Microplan clone()
        {
            Microplan p = new Microplan();
            for(int i = 1; i <this.steps.Count; ++i)
            {
                p.steps.Add(this.steps[i]);
            }
            for (int i = 1; i < this.executed.Count; ++i)
            {
                p.executed.Add(this.executed[i]);
            }
            for (int i = 0; i < this.links.Count; ++i)
            {
                p.links.Add(this.links[i].clone());
            }
            return p;
        }
    }
}

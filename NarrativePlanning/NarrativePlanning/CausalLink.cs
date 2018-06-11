using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarrativePlanning
{
    public class CausalLink
    {
        public WorldState first
        {
            get;
            set;
        }
        public WorldState second
        {
            get;
            set;
        }
        public String literal
        {
            get;
            set;
        }
        public String bState
        {
            get;
            set;
        }

        public bool active
        {
            get;
            set;
        }

        public CausalLink(WorldState first, WorldState second, string literal, string bState)
        {
            this.first = first;
            this.second = second;
            this.literal = literal;
            this.bState = bState;
            this.active = false;
        }

		public static List<CausalLink> findLinks(Plan p)
        {
			PlanningProblem pp = p.pp;
			List<CausalLink> links = new List<CausalLink>();
            //basically go backward from the plan looking at each action's precondition and previous action's post condition
            //and when you find a common literal create a causal link for it.
            
            //keys of hashtable is the string, value is a list of step indexes
            Hashtable openbplus = new Hashtable();
            Hashtable openbminus = new Hashtable();
            Hashtable openunsure = new Hashtable();

            for (int i = p.steps.Count - 1; i >= 0; --i)
            {
                Tuple<String, WorldState> step = p.steps[i];
                Operator op = pp.groundedoperators.Find(xy => xy.text.Equals(step.Item1));

                //check if any effects are in the open preconditions
                foreach(String lit in op.effBPlus.Keys)
                {
                    if (openbplus.ContainsKey(lit))
                    {
                        //create causal links with all the steps that rely on this literal
                        List<int> l = (List<int>)openbplus[lit];
						foreach(int index in l){
							WorldState first = p.steps[i].Item2;
							WorldState second = p.steps[index].Item2;
							CausalLink link = new CausalLink(first, second, lit, "bplus");
							links.Add(link);
						}
                    }
                }
				foreach (String lit in op.effBMinus.Keys)
                {
					if (openbminus.ContainsKey(lit))
                    {
                        //create causal links with all the steps that rely on this literal
						List<int> l = (List<int>)openbminus[lit];
                        foreach (int index in l)
                        {
                            WorldState first = p.steps[i].Item2;
                            WorldState second = p.steps[index].Item2;
                            CausalLink link = new CausalLink(first, second, lit, "bminus");
                            links.Add(link);
                        }
                    }
                }
				foreach (String lit in op.effUnsure.Keys)
                {
					if (openunsure.ContainsKey(lit))
                    {
                        //create causal links with all the steps that rely on this literal
						List<int> l = (List<int>)openunsure[lit];
                        foreach (int index in l)
                        {
                            WorldState first = p.steps[i].Item2;
                            WorldState second = p.steps[index].Item2;
                            CausalLink link = new CausalLink(first, second, lit, "unsure");
                            links.Add(link);
                        }
                    }
                }
    
                //compose open preconditions
                foreach (String lit in op.preBPlus.Keys)
                {
                    if (!openbplus.ContainsKey(lit))
                    {
                        List<int> l = new List<int>();
                        l.Add(i);
                        openbplus.Add(lit, l);
                    }
                    else
                    {
                        ((List<int>)openbplus[lit]).Add(i);
                    }
                }
                foreach (String lit in op.preBMinus.Keys)
                {
                    if (!openbminus.ContainsKey(lit))
                    {
                        List<int> l = new List<int>();
                        l.Add(i);
                        openbminus.Add(lit, l);
                    }
                    else
                    {
                        ((List<int>)openbminus[lit]).Add(i);
                    }
                }
                foreach (String lit in op.preUnsure.Keys)
                {
                    if (!openunsure.ContainsKey(lit))
                    {
                        List<int> l = new List<int>();
                        l.Add(i);
                        openunsure.Add(lit, l);
                    }
                    else
                    {
                        ((List<int>)openunsure[lit]).Add(i);
                    }
                }
               

            }
			return links;
        }
    }
}

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
        public int first
        {
            get;
            set;
        }
        public int second
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
        public String character
        {
            get;
            set;
        }

        public bool active
        {
            get;
            set;
        }

        public CausalLink(int first, int second, string literal, string bState, string character)
        {
            this.first = first;
            this.second = second;
            this.literal = literal;
            this.bState = bState;
            this.character = character;
            this.active = false;
        }

		public static List<CausalLink> findLinks(Microplan p, List<Operator> groundedoperators, Character initial, Character goal)
        {
			//PlanningProblem pp = p.pp;
			List<CausalLink> links = new List<CausalLink>();
            //basically go backward from the plan looking at each action's precondition and previous action's post condition
            //and when you find a common literal create a causal link for it.
            
            //keys of hashtable is the string, value is a list of step indexes
            Hashtable openbplus = new Hashtable();
            Hashtable openbminus = new Hashtable();
            Hashtable openunsure = new Hashtable();

            for (int i = p.steps.Count - 1; i >= 0; --i)
            {
                String step = p.steps[i];

                if(step.Equals("null1"))
                {
                    //this is representing the first step, all its literals are technically open preconditions

                    foreach (String lit in initial.bPlus.Keys)
                    {
                        if (openbplus.ContainsKey(lit))
                        {
                            //create causal links with all the steps that rely on this literal
                            List<int> l = (List<int>)openbplus[lit];
                            foreach (int index in l)
                            {
                                int first = i;
                                int second = index;
                                CausalLink link = new CausalLink(first, second, lit, "bplus", initial.name);
                                links.Add(link);
                            }
                        }
                    }
                    foreach (String lit in initial.bMinus.Keys)
                    {
                        if (openbminus.ContainsKey(lit))
                        {
                            //create causal links with all the steps that rely on this literal
                            List<int> l = (List<int>)openbminus[lit];
                            foreach (int index in l)
                            {
                                int first = i;
                                int second = index;
                                CausalLink link = new CausalLink(first, second, lit, "bminus", initial.name);
                                links.Add(link);
                            }
                        }
                    }
                    foreach (String lit in initial.unsure.Keys)
                    {
                        if (openunsure.ContainsKey(lit))
                        {
                            //create causal links with all the steps that rely on this literal
                            List<int> l = (List<int>)openunsure[lit];
                            foreach (int index in l)
                            {
                                int first = i;
                                int second = index;
                                CausalLink link = new CausalLink(first, second, lit, "unsure", initial.name);
                                links.Add(link);
                            }
                        }
                    }
                }
                if(step.Equals("null2"))
                {
                    //this is the last step, all of the literals in this should become open conditions
                    foreach (String lit in goal.bPlus.Keys)
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
                    foreach (String lit in goal.bMinus.Keys)
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
                    foreach (String lit in goal.unsure.Keys)
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

                Operator op = groundedoperators.Find(xy => xy.text.Equals(step));
                if (op == null)
                    continue;
                //check if any effects are in the open preconditions
                foreach(String lit in op.effBPlus.Keys)
                {
                    if (openbplus.ContainsKey(lit))
                    {
                        //create causal links with all the steps that rely on this literal
                        List<int> l = (List<int>)openbplus[lit];
						foreach(int index in l){
							int first = i;
							int second = index;
							CausalLink link = new CausalLink(first, second, lit, "bplus", op.character);
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
                            int first = i;
                            int second = index;
                            CausalLink link = new CausalLink(first, second, lit, "bminus", op.character);
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
                            int first = i;
                            int second = index;
                            CausalLink link = new CausalLink(first, second, lit, "unsure", op.character);
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

        //this version is for non-intention plan causal links. Not used by HeadSpace itself
        public static List<CausalLink> findLinks(Microplan p, List<Operator> groundedoperators, WorldState initial, WorldState goal)
        {
            //PlanningProblem pp = p.pp;
            List<CausalLink> links = new List<CausalLink>();
            //basically go backward from the plan looking at each action's precondition and previous action's post condition
            //and when you find a common literal create a causal link for it.

            //keys of hashtable is the string, value is a list of step indexes
            Hashtable opent = new Hashtable();
            Hashtable openf = new Hashtable();

            for (int i = p.steps.Count - 1; i >= 0; --i)
            {
                String step = p.steps[i];

                if (step.Equals("null1"))
                {
                    //this is representing the first step, all its literals are technically open preconditions

                    foreach (String lit in initial.tWorld.Keys)
                    {
                        if (opent.ContainsKey(lit))
                        {
                            //create causal links with all the steps that rely on this literal
                            List<int> l = (List<int>)opent[lit];
                            foreach (int index in l)
                            {
                                int first = i;
                                int second = index;
                                CausalLink link = new CausalLink(first, second, lit, "t", "");
                                links.Add(link);
                            }
                        }
                    }
                    foreach (String lit in initial.fWorld.Keys)
                    {
                        if (openf.ContainsKey(lit))
                        {
                            //create causal links with all the steps that rely on this literal
                            List<int> l = (List<int>)openf[lit];
                            foreach (int index in l)
                            {
                                int first = i;
                                int second = index;
                                CausalLink link = new CausalLink(first, second, lit, "f", "");
                                links.Add(link);
                            }
                        }
                    }
                }
                if (step.Equals("null2"))
                {
                    //this is the last step, all of the literals in this should become open conditions
                    foreach (String lit in goal.tWorld.Keys)
                    {
                        if (!opent.ContainsKey(lit))
                        {
                            List<int> l = new List<int>();
                            l.Add(i);
                            opent.Add(lit, l);
                        }
                        else
                        {
                            ((List<int>)opent[lit]).Add(i);
                        }
                    }
                    foreach (String lit in goal.fWorld.Keys)
                    {
                        if (!openf.ContainsKey(lit))
                        {
                            List<int> l = new List<int>();
                            l.Add(i);
                            openf.Add(lit, l);
                        }
                        else
                        {
                            ((List<int>)openf[lit]).Add(i);
                        }
                    }
                }

                Operator op = groundedoperators.Find(xy => xy.text.Equals(step));
                if (op == null)
                    continue;
                //check if any effects are in the open preconditions
                foreach (String lit in op.effT.Keys)
                {
                    if (opent.ContainsKey(lit))
                    {
                        //create causal links with all the steps that rely on this literal
                        List<int> l = (List<int>)opent[lit];
                        foreach (int index in l)
                        {
                            int first = i;
                            int second = index;
                            CausalLink link = new CausalLink(first, second, lit, "t", "");
                            links.Add(link);
                        }
                    }
                }
                foreach (String lit in op.effF.Keys)
                {
                    if (openf.ContainsKey(lit))
                    {
                        //create causal links with all the steps that rely on this literal
                        List<int> l = (List<int>)openf[lit];
                        foreach (int index in l)
                        {
                            int first = i;
                            int second = index;
                            CausalLink link = new CausalLink(first, second, lit, "f", "");
                            links.Add(link);
                        }
                    }
                }

                //compose open preconditions
                foreach (String lit in op.preT.Keys)
                {
                    if (!opent.ContainsKey(lit))
                    {
                        List<int> l = new List<int>();
                        l.Add(i);
                        opent.Add(lit, l);
                    }
                    else
                    {
                        ((List<int>)opent[lit]).Add(i);
                    }
                }
                foreach (String lit in op.preF.Keys)
                {
                    if (!openf.ContainsKey(lit))
                    {
                        List<int> l = new List<int>();
                        l.Add(i);
                        openf.Add(lit, l);
                    }
                    else
                    {
                        ((List<int>)openf[lit]).Add(i);
                    }
                }
            }
            return links;
        }

        public static bool isLinkThreatened(Microplan p, WorldState w)
        {
            //figure out which steps have been executed in the plan
            int i = 0;
            for (i =0;  i<p.executed.Count; ++i )
            {
                if (!p.executed[i])
                    break;
            }
            
            //i is the index of step which was not executed,
            //i-1 is the last executed step

            List<CausalLink> openlinks = new List<CausalLink>();

            foreach(CausalLink link in p.links)
            {
                if (link.first < i && link.second >= i)
                    openlinks.Add(link);
            }

            //check if the openlinks literal/bstate
            // are satisfied.
            foreach(CausalLink link in openlinks)
            {
                Character c = w.characters.Find(xy => xy.name.Equals(link.character));
                if (link.bState.Equals("bplus"))
                {
                    if (!c.bPlus.Contains(link.literal))
                    {
                        //check if there is another causal link 
                        bool flag = false;
                        foreach (CausalLink other in p.links)
                        {
                            if (other.character.Equals(link.character)
                                && other.bState.Equals(link.bState)
                                && other.literal.Equals(link.literal)
                                && other.second == link.second
                                && other.first > link.first)
                                flag = true;
                        }
                        if (!flag)
                            return true;
                    }
                }
                else if (link.bState.Equals("bminus"))
                {
                    if (!c.bMinus.Contains(link.literal))
                    {
                        //check if there is another causal link 
                        bool flag = false;
                        foreach (CausalLink other in p.links)
                        {
                            if (other.character.Equals(link.character)
                                && other.bState.Equals(link.bState)
                                && other.literal.Equals(link.literal)
                                && other.second == link.second
                                && other.first > link.first)
                                flag = true;
                        }
                        if (!flag)
                            return true;
                    }
                }
                else if (link.bState.Equals("unsure"))
                {
                    if (!c.unsure.Contains(link.literal))
                    {
                        //check if there is another causal link 
                        bool flag = false;
                        foreach (CausalLink other in p.links)
                        {
                            if (other.character.Equals(link.character)
                                && other.bState.Equals(link.bState)
                                && other.literal.Equals(link.literal)
                                && other.second == link.second
                                && other.first > link.first)
                                flag = true;
                        }
                        if (!flag)
                            return true;
                    }
                }

            }
            return false;
        }

        //created for ShowRunner
        public static bool isMaterialLinkThreatened(Microplan p, WorldState w)
        {
            //figure out which steps have been executed in the plan
            int i = 0;
            for (i = 0; i < p.executed.Count; ++i)
            {
                if (!p.executed[i])
                    break;
            }

            //i is the index of step which was not executed,
            //i-1 is the last executed step

            List<CausalLink> openlinks = new List<CausalLink>();

            foreach (CausalLink link in p.links)
            {
                if (link.first < i && link.second >= i)
                    openlinks.Add(link);
            }

            //check if the openlinks literal/bstate
            // are satisfied.
            foreach (CausalLink link in openlinks)
            {
                
                if (link.bState.Equals("t"))
                {
                    if (!w.tWorld.Contains(link.literal))
                    {
                        //check if there is another causal link 
                        bool flag = false;
                        foreach (CausalLink other in p.links)
                        {
                            if (other.character.Equals(link.character)
                                && other.bState.Equals(link.bState)
                                && other.literal.Equals(link.literal)
                                && other.second == link.second
                                && other.first > link.first)
                                flag = true;
                        }
                        if (!flag)
                            return true;
                    }
                }
                else if (link.bState.Equals("f"))
                {
                    if (!w.fWorld.Contains(link.literal))
                    {
                        //check if there is another causal link 
                        bool flag = false;
                        foreach (CausalLink other in p.links)
                        {
                            if (other.character.Equals(link.character)
                                && other.bState.Equals(link.bState)
                                && other.literal.Equals(link.literal)
                                && other.second == link.second
                                && other.first > link.first)
                                flag = true;
                        }
                        if (!flag)
                            return true;
                    }
                }
            }
            return false;
        }

        public CausalLink clone()
        {
            CausalLink res = new CausalLink(this.first, this.second, this.literal, this.bState, this.character);
            return res;
        }
    }
}

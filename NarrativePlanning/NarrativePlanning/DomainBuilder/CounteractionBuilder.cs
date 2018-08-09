using NarrativePlanning;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarrativePlanning.DomainBuilder
{
    class CounteractionBuilder
    {
        public static List<NarrativePlanning.CounterAction> parseCounteractions(JSONDomain.Counteraction[] cas)
        {
            List<NarrativePlanning.CounterAction> res = new List<CounterAction>();
            foreach (JSONDomain.Counteraction ca in cas)
            {
                
                WorldState w = NarrativePlanning.DomainBuilder.StateCreator.getState(ca.Conditions);
                NarrativePlanning.CounterAction counterAction = new NarrativePlanning.CounterAction(w, ca.Groundedoperator);
                
                res.Add(counterAction);
            }

            return res;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarrativePlanning
{
    public class CounterAction
    {
        public WorldState conditions
        {
            get;
            set;
        }

        public String groundedoperator
        {
            get;
            set;
        }

        public CounterAction(WorldState conditions, string groundedoperator)
        {

            this.conditions = conditions;
            this.groundedoperator = groundedoperator;
        }
    }
}

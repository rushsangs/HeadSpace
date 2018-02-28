using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class Plan
    {
        PlanningProblem pp;
        List<Tuple<Operator, WorldState>> steps;
        public Plan(PlanningProblem pp)
        {
            this.pp = pp;
            steps = new List<Tuple<Operator, WorldState>>();
            steps.Add(new Tuple<Operator, WorldState>(null, pp.w0));
        }
    }
}

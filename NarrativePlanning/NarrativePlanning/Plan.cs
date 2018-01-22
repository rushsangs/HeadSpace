using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class Plan
    {
        PlanningProblem pp;
        List<Tuple<GroundOperator, WorldState>> steps;
        public Plan(PlanningProblem pp)
        {
            this.pp = pp;
            steps = new List<Tuple<GroundOperator, WorldState>>();
            steps.Add(new Tuple<GroundOperator, WorldState>(null, pp.w0));
        }
    }
}

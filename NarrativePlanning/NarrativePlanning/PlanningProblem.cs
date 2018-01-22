using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class PlanningProblem
    {
        public WorldFrame wf;
        public WorldState w0;
        public MasterGoal mgs;
        public List<GroundOperator> go;

        public PlanningProblem()
        {
        }
    }
}

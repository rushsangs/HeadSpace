using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NarrativePlanning
{
    [Serializable]
    public class PlanningProblem
    {
        public WorldState w0;
        public WorldState goal;
        public List<Operator> groundedoperators;

        /// <summary>
        /// A Planning Problem consists of the initial state, the goal state
        /// and the operators possible.
        /// </summary>
        /// <param name="initial">initial WorldState</param>
        /// <param name="goal">Goal worldstate</param>
        /// <param name="operators">List of grounded operators</param>
        public PlanningProblem(WorldState initial, WorldState goal, List<Operator> operators)
        {
            w0 = initial;
            this.goal = goal;
            this.groundedoperators = operators;
        }



        /// <summary>
        /// DFS didn't run because it would result in infinite plans
        /// with recursive actions, hence not reaching the goal state.
        /// </summary>
        /// <returns>Nothing.</returns>
        //public List<Tuple<String, WorldState>> DFS(WorldState w, List<Tuple<String, WorldState>> steps){
        //    List<Tuple<String, WorldState>> nextStateTuples = w.getPossibleNextStatesTuples(operators, gops);
        //    foreach(Tuple<String,WorldState> next in nextStateTuples){
        //        if (next.Item2.isGoalState(goal)){
        //            steps.Add(next);
        //            return steps;
        //        }
        //        else{
        //            steps.AddRange(DFS(next.Item2, new List<Tuple<string, WorldState>>()));
        //            return steps;
        //        }
        //    }
        //}

        public Plan BFSSolution()
        {
            List<Tuple<String, WorldState>> nextStateTuples = w0.getPossibleApparentNextStatesTuples(groundedoperators);
            int depth = 1;
            int bfactor = 0;
            int nnodes = 0;
            Plan solutionPlan = null;
            Queue<Plan> queue = new Queue<Plan>();
            while (solutionPlan == null)
            {
                //if first time do not look at queue
                if (depth == 1)
                {
                    foreach (Tuple<String, WorldState> next in w0.getPossibleApparentNextStatesTuples(groundedoperators))
                    {
                        nnodes++;
                        bfactor++;
                        Plan p = new Plan(this);
                        p.steps.Add(next);
                        queue.Enqueue(p);
                        if (next.Item2.isGoalState(this.goal))
                        {
                            //solution found!
                            solutionPlan = p;
                            Console.Write("\n Number of nodes = " + nnodes + " and branching factor = " + bfactor);
                            return solutionPlan;
                        }

                    }
                }
                else
                {
                    while (queue.Peek().steps.Count == depth)
                    {
                        Plan p = queue.Dequeue();
                        WorldState w = p.steps[p.steps.Count - 1].Item2;
                        int x = 0;
                        foreach (Tuple<String, WorldState> next in w.getPossibleApparentNextStatesTuples(groundedoperators))
                        {
                            x++;
                            nnodes++;
                            Plan q = p.clone();
                            q.steps.Add(next);
                            queue.Enqueue(q);
                            if (next.Item2.isGoalState(this.goal))
                            {
                                //solution found!
                                solutionPlan = q;
                                Console.Write("\n Number of nodes = " + nnodes + " and branching factor = " + Math.Max(bfactor, x) + ".");
                                return solutionPlan;
                            }
                        }
                        if (x > bfactor)
                            bfactor = x;
                    }
                }
                depth++;
            }
            return solutionPlan;
        }

        /// <summary>
        /// Returns a plan using a FF-based solution.
        /// </summary>
        /// <returns> A solution plan</returns>
        public Plan FFSolution()
        {
            List<Tuple<String, WorldState>> nextStateTuples = w0.getPossibleApparentNextStatesTuples(groundedoperators);
            int depth = 1;
            int bfactor = 0;
            int avg_branching_factor = 0;
            int nnodes = 0;
            Plan solutionPlan = null;
            Plan current = new Plan(this);
            //Queue<Plan> queue = new Queue<Plan>();
            int min = -1;
            Tuple<String, WorldState> best = null;
            while (solutionPlan == null)
            {
                min = 100;
                WorldState w = current.steps[current.steps.Count - 1].Item2;
                int tmp = 0;

                //check every node in the frontier    
                foreach (Tuple<String, WorldState> next in w.getPossibleApparentNextStatesTuples(groundedoperators))
                {
                    if (next.Item1.Contains("-false"))
                        continue;
                    nnodes++;
                    tmp++;
                    Plan p = new Plan(this);
                    //queue.Enqueue(p);
                    Operator op = groundedoperators.Find(xy => xy.text.Equals(next.Item1));
                    int x;

                    String charactername = op.character;
                    int y = FastForward.extractCharacterRPSize(FastForward.computeCharacterRPG(groundedoperators, next.Item2, this.goal, charactername), this.goal, groundedoperators, charactername);

                    Tuple<string, WorldState> res;
                    if (!WorldState.isExecutable(op, w))
                    {

                        NarrativePlanning.Operator failedop = NarrativePlanning.Operator.getFailedOperator(groundedoperators, op);
                        res = new Tuple<string, WorldState>(failedop.text, WorldState.getNextState(w, failedop));
                        p.steps.Add(res);
                        x = FastForward.extractRPSize(FastForward.computeRPG(groundedoperators, res.Item2, this.goal), this.goal, groundedoperators);
                    }
                    else
                    {
                        res = next;
                        p.steps.Add(next);
                        x = FastForward.extractRPSize(FastForward.computeRPG(groundedoperators, next.Item2, this.goal), this.goal, groundedoperators);
                    }

                    Console.Write("Possible apparent next step " + next.Item1 + ", but actual step " + res.Item1 + " with global hueristic of " + x + " and character heuristic of " + y + "\n");
                    if (y < min && y != -1)
                    {
                        best = res;
                        min = y;
                    }

                    if (res.Item2.isGoalState(this.goal))
                    {
                        //solution found!
                        current.steps.Add(res);
                        solutionPlan = current;
                        avg_branching_factor = avg_branching_factor / depth;
                        Console.Write("\n Number of nodes = " + nnodes
                                      + ", max branching factor = " + bfactor
                                      + ", avg branching factor = " + avg_branching_factor);
                        return solutionPlan;
                    }
                }


                Console.Write("STEP SELECTED: " + best.Item1 + "\n");
                Console.Write("----------\n");
                avg_branching_factor += tmp;
                if (tmp > bfactor)
                    bfactor = tmp;

                //add best node to plan
                current.steps.Add(best);
                if (current.steps[current.steps.Count - 1].Item2.isGoalState(this.goal))
                {
                    //solution found!
                    solutionPlan = current;
                    Console.Write("\n Number of nodes = " + nnodes + " and branching factor = " + bfactor);
                    return solutionPlan;
                }
                depth++;
            }
            return solutionPlan;
        }

        public PlanningProblem clone()
        {
            WorldState i = this.w0.clone();
            WorldState g = this.goal.clone();
            List<Operator> o = new List<Operator>();
            foreach (Operator oper in this.groundedoperators)
            {
                o.Add(oper.clone());
            }
            //List<String> gs = new List<string>();
            //foreach(String gop in this.gops){
            //    gs.Add(gop.Clone() as String);
            //}
            return new PlanningProblem(i, g, o);
        }
    }
}

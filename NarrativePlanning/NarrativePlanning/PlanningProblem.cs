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
        public List<Desire> desires;

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
        /// A Planning Problem consists of the initial state, the goal state
        /// and the operators possible.
        /// </summary>
        /// <param name="initial">initial WorldState</param>
        /// <param name="goal">Goal worldstate</param>
        /// <param name="operators">List of grounded operators</param>
        /// <param name="desires">List of desires</param>
        public PlanningProblem(WorldState initial, WorldState goal, List<Operator> operators, List<Desire> desires)
        {
            w0 = initial;
            this.goal = goal;
            this.groundedoperators = operators;
            this.desires = desires;
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
                List<Tuple<String, WorldState>> n = w.getPossibleApparentNextStatesTuples(groundedoperators);
                //check every node in the frontier    
                foreach (Tuple<String, WorldState> next in n)
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
                        Console.Write("\n Number of nodes = " + nnodes + " and branching factor = " + bfactor);
                        return solutionPlan;
                    }
                }

                if (n.Count == 0)
                    return null;

                Console.Write("STEP SELECTED: " + best.Item1 + "\n");
                Console.Write("----------\n");
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

        public Plan HeadSpaceXSolution()
        {
            List<Tuple<String, WorldState>> nextStateTuples = w0.getPossibleApparentNextStatesTuples(groundedoperators);
            int depth = 1;
            int bfactor = 0;
            int nnodes = 0;
            Plan solutionPlan = null;
            Plan current = new Plan(this);
            //Queue<Plan> queue = new Queue<Plan>();
            int min = -1;
            Tuple<String, WorldState> best = null;



            return solutionPlan;
        }

        public Plan HeadSpaceX(List<Desire> desires, Plan plan, WorldState goal, List<Operator> groundedoperators)
        {
            WorldState w = plan.steps[plan.steps.Count - 1].Item2;
            List<Intention> intentions = w.extractArisingIntentions(desires);
            foreach(Intention i in intentions)
            {
                if (!Intention.containsIntention(w.intentions, i))
                {
                    // create the plan for the intention frame
                    PlanningProblem pp = new PlanningProblem(w, Character.createCharacterGoal(i.goals, i.character), groundedoperators);
                    Plan p = pp.FFSolution();

                    // if no plan found do not add intention frame at all
                    if (p != null)
                    {
                        i.plan = p;
                        w.intentions.Add(i);
                        // add motivating step to plan
                        plan.steps.Add(new Tuple<string, WorldState>(i.getDescription(), w.clone()));
                    }

                }
                else
                {
                    //that intention is already present, add motivations to the intention frame instead.
                    foreach(Intention i2 in w.intentions)
                    {
                        if(i2.character.Equals(i.character) && i2.goals.Equals(i.goals))
                        {
                            foreach (Character m in i.motivations)
                                if (!i2.motivations.Contains(m))
                                    i2.motivations.Add(m);
                        }
                    }
                }
            }
            if (w.isGoalState(goal))
                return plan;
            List<Tuple<String, WorldState>> possibleSteps = new List<Tuple<string, WorldState>>();
            foreach(Intention intention in w.intentions)
            {
                //TODO: add next step from each plan in the intentions to possible next steps
            }
            if (possibleSteps.Count > 0)
            {
                //TODO: Come up with a better way to choose an action!! Future Work
                Tuple<String, WorldState> step = possibleSteps[0];
                // Ensure step is the failing step if not executable
                Tuple<string, WorldState> res;
                Operator op = Operator.getOperator(groundedoperators, step.Item1);
                if (!WorldState.isExecutable(op, w))
                {

                    NarrativePlanning.Operator failedop = NarrativePlanning.Operator.getFailedOperator(groundedoperators, op);
                    res = new Tuple<string, WorldState>(failedop.text, WorldState.getNextState(w, failedop));
                }
                else
                {
                    res = step;
                }
                plan.steps.Add(res);
                WorldState newState = res.Item2;
                WorldState tmp = res.Item2.clone();
                foreach (Intention intention in tmp.intentions)
                {
                    if(newState.getCharacter(intention.character).isGoalState(intention.goals)
                        || !newState.getCharacter(intention.character).hasMotivations(intention.motivations))
                    {
                        newState.intentions.Remove(intention);
                    }
                    else
                    {
                        //TODO: if a causal link in the plan for the intention is threatened, replace with new plan
                        // if no plan was found, then remove intention from newstate
                    }
                }
            }
            WorldState last = plan.steps[plan.steps.Count - 1].Item2;
            if (!last.isGoalState(goal) && last.intentions.Count == 0)
                return null; //FAIL
            return HeadSpaceX(desires, plan, goal, groundedoperators);
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

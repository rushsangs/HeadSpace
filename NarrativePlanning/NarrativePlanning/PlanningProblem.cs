using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace NarrativePlanning
{
    [Serializable]
    public class PlanningProblem
    {
        //public WorldFrame wf;
        public WorldState w0;
        public WorldState goal;
        public List<Operator> operators;
        public List<String> gops;

        public PlanningProblem(WorldState initial, WorldState goal, List<Operator> operators, List<String> groundOps)
        {
            w0 = initial;
            this.goal = goal;
            this.operators = operators;
            this.gops = groundOps;
        }


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

        public Plan BFSSolution(){
            List<Tuple<String, WorldState>> nextStateTuples = w0.getPossibleNextStatesTuples(operators, gops);
            int depth = 1;
            Plan solutionPlan = null;
            Queue<Plan> queue = new Queue<Plan>();
            while (solutionPlan==null)
            {
                if (depth == 1)
                {
                    foreach (Tuple<String, WorldState> next in w0.getPossibleNextStatesTuples(operators, gops))
                    {
                        Plan p = new Plan(this);
                        p.steps.Add(next);
                        queue.Enqueue(p);
                        if(next.Item2.isGoalState(this.goal))
                        {
                            //solution found!
                            solutionPlan = p;
                            break;
                        }

                    }
                }
                else
                {
                    while (queue.Peek().steps.Count == depth)
                    {
                        Plan p = queue.Dequeue();
                        WorldState w = p.steps[p.steps.Count - 1].Item2;
                        foreach (Tuple<String, WorldState> next in w.getPossibleNextStatesTuples(operators, gops))
                        {
                            Plan q = DeepCopy<Plan>(p);
                            q.steps.Add(next);
                            queue.Enqueue(q);
                            if (next.Item2.isGoalState(this.goal))
                            {
                                //solution found!
                                solutionPlan = q;
                                break;
                            }
                        }
                    }
                }
                depth++;
            }
            return solutionPlan;
        }

        public static T DeepCopy<T>(T other)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, other);
                ms.Position = 0;
                return (T)formatter.Deserialize(ms);
            }
        }
    }
}

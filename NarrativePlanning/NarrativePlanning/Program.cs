using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NarrativePlanning
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Stopwatch watch = new Stopwatch();
            watch.Start();

            // UNCOMMENT THIS IF YOU WANT TO RECREATE OR UPDATE DOMAIN
            //DomainBuilder.TypeTreeBuilder t = new DomainBuilder.TypeTreeBuilder();
            //DomainBuilder.InstanceAdder i = new DomainBuilder.InstanceAdder(t.root);
            //DomainBuilder.OperationBuilder opb = new DomainBuilder.OperationBuilder(t.root);
            //DomainBuilder.GroundGenerator gg = new DomainBuilder.GroundGenerator(t.root, opb.operators);
            //DomainBuilder.OperationBuilder.storeOperators(gg.grounds, opb.operators, "serialized-ops.txt");

            List<Operator> o = DomainBuilder.OperationBuilder.getStoredOperators("serialized-ops.txt");
            WorldState initial = DomainBuilder.StateCreator.getState("/Users/abc/Desktop/UoU/Research/HeadSpace/NarrativePlanning/NarrativePlanning/Text Files/beanstalk-initial.txt");
            WorldState goal = DomainBuilder.StateCreator.getState("/Users/abc/Desktop/UoU/Research/HeadSpace/NarrativePlanning/NarrativePlanning/Text Files/beanstalk-goal.txt");

            watch.Stop();
            Console.WriteLine("Time taken to prepare everything: " + watch.ElapsedMilliseconds + " milliseconds.");
            watch.Restart();
            PlanningProblem problem = new PlanningProblem(initial, goal, o);
            Console.Write(problem.FFSolution().toString());
            watch.Stop();
            Console.WriteLine("Complete, planning algorithm time = " + watch.ElapsedMilliseconds + " milliseconds.");
        }
    }
}

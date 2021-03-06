﻿using System;
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

            DomainBuilder.JSONDomainBuilder j = new DomainBuilder.JSONDomainBuilder("../../JSON Files/breakout.json");
            //List<NarrativePlanning.Operator> o = DomainBuilder.OperationBuilder.getStoredOperators("serialized-ops.txt");
            //WorldState initial = DomainBuilder.StateCreator.getState("../../Text Files/beanstalk-initial.txt");
            //WorldState goal = DomainBuilder.StateCreator.getState("../../Text Files/beanstalk-goal.txt");

            watch.Stop();
            Console.WriteLine("Time taken to prepare everything: " + watch.ElapsedMilliseconds + " milliseconds.");
            watch.Restart();
            PlanningProblem problem = new PlanningProblem(j.initial, j.goal, j.operators);
            Console.Write(problem.FFSolution().toString());
            watch.Stop();
            Console.WriteLine("Complete, planning algorithm time = " + watch.ElapsedMilliseconds + " milliseconds.");
        }
    }
}

using System;

namespace NarrativePlanning
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            DomainBuilder.TypeTreeBuilder t = new DomainBuilder.TypeTreeBuilder();
            DomainBuilder.InstanceAdder i = new DomainBuilder.InstanceAdder(t.root);
            DomainBuilder.OperationBuilder opb = new DomainBuilder.OperationBuilder(t.root);
            DomainBuilder.GroundGenerator gg = new DomainBuilder.GroundGenerator(t.root, opb.operators);
            WorldState initial = DomainBuilder.StateCreator.getState("/Users/abc/Desktop/UoU/Research/HeadSpace/NarrativePlanning/NarrativePlanning/Text Files/beanstalk-initial.txt");
            WorldState goal = DomainBuilder.StateCreator.getState("/Users/abc/Desktop/UoU/Research/HeadSpace/NarrativePlanning/NarrativePlanning/Text Files/beanstalk-goal.txt");

            PlanningProblem problem = new PlanningProblem(initial, goal, opb.operators, gg.grounds);
            Console.Write(problem.BFSSolution().toString());
            Console.WriteLine("Complete");
        }
    }
}

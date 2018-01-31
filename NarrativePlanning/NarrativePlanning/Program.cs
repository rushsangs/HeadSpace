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
        }
    }
}

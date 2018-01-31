using System;
namespace NarrativePlanning.DomainBuilder
{
    public class InstanceAdder
    {
        TypeNode tree;
        String file;
        public InstanceAdder(TypeNode tree)
        {
            this.tree = tree;
            file = "/Users/abc/Desktop/UoU/Research/HeadSpace/NarrativePlanning/NarrativePlanning/Text Files/instances.txt";
            addInstances();
        }
        public void addInstances(){
            String[] lines = readFile(file);
            foreach(String line in lines){
                tree.addInstance(getLeftTerm(line), getRightTerm(line));
            }
            Console.WriteLine("added instances!");
        }
        public String[] readFile(String file)
        {
            return System.IO.File.ReadAllLines(file);
        }

        public String getLeftTerm(String line)
        {
            return line.Substring(0, line.IndexOf("IS-A", StringComparison.CurrentCulture) - 1).Trim();
        }

        public String getRightTerm(String line)
        {
            int startIndex = line.IndexOf("IS-A", StringComparison.CurrentCulture) + 4;
            return line.Substring(startIndex, line.Length - startIndex).Trim();
        }
    }
}

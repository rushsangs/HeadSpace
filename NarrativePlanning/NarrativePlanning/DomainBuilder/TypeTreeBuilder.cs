using System;
namespace NarrativePlanning.DomainBuilder
{
    public class TypeTreeBuilder
    {
        String filename;
        public TypeNode root;
        public TypeTreeBuilder()
        {
            filename = "/Users/abc/Desktop/UoU/Research/HeadSpace/NarrativePlanning/NarrativePlanning/Text Files/types.txt";
            buildTypeTree();
        }

        public void buildTypeTree(){
            
            String[] lines = readFile(filename);
            root = new TypeNode(getRightTerm(lines[0]));
            root.addNode(getRightTerm(lines[0]), getLeftTerm(lines[0]));
            for (int i = 1; i < lines.Length; ++i){
                root.addNode(getRightTerm(lines[i]), getLeftTerm(lines[i]));
            }
            Console.WriteLine("done!");
        }

        public String[] readFile(String file){
            return System.IO.File.ReadAllLines(file);
        }

        public String getLeftTerm(String line){
            return line.Substring(0, line.IndexOf("IS-A", StringComparison.CurrentCulture)-1).Trim();
        }

        public String getRightTerm(String line){
            int startIndex = line.IndexOf("IS-A", StringComparison.CurrentCulture) + 4;
            return line.Substring(startIndex, line.Length - startIndex).Trim();
        }
    }
}

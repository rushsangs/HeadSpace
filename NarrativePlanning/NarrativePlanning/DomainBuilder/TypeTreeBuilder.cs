using System;
namespace NarrativePlanning.DomainBuilder
{
    public class TypeTreeBuilder
    {
        String filename;
        public TypeNode root;
        public TypeTreeBuilder()
        {
            filename = "/Users/abc/Desktop/UoU/Research/HeadSpace/NarrativePlanning/NarrativePlanning/Text Files/beanstalk-types.txt";
            buildTypeTree();
        }

        public void buildTypeTree(){
            
            String[] lines = readFile(filename);
            root = new TypeNode(getRightTerm(lines[0]));
            root.addNode(getRightTerm(lines[0]), getLeftTerm(lines[0]));
            for (int i = 1; i < lines.Length; ++i){
                root.addNode(getRightTerm(lines[i]), getLeftTerm(lines[i]));
            }
            UnityConsole.WriteLine("done!");
        }

        public static TypeNode buildTypeTree(JSONDomain.TypeElement[] types)
        {
            TypeNode root = new TypeNode(getRightTerm(types[0]));
            root.addNode(getRightTerm(types[0]), getLeftTerm(types[0]));
            for (int i = 1; i < types.Length; ++i)
            {
                root.addNode(getRightTerm(types[i]), getLeftTerm(types[i]));
            }
            return root;
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

        public static String getRightTerm(JSONDomain.TypeElement line)
        {
            return line.Type;
        }

        public static String getLeftTerm(JSONDomain.TypeElement line)
        {
            return line.Name;
        }
    }
}

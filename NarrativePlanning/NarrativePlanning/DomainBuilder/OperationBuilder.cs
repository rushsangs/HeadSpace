using System;
using System.Collections.Generic;

namespace NarrativePlanning.DomainBuilder
{
    public class OperationBuilder
    {
        String filename;
        TypeNode root;
        public List<Operator> operators;
        public OperationBuilder(TypeNode root)
        {
            filename = "/Users/abc/Desktop/UoU/Research/HeadSpace/NarrativePlanning/NarrativePlanning/Text Files/operators.txt";
            this.root = root;
            operators = new List<Operator>();
            parse();
        }
        public void parse(){
            String[] lines = readFile(filename);
            for (int i = 0; i < lines.Length;++i)
            {
                if(lines[i].Contains("{"))
                {
                    for (int j = i; i < lines.Length && j<lines.Length;++j){
                        if(lines[j].Contains("}"))
                        {
                            //basically create new array, with lines starting from i to j, including both
                            String[] op = new string[j - i + 1];
                            for (int x = i; x <= j; ++x){
                                op[x - i] = lines[x];
                            }
                            //ArraySegment<String> op = new ArraySegment<string>(lines, i, j - i);
                            parseOperator(op);
                            break;
                        }
                    }
                }
            }
            foreach(String line in lines){
                line.Trim();
            }
        }

        private void parseOperator(String[] op)
        {
            Operator newOperator = new Operator();
            newOperator.name = op[1].Trim();


            String l = op[2].Trim();
            if(!l.Equals("args:")){
                return;
            }

            //one or more args
            int i = 3;
            for ( ; i < op.Length; ++i){
                if (op[i].Trim().Contains("char:"))
                    break;
                parseArgument(op[i].Trim(), newOperator.args);
            }

            //bind char and loc
            newOperator.character = op[i].Trim().Substring(op[i].IndexOf("char:") + 5).Trim();
            ++i;
            newOperator.location = op[i].Trim().Substring(op[i].IndexOf("loc:") + 4).Trim();


            readLiterals(newOperator.preT, op, "pre-t");
            readLiterals(newOperator.preF, op, "pre-f");
            readLiterals(newOperator.effT, op, "eff-t");
            readLiterals(newOperator.effF, op, "eff-f");
            readLiterals(newOperator.preBPlus, op, "pre-bplus");
            readLiterals(newOperator.preBMinus, op, "pre-bminus");
            readLiterals(newOperator.preUnsure, op, "pre-u");
            readLiterals(newOperator.effBPlus, op, "eff-bplus");
            readLiterals(newOperator.effBMinus, op, "eff-bminus");
            readLiterals(newOperator.effUnsure, op, "eff-u");

            operators.Add(newOperator);
        }

        private void parseArgument(string v, Dictionary<string, TypeNode> args)
        {
            //get variable name
            String[] a = v.Split(' ');
            String variable = a[0].Substring(a[0].IndexOf('?'));

            String type = a[1].Trim(')').Trim();
            args.Add(variable, root.getSubTree(type));
            //root.addInstance(variable, type);
        }

        private void readLiterals(List<Literal> literals, string[] op, string v)
        {
            //search for index that contains v
            int index = -1;
            int i = 0;
            for (; i < op.Length; ++i){
                if (op[i].Contains(v)){
                    index = i+1;
                    break;
                }
            }
            if (index == -1)
            {
                Console.WriteLine("Some error has occured, cannot read the " + v + " in the operation file");
                return;
            }

            //starting with index, process literals, one on each line
            while(index<op.Length-1 && !op[index].Contains(":")){
                
                String[] terms = op[index].Trim('(', ')').Trim().Split(' ');

                Literal l = new Literal();
                l.relation = terms[0];
                for (int j = 1; j < terms.Length; ++j){
                    if (terms[j].Trim().Length > 0)
                        l.terms.Add(terms[j].Trim());
                }
                literals.Add(l);
                index++;
            }

        }

        public String[] readFile(String file)
        {
            return System.IO.File.ReadAllLines(file);
        }
    }
}

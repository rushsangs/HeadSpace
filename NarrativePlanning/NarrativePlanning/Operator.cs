using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class Operator
    {
        public String name, character, location;
        public Dictionary<String, TypeNode> args;
        public List<Literal> preT,
                            preF,
                            preBPlus,
                            preBMinus,
                            preUnsure,
                            effT,
                            effF,
                            effBPlus,
                            effBMinus,
                            effUnsure;

        public Operator()
        {
            args = new Dictionary<string, TypeNode>();
            preT = new List<Literal>();
            preF = new List<Literal>();
            effT = new List<Literal>();
            effF = new List<Literal>();
            preBPlus = new List<Literal>();
            preBMinus = new List<Literal>();
            preUnsure = new List<Literal>();
            effBPlus = new List<Literal>();
            effBMinus = new List<Literal>();
            effUnsure = new List<Literal>();
        }

        public static Operator getOperator(List<Operator> operators, String ground){
            Operator result = null;
            String[] words = ground.Split(' ');
            foreach(Operator op in operators){
                if(op.name.Equals(words[0])){
                    //found the operator object, now populate it.
                    Dictionary<String, TypeNode>.Enumerator dict =  op.args.GetEnumerator();

                    for (int i = 0; i < op.args.Count; ++i){
                        

                        dict.MoveNext();

                        //words[i+1] should be an instance of op.args[i] typenode
                        if(dict.Current.Value.getAllInstancesStrings().Contains(words[i+1]))
                        {
                            if (op.character.Equals(dict.Current.Key))
                                op.character = words[i + 1];
                            if (op.location.Equals(dict.Current.Key))
                                op.location = words[i + 1];
                            foreach (Literal l in op.preT)
                            {
                                for (int j = 0; j < l.terms.Count; ++j){
                                    if (dict.Current.Key.Equals(l.terms[j]))
                                        l.terms[j] = words[i + 1];
                                }
                            }
                            foreach (Literal l in op.preF)
                            {
                                for (int j = 0; j < l.terms.Count; ++j)
                                {
                                    if (dict.Current.Key.Equals(l.terms[j]))
                                        l.terms[j] = words[i + 1];
                                }
                            }
                            foreach (Literal l in op.preBPlus)
                            {
                                for (int j = 0; j < l.terms.Count; ++j)
                                {
                                    if (dict.Current.Key.Equals(l.terms[j]))
                                        l.terms[j] = words[i + 1];
                                }
                            }
                            foreach (Literal l in op.preBMinus)
                            {
                                for (int j = 0; j < l.terms.Count; ++j)
                                {
                                    if (dict.Current.Key.Equals(l.terms[j]))
                                        l.terms[j] = words[i + 1];
                                }
                            }
                            foreach (Literal l in op.preUnsure)
                            {
                                for (int j = 0; j < l.terms.Count; ++j)
                                {
                                    if (dict.Current.Key.Equals(l.terms[j]))
                                        l.terms[j] = words[i + 1];
                                }
                            }
                            foreach (Literal l in op.effT)
                            {
                                for (int j = 0; j < l.terms.Count; ++j)
                                {
                                    if (dict.Current.Key.Equals(l.terms[j]))
                                        l.terms[j] = words[i + 1];
                                }
                            }
                            foreach (Literal l in op.effF)
                            {
                                for (int j = 0; j < l.terms.Count; ++j)
                                {
                                    if (dict.Current.Key.Equals(l.terms[j]))
                                        l.terms[j] = words[i + 1];
                                }
                            }
                            foreach (Literal l in op.effBPlus)
                            {
                                for (int j = 0; j < l.terms.Count; ++j)
                                {
                                    if (dict.Current.Key.Equals(l.terms[j]))
                                        l.terms[j] = words[i + 1];
                                }
                            }
                            foreach (Literal l in op.effBMinus)
                            {
                                for (int j = 0; j < l.terms.Count; ++j)
                                {
                                    if (dict.Current.Key.Equals(l.terms[j]))
                                        l.terms[j] = words[i + 1];
                                }
                            }
                            foreach (Literal l in op.effUnsure)
                            {
                                for (int j = 0; j < l.terms.Count; ++j)
                                {
                                    if (dict.Current.Key.Equals(l.terms[j]))
                                        l.terms[j] = words[i + 1];
                                }
                            }
                            result = op;
                            break;
                        }
                        else{
                            Console.WriteLine("there seems to be an instance mismatch!");
                        }
                    }
                }
            }
            return result;
        }
    }
}

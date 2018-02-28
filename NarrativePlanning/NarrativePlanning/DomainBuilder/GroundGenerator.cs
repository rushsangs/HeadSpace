using System;
using System.Collections.Generic;

namespace NarrativePlanning.DomainBuilder
{
    public class GroundGenerator
    {
        String file;
        TypeNode root;
        List<Operator> operators;
        public List<String> grounds;
        public GroundGenerator(TypeNode root, List<Operator> operators)
        {
            file = "";
            this.root = root;
            this.operators = operators;
            grounds = generateGrounds();
        }
        public List<String> generateGrounds(){
            List<String> grounds = new List<string>();
            foreach(Operator o in operators){
                Dictionary<String, TypeNode>.Enumerator e = o.args.GetEnumerator();

                Queue<List<String>> queue = new Queue<List<string>>();
                //add the operator string to the queue
                List<String> op_list = new List<string>();
                op_list.Add(o.name);
                queue.Enqueue(op_list);
                int count = 1;
                e.MoveNext();
                do
                {
                    KeyValuePair<String, TypeNode> current = e.Current;
                    List<Instance> instances = current.Value.getAllInstances();

                    while(queue.Peek().Count==count){

                        //dequeue item, pair it with all instances, and enqueue
                        //while pairing, don't pair if the instance already contains item in list
                        List<String> item = queue.Dequeue();
                        foreach(Instance instance in instances){
                            List<String> new_item = new List<string>();
                            int ignore = 0;
                            foreach(String x in item){
                                if (x.Equals(instance.name))
                                    ignore = 1;
                                new_item.Add(x);
                            }
                            new_item.Add(instance.name);
                            if(ignore==0)
                                queue.Enqueue(new_item);
                        }
                    }
                    count++;
                } while (e.MoveNext());

                //simply print out the queue
                foreach(List<String> item in queue){
                    Console.WriteLine(prettyPrint(item));
                    grounds.Add(prettyPrint(item));
                }
            }
            return grounds;
        }

        private String prettyPrint(List<string> item)
        {
            String res="";
            foreach(String i in item){
                res = res + " " + i;
            }
            return res;
        }
    }


}

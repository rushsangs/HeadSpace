using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    /// <summary>
    /// This class is used as a data structure for the type heirarchies.
    /// </summary>
    [Serializable]
    public class TypeNode
    {
        public String name;
        public List<Instance> instances;
        public List<TypeNode> children;

        public TypeNode(String name)
        {
            this.name = name;
            children = new List<TypeNode>();
            instances = new List<Instance>();
        }

        public void addNode(String parent, String child){
            TypeNode n = getSubTree(parent);
            TypeNode newNode = new TypeNode(child);
            UnityConsole.WriteLine("Will attempt to add " + child + " to " + parent);
            addNode(newNode, n);
            UnityConsole.WriteLine("Added " + child + " to " + parent);
        }

        public void addNode(TypeNode child, TypeNode parent)
        {
            parent.children.Add(child);
        }

        public void addInstance(String instance, String parent){
            Instance i = new Instance(instance);
            getSubTree(parent).addInstance(i);
        }

        public void addInstance(Instance i){
            this.instances.Add(i);
        }

        public TypeNode getSubTree(String name){
            if (this.name.Equals(name))
                return this;   
            else if (this.children.Count > 0)
            {
                foreach (TypeNode child in this.children)
                {
                    TypeNode x = child.getSubTree(name);
                    if (x == null)
                        continue;
                    else return x;
                }
            }
            return null;
        }

        public TypeNode getSubTree(TypeNode node)
        {
            if (this.Equals(node))
                return this;
            else if(this.children.Count>0)
            {
                foreach(TypeNode child in this.children){
                    TypeNode x = child.getSubTree(node);
                    if (x == null)
                        return null;
                    else return x;
                }
            }
            return null;
        }


        public List<Instance> getAllInstances(){
            List<Instance> res = new List<Instance>();
            res.AddRange(this.instances.AsReadOnly());
            foreach(TypeNode child in this.children){
                res.AddRange(child.getAllInstances().AsReadOnly());
            }
            return res;
        }

        public List<String> getAllInstancesStrings()
        {
            List<String> res = new List<String>();
            foreach(Instance i in this.instances){
                res.Add(i.name);
            }
            foreach (TypeNode child in this.children)
            {
                res.AddRange(child.getAllInstancesStrings().AsReadOnly());
            }
            return res;
        }

        public bool containsInstance(String instancename){
            List<Instance> instances = this.getAllInstances();
            foreach(Instance i in instances){
                if (i.name.Equals(instancename))
                    return true;
            }
            return false;
        }
    }
}

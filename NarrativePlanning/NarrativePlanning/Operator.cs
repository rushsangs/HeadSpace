using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace NarrativePlanning
{
    [Serializable]
    public class Operator : ISerializable
    {
        public String name, character, location;
        public String text;
        public Dictionary<String, TypeNode> args;
        public Hashtable preT
        {
            get;
            set;
        }
        public Hashtable preF
        {
            get;
            set;
        }
        public Hashtable effT
        {
            get;
            set;
        }
        public Hashtable effF
        {
            get;
            set;
        }
        public Hashtable preBPlus
        {
            get;
            set;
        }
        public Hashtable preBMinus
        {
            get;
            set;
        }
        public Hashtable preUnsure
        {
            get;
            set;
        }
        public Hashtable effBPlus
        {
            get;
            set;
        }
        public Hashtable effBMinus
        {
            get;
            set;
        }
        public Hashtable effUnsure
        {
            get;
            set;
        }
        public Hashtable privateEffects
        {
            get;
            set;
        }

        //public List<Literal> preT,
                            //preF,
                            //preBPlus,
                            //preBMinus,
                            //preUnsure,
                            //effT,
                            //effF,
                            //effBPlus,
                            //effBMinus,
                            //effUnsure;

        public Operator()
        {
            args = new Dictionary<string, TypeNode>();
            preT = new Hashtable();
            preF = new Hashtable();
            effT = new Hashtable();
            effF = new Hashtable();
            preBPlus = new Hashtable();
            preBMinus = new Hashtable();
            preUnsure = new Hashtable();
            effBPlus = new Hashtable();
            effBMinus = new Hashtable();
            effUnsure = new Hashtable();
            privateEffects = new Hashtable();
        }

        public Operator(SerializationInfo info, StreamingContext context){
            name = Convert.ToString(info.GetValue("name", typeof(String)));
            character = Convert.ToString(info.GetValue("character", typeof(String)));
            location = Convert.ToString(info.GetValue("location", typeof(String)));
            text = Convert.ToString(info.GetValue("text", typeof(String)));
            args = info.GetValue("args", typeof(Dictionary<String, TypeNode>)) as Dictionary<String, TypeNode>;

            preT = info.GetValue("preT", typeof(Hashtable)) as Hashtable;
            preF = info.GetValue("preF", typeof(Hashtable)) as Hashtable;
            effT = info.GetValue("effT", typeof(Hashtable)) as Hashtable;
            effF = info.GetValue("effF", typeof(Hashtable)) as Hashtable;
            preBPlus = info.GetValue("preBPlus", typeof(Hashtable)) as Hashtable;
            preBMinus = info.GetValue("preBMinus", typeof(Hashtable)) as Hashtable;
            preUnsure = info.GetValue("preUnsure", typeof(Hashtable)) as Hashtable;
            effBPlus = info.GetValue("effBPlus", typeof(Hashtable)) as Hashtable;
            effBMinus = info.GetValue("effBMinus", typeof(Hashtable)) as Hashtable;
            effUnsure = info.GetValue("effUnsure", typeof(Hashtable)) as Hashtable;
            privateEffects = info.GetValue("privateEffects", typeof(Hashtable)) as Hashtable;

        }

        //DO NOT GIVE THE GROUNDED VERSIONS!
        public static Operator getOperator(List<Operator> operators, String ground){
            Operator op = null;
            String[] words = ground.Trim().Split(' ');
            foreach(Operator oper in operators){
                if(oper.name.Equals(words[0])){
                    //found the operator object, now populate it.
                    op = oper.clone();
                    op.text = ground.Trim();
                    Dictionary<String, TypeNode>.Enumerator dict = op.args.GetEnumerator();
                    for (int i = 0; i < op.args.Count; ++i){
                        

                        dict.MoveNext();

                        //words[i+1] should be an instance of op.args[i] typenode
                        if(dict.Current.Value.getAllInstancesStrings().Contains(words[i+1]))
                            {
                            if (op.character.Equals(dict.Current.Key))
                                op.character = words[i + 1];
                            if (op.location.Equals(dict.Current.Key))
                                op.location = words[i + 1];
                            Hashtable tmp = (Hashtable)op.preT.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j){
                                    if (dict.Current.Key.Equals(terms[j])){
                                        builder.Replace(terms[j], words[i+1]);
                                    }
                                }
                                op.preT.Remove(l);
                                op.preT.Add(builder.ToString(), 1);
                            }
                            tmp = (Hashtable)op.preF.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j)
                                {
                                    if (dict.Current.Key.Equals(terms[j]))
                                    {
                                        builder.Replace(terms[j], words[i + 1]);
                                    }
                                }
                                op.preF.Remove(l);
                                op.preF.Add(builder.ToString(), 1);
                            }
                            tmp = (Hashtable)op.preBPlus.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j)
                                {
                                    if (dict.Current.Key.Equals(terms[j]))
                                    {
                                        builder.Replace(terms[j], words[i + 1]);
                                    }
                                }
                                op.preBPlus.Remove(l);
                                op.preBPlus.Add(builder.ToString(), 1);
                            }
                            tmp = (Hashtable)op.preBMinus.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j)
                                {
                                    if (dict.Current.Key.Equals(terms[j]))
                                    {
                                        builder.Replace(terms[j], words[i + 1]);
                                    }
                                }
                                op.preBMinus.Remove(l);
                                op.preBMinus.Add(builder.ToString(), 1);
                            }
                            tmp = (Hashtable)op.preUnsure.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j)
                                {
                                    if (dict.Current.Key.Equals(terms[j]))
                                    {
                                        builder.Replace(terms[j], words[i + 1]);
                                    }
                                }
                                op.preUnsure.Remove(l);
                                op.preUnsure.Add(builder.ToString(), 1);
                            }
                            tmp = (Hashtable)op.effT.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j)
                                {
                                    if (dict.Current.Key.Equals(terms[j]))
                                    {
                                        builder.Replace(terms[j], words[i + 1]);
                                    }
                                }
                                op.effT.Remove(l);
                                op.effT.Add(builder.ToString(), 1);
                            }
                            tmp = (Hashtable)op.effF.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j)
                                {
                                    if (dict.Current.Key.Equals(terms[j]))
                                    {
                                        builder.Replace(terms[j], words[i + 1]);
                                    }
                                }
                                op.effF.Remove(l);
                                op.effF.Add(builder.ToString(), 1);
                            }
                            tmp = (Hashtable)op.effBPlus.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j)
                                {
                                    if (dict.Current.Key.Equals(terms[j]))
                                    {
                                        builder.Replace(terms[j], words[i + 1]);
                                    }
                                }
                                op.effBPlus.Remove(l);
                                op.effBPlus.Add(builder.ToString(), 1);
                            }
                            tmp = (Hashtable)op.effBMinus.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j)
                                {
                                    if (dict.Current.Key.Equals(terms[j]))
                                    {
                                        builder.Replace(terms[j], words[i + 1]);
                                    }
                                }
                                op.effBMinus.Remove(l);
                                op.effBMinus.Add(builder.ToString(), 1);
                            }
                            tmp = (Hashtable)op.effUnsure.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j)
                                {
                                    if (dict.Current.Key.Equals(terms[j]))
                                    {
                                        builder.Replace(terms[j], words[i + 1]);
                                    }
                                }
                                op.effUnsure.Remove(l);
                                op.effUnsure.Add(builder.ToString(), 1);
                            }
                            tmp = (Hashtable)op.privateEffects.Clone();
                            foreach (String l in tmp.Keys)
                            {
                                String[] terms = l.Split(' ');
                                StringBuilder builder = new StringBuilder(l);
                                for (int j = 0; j < terms.Length; ++j)
                                {
                                    if (dict.Current.Key.Equals(terms[j]))
                                    {
                                        builder.Replace(terms[j], words[i + 1]);
                                    }
                                }
                                op.privateEffects.Remove(l);
                                op.privateEffects.Add(builder.ToString(), 1);
                            }
                        }
                        else{
                            Console.WriteLine("there seems to be an instance mismatch!");
                        }
                    }
                    return op;
                }
            }
           
            return op;
        }

        public static Operator getFailedOperator(List<Operator> operators, Operator trueop)
        {
            String n = trueop.name.Substring(0, trueop.name.IndexOf("true") - 1);
            n = n + "-false";
            foreach(Operator o in operators)
            {
                if (o.name.Equals(n))
                {
                    String a = o.text.Substring(o.text.IndexOf(" ") + 1);
                    String b = trueop.text.Substring(trueop.text.IndexOf(" ") + 1);
                    if (a.Equals(b))
                        return o;
                }
            }
            throw new Exception("Operator not found!!");
        }

        //public static T DeepCopy<T>(T other)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {
        //        BinaryFormatter formatter = new BinaryFormatter();
        //        formatter.Serialize(ms, other);
        //        ms.Position = 0;
        //        return (T)formatter.Deserialize(ms);
        //    }
        //}

        public Operator clone(){
            Operator o = new Operator();
            o.name = this.name.Clone() as String;
            o.character = this.character.Clone() as String;
            o.location = this.location.Clone() as String;
            o.text = this.text.Clone() as String;
            o.args = this.args;
            o.preT = this.preT.Clone() as Hashtable;
            o.preF = this.preF.Clone() as Hashtable;
            o.effT = this.effT.Clone() as Hashtable;
            o.effF = this.effF.Clone() as Hashtable;
            o.preBPlus = this.preBPlus.Clone() as Hashtable;
            o.preBMinus = this.preBMinus.Clone() as Hashtable;
            o.preUnsure = this.preUnsure.Clone() as Hashtable;
            o.effBPlus = this.effBPlus.Clone() as Hashtable;
            o.effBMinus = this.effBMinus.Clone() as Hashtable;
            o.effUnsure = this.effUnsure.Clone() as Hashtable;
            o.privateEffects = this.privateEffects.Clone() as Hashtable;
            return o;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("name", name);
            info.AddValue("character", character);
            info.AddValue("location", location);
            info.AddValue("text", text);
            info.AddValue("args", args);
            info.AddValue("preT", preT);
            info.AddValue("preF", preF);
            info.AddValue("effT", effT);
            info.AddValue("effF", effF);
            info.AddValue("preBPlus", preBPlus);
            info.AddValue("preBMinus", preBMinus);
            info.AddValue("preUnsure", preUnsure);
            info.AddValue("effBPlus", effBPlus);
            info.AddValue("effBMinus", effBMinus);
            info.AddValue("effUnsure", effUnsure);
            info.AddValue("privateEffects", privateEffects);
        }
    }
}

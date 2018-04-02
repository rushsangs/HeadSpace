using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    [Serializable]
    public class Literal
    {
        public String relation
        {
            get;
            set;
        }
        public List<String> terms
        {
            get;
            set;
        }
        public Literal()
        {
            terms = new List<string>();
        }

        public override bool Equals(object obj){
            Literal ot = obj as Literal;
            if (ot == null)
                return false;
            if (!ot.relation.Equals(this.relation))
                return false;
            if (ot.terms.Count != this.terms.Count)
                return false;
            for (int i = 0; i < ot.terms.Count; ++i)
            {
                if (!ot.terms[i].Equals(this.terms[i]))
                    return false;
            }
            return true;
        }

        public override int GetHashCode()
        {
            int res = 0;
            //res += relation.GetHashCode();
            //foreach(string term in terms){
            //    res += term.GetHashCode();
            //}
            return res;
        }

    }
}

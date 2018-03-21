using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    [Serializable]
    public class Literal
    {
        public String relation;
        public List<String> terms;
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

    }
}

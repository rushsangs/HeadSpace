using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class Literal
    {
        public String relation;
        public List<String> terms;
        public Literal()
        {
            terms = new List<string>();
        }
    }
}

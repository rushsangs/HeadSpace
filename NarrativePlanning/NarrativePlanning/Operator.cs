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
    }
}

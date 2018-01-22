using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
    public class GroundOperator
    {
        Character c;
        public List<GroundLiteral> preT, 
                            preF,
                            preBPlus,
                            preBMinus,
                            preUnsure, 
                            effT,
                            effF,
                            effBPlus,
                            effBMinus, 
                            effUnsure;


        public GroundOperator()
        {
        }
    }
}

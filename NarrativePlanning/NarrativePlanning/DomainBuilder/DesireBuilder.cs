using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarrativePlanning.DomainBuilder
{
    class DesireBuilder
    {
        
        public static List<NarrativePlanning.Desire> parseDesires(JSONDomain.Desire[] desires)
        {
            List<NarrativePlanning.Desire> res = new List<Desire>();
            foreach(JSONDomain.Desire des in desires)
            {
                NarrativePlanning.Desire d = new NarrativePlanning.Desire();
                d.character = des.Character;
                foreach(String lit in des.Motivations.Bplus)
                {
                    String l = lit.Trim().Trim('(', ')').Trim();
                    d.motivations.bPlus.Add(l, 1);
                }
                foreach (String lit in des.Motivations.Bminus)
                {
                    String l = lit.Trim().Trim('(', ')').Trim();
                    d.motivations.bMinus.Add(l, 1);
                }
                foreach (String lit in des.Motivations.Unsure)
                {
                    String l = lit.Trim().Trim('(', ')').Trim();
                    d.motivations.unsure.Add(l, 1);
                }

                if (des.Goal.Bplus.Length > 0)
                {
                    String l = des.Goal.Bplus[0].Trim().Trim('(', ')').Trim();
                    d.goals.bPlus.Add(l, 1);
                }
                if (des.Goal.Bminus.Length > 0)
                {
                    String l = des.Goal.Bminus[0].Trim().Trim('(', ')').Trim();
                    d.goals.bMinus.Add(l, 1);
                }
                if (des.Goal.Unsure.Length > 0)
                {
                    String l = des.Goal.Unsure[0].Trim().Trim('(', ')').Trim();
                    d.goals.unsure.Add(l, 1);
                }
                res.Add(d);
            }

            return res;
        }
    }
}

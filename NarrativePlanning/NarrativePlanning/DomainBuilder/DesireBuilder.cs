using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarrativePlanning.DomainBuilder
{
    class DesireBuilder
    {
        public List<NarrativePlanning.Desire> desires;
        public static List<NarrativePlanning.Desire> parseDesires(JSONDomain.Desire[] desires)
        {
            List<NarrativePlanning.Desire> res = new List<Desire>();
            foreach(JSONDomain.Desire des in desires)
            {
                NarrativePlanning.Desire d = new NarrativePlanning.Desire();
                d.character = des.Character;
                foreach(String lit in des.Motivations.Bplus)
                {
                    d.motivations.bPlus.Add(lit, 1);
                }
                foreach (String lit in des.Motivations.Bminus)
                {
                    d.motivations.bMinus.Add(lit, 1);
                }
                foreach (String lit in des.Motivations.Unsure)
                {
                    d.motivations.unsure.Add(lit, 1);
                }

                if (des.Goal.Bplus.Length > 0)
                    d.goals.bPlus.Add(des.Goal.Bplus[0], 1);
                if (des.Goal.Bminus.Length > 0)
                    d.goals.bMinus.Add(des.Goal.Bminus[0], 1);
                if (des.Goal.Unsure.Length > 0)
                    d.goals.unsure.Add(des.Goal.Unsure[0], 1);

                res.Add(d);
            }

            return res;
        }
    }
}

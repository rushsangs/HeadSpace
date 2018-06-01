using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NarrativePlanning
{
    class Desire
    {
        /// <summary>
        /// The Character asically stores tyhe name and ONE literal 
        /// in either the B+, B- or Unsure.
        /// </summary>
        Character character
        {
            get;
            set;
        }
        List<String> motivations
        {
            get;
            set;
        }
    }
}

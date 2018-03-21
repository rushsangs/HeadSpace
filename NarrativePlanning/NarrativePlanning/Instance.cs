using System;

namespace NarrativePlanning
{
    [Serializable]
    public class Instance
    {
        public String name;
        public Instance(String name)
        {
            this.name = name;
        }
    }
}
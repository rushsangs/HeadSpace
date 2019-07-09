using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
	[Serializable]
    public class EffectTuple
    {
		public String effect;

		public List<ObservabilityRule> observabilityrules
		{
			get;
			set;
		}

		public EffectTuple(String effect, List<ObservabilityRule> observabilityrules)
        {
			this.effect = effect;
			this.observabilityrules = observabilityrules;
        }
    }

	[Serializable]
    public class ObservabilityRule
	{
		public String fName;
		public List<String> args;
        public ObservabilityRule(string fName, List<string> args)
		{
			this.fName = fName;
			this.args = args;
		}
	}
}

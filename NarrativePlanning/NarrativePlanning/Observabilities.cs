using System;
using System.Collections.Generic;

namespace NarrativePlanning
{
	public class Observabilities
	{
		public Observabilities()
		{
		}
        
		public List<Character> publicObs (WorldState world, List<String> args)
		{
			//this applies the effect to ALL characters in the world
			List<Character> characters = new List<Character>(world.characters);
            
			return characters;
		}

		public List<Character> localObs (WorldState world, List<String> args)
		{
			//local(?location) where ?location should be a location
			//args should have the location instance as first argument
			//literal to be checked will be (at character ?location)
			List<Character> res = new List<Character>();
			foreach(Character c in world.characters)
			{
				String lit = "at " + c.name + " " + args[0].Trim();
				if (world.tWorld.ContainsKey(lit))
					res.Add(c);

			}
			return res;
		}
    }
}

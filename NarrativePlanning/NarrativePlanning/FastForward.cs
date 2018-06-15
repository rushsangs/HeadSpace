using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NarrativePlanning
{
    public class FastForward
    {
        public class Layers
        {
            public Layers(){
                F = new Hashtable();
                A = new Hashtable();
            }

            public Hashtable F {
                get;
                set;
            }
            public Hashtable A {
                get;
                set;
            }
            public int k {
                get;
                set;
            }

            //public void add(Hashtable table, int t, Literal l){
            //    if (table.ContainsKey(t))
            //        table.Add(t, new Hashtable());
            //    table.Add(t, );
            //}
        }

        public FastForward()
        {
        }

        /// <summary>
        /// Accessor function for accessing the right hashtable in the object.
        /// </summary>
        /// <param name="obj">Basically takes a character or worldstate object</param>
        /// <returns>Should return the correct hashtable</returns>
        delegate Hashtable del(Object obj);

        delegate List<Operator> del2(Object obj);

        /// <summary>
        /// Computes the relaxed plan graph for a given input
        /// </summary>
        /// <param name="operators">Grounded operators</param>
        /// <param name="initial">Initial world state</param>
        /// <param name="goal">Goal worldstate</param>
        /// <returns>Returns the RPG in a Layers form.</returns>
        public static Layers computeRPG(List<Operator> operators, WorldState initial, WorldState goal){
            Layers l = null;
            int t = 0;
            l = new Layers();
            l.F.Add(0, initial);

            while(!((WorldState)l.F[t]).isGoalState(goal)){
                t++;
                List<Operator> At = new List<Operator>();
                foreach(Operator o in operators){
                    if(WorldState.isExecutable(o, ((WorldState)l.F[t-1]))){
                        At.Add(o);
                    }
                }
                l.A.Add(t, At);
                l.F.Add(t, ((WorldState)l.F[t-1]).clone());
                foreach(Operator o in At){
                    l.F[t] = WorldState.getNextRelaxedState(((WorldState)l.F[t]), o);
                }
                if ((l.F[t] as WorldState).HasChangedFrom(l.F[t - 1] as WorldState)){
                    l.k = t;
                    return l;
                }
                    
                
            }
            l.k = t;
            return l;
        }

        /// <summary>
        /// Computes the relaxed plan graph but for the character 
        /// and not the world
        /// </summary>
        /// <param name="operators">Lisat of grounded operators</param>
        /// <param name="initial">Initial worldstate</param>
        /// <param name="goal">Goal worldstate</param>
        /// <param name="charactername">Character to compute the RPG for.</param>
        /// <returns>The RPG in layers form constructed using acitons that the character can perform.</returns>
        public static Layers computeCharacterRPG(List<Operator> operators, WorldState initial, WorldState goal, String charactername)
        {
            Character characteri = initial.characters.Find(x => x.name.Equals(charactername));
            Layers l = null;
            int t = 0;
            l = new Layers();
            l.F.Add(0, characteri);

            Character characterf = goal.characters.Find(x => x.name.Equals(charactername));
            if(characterf == null)
            {
                //the character had no goal.
                l.k = t;
                return l;
            }

            while (!((Character)l.F[t]).isGoalState(characterf))
            {
                t++;
                List<Operator> At = new List<Operator>();
                foreach (Operator o in operators)
                {
                    if (o.character.Equals(charactername) && Character.isApparentlyExecutable(o, ((Character)l.F[t - 1])))
                    {
                        At.Add(o);
                    }
                }
                l.A.Add(t, At);
                l.F.Add(t, ((Character)l.F[t - 1]).clone());
                foreach (Operator o in At)
                {
                    l.F[t] = Character.getNextRelaxedState(((Character)l.F[t]), o);
                }
                if ((l.F[t] as Character).Equals(l.F[t - 1] as Character))
                {
                    //could not reach goal.
                    l.k = t;
                    return l;
                }


            }
            l.k = t;
            return l;
        }

        /// <summary>
        /// Finds the hueristic measure for the RPG.
        /// </summary>
        /// <param name="l">The layers</param>
        /// <param name="g">Goal worldstate</param>
        /// <param name="operators">List of grounded operators</param>
        /// <returns> A heuristic number, -1 for failure. Lower number is better.</returns>
        public static int extractRPSize(Layers l, WorldState g, List<Operator> operators){
            int selectedActions = 0;

            if(!((WorldState)l.F[l.k]).isGoalState(g))
            {
                //if (l.k == 0)
                //    return 0;
                //else
                    return -1;
            }
            List<int> firstlevels = new List<int>();
            foreach(String lit in g.tWorld.Keys){
                firstlevels.Add(firstLevel(lit, l.F, (obj) => ((WorldState)obj).tWorld));
            }
            foreach (String lit in g.fWorld.Keys)
            {
                firstlevels.Add(firstLevel(lit, l.F, (obj) => ((WorldState)obj).fWorld));
            }
            //character states ignored!!
            int m = firstlevels.Max();

            Hashtable Gt = new Hashtable();
            for (int t = 0; t <= m; ++t){
                WorldState w = new WorldState(new Hashtable(), new Hashtable(), null);
                foreach(String lit in g.tWorld.Keys){
                    if(firstLevel(lit, l.F, (obj) => ((WorldState)obj).tWorld) == t){
                        w.tWorld.Add(lit, 1);
                    }
                }
                foreach (String lit in g.fWorld.Keys)
                {
                    if (firstLevel(lit, l.F, (obj) => ((WorldState)obj).fWorld) == t)
                    {
                        w.fWorld.Add(lit, 1);
                    }
                }
                Gt.Add(t, w);
            }

            for (int t = m; t >= 1; --t){
                foreach(String lit in (Gt[t] as WorldState).tWorld.Keys){
                    foreach(Operator o in operators){
                        //check if effects of action have literal as a component
                        if(o.effT.Contains(lit) || o.preT.Contains(lit)){
                            //check if that action appeared first time at t
                            if(firstLevel(o, l.A, ((obj) => (obj as List<Operator>))) == t){
                                selectedActions++;
                                //now add all of its preconditions as subgoals in Gts
                                foreach(String prelit in o.preT.Keys){
                                    int level = firstLevel(prelit, l.F, (obj) => ((WorldState)obj).tWorld);
                                    if (level == -1 || level>= t)
                                        level = t - 1;
                                    if (!(Gt[level] as WorldState).tWorld.Contains(prelit))
                                        (Gt[level] as WorldState).tWorld.Add(prelit, 1);
                                }
                                foreach (String prelit in o.preF.Keys)
                                {
                                    int level = firstLevel(prelit, l.F, (obj) => ((WorldState)obj).fWorld);
                                    if (level == -1 || level >= t)
                                        level = t - 1;
                                    if (!(Gt[level] as WorldState).fWorld.Contains(prelit))
                                        (Gt[level] as WorldState).fWorld.Add(prelit, 1);
                                }
                            }
                        }
                    }
                }
                foreach (String lit in (Gt[t] as WorldState).fWorld.Keys)
                {
                    foreach (Operator o in operators)
                    {
                        //check if effects of action have literal as a component
                        if (o.effF.Contains(lit) || o.preF.Contains(lit))
                        {
                            //check if that action appeared first time at t
                            if (firstLevel(o, l.A, ((obj) => (obj as List<Operator>))) == t)
                            {
                                selectedActions++;
                                //now add all of its preconditions as subgoals in Gts
                                foreach (String prelit in o.preT.Keys)
                                {
                                    int level = firstLevel(prelit, l.F, (obj) => ((WorldState)obj).tWorld);
                                    if (level == -1 || level >= t)
                                        level = t - 1;
                                    if(!(Gt[level] as WorldState).tWorld.Contains(prelit))
                                        (Gt[level] as WorldState).tWorld.Add(prelit, 1);
                                }
                                foreach (String prelit in o.preF.Keys)
                                {
                                    int level = firstLevel(prelit, l.F, (obj) => ((WorldState)obj).fWorld);
                                    if (level == -1 || level >= t)
                                        level = t - 1;
                                    if (!(Gt[level] as WorldState).fWorld.Contains(prelit))
                                        (Gt[level] as WorldState).fWorld.Add(prelit, 1);
                                }
                                //break;
                            }
                        }
                    }
                }
            }
            return selectedActions;
        }

        /// <summary>
        /// Finds the hueristic measure for the character RPG
        /// </summary>
        /// <param name="l">Layers</param>
        /// <param name="g">Goal worldstate</param>
        /// <param name="operators">List of grounded operators</param>
        /// <param name="charactername"> The character name</param>
        /// <returns> A heuristic number, -1 for failure. Lower number is better.</returns>
        public static int extractCharacterRPSize(Layers l, WorldState g, List<Operator> operators, String charactername)
		{
			int selectedActions = 0;
			Character characterg = g.characters.Find(x => x.name.Equals(charactername));
            if(characterg == null)
            {
                //no goal exists for the character
                return -1;
            }
			if (!((Character)l.F[l.k]).isGoalState(characterg))
			{
				//if (l.k == 0)
				//    return 0;
				//else
				return -1;
			}
            if(l.k==0)
            {
                return 0;
            }
			List<int> firstlevels = new List<int>();
			foreach (String lit in characterg.bPlus.Keys)
			{
				firstlevels.Add(firstLevel(lit, l.F, (obj) => ((Character)obj).bPlus));
			}
			foreach (String lit in characterg.bMinus.Keys)
			{
				firstlevels.Add(firstLevel(lit, l.F, (obj) => ((Character)obj).bMinus));
			}
			foreach (String lit in characterg.unsure.Keys)
			{
				firstlevels.Add(firstLevel(lit, l.F, (obj) => ((Character)obj).unsure));
			}

			int m = firstlevels.Max();

			Hashtable Gt = new Hashtable();
			for (int t = 0; t <= m; ++t)
			{
				Character newCharacter = new Character(new Hashtable(), new Hashtable(), new Hashtable());
				foreach (String lit in characterg.bPlus.Keys)
				{
					if (firstLevel(lit, l.F, (obj) => ((Character)obj).bPlus) == t)
					{
						newCharacter.bPlus.Add(lit, 1);
					}
				}
				foreach (String lit in characterg.bMinus.Keys)
				{
					if (firstLevel(lit, l.F, (obj) => ((Character)obj).bMinus) == t)
					{
						newCharacter.bMinus.Add(lit, 1);
					}
				}
				foreach (String lit in characterg.unsure.Keys)
				{
					if (firstLevel(lit, l.F, (obj) => ((Character)obj).unsure) == t)
					{
						newCharacter.unsure.Add(lit, 1);
					}
				}
				Gt.Add(t, newCharacter);
			}

			for (int t = m; t >= 1; --t)
			{
				foreach (String lit in (Gt[t] as Character).bPlus.Keys)
				{
					foreach (Operator o in operators)
					{
						//ensure character is the one performing the operation
						if (o.character.Equals(charactername))
						{
							//check if perceived positive effects of action have literal as a component
							if (o.effBPlus.Contains(lit))
							{
								//check if that action appeared first time at t
								if (firstLevel(o, l.A, ((obj) => (obj as List<Operator>))) == t)
								{
									selectedActions++;
									//now add all of its preconditions as subgoals in Gts
									foreach (String prelit in o.preBPlus.Keys)
									{
										int level = firstLevel(prelit, l.F, (obj) => ((Character)obj).bPlus);
										if (level == -1 || level >= t)
											level = t - 1;
										if (!(Gt[level] as Character).bPlus.Contains(prelit))
											(Gt[level] as Character).bPlus.Add(prelit, 1);
									}
									foreach (String prelit in o.preBMinus.Keys)
									{
										int level = firstLevel(prelit, l.F, (obj) => ((Character)obj).bMinus);
										if (level == -1 || level >= t)
											level = t - 1;
										if (!(Gt[level] as Character).bMinus.Contains(prelit))
											(Gt[level] as Character).bMinus.Add(prelit, 1);
									}
									foreach (String prelit in o.preUnsure.Keys)
									{
										int level = firstLevel(prelit, l.F, (obj) => ((Character)obj).unsure);
										if (level == -1 || level >= t)
											level = t - 1;
										if (!(Gt[level] as Character).unsure.Contains(prelit))
											(Gt[level] as Character).unsure.Add(prelit, 1);
									}
								}
							}
						}
					}
				}
				foreach (String lit in (Gt[t] as Character).bMinus.Keys)
				{
					foreach (Operator o in operators)
					{
						//ensure character is the one performing the operation
						if (o.character.Equals(charactername))
						{
							//check if perceived negative effects of action have literal as a component
							if (o.effBMinus.Contains(lit))
							{
								//check if that action appeared first time at t
								if (firstLevel(o, l.A, ((obj) => (obj as List<Operator>))) == t)
								{
									selectedActions++;
									//now add all of its preconditions as subgoals in Gts
									foreach (String prelit in o.preBPlus.Keys)
									{
										int level = firstLevel(prelit, l.F, (obj) => ((Character)obj).bPlus);
										if (level == -1 || level >= t)
											level = t - 1;
										if (!(Gt[level] as Character).bPlus.Contains(prelit))
											(Gt[level] as Character).bPlus.Add(prelit, 1);
									}
									foreach (String prelit in o.preBMinus.Keys)
									{
										int level = firstLevel(prelit, l.F, (obj) => ((Character)obj).bMinus);
										if (level == -1 || level >= t)
											level = t - 1;
										if (!(Gt[level] as Character).bMinus.Contains(prelit))
											(Gt[level] as Character).bMinus.Add(prelit, 1);
									}
									foreach (String prelit in o.preUnsure.Keys)
									{
										int level = firstLevel(prelit, l.F, (obj) => ((Character)obj).unsure);
										if (level == -1 || level >= t)
											level = t - 1;
										if (!(Gt[level] as Character).unsure.Contains(prelit))
											(Gt[level] as Character).unsure.Add(prelit, 1);
									}
								}
							}
						}
					}
				}
				foreach (String lit in (Gt[t] as Character).unsure.Keys)
				{
					foreach (Operator o in operators)
					{
						//ensure character is the one performing the operation
						if (o.character.Equals(charactername))
						{
							//check if perceived unsure effects of action have literal as a component
							if (o.effUnsure.Contains(lit))
							{
								//check if that action appeared first time at t
								if (firstLevel(o, l.A, ((obj) => (obj as List<Operator>))) == t)
								{
									selectedActions++;
									//now add all of its preconditions as subgoals in Gts
									foreach (String prelit in o.preBPlus.Keys)
									{
										int level = firstLevel(prelit, l.F, (obj) => ((Character)obj).bPlus);
										if (level == -1 || level >= t)
											level = t - 1;
										if (!(Gt[level] as Character).bPlus.Contains(prelit))
											(Gt[level] as Character).bPlus.Add(prelit, 1);
									}
									foreach (String prelit in o.preBMinus.Keys)
									{
										int level = firstLevel(prelit, l.F, (obj) => ((Character)obj).bMinus);
										if (level == -1 || level >= t)
											level = t - 1;
										if (!(Gt[level] as Character).bMinus.Contains(prelit))
											(Gt[level] as Character).bMinus.Add(prelit, 1);
									}
									foreach (String prelit in o.preUnsure.Keys)
									{
										int level = firstLevel(prelit, l.F, (obj) => ((Character)obj).unsure);
										if (level == -1 || level >= t)
											level = t - 1;
										if (!(Gt[level] as Character).unsure.Contains(prelit))
											(Gt[level] as Character).unsure.Add(prelit, 1);
									}
								}
							}
						}
					}
				}
			}
			return selectedActions;
		}


        private static int firstLevel(String l, Hashtable table, del accessor){
            ArrayList keys = new ArrayList(table.Keys);
            keys.Sort();

            int last = (int)keys[keys.Count - 1];
            int first = (int)keys[0];
            for (int i = first; i <= last; ++i){
                if (accessor(table[i]).Contains(l))
                    return i;
            }
            return -1;
        }

        private static int firstLevel(Operator l, Hashtable table, del2 accessor)
        {
            ArrayList keys = new ArrayList(table.Keys);
            keys.Sort();
            int last = (int)keys[keys.Count - 1];
            int first = (int)keys[0];
            for (int i = first; i <= last; ++i)
            {
                if (accessor(table[i]).Contains(l))
                    return i;
            }
            return -1;
        }
    }
}

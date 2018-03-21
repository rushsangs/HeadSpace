using System;
using System.Collections.Generic;

namespace NarrativePlanning.DomainBuilder
{
    public class StateCreator
    {
        public StateCreator()
        {
        }

        public static WorldState getState(String filename){
            WorldState state = null;
            List<Literal> tWorld = new List<Literal>();
            List<Literal> fWorld = new List<Literal>();
            List<Character> characters = new List<Character>();

            String[] lines = readFile(filename);
            int i = 0;
            if(lines[0].Trim().Equals("t:")){
                i++;
                //each next line is a literal which is true;
                while(!lines[i].Trim().Equals("f:")){
                    Literal l = new Literal();
                    String[] allterms =lines[i].Trim().Trim('(', ')').Trim().Split(' ');

                    l.relation = allterms[0];
                    for (int j = 1; j < allterms.Length; ++j){
                        l.terms.Add(allterms[j]);
                    }
                    tWorld.Add(l);
                    ++i;
                }
                //now i is at f:
                i++;
                while (!lines[i].Trim().Contains("{"))
                {
                    Literal l = new Literal();
                    String[] allterms = lines[i].Trim().Trim('(', ')').Trim().Split(' ');

                    l.relation = allterms[0];
                    for (int j = 1; j < allterms.Length; ++j)
                    {
                        l.terms.Add(allterms[j]);
                    }
                    fWorld.Add(l);
                    ++i;
                }
                //now i is at the first character opening brace
                for (int j = i; j < lines.Length; ++j){
                    if(lines[j].Trim().Equals("}")){
                        Character c = createCharacter(lines, i, j);
                        characters.Add(c);
                        i = j + 1;
                    }
                }
            }
            state = new WorldState(tWorld, fWorld, characters);
            return state;
        }

        private static Character createCharacter(string[] lines, int i, int j)
        {
            Character c = new Character();
            int x = i + 1;
            c.name = lines[x].Trim();
            x++;
            x++;
            while(!lines[x].Trim().Equals("bminus:")){
                Literal l = new Literal();
                String[] allterms = lines[x].Trim().Trim('(', ')').Trim().Split(' ');

                l.relation = allterms[0];
                for (int k = 1; k < allterms.Length; ++k)
                {
                    l.terms.Add(allterms[k]);
                }
                c.bs.bPlus.Add(l);
                ++x;
            }
            ++x;
            while (!lines[x].Trim().Equals("unsure:"))
            {
                Literal l = new Literal();
                String[] allterms = lines[x].Trim().Trim('(', ')').Trim().Split(' ');

                l.relation = allterms[0];
                for (int k = 1; k < allterms.Length; ++k)
                {
                    l.terms.Add(allterms[k]);
                }
                c.bs.bMinus.Add(l);
                ++x;
            }
            ++x;
            while (!lines[x].Trim().Equals("}"))
            {
                Literal l = new Literal();
                String[] allterms = lines[x].Trim().Trim('(', ')').Trim().Split(' ');

                l.relation = allterms[0];
                for (int k = 1; k < allterms.Length; ++k)
                {
                    l.terms.Add(allterms[k]);
                }
                c.bs.unsure.Add(l);
                ++x;
            }
            return c;
        }

        public static String[] readFile(String file)
        {
            return System.IO.File.ReadAllLines(file);
        }
    }
}

using System;
using System.Collections.Generic;

namespace ScriptTest.Core
{
    public class AgentGenerator
    {
        private readonly Random rnd = new Random();

        
        private List<Agent> GenerateEven(int numAgents)
        {
            var agents = new List<Agent>();

            int amountPerType = numAgents / 6;

            for (int i = 0; i < amountPerType; i++)
            {
                agents.Add(new AlwaysCooperateAgent());
                agents.Add(new AlwaysDefectAgent());
                agents.Add(new TitForTatAgent());
                agents.Add(new TitForTwoTatsAgent());
                agents.Add(new GrimAgent());
                agents.Add(new RandomAgent());
                agents.Add(new GenerousTitForTatAgent());
            }

            return agents;
        }

        public List<Agent> GenerateFromCounts(AgentCounts counts)
        {
            List<Agent> agents = new List<Agent>();

            for (int i = 0; i < counts.AlwaysCooperate; i++)
                agents.Add(new AlwaysCooperateAgent());

            for (int i = 0; i < counts.AlwaysDefect; i++)
                agents.Add(new AlwaysDefectAgent());

            for (int i = 0; i < counts.TitForTat; i++)
                agents.Add(new TitForTatAgent());

            for (int i = 0; i < counts.TitForTwoTats; i++)
                agents.Add(new TitForTwoTatsAgent());

            for (int i = 0; i < counts.Grim; i++)
                agents.Add(new GrimAgent());

            for (int i = 0; i < counts.Random; i++)
                agents.Add(new RandomAgent());

            for (int i = 0; i < counts.GenerousTitForTat; i++)
                agents.Add(new GenerousTitForTatAgent());

            return agents;
        }


        

        public void ShuffleAgents(List<Agent> agents)
        {
            for (int i = agents.Count - 1; i > 0; i--)
            {
                int j = rnd.Next(0, i + 1);
                (agents[i], agents[j]) = (agents[j], agents[i]);
            }
        }

        public Agent CreateRandomAgent()
        {
            int choice = rnd.Next(1, 7); 

            return choice switch
            {
                1 => new AlwaysCooperateAgent(),
                2 => new AlwaysDefectAgent(),
                3 => new TitForTatAgent(),
                4 => new TitForTwoTatsAgent(),
                5 => new GrimAgent(),
                6 => new RandomAgent(),
                _ => new AlwaysCooperateAgent()
            };
        }

    }
}

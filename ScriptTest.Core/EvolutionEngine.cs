using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace ScriptTest.Core
{
    public class EvolutionEngine
    {
        private readonly AgentGenerator generator;
        private readonly SimulationSettings settings;

        private static readonly Random rnd = new Random();

        private const int MAX_GENERATIONS = 10_000;
        private const int MAX_RUNTIME_SECONDS = 60;

        public EvolutionEngine(AgentGenerator generator, SimulationSettings settings)
        {
            this.generator = generator;
            this.settings = settings;
        }

        public (
            Agent? winner,
            int generations,
            List<Dictionary<string, int>> history,
            bool converged,
            bool timedOut,
            bool maxGenHit
        )
        Run(List<Agent> agents)
        {
            int generation = 0;
            var timer = Stopwatch.StartNew();

            bool timedOut = false;
            bool maxGenHit = false;

            var history = new List<Dictionary<string, int>>
            {
                CountStrategies(agents)
            };

            while (true)
            {
                if (CheckConvergence(agents))
                    break;

                if (generation >= MAX_GENERATIONS)
                {
                    maxGenHit = true;
                    break;
                }

                if (timer.Elapsed.TotalSeconds >= MAX_RUNTIME_SECONDS)
                {
                    timedOut = true;
                    break;
                }

                generation++;

                RunRoundRobin(agents);
                agents = SelectAndReproduce(agents);
                ResetFitness(agents);

                history.Add(CountStrategies(agents));
            }

            bool converged = CheckConvergence(agents);

            Agent? winner = converged
                ? agents[0]
                : GetMostCommonAgent(agents);

            return (
                winner,
                generation,
                history,
                converged,
                timedOut,
                maxGenHit
            );
        }

        private Agent GetMostCommonAgent(List<Agent> agents)
        {
            return agents
                .GroupBy(a => a.Name)
                .OrderByDescending(g => g.Count())
                .First()
                .First();
        }


        private void RunRoundRobin(List<Agent> agents)
        {
            int rounds = settings.RoundsPerMatch;

            for (int i = 0; i < agents.Count; i++)
            {
                for (int j = i + 1; j < agents.Count; j++)
                {
                    Agent A = agents[i];
                    Agent B = agents[j];

                    MatchState state = new MatchState();

                    for (int r = 0; r < rounds; r++)
                    {
                        MatchEngine.Playoff(A, B, state, settings);
                    }
                }
            }
        }

        private List<Agent> SelectAndReproduce(List<Agent> agents)
        {
            agents.Sort((a, b) => b.Fitness.CompareTo(a.Fitness));

            int survivorsCount = agents.Count / 2;

            List<Agent> survivors = agents.Take(survivorsCount).ToList();

            List<Agent> nextGen = new();

            foreach (var parent in survivors)
            {
                nextGen.Add(parent);

                Agent child = parent.Clone();

                if (rnd.NextDouble() < settings.MutationRate)
                {
                    Agent mutated = generator.CreateRandomAgent();
                    child = mutated;
                }

                nextGen.Add(child);
            }

            return nextGen;
        }


        private void ResetFitness(List<Agent> agents)
        {
            foreach (var agent in agents)
                agent.ResetFitness();
        }


        private bool CheckConvergence(List<Agent> agents)
        {
            var first = agents[0].GetType();

            foreach (var agent in agents)
            {
                if (agent.GetType() != first)
                    return false;
            }

            return true;
        }

        private Dictionary<string, int> CountStrategies(List<Agent> agents)
        {
            var counts = new Dictionary<string, int>();

            foreach (var agent in agents)
            {
                counts[agent.Name] =
                    counts.GetValueOrDefault(agent.Name) + 1;
            }

            return counts;
        }
    }
}

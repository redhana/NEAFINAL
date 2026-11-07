using System;

namespace ScriptTest.Core
{
    public class MatchEngine
    {
        private static readonly Random rnd = new Random();

        public static void Playoff(
            Agent a,
            Agent b,
            MatchState state,
            SimulationSettings settings)
        {
            // Decide moves
            char moveA = a.DecideMove(
                oppLastMove: state.LastMoveB,
                oppDefectStreak: state.DefectStreakB,
                oppHasDefected: state.HasBDefected
            );

            char moveB = b.DecideMove(
                oppLastMove: state.LastMoveA,
                oppDefectStreak: state.DefectStreakA,
                oppHasDefected: state.HasADefected
            );

            // Apply noise
            moveA = ApplyNoise(moveA, settings.NoiseRate);
            moveB = ApplyNoise(moveB, settings.NoiseRate);

            // Adjustable payoff logic
            var p = settings.Payoffs;

            int payoffA;
            int payoffB;

            if (moveA == 'C' && moveB == 'C')
            {
                payoffA = p.CooperateCooperate;
                payoffB = p.CooperateCooperate;
            }
            else if (moveA == 'D' && moveB == 'D')
            {
                payoffA = p.DefectDefect;
                payoffB = p.DefectDefect;
            }
            else if (moveA == 'D' && moveB == 'C')
            {
                payoffA = p.DefectCooperate;
                payoffB = p.CooperateDefect;
            }
            else // A cooperates, B defects
            {
                payoffA = p.CooperateDefect;
                payoffB = p.DefectCooperate;
            }

            // Apply fitness
            a.AddToFitness(payoffA);
            b.AddToFitness(payoffB);

            // Update last moves
            state.LastMoveA = moveA;
            state.LastMoveB = moveB;

            // Update defect streaks
            if (moveA == 'D')
            {
                state.DefectStreakA++;
                state.HasADefected = true;
            }
            else
            {
                state.DefectStreakA = 0;
            }

            if (moveB == 'D')
            {
                state.DefectStreakB++;
                state.HasBDefected = true;
            }
            else
            {
                state.DefectStreakB = 0;
            }
        }

        private static char ApplyNoise(char move, double noiseRate)
        {
            if (noiseRate <= 0) return move;

            if (rnd.NextDouble() < noiseRate)
                return move == 'C' ? 'D' : 'C';

            return move;
        }
    }
}

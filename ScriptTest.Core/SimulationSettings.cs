using System;


namespace ScriptTest.Core
{
    public class SimulationSettings
    {
        // Probability flipped (C↔D) after the strategy decides
        public double NoiseRate { get; set; } = 0.0;

        // Probability cloned child mutates into a different strategy type
        public double MutationRate { get; set; } = 0.0;

        // Rounds per match 
        public int RoundsPerMatch { get; set; } = 10;

        public PayoffMatrix Payoffs { get; set; } = PayoffMatrix.Default();

    }
}

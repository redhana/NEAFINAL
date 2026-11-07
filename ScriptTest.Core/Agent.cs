namespace ScriptTest.Core
{
    public abstract class Agent
    {
        public int Fitness { get; private set; }


        public void AddToFitness(int amount)
        {
            Fitness += amount;
        }

        public void ResetFitness()
        {
            Fitness = 0;
        }

        public abstract char DecideMove(
            char oppLastMove,
            int oppDefectStreak,
            bool oppHasDefected
        );

        public abstract string Name { get; }

        
        public abstract Agent Clone();

        public override string ToString()
        {
            return $"{Name} Fitness: {Fitness}";
        }
    }
}



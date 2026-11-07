using System;

namespace ScriptTest.Core
{
    public class RandomAgent : Agent
    {
        private static readonly Random rnd = new Random();

        public override string Name => "Random";

        public override char DecideMove(char o, int s, bool d)
        {
            return rnd.Next(2) == 0 ? 'C' : 'D';
        }

        public override Agent Clone()
        {
            return new RandomAgent();
        }
    }
}

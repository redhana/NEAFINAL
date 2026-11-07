using System;

namespace ScriptTest.Core
{
    public class GenerousTitForTatAgent : Agent
    {
        private static readonly Random rnd = new Random();

        public override string Name => "Generous Tit For Tat";

        
        private readonly double forgiveness;

        public GenerousTitForTatAgent(double forgiveness = 0.3)
        {
            this.forgiveness = forgiveness;
        }

        public override char DecideMove(char oppLastMove, int oppDefectStreak, bool oppHasDefected)
        {
            
            if (oppLastMove == '\0') return 'C';

            if (oppLastMove == 'D')
            {
                
                return rnd.NextDouble() < forgiveness ? 'C' : 'D';
            }

            return 'C';
        }

        public override Agent Clone()
        {
          
            return new GenerousTitForTatAgent(forgiveness);
        }
    }
}

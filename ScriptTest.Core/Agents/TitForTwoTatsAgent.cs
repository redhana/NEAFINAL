namespace ScriptTest.Core
{
    public class TitForTwoTatsAgent : Agent
    {
        public override string Name => "Tit For Two Tats";

        public override char DecideMove(char o, int oppDefectStreak, bool d)
        {
            return oppDefectStreak >= 2 ? 'D' : 'C';
        }

        public override Agent Clone()
        {
            return new TitForTwoTatsAgent();
        }
    }
}

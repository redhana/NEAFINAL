namespace ScriptTest.Core
{
    public class TitForTatAgent : Agent
    {
        public override string Name => "Tit For Tat";

        public override char DecideMove(char oppLastMove, int s, bool d)
        {
            return oppLastMove == '\0' ? 'C' : oppLastMove;
        }

        public override Agent Clone()
        {
            return new TitForTatAgent();
        }
    }
}

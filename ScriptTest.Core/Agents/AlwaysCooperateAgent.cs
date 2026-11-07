namespace ScriptTest.Core
{
    public class AlwaysCooperateAgent : Agent
    {
        public override string Name => "Always Cooperate";

        public override char DecideMove(char o, int s, bool d)
        {
            return 'C';
        }

        public override Agent Clone()
        {
            return new AlwaysCooperateAgent();
        }
    }
}

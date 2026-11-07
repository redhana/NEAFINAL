namespace ScriptTest.Core
{
    public class GrimAgent : Agent
    {
        public override string Name => "Grim";

        public override char DecideMove(char o, int s, bool oppHasDefected)
        {
            return oppHasDefected ? 'D' : 'C';
        }

        public override Agent Clone()
        {
            return new GrimAgent();
        }
    }
}

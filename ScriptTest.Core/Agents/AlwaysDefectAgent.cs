namespace ScriptTest.Core
{
    public class AlwaysDefectAgent : Agent
    {
        public override string Name => "Always Defect";

        public override char DecideMove(char o, int s, bool d)
        {
            return 'D';
        }

        public override Agent Clone()
        {
            return new AlwaysDefectAgent();
        }
    }
}

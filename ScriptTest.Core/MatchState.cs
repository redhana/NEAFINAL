using System;
namespace ScriptTest.Core
{

    // holds the match state containg a and b agents state
    public class MatchState
    {
        
        public char LastMoveA { get; set; } = '\0';

        public char LastMoveB { get; set; } = '\0';



     
        public int DefectStreakA { get; set; } = 0;

        public int DefectStreakB { get; set; } = 0;



       
        public bool HasADefected { get; set; } = false;

        public bool HasBDefected { get; set; } = false;
    }
}


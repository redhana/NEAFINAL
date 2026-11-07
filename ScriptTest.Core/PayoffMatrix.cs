using System;

namespace ScriptTest.Core;

public class PayoffMatrix
{
    public int CooperateCooperate { get; set; }
    public int CooperateDefect { get; set; }
    public int DefectCooperate { get; set; }
    public int DefectDefect { get; set; }

    public static PayoffMatrix Default() => new()
    {
        CooperateCooperate = 3,
        CooperateDefect = 0,
        DefectCooperate = 5,
        DefectDefect = 1
    };
}

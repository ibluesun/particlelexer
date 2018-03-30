using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleLexer;

namespace ParticleLexerUnitTest
{
    class AtWordToken:TokenClass
    {
    }

    class DollarWordToken : TokenClass
    {
    }

    class TripleToken : TokenClass
    {
    }


    /// <summary>
    /// unit token form &lt;unit&gt;
    /// </summary>
    [TokenPattern(
        RegexPattern = "<(°?\\w+!?(\\^\\d+)?\\.?)+(/(°?\\w+!?(\\^\\d+)?\\.?)+)?>"
        , ShouldBeginWith = "<"
        )
    ]
    public class UnitToken : TokenClass { }



    [TokenPattern(RegexPattern = "Ahmed", ExactWord = true)]
    public class AhmedToken : TokenClass { }

    /// <summary>
    /// ..>  token
    /// </summary>
    [TokenPattern(RegexPattern = "\\.\\.>", ExactWord = true)]
    public class PositiveSequenceToken : TokenClass
    {
    }

    [TokenPattern(RegexPattern = "When", ExactWord = true)]
    public class WhenToken : TokenClass
    {
    }

}

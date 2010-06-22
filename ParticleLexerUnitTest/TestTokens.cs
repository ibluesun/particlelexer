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

    /// <summary>
    /// Unitized number is
    /// 90, 90.9
    /// 90.9e2, 90.9e+2, 90.9e-2
    /// 90.09&lt;m&gt;, 90.2e+2&lt;m&gt;, etc.
    /// </summary>
    [TokenPattern(
        RegexPattern = @"\d+(\.|\.\d+)?([eE][-+]?\d+)?"                       //floating number
        + "(\\s*<(°?\\w+!?(\\^\\d+)?\\.?)+(/(°?\\w+!?(\\^\\d+)?\\.?)+)?>)?"          // unit itself.
        )
    ]
    public class UnitizedNumberToken : TokenClass
    {

    }

    [TokenPattern(RegexPattern = "Ahmed", ExactWord = true)]
    public class AhmedToken : TokenClass { }

    /// <summary>
    /// ..>  token
    /// </summary>
    [TokenPattern(RegexPattern = "\\.\\.>", ExactWord = true)]
    public class PositiveSequenceToken : TokenClass
    {
    }

}

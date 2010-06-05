using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ParticleLexer;
using ParticleLexer.StandardTokens;

namespace ParticleLexerViewer
{
    [TokenPattern(RegexPattern = @"\[\*")]
    public class MagicLeftBrackets : TokenClass
    {
    }

    [TokenPattern(RegexPattern = @"\*\]")]
    public class MagicRightBrackets : TokenClass
    {
    }

    /// <summary>
    /// [*hello there*]
    /// </summary>
    public class MagicGroupToken : GroupTokenClass
    {
        public MagicGroupToken()
            : base(new MagicLeftBrackets(), new MagicRightBrackets())
        {
        }
    }

    /// <summary>
    /// Call[*32 32 11 34*]
    /// </summary>
    public class MagicCallToken : CallTokenClass
    {
        public MagicCallToken()
            : base(new MagicGroupToken(), new MultipleSpaceToken())
        {
        }
    }
}

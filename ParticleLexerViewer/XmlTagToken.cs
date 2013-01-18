using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleLexer;

namespace ParticleLexerViewer
{

    [TokenPattern(RegexPattern = @"&lt;", ExactWord = true)]
    class XmlLessThanToken : TokenClass { }

    [TokenPattern(RegexPattern = @"&gt;", ExactWord = true)]
    class XmlGreaterThanToken : TokenClass { }

    class XmlCompleteTagToken : TokenClass
    {
    }
}

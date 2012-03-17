using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleLexer;


namespace ParticleLexerViewer
{
    [TokenPattern(RegexPattern = @"</.+>", ShouldBeginWith = "<", ShouldEndWith = ">")]
    public class XmlCloseTag : TokenClass
    {
    }
}

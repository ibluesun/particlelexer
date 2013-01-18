using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleLexer;


namespace ParticleLexerViewer
{
    [TokenPattern(RegexPattern = @"</", ExactWord = true)]
    public class XmlCloseTagFirstSymbol : TokenClass { }

    [TokenPattern(RegexPattern = @"/>", ExactWord = true)]
    public class XmlCloseTagEndSymbol : TokenClass { }

    public class XmlCloseTag : TokenClass
    {
    }
}

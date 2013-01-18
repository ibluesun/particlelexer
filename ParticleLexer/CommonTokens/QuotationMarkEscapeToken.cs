using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ParticleLexer.CommonTokens
{

    /// <summary>
    /// Mathches \"    
    /// </summary>
    [TokenPattern(RegexPattern = @"\\""", ExactWord = true)]
    public class QuotationMarkEscapeToken : TokenClass
    {

    }


}

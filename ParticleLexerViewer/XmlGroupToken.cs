using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ParticleLexer;
using ParticleLexer.StandardTokens;

namespace ParticleLexerViewer
{
    class XmlGroupToken : GroupTokenClass
    {
        public XmlGroupToken()
            : base(new XmlStartTag(),  new XmlCloseTag())
        {
        }
    }
}

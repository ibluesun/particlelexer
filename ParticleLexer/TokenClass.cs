using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ParticleLexer
{
    public abstract class TokenClass
    {

        /// <summary>
        /// The pattern that we will use to tokenizing and merging tokens.
        /// </summary>
        public Regex Regex
        {
            get; 
            private set;
        }


        /// <summary>
        /// Tells if the regex is an exact word without regex classes.
        /// </summary>
        public bool ExactWord
        {
            get;
            private set;
        }

        /// <summary>
        /// The pattern text.
        /// </summary>
        public string RegexPattern
        {
            get;
            private set;
        }

        //cache token regexes
        static Dictionary<Type, Regex> regexes = new Dictionary<Type, Regex>();
        static Dictionary<Type, bool> exactwords = new Dictionary<Type, bool>();
        static Dictionary<Type, string> words = new Dictionary<Type, string>();

        /*
         * worth mentioned note that when I cached the regexes in this part the console calculations went very fast
         * I couldn't imagine that the reflection here make a lot of slowness.
         */

        private Type _ThisTokenType;
        public TokenClass()
        {

            _ThisTokenType = this.GetType();
            
            Regex j;

            // Try Cached versions first
            if (regexes.TryGetValue(_ThisTokenType, out j))
            {
                Regex = j;

                ExactWord = exactwords[_ThisTokenType];

                RegexPattern = words[_ThisTokenType];
            }
            else
            {

                // Cache the regexes due to the multiple creation  of the inherited types
                var rxs = this.GetType().GetCustomAttributes(false);

                if (rxs.Length > 0)
                {
                    TokenPatternAttribute TPA = rxs[0] as TokenPatternAttribute;

                    if (TPA != null)
                    {
                        Regex = new Regex("^" + TPA.RegexPattern + "$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
                        ExactWord = TPA.ExactWord;
                        RegexPattern = TPA.RegexPattern;
                    }

                    regexes.Add(_ThisTokenType, Regex);
                    exactwords.Add(_ThisTokenType, ExactWord);
                    words.Add(_ThisTokenType, RegexPattern);
                }
            }
        }



        /// <summary>
        /// Type of TokenClass 
        /// </summary>
        public Type Type
        {
            get
            {
                return _ThisTokenType;
            }
        }
    }
}

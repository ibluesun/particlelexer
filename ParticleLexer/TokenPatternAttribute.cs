using System;

namespace ParticleLexer
{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class TokenPatternAttribute : Attribute
    {
        /// <summary>
        /// The regex pattern that will be used to compare when tokenizing the target.
        /// </summary>
        public string RegexPattern { get; set; }

        /// <summary>
        /// Indicates that Regex Pattern is a whole exact complete word without special regex classes.
        /// </summary>
        public bool ExactWord { get; set; }



    }
}


namespace ParticleLexer.StandardTokens
{
    [TokenPattern(RegexPattern = "\\w+")]
    public class WordToken : TokenClass
    {
       
    }

    [TokenPattern(RegexPattern = @"\s+")]
    public class MultipleSpaceToken : TokenClass
    {
    }
}

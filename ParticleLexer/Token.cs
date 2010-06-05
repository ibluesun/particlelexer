﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using System.Diagnostics;
using ParticleLexer.StandardTokens;

namespace ParticleLexer
{
    /// <summary>
    /// Token class
    /// A recursive self contained class that contain tokens with token classes.
    /// </summary>
    public sealed partial class Token : IEnumerable<Token>
    {
        public string Value = string.Empty;
        public Type TokenClassType { get; set; }

        private int _IndexInText;

        /// <summary>
        /// Test if one of the childs of this token is from the given token class
        /// </summary>
        /// <typeparam name="TargetTokenClass">Type of Token Class</typeparam>
        /// <returns></returns>
        public bool Contains<TargetTokenClass>() where TargetTokenClass : TokenClass
        {
            if (childTokens.Count(o => o.TokenClassType == typeof(TargetTokenClass)) > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenClassType">Type of token class</param>
        /// <returns></returns>
        public bool Contains(Type tokenClassType)
        {
            if (childTokens.Count(o => o.TokenClassType == tokenClassType) > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenClassTypes">Types of token classes</param>
        /// <returns></returns>
        public bool Contains(params Type[] tokenClassTypes)
        {
            foreach(var tokenClassType in tokenClassTypes)
            {
                if (childTokens.Count(o => o.TokenClassType == tokenClassType) > 0)
                    return true;
            }
            
            return false;
        }

        #region structure & methods

        private List<Token> childTokens = new List<Token>();
        public ICollection<Token> ChildTokens
        {
            get
            {
                return childTokens;
            }
        }

        public Token AppendSubToken()
        {
            return AppendSubToken(string.Empty);
        }

        public Token AppendSubToken(char value)
        {
            return AppendSubToken(value.ToString());
        }

        public Token AppendSubToken(string value)
        {
            Token token = new Token() { Value = value, ParentToken = this };

            childTokens.Add(token);

            return token;
        }
        public void AppendSubToken(Token token)
        {
            token.ParentToken = this;
            childTokens.Add(token);
        }

        public void AppendToken(Token token)
        {
            ParentToken.AppendSubToken(token);
        }


        /// <summary>
        /// Indexing Child Tokens
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Token this[int index]
        {
            get
            {
                return childTokens[index];
            }
            set
            {
                childTokens[index] = value;
            }
        }


        /// <summary>
        /// Count of child tokens.
        /// </summary>
        public int Count
        {
            get { return childTokens.Count; }
        }

        public Token ParentToken { get; set; }


        /// <summary>
        /// Token Total String Value
        /// </summary>
        public string TokenValue
        {
            get
            {
                if (childTokens.Count > 0)
                {
                    string total = string.Empty;
                    foreach (Token t in childTokens)
                    {
                        total += t.TokenValue;
                    }
                    return total;
                }
                else
                {
                    return Value;
                }
            }
        }

        public int TokenValueLength
        {
            get
            {
                return TokenValue.Length;
            }
        }


        /// <summary>
        /// Token index in the original text.
        /// </summary>
        public int IndexInText
        {
            get
            {
                if (this.childTokens.Count > 0)
                    return this.childTokens[0].IndexInText;
                else
                    return _IndexInText;
            }
        }

        #endregion

        public string DebugView
        {
            get
            {
                return "[" + this.IndexInText.ToString() + ", "+TokenClassType.Name + "]: " + TokenValue;
            }
        }
        public override string ToString()
        {
            return DebugView;
        }

        #region Tokenization operations.


        public Token TrimStart<TargetTokenClass>()
            where TargetTokenClass : TokenClass
        {
            return TrimStart(typeof(TargetTokenClass));
        }


        /// <summary>
        /// remove the token from the start of tokens.
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public Token TrimStart(Type tokenType)
        {
            Token Trimmed = new Token();

            int ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];
                if (tok.TokenClassType == tokenType)
                {
                    //ignore this token
                }
                else
                {
                    //from here take the rest tokens
                    while (ci < childTokens.Count)
                    {
                        tok = childTokens[ci];
                        Trimmed.AppendSubToken(tok);
                        ci++;
                    }
                    break;
                }
                ci++;
            }

            return Trimmed;
        }

        public Token TrimEnd<TargetTokenClass>()
            where TargetTokenClass : TokenClass
        {
            return TrimEnd(typeof(TargetTokenClass));
        }

        /// <summary>
        /// Remove the token type from the end of tokens
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public Token TrimEnd(Type tokenType)
        {
            Token Trimmed = new Token();

            int ci = childTokens.Count - 1;
            while (ci >= 0)
            {
                var tok = childTokens[ci];
                if (tok.TokenClassType == tokenType)
                {
                    //ignore this token
                }
                else
                {
                    //from here take the rest tokens
                    for (int i = 0; i <= ci; i++)
                    {
                        tok = childTokens[i];
                        Trimmed.AppendSubToken(tok);   
                    }
                    break;
                }

                ci--;
            }

            return Trimmed;
        }



        /// <summary>
        /// Merge all tokens into the one and exclude specific token
        /// </summary>
        /// <param name="tokenType">Excluded tokens or Token(s) that act as separators.</param>
        /// <param name="mergedTokensType">The type of the new token merged from sub tokens.</param>
        /// <returns></returns>
        public Token MergeAllBut(Type mergedTokensType, params TokenClass[] tokenTypes)
        {
            return MergeAllBut(0, mergedTokensType, tokenTypes);
        }

        /// <summary>
        /// Merge all tokens into the one and exclude specific token
        /// </summary>
        /// <param name="tokenType">Excluded tokens or Token(s) that act as separators.</param>
        /// <param name="mergedTokensType">The type of the new token merged from sub tokens.</param>
        /// <returns></returns>
        public Token MergeAllBut<MergedTokenClass>(params TokenClass[] tokenClasses)
            where MergedTokenClass: TokenClass
        {
            return MergeAllBut(0, typeof(MergedTokenClass), tokenClasses);
        }

        /// <summary>
        /// Merge all tokens into the one Token with specific TokenClass and exclude specific token classes.
        /// </summary>
        /// <param name="tokenClasses">Excluded token or the separator token.</param>
        /// <param name="mergedTokensClassType">The type of the new token class that was merged from sub tokens.</param>
        /// <param name="startIndex">Starting from token index</param>
        /// <returns></returns>
        public Token MergeAllBut(int startIndex, Type mergedTokensClassType, params TokenClass[] tokenClasses)
        {
            Token first = this.MergeTokens(tokenClasses[0]);
            for (int i = 1; i < tokenClasses.Length; i++)
            {
                first = first.MergeTokens(tokenClasses[i]);
            }
            
            Debug.Assert(first != null);

            Token current = new Token();

            // walk on all tokens and accumulate them unitl you encounter separator

            int ci = 0;

            Token mergedTokens = new Token();

            while (ci < first.Count)
            {

                var c = first[ci];

                if (ci < startIndex)
                {
                    current.AppendSubToken(c);
                }
                else
                {
                    if (tokenClasses.Count(tok => tok.GetType() == c.TokenClassType) == 0)
                    {
                        mergedTokens.AppendSubToken(c);
                    }
                    else
                    {
                        //found a separator
                        if (mergedTokens.Count > 0)
                        {
                            mergedTokens.TokenClassType = mergedTokensClassType;

                            current.AppendSubToken(mergedTokens);
                        }
                        current.AppendSubToken(c);

                        mergedTokens = new Token();

                    }
                }
                
                ci++;
            }

            if (mergedTokens.Count > 0)
            {
                //the rest of merged tokens
                mergedTokens.TokenClassType = mergedTokensClassType;

                current.AppendSubToken(mergedTokens);
            }


            current.TokenClassType = first.TokenClassType;

            return Zabbat(current);

        }

        /// <summary>
        /// Merge Single Tokens into one token guided by regular expression.        
        /// </summary>
        /// <returns></returns>
        public  Token MergeTokens(TokenClass tokenClassType)
        {
            
            Regex rx = tokenClassType.Regex;

            Token current = new Token();

            Token merged = new Token();

            int tokIndex = 0;
            while (tokIndex < childTokens.Count)
            {
            loopHead:
                Token tok = childTokens[tokIndex];

                if (rx.Match(merged.TokenValue + tok.TokenValue).Success)
                {
                    //continue merge until merged value fail then last merged value is the desired value.

                    merged.AppendSubToken(tok);
                    
                    merged.TokenClassType = tokenClassType.GetType();
                }
                else
                {
                    //merge failed on last token value

                    //now there is a chance that if we consume another letters that we back into the success again
                    //   it is like if we want to compare  tamer , begining with t,a,m,e, will fail untill we reach ,r
                    //     I will make dirty solution to try
                    //      consume rest of tokens until found a success or end the discussion (end of tokens) :)
                    //
                    //   The behaviour above now is modified when marked the token class as ExactWord

                    if (!string.IsNullOrEmpty(merged.TokenValue) && tokenClassType.Type != typeof(WordToken) )
                    {                   
                        // inner sneaky loop. :)
                        int rtokIndex = tokIndex;
                        string emval = merged.TokenValue;

                        while (rtokIndex < childTokens.Count)
                        {
                            emval += childTokens[rtokIndex].TokenValue;

                            //however if the token is marked in tokenclass as ExactWord
                            //  then comparing more characters than actual ones is useless 
                            //  and will result un-needed cycles.
                            if (tokenClassType.ExactWord)
                            {
                                if (emval.Length > tokenClassType.RegexPattern.Length)
                                {
                                    // no need to compare extra charachters
                                    // so BREAAAAAAAAAAK
                                    break;
                                }
                            }
                            

                            // go with comparing.
                            if (rx.IsMatch(emval))
                            {
                                //yaaaaahoooo

                                //  after we run over tokens for unknown steps we found a success

                                // merge all tokens that made the success

                                // alter the original loop index and go to the loop tail

                                for (; tokIndex <= rtokIndex; tokIndex++)
                                {
                                    merged.AppendSubToken(childTokens[tokIndex]);
                                }

                                if (tokIndex < childTokens.Count)
                                {
                                    // there is another tokens to be tested.

                                    goto loopHead;
                                    
                                    //goto continueMerging;
                                }
                                else
                                {
                                    goto loopTail;
                                }
                            }
                            rtokIndex++;
                        }
                    }


                    // if merged token is not null put the merged value 
                    //  continue to test the last token with next tokens to the same regex
                    if (!string.IsNullOrEmpty(merged.TokenValue))
                    {
                        if (rx.IsMatch(merged.TokenValue)) merged.TokenClassType = tokenClassType.GetType();
                        current.AppendSubToken(merged);
                        merged = new Token();


                        // for begining another test with the new token
                        merged.AppendSubToken(tok);  
                        
                        merged.TokenClassType = tok.TokenClassType;
                    }
                    else
                    {
                        //merged token is null
                        merged.AppendSubToken(tok);
                        merged.TokenClassType = tok.TokenClassType;
                    }
                }

            loopTail:                
                tokIndex++;

            }

            if (!string.IsNullOrEmpty(merged.TokenValue))
            {
                if (rx.IsMatch(merged.TokenValue)) merged.TokenClassType = tokenClassType.GetType();
                current.AppendSubToken(merged);
            }

            current.TokenClassType = this.TokenClassType;
            return Zabbat(current);
        }

        /// <summary>
        /// Merge tokens based on token class.
        /// </summary>
        /// <typeparam name="DesiredTokenClass">Token Class Type like <see cref="WordToken"/></typeparam>
        /// <returns></returns>
        public Token MergeTokens<DesiredTokenClass>() where DesiredTokenClass: TokenClass, new()
        {
            ParticleLexer.TokenClass instance = new DesiredTokenClass();
            return MergeTokens(instance);
        }


        /// <summary>
        /// Merge a set of known sequence of tokens into a single with specific token class type.
        /// </summary>
        /// <typeparam name="DesiredTokenClass"></typeparam>
        /// <param name="tokenTypes"></param>
        /// <returns></returns>
        public Token MergeSequenceTokens<DesiredTokenClass>(params Type[] tokenTypes) where DesiredTokenClass : TokenClass, new()
        {

            int ComparisonTokensNumber = tokenTypes.Length;

            int CurrentTokenTypeIndex = 0;    // hold the current index of the required tokens
                                              // when the index reaches the ComparisonTokensNumber the compare ends and 
                                              //  the merge occure.

            Token current = new Token();

            Token merged = new Token() { TokenClassType = typeof(DesiredTokenClass) };
            

            int tokIndex = 0;
            while (tokIndex < childTokens.Count)
            {
                if (childTokens[tokIndex].TokenClassType == tokenTypes[CurrentTokenTypeIndex])
                {
                    merged.AppendSubToken(childTokens[tokIndex]);

                    CurrentTokenTypeIndex++;

                    if (CurrentTokenTypeIndex == ComparisonTokensNumber)
                    {
                        // include the merge token
                        current.AppendSubToken(merged);
                        merged = new Token() { TokenClassType = typeof(DesiredTokenClass) };
                        CurrentTokenTypeIndex = 0;
                    }
                    else
                    {
                        // do nothing :) :) :) :D :D :D
                    }
                }
                else
                {
                    if (CurrentTokenTypeIndex > 0)
                    {
                        // failure after merging some tokens so we have to empty the merged token 
                        //  then roll back to the first token we were in to begin after that token again
                        //    illustration
                        //      imagine we  merge Dollar Sign Token + Word Token 
                        //      imagine we have this sequence   $$Hello  the second dollar will fail the comparison
                        //      so we have to start from the second dollar token :)

                        tokIndex -= merged.Count;  // return the index to the first index of this token

                        merged = new Token() { TokenClassType = typeof(DesiredTokenClass) };
                        CurrentTokenTypeIndex = 0;
                    }

                    // add the current index.
                    current.AppendSubToken(childTokens[tokIndex]);

                }

                tokIndex++;
            }

            if (merged.Count > 0)
            {
                // then tokens were run out before we know if we can merge this into current or not
                // so take all tokens in merged and add it to current.
                for (int i = 0; i < merged.Count; i++)
                    current.AppendSubToken(merged[i]);

                merged = null;
            }

            return Zabbat(current);
        }

        /// <summary>
        /// This function make sure that inner tokens are not the same as outer tokens by popping out the
        /// buried tokens to the surface.
        /// </summary>
        /// <param name="melakhbat"></param>
        /// <returns></returns>
        public static Token Zabbat(Token melakhbat)
        {
            Token Metzabbat = new Token();
            foreach (Token h in melakhbat)
            {
                if (h.Count == 1)
                {
                    if (h.TokenClassType == h[0].TokenClassType) Metzabbat.AppendSubToken(h[0]);
                    else Metzabbat.AppendSubToken(h);
                }
                else
                {
                    Metzabbat.AppendSubToken(h);
                }
            }

            Metzabbat.TokenClassType = melakhbat.TokenClassType;

            return Metzabbat;
        }


        /// <summary>
        /// Removes specific tokens from the current tokens.
        /// </summary>
        /// <param name="tokenType">Array of tokens that should be removed.</param>
        /// <returns></returns>
        public Token RemoveTokens(params Type[] tokenTypes)
        {
            Token first = new Token();
            Token current = first;

            int ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];

                //make sure all chars in value are white spaces

                //if (tok.TokenValue.ToCharArray().Count(w => char.IsWhiteSpace(w)) == tok.TokenValue.Length)
                if (tokenTypes.Count(f => f == tok.TokenClassType) > 0)
                {
                    //all string are white spaces
                }
                else
                {
                    current.AppendSubToken(tok);
                }

                ci++;
            }

            return first;
        }

        /// <summary>
        /// Returns the first occurance of specific token class type
        /// </summary>
        /// <typeparam name="TargetTokenClass">Desired Class Token Type</typeparam>
        /// <returns></returns>
        public int IndexOf<TargetTokenClass>()
            where TargetTokenClass : TokenClass
        {
            return IndexOf(typeof(TargetTokenClass));
        }

        /// <summary>
        /// Returns the first occurance of specific token class type
        /// </summary>
        /// <param name="tokenType"></param>
        /// <returns></returns>
        public int IndexOf(Type tokenClassType)
        {
            int idx = -1;
            for (int i = 0; i < this.Count; i++)
            {
                if (this[i].TokenClassType == tokenClassType)
                {
                    idx = i;
                    break;
                }
            }
            return idx;
        }


        /// <summary>
        /// Remove specific token starting from the first token till the terminating token encountered while removing then the process ends.
        /// </summary>
        /// <typeparam name="TargetTokenClass"></typeparam>
        /// <typeparam name="TerminatingTokenClass"></typeparam>
        /// <returns></returns>
        public Token RemoveTokenUntil<TargetTokenClass, TerminatingTokenClass>()
            where TargetTokenClass: TokenClass
            where TerminatingTokenClass: TokenClass
        {
            return RemoveTokenUntil(typeof(TargetTokenClass), typeof(TerminatingTokenClass));
        }

        /// <summary>
        /// Remove specific token until we reach a closing token.
        /// </summary>
        /// <returns></returns>
        public Token RemoveTokenUntil(Type tokenType, Type untilToken)
        {
            Token first = new Token();
            Token current = first;

            bool reached = false;  //specifiy if we reach the close token or not.

            int ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];

                //make sure all chars in value are white spaces

                if (tokenType == tok.TokenClassType && reached == false)
                {
                    //all string are white spaces
                }
                else
                {
                    if (tok.TokenClassType == untilToken) reached = true;

                    //not the required token add it to the return value.
                    current.AppendSubToken(tok);
                }

                ci++;
            }

            return first;
        }


        /// <summary>
        /// Search for <see cref="MultipleSpaceToken"/> tokens and remove them from the children tokens.
        /// Used after Merging tokens with <see cref="MultipleSpaceToken"/> 
        /// </summary>
        /// <returns></returns>
        public Token RemoveSpaceTokens()
        {
            Token first = new Token();
            Token current = first;

            int ci = 0;
            while (ci < childTokens.Count)
            {
                var tok = childTokens[ci];
                
                //make sure all chars in value are white spaces
                
                //if (tok.TokenValue.ToCharArray().Count(w => char.IsWhiteSpace(w)) == tok.TokenValue.Length)
                if(tok.TokenClassType == typeof(MultipleSpaceToken))
                {
                    //all string are white spaces
                }
                else
                {
                    current.AppendSubToken(tok);
                }

                ci++;
            }

            return first;
        }
        
        #endregion

        #region Helper Functions

        /// <summary>
        /// Convert all charachters in the string into tokens with their classes types and CharachterToken for unknown ones.
        /// This function represent the entry point of the tokenization process.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Token ParseText(string text)
        {
            Token current = new Token();

            int ci = 0;
            while (ci < text.Length)
            {
                char c = text[ci];
                {
                    Token tk = current.AppendSubToken(c);
                    tk.TokenClassType = GetTokenClassType(c);
                    tk._IndexInText = ci;
                }

                ci++;
            }
            return current;
        }

        #endregion



        #region IEnumerable<Token> Members

        public IEnumerator<Token> GetEnumerator()
        {
            return childTokens.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return childTokens.GetEnumerator();
        }

        #endregion
    }
}
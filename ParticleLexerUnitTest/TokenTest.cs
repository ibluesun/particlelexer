﻿using ParticleLexer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

using ParticleLexer.StandardTokens;

namespace ParticleLexerUnitTest
{
    
    
    /// <summary>
    ///This is a test class for TokenTest and is intended
    ///to contain all TokenTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TokenTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for MergeCommonSingleTokens
        ///</summary>
        [TestMethod()]
        public void ParseTextTest()
        {
            Token toks = Token.ParseText("`~!@#$%^&*()-_+=");

            

            Assert.AreEqual(typeof(GraveAccentToken), toks[0].TokenClassType);
            Assert.AreEqual(typeof(TildeToken), toks[1].TokenClassType);
            Assert.AreEqual(typeof(ExclamationToken), toks[2].TokenClassType);
            Assert.AreEqual(typeof(AtSignToken), toks[3].TokenClassType);
            Assert.AreEqual(typeof(HashToken), toks[4].TokenClassType);
            Assert.AreEqual(typeof(DollarToken), toks[5].TokenClassType);
            Assert.AreEqual(typeof(PercentToken), toks[6].TokenClassType);
            Assert.AreEqual(typeof(CaretToken), toks[7].TokenClassType);
            Assert.AreEqual(typeof(AmpersandToken), toks[8].TokenClassType);
            Assert.AreEqual(typeof(AsteriskToken), toks[9].TokenClassType);
            Assert.AreEqual(typeof(LeftParenthesisToken), toks[10].TokenClassType);
            Assert.AreEqual(typeof(RightParenthesisToken), toks[11].TokenClassType);
            Assert.AreEqual(typeof(MinusToken), toks[12].TokenClassType);
            Assert.AreEqual(typeof(UnderscoreToken), toks[13].TokenClassType);
            Assert.AreEqual(typeof(PlusToken), toks[14].TokenClassType);
            Assert.AreEqual(typeof(EqualToken), toks[15].TokenClassType);




            toks = Token.ParseText("[{]}\\|;:'\",<.>/?");

            
            Assert.AreEqual(typeof(LeftSquareBracketToken), toks[0].TokenClassType);
            Assert.AreEqual(typeof(LeftCurlyBracketToken), toks[1].TokenClassType);
            Assert.AreEqual(typeof(RightSquareBracketToken), toks[2].TokenClassType);
            Assert.AreEqual(typeof(RightCurlyBracketToken), toks[3].TokenClassType);
            Assert.AreEqual(typeof(BackSlashToken), toks[4].TokenClassType);
            Assert.AreEqual(typeof(VerticalBarToken), toks[5].TokenClassType);
            Assert.AreEqual(typeof(SemiColonToken), toks[6].TokenClassType);
            Assert.AreEqual(typeof(ColonToken), toks[7].TokenClassType);
            Assert.AreEqual(typeof(ApostropheToken), toks[8].TokenClassType);
            Assert.AreEqual(typeof(QuotationMarkToken), toks[9].TokenClassType);
            Assert.AreEqual(typeof(CommaToken), toks[10].TokenClassType);
            Assert.AreEqual(typeof(LessThanToken), toks[11].TokenClassType);
            Assert.AreEqual(typeof(PeriodToken), toks[12].TokenClassType);
            Assert.AreEqual(typeof(GreaterThanToken), toks[13].TokenClassType);
            Assert.AreEqual(typeof(SlashToken), toks[14].TokenClassType);
            Assert.AreEqual(typeof(QuestionMarkToken), toks[15].TokenClassType);


        }



        /// <summary>
        ///A test for MergeSequenceTokens
        ///</summary>
        [TestMethod()]
        public void MergeSequenceTokensTest()
        {
            Token r = Token.ParseText("Hello @Ahmed !Triple! $Sadek @!BAD @@BaD $$bAd @Mohammed $Tawfik");

            r = r.MergeTokens<WordToken>();

            r = r.MergeSequenceTokens<AtWordToken>(typeof(AtSignToken), typeof(WordToken));
            r = r.MergeSequenceTokens<DollarWordToken>(typeof(DollarToken), typeof(WordToken));
            r = r.MergeSequenceTokens<TripleToken>(typeof(ExclamationToken), typeof(WordToken), typeof(ExclamationToken));


            Assert.AreEqual(typeof(AtWordToken), r[2].TokenClassType);
            Assert.AreEqual(typeof(TripleToken), r[4].TokenClassType);

        }

        [TestMethod]
        public void TestParsingUnits()
        {

            string line = "Ahmed ..>  90<L> + 50<g> / 300<J/k> * 99<R/H>; # and please don't hestiate any more";

            Token l = Token.ParseText(line);

            l = l.MergeTokens<WordToken>();
            l = l.MergeTokens<AhmedToken>();
            l = l.MergeTokens<UnitToken>();
            l = l.MergeTokens<PositiveSequenceToken>();

        }

        /// <summary>
        ///A test for MergeMultipleWordTokens
        ///</summary>
        [TestMethod()]
        public void MergeMultipleWordTokensTest()
        {
            Token tense = Token.ParseText("Hello There And How Are you today or Ahmed ..> can make something strange ..>");
            var o = tense.MergeTokens<WordToken>();
            o = o.MergeMultipleWordTokens(typeof(AhmedToken), typeof(AndWordToken), 
                typeof(PositiveSequenceToken), typeof(OrWordToken));



        }
    }
}

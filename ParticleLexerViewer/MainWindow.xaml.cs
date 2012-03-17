using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ParticleLexer;
using ParticleLexer.StandardTokens;

namespace ParticleLexerViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Token Tokens = null;

        private void ParseButton_Click(object sender, RoutedEventArgs e)
        {
            Tokens = Token.ParseText(SourceTextBox.Text);

            Tokens = Tokens.MergeTokens<WordToken>();

            Tokens = Tokens.MergeTokens<MultipleSpaceToken>();

            Tokens = Tokens.MergeTokens<MagicLeftBrackets>();

            Tokens = Tokens.MergeTokens<MagicRightBrackets>();

            Tokens = Tokens.MergeTokensInGroups(
                new ParenthesisGroupToken(),
                new SquareBracketsGroupToken(),
                new MagicGroupToken()
            );

            Tokens = Tokens.DiscoverCalls(
                new ParenthesisCallToken(),
                new SquareBracketsCallToken(), 
                new MagicCallToken()
                );


            if (ParseTreeView.ItemsSource == null)
            {
                ParseTreeView.Items.Clear();
             
            }
            ParseTreeView.ItemsSource = Tokens;
        }

        private void XmlParseButton_Click(object sender, RoutedEventArgs e)
        {
            Tokens = Token.ParseText(XmlTextBox.Text);

            Tokens = Tokens.MergeTokens<MultipleSpaceToken>();
            Tokens = Tokens.MergeTokens<WordToken>();
            Tokens = Tokens.RemoveSpaceTokens();
            Tokens = Tokens.MergeTokens<XmlTagToken>();
            
            //Tokens = Tokens.MergeTokensInGroups(new XmlGroupToken());


            if (ParseTreeView.ItemsSource == null)
            {
                ParseTreeView.Items.Clear();

            }
            ParseTreeView.ItemsSource = Tokens;

        }

    }
}

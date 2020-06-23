using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public enum STATE { Start, Num1, Num2, Num3, Identifier, String, Comment1, Comment2, Comment3, error }
        private string input;
        private LinkedList<TokenReg> listOfTokens = new LinkedList<TokenReg>();

        public LinkedList<TokenReg> ListOfTokens { get { return listOfTokens; } }
        public string Input { get { return input; } set { input = value; } }
        public bool Flag { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        private void RunButton_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Text = ""; //reset output text
            Input = txtInput.Text; //reset input text to new text
            ListOfTokens.Clear(); //reset table

            getTokens(Input);

            if (Flag) //check if there is an error
            {
                foreach (TokenReg token in ListOfTokens)
                {
                    txtOutput.Text += token.ToString();
                }

                //Parser program
                try
                {
                    removeCommentTokens(); //remove comment tokens from list as we dont need them in the parsing process
                    Parser p = new Parser(ListOfTokens);
                    SyntaxTree s = p.program();

                    TreeViewItem treeViewItem = makeTreeView(s);
                    Window1 secondWindow = new Window1();
                    secondWindow.mainTreeView.Items.Add(treeViewItem);
                    secondWindow.Show();
                }
                catch(ParsingErrorException ex)
                {
                    MessageBox.Show(ex.Message, ex.Source,
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private TreeViewItem makeTreeView(SyntaxTree tree)
        {
            //Initialize a tree view item and set its header to the tree's key lexeme
            TreeViewItem mainTreeViewItem = new TreeViewItem();
            mainTreeViewItem.Header = tree.Key.Lexeme;

            foreach (SyntaxTree s in tree.ChildNodes){
                TreeViewItem t = makeTreeView(s);
                mainTreeViewItem.Items.Add(t);
            }

            return mainTreeViewItem;
        }

        /// <summary>
        /// takes a string as input and divides it into lexemes and tokens adding them into the listOfTokens variable
        /// </summary>
        /// <param name="input">input string to compute</param>
        private void getTokens(string input)
        {
            Flag = true; //this variable indicates that the function finished succesfully with no errors
            int j = 0; //integer to keep track of the first index of the lexeme
            int i; //integer to keep track of the last index of the lexeme
            STATE currentState = STATE.Start;
            for (i = 0; i < input.Length; i++)
            {
                try
                {
                    switch (currentState)
                    {
                        case STATE.Start:
                            j = i;  //to keep track of the first element's index
                            if (isLetter(input.ElementAt(i)))
                            {
                                if (i == input.Length - 1) //we do this check to not neglect the last letter
                                {
                                    i--;
                                }
                                currentState = STATE.Identifier;
                            }
                            else if (isDigit(input.ElementAt(i)))
                            {
                                if (i == input.Length - 1) //we do this check to not neglect the last digit
                                {
                                    i--;
                                }
                                currentState = STATE.Num1;
                            }
                            else if (isWhiteSpace(input.ElementAt(i)))
                            {
                                //do nothing
                            }
                            else if (input.ElementAt(i) == '/')
                            {
                                if (i == input.Length - 1)
                                {
                                    i--;
                                }
                                currentState = STATE.Comment1;
                            }
                            else if (input.ElementAt(i) == '"')
                            {

                                currentState = STATE.String;
                            }
                            else if (input.ElementAt(i) == '(')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_LEFTBRACKET, "("));
                            }
                            else if (input.ElementAt(i) == ')')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_RIGHTBRACKET, ")"));
                            }
                            else if (input.ElementAt(i) == ';')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_SEMICOLON, ";"));
                            }
                            else if (input.ElementAt(i) == ',')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_COMMA, ","));
                            }
                            else if (input.ElementAt(i) == '+')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_PLUS, "+"));
                            }
                            else if (input.ElementAt(i) == '-')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_MINUS, "-"));
                            }
                            else if (input.ElementAt(i) == '*')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_MULTIPLICATION, "*"));
                            }
                            else if (input.ElementAt(i) == ':')
                            {
                                if (input.ElementAt(i + 1) == '=')
                                {
                                    ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_ASSIGN, ":="));
                                    i++; //we increment i to skip the equal sign evaluation
                                }
                                else
                                {
                                    i--; //to catch the symbol that did the error
                                    currentState = STATE.error; //since the colon char is never used alone it must always accompany the equal sign
                                }
                            }
                            else if (input.ElementAt(i) == '=')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_EQUAL, "="));
                            }
                            else if (input.ElementAt(i) == '<')
                            {
                                if (input.ElementAt(i + 1) == '>')
                                {
                                    listOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_NOTEQUAL, "<>"));
                                    i++; //we increment i to skip the > evaluation
                                }
                                else
                                    ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_LESSTHAN, "<"));
                            }
                            else if (input.ElementAt(i) == '>')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_GREATERTHAN, ">"));
                            }
                            else if (input.ElementAt(i) == '&')
                            {
                                if (input.ElementAt(i + 1) == '&')
                                {
                                    ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_AND, "&&"));
                                    i++; //we increment i to skip the & evaluation
                                }
                                else
                                    currentState = STATE.error; //same as the assign lexeme
                            }
                            else if (input.ElementAt(i) == '|')
                            {
                                if (input.ElementAt(i + 1) == '|')
                                {
                                    ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_OR, "||"));
                                    i++; //we increment i to skip the | evaluation
                                }
                                else
                                    currentState = STATE.error; //same as the assign lexeme
                            }
                            else if (input.ElementAt(i) == '{')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_LEFTBRACE, "{"));
                            }
                            else if (input.ElementAt(i) == '}')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_RIGHTBRACE, "}"));
                            }
                            else
                            {
                                i--; //we decrement it as if it was the last index in the string we wouldnt cycle back to indicate an error occured
                                currentState = STATE.error; //anything else is out of the alphabet and therefore an error
                            }
                            break;
                        case STATE.Num1:
                            if (isDigit(input.ElementAt(i)) && i != input.Length - 1)
                            {
                                //do nothing
                            }
                            else if (input.ElementAt(i) == '.')
                            {
                                if (i == input.Length - 1)
                                {
                                    i--; //decrement to not go out of loop
                                    currentState = STATE.error; //because you cant end string with dot
                                }
                                else
                                {

                                    currentState = STATE.Num2;
                                }
                            }
                            else if (isLetter(input.ElementAt(i)))
                            {
                                if (i == input.Length - 1)
                                {
                                    i--;
                                }
                                currentState = STATE.error; //if a letter follows a number without another delimeter that means that this was supposed to be an invalid identifier and therefore we move it to the error state.
                            }
                            else
                            {
                                if (!isDigit(input.ElementAt(i))) //if last character in string is digit then we dont decrement the i
                                {
                                    i--; //we decrement i as we do not want to include the [other] character in our number
                                }
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_NUMBER, input.Substring(j, i - j + 1)));
                                currentState = STATE.Start; //go back to start state to accept new token
                            }
                            break;
                        case STATE.Num2:
                            if (isDigit(input.ElementAt(i)))
                            {
                                if (i == input.Length - 1) //we do this check to not neglect the last number after the dot
                                {
                                    i--;
                                }
                                currentState = STATE.Num3;
                            }
                            else
                            {
                                if (i == input.Length - 1) //we do this check to not neglect the last symbol after the dot and advance to the error state
                                {
                                    i--;
                                }
                                currentState = STATE.error; //as we dont expect anything other than a digit to follow
                            }
                            break;
                        case STATE.Num3:
                            if (isDigit(input.ElementAt(i)) && i != input.Length - 1)
                            {
                                //do nothing
                            }
                            else if (isLetter(input.ElementAt(i)))
                            {
                                if (i == input.Length - 1)
                                {
                                    i--;
                                }
                                currentState = STATE.error; //if a letter follows a number without another delimeter that means that this was supposed to be an invalid identifier and therefore we move it to the error state.
                            }
                            else
                            {
                                if (!isDigit(input.ElementAt(i)))
                                {
                                    i--; //we decrement i as we do not want to include the [other] character in our number
                                }
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_NUMBER, input.Substring(j, i - j + 1)));
                                currentState = STATE.Start; //go back to start state to accept new token
                            }
                            break;
                        case STATE.Identifier:
                            if ((isLetter(input.ElementAt(i)) || isDigit(input.ElementAt(i))) && i != input.Length - 1) // i != input.Length - 1 to make sure that the last lexeme is taken into consideration without doing nothing
                            {
                                //do nothing
                            }
                            else
                            {
                                if (!(isLetter(input.ElementAt(i)) || isDigit(input.ElementAt(i)))) //we dont want to decrement i if it is the last character of a lexeme so we make that check
                                {
                                    i--; //we decrement i as we do not want to include the [other] character in our identifier
                                }
                                TokenReg.TOKEN_TYPE tokenVal = isReservedWord(input.Substring(j, i - j + 1)); //we check if it is one of the reserved keywords first and register it in the tokenVal variable, if its not token Val will have a value of T_ID
                                ListOfTokens.AddLast(new TokenReg(tokenVal, input.Substring(j, i - j + 1)));
                                currentState = STATE.Start; //go back to start state to accept new token
                            }
                            break;
                        case STATE.String:
                            if (input.ElementAt(i) == '"')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_STRINGVAL, input.Substring(j + 1, i - j - 1))); //we put the start of substring as j + 1 to ignore the quotation marks; same thing for i - j - 1 instead of i - j + 1
                                currentState = STATE.Start; //go back to start state to accept new token
                            }
                            break;
                        case STATE.Comment1:
                            if (input.ElementAt(i) == '*')
                            {
                                currentState = STATE.Comment2;
                            }
                            else
                            {
                                if (input.ElementAt(i) != '/')
                                    i--; //as we just want the division lexeme so we decrement and go back to start state

                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_DIVISION, "/")); //as there was a / character and another character other than * appeared then we consider it as a division symbol rather than the start of a comment
                                currentState = STATE.Start; //go back to start state to accept new token
                            }
                            break;
                        case STATE.Comment2:
                            if (input.ElementAt(i) == '*')
                                currentState = STATE.Comment3;
                            break;
                        case STATE.Comment3:
                            if (input.ElementAt(i) == '/')
                            {
                                ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_COMMENT, input.Substring(j, i - j + 1)));
                                currentState = STATE.Start; //go back to start state to accept new token
                            }
                            else if (input.ElementAt(i) == '*')
                            {
                                //do nothing
                            }
                            else
                            {
                                currentState = STATE.Comment2;
                            }
                            break;
                        case STATE.error:
                            string errorString = input.Substring(0, i + 1); //split the string from start to where the error occured
                            int numLines = errorString.Split('\n').Length; //get the number of lines of where the error occured
                            txtOutput.Text = "error at line " + numLines + ".";
                            Flag = false; //indicating that an error has occured
                            return;
                    }
                }
                catch (ArgumentOutOfRangeException)
                {
                    if (input.ElementAt(i) == '<')
                    {
                        ListOfTokens.AddLast(new TokenReg(TokenReg.TOKEN_TYPE.T_LESSTHAN, "<"));
                    }
                    else
                    {
                        if (i == input.Length - 1)
                            i--; //to not get out of loop before going to error state
                        currentState = STATE.error;
                    }
                }
            }
        }

        private bool isLetter(char c)
        {
            if ((int)c >= 65 && (int)c <= 122)
                return true;
            return false;
        }

        private bool isDigit(char c)
        {
            if ((int)c >= 48 && (int)c <= 57)
                return true;
            return false;
        }

        private bool isWhiteSpace(char c)
        {
            if (c == '\r' || c == '\n' || c == '\t' || c == ' ')
                return true;
            return false;
        }

        private void removeCommentTokens()
        {
            for(int i = 0; i < ListOfTokens.Count; i++)
            {
                if(ListOfTokens.ElementAt(i).Token == TokenReg.TOKEN_TYPE.T_COMMENT)
                {
                    ListOfTokens.Remove( ListOfTokens.ElementAt(i) );
                }
            }
        }

        private TokenReg.TOKEN_TYPE isReservedWord(string s)
        {
            string testString = s.ToLower();
            switch (testString)
            {
                case "int":
                    return TokenReg.TOKEN_TYPE.T_INT;
                case "float":
                    return TokenReg.TOKEN_TYPE.T_FLOAT;
                case "string":
                    return TokenReg.TOKEN_TYPE.T_STRING;
                case "read":
                    return TokenReg.TOKEN_TYPE.T_READ;
                case "write":
                    return TokenReg.TOKEN_TYPE.T_WRITE;
                case "repeat":
                    return TokenReg.TOKEN_TYPE.T_REPEAT;
                case "until":
                    return TokenReg.TOKEN_TYPE.T_UNTIL;
                case "if":
                    return TokenReg.TOKEN_TYPE.T_IF;
                case "elseif":
                    return TokenReg.TOKEN_TYPE.T_ELSEIF;
                case "else":
                    return TokenReg.TOKEN_TYPE.T_ELSE;
                case "then":
                    return TokenReg.TOKEN_TYPE.T_THEN;
                case "return":
                    return TokenReg.TOKEN_TYPE.T_RETURN;
                case "endl":
                    return TokenReg.TOKEN_TYPE.T_ENDL;
                case "begin":
                    return TokenReg.TOKEN_TYPE.T_BEGIN;
                case "end":
                    return TokenReg.TOKEN_TYPE.T_END;
                default:
                    return TokenReg.TOKEN_TYPE.T_ID;
            }
        }

    }
}

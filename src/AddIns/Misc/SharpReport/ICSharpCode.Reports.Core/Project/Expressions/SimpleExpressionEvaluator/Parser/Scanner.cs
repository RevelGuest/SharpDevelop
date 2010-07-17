// Generated by TinyPG v1.2 available at www.codeproject.com

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SimpleExpressionEvaluator.Parser
{
    #region Scanner

    public partial class Scanner
    {
        public string Input;
        public int StartPos = 0;
        public int EndPos = 0;
        public int CurrentLine;
        public int CurrentColumn;
        public int CurrentPosition;
        public List<Token> Skipped; // tokens that were skipped
        public Dictionary<TokenType, Regex> Patterns;

        private Token LookAheadToken;
        private List<TokenType> Tokens;
        private List<TokenType> SkipList; // tokens to be skipped

        public Scanner()
        {
            Regex regex;
            Patterns = new Dictionary<TokenType, Regex>();
            Tokens = new List<TokenType>();
            LookAheadToken = null;

            SkipList = new List<TokenType>();
            SkipList.Add(TokenType.WHITESPACE);
            SkipList.Add(TokenType.COMMENT);

            regex = new Regex(@"(\+|-)", RegexOptions.Compiled);
            Patterns.Add(TokenType.PLUSMINUS, regex);
            Tokens.Add(TokenType.PLUSMINUS);

            regex = new Regex(@"\*|/|%", RegexOptions.Compiled);
            Patterns.Add(TokenType.MULTDIV, regex);
            Tokens.Add(TokenType.MULTDIV);

            regex = new Regex(@"\^", RegexOptions.Compiled);
            Patterns.Add(TokenType.POWER, regex);
            Tokens.Add(TokenType.POWER);

            regex = new Regex(@"\(", RegexOptions.Compiled);
            Patterns.Add(TokenType.BROPEN, regex);
            Tokens.Add(TokenType.BROPEN);

            regex = new Regex(@"\)", RegexOptions.Compiled);
            Patterns.Add(TokenType.BRCLOSE, regex);
            Tokens.Add(TokenType.BRCLOSE);

            regex = new Regex(@"\[", RegexOptions.Compiled);
            Patterns.Add(TokenType.SBOPEN, regex);
            Tokens.Add(TokenType.SBOPEN);

            regex = new Regex(@"\]", RegexOptions.Compiled);
            Patterns.Add(TokenType.SBCLOSE, regex);
            Tokens.Add(TokenType.SBCLOSE);

            regex = new Regex(@",", RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMA, regex);
            Tokens.Add(TokenType.COMMA);

            regex = new Regex(@"\:", RegexOptions.Compiled);
            Patterns.Add(TokenType.COLON, regex);
            Tokens.Add(TokenType.COLON);

            regex = new Regex(@"[Tt][Rr][Uu][Ee]", RegexOptions.Compiled);
            Patterns.Add(TokenType.TRUE, regex);
            Tokens.Add(TokenType.TRUE);

            regex = new Regex(@"[Ff][Aa][Ll][Ss][Ee]", RegexOptions.Compiled);
            Patterns.Add(TokenType.FALSE, regex);
            Tokens.Add(TokenType.FALSE);

            regex = new Regex(@"([Nn][Uu][Ll][Ll])|([Nn][Oo][Tt][Hh][Ii][Nn][Gg])", RegexOptions.Compiled);
            Patterns.Add(TokenType.NULL, regex);
            Tokens.Add(TokenType.NULL);

            regex = new Regex(@"[Ii][Ff]", RegexOptions.Compiled);
            Patterns.Add(TokenType.IF, regex);
            Tokens.Add(TokenType.IF);

            regex = new Regex(@"[Tt][Hh][Ee][Nn]", RegexOptions.Compiled);
            Patterns.Add(TokenType.THEN, regex);
            Tokens.Add(TokenType.THEN);

            regex = new Regex(@"[Ee][Ll][Ss][Ee]|[Oo][Tt][Hh][Ee][Rr][Ww][Ii][Ss][Ee]", RegexOptions.Compiled);
            Patterns.Add(TokenType.ELSE, regex);
            Tokens.Add(TokenType.ELSE);

            regex = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*(?=\()", RegexOptions.Compiled);
            Patterns.Add(TokenType.METHODNAME, regex);
            Tokens.Add(TokenType.METHODNAME);

            regex = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*(?=\[)", RegexOptions.Compiled);
            Patterns.Add(TokenType.ARRAYNAME, regex);
            Tokens.Add(TokenType.ARRAYNAME);

            regex = new Regex(@"[a-zA-Z_][a-zA-Z0-9_]*", RegexOptions.Compiled);
            Patterns.Add(TokenType.IDENTIFIER, regex);
            Tokens.Add(TokenType.IDENTIFIER);

            regex = new Regex(@"(\+|-)?[0-9]+", RegexOptions.Compiled);
            Patterns.Add(TokenType.INTEGER, regex);
            Tokens.Add(TokenType.INTEGER);

            regex = new Regex(@"(\+|-)?[0-9]*\.[0-9]+", RegexOptions.Compiled);
            Patterns.Add(TokenType.NUMBER, regex);
            Tokens.Add(TokenType.NUMBER);

            regex = new Regex(@"\""(\""\""|[^\""])*\""", RegexOptions.Compiled);
            Patterns.Add(TokenType.STRING, regex);
            Tokens.Add(TokenType.STRING);

            regex = new Regex(@"!=~|=~|==|<>|<=|>=|>|<|=|!=", RegexOptions.Compiled);
            Patterns.Add(TokenType.COMPARE, regex);
            Tokens.Add(TokenType.COMPARE);

            regex = new Regex(@"=", RegexOptions.Compiled);
            Patterns.Add(TokenType.EQUALS, regex);
            Tokens.Add(TokenType.EQUALS);

            regex = new Regex(@"^$", RegexOptions.Compiled);
            Patterns.Add(TokenType.EOF, regex);
            Tokens.Add(TokenType.EOF);

            regex = new Regex(@"\.", RegexOptions.Compiled);
            Patterns.Add(TokenType.DOT, regex);
            Tokens.Add(TokenType.DOT);

            regex = new Regex(@"&&|\|\||[Aa][Nn][Dd]|[Oo][Rr]", RegexOptions.Compiled);
            Patterns.Add(TokenType.BOOLEAN, regex);
            Tokens.Add(TokenType.BOOLEAN);

            regex = new Regex(@"!|-", RegexOptions.Compiled);
            Patterns.Add(TokenType.UNARY, regex);
            Tokens.Add(TokenType.UNARY);

            regex = new Regex(@"\s+", RegexOptions.Compiled);
            Patterns.Add(TokenType.WHITESPACE, regex);
            Tokens.Add(TokenType.WHITESPACE);

            regex = new Regex(@"/\*\s*.*\*/", RegexOptions.Compiled);
            Patterns.Add(TokenType.COMMENT, regex);
            Tokens.Add(TokenType.COMMENT);


        }

        public void Init(string input)
        {
            this.Input = input;
            StartPos = 0;
            EndPos = 0;
            CurrentLine = 0;
            CurrentColumn = 0;
            CurrentPosition = 0;
            Skipped = new List<Token>();
            LookAheadToken = null;
        }

        public Token GetToken(TokenType type)
        {
            Token t = new Token(this.StartPos, this.EndPos);
            t.Type = type;
            return t;
        }

         /// <summary>
        /// executes a lookahead of the next token
        /// and will advance the scan on the input string
        /// </summary>
        /// <returns></returns>
        public Token Scan(params TokenType[] expectedtokens)
        {
            Token tok = LookAhead(expectedtokens); // temporarely retrieve the lookahead
            LookAheadToken = null; // reset lookahead token, so scanning will continue
            StartPos = tok.EndPos;
            EndPos = tok.EndPos; // set the tokenizer to the new scan position
            return tok;
        }

        /// <summary>
        /// returns token with longest best match
        /// </summary>
        /// <returns></returns>
        public Token LookAhead(params TokenType[] expectedtokens)
        {
            int i;
            int startpos = StartPos;
            Token tok = null;
            List<TokenType> scantokens;


            // this prevents double scanning and matching
            // increased performance
            if (LookAheadToken != null 
                && LookAheadToken.Type != TokenType._UNDETERMINED_ 
                && LookAheadToken.Type != TokenType._NONE_) return LookAheadToken;

            // if no scantokens specified, then scan for all of them (= backward compatible)
            if (expectedtokens.Length == 0)
                scantokens = Tokens;
            else
            {
                scantokens = new List<TokenType>(expectedtokens);
                scantokens.AddRange(SkipList);
            }

            do
            {

                int len = -1;
                TokenType index = (TokenType)int.MaxValue;
                string input = Input.Substring(startpos);

                tok = new Token(startpos, EndPos);

                for (i = 0; i < scantokens.Count; i++)
                {
                    Regex r = Patterns[scantokens[i]];
                    Match m = r.Match(input);
                    if (m.Success && m.Index == 0 && ((m.Length > len) || (scantokens[i] < index && m.Length == len )))
                    {
                        len = m.Length;
                        index = scantokens[i];  
                    }
                }

                if (index >= 0 && len >= 0)
                {
                    tok.EndPos = startpos + len;
                    tok.Text = Input.Substring(tok.StartPos, len);
                    tok.Type = index;
                }
                else
                {
                    if (tok.StartPos < tok.EndPos - 1)
                        tok.Text = Input.Substring(tok.StartPos, 1);
                }

                if (SkipList.Contains(tok.Type))
                {
                    startpos = tok.EndPos;
                    Skipped.Add(tok);
                }
            }
            while (SkipList.Contains(tok.Type));

            LookAheadToken = tok;
            return tok;
        }
    }

    #endregion

    #region Token

    public enum TokenType
    {

            //Non terminal tokens:
            _NONE_  = 0,
            _UNDETERMINED_= 1,

            //Non terminal tokens:
            Start   = 2,
            Conditional= 3,
            Identifier= 4,
            BooleanExpr= 5,
            CompareExpr= 6,
            AddExpr = 7,
            MultExpr= 8,
            PowerExpr= 9,
            Params  = 10,
            Method  = 11,
            Array   = 12,
            QualifiedName= 13,
            UnaryExpr= 14,
            Atom    = 15,

            //Terminal tokens:
            PLUSMINUS= 16,
            MULTDIV = 17,
            POWER   = 18,
            BROPEN  = 19,
            BRCLOSE = 20,
            SBOPEN  = 21,
            SBCLOSE = 22,
            COMMA   = 23,
            COLON   = 24,
            TRUE    = 25,
            FALSE   = 26,
            NULL    = 27,
            IF      = 28,
            THEN    = 29,
            ELSE    = 30,
            METHODNAME= 31,
            ARRAYNAME= 32,
            IDENTIFIER= 33,
            INTEGER = 34,
            NUMBER  = 35,
            STRING  = 36,
            COMPARE = 37,
            EQUALS  = 38,
            EOF     = 39,
            DOT     = 40,
            BOOLEAN = 41,
            UNARY   = 42,
            WHITESPACE= 43,
            COMMENT = 44
    }

    public class Token
    {
        private int startpos;
        private int endpos;
        private string text;
        private object value;

        public int StartPos { 
            get { return startpos;} 
            set { startpos = value; }
        }

        public int Length { 
            get { return endpos - startpos;} 
        }

        public int EndPos { 
            get { return endpos;} 
            set { endpos = value; }
        }

        public string Text { 
            get { return text;} 
            set { text = value; }
        }

        public object Value { 
            get { return value;} 
            set { this.value = value; }
        }

        public TokenType Type;

        public Token()
            : this(0, 0)
        {
        }

        public Token(int start, int end)
        {
            Type = TokenType._UNDETERMINED_;
            startpos = start;
            endpos = end;
            Text = ""; // must initialize with empty string, may cause null reference exceptions otherwise
            Value = null;
        }

        public void UpdateRange(Token token)
        {
            if (token.StartPos < startpos) startpos = token.StartPos;
            if (token.EndPos > endpos) endpos = token.EndPos;
        }

        public override string ToString()
        {
            if (Text != null)
                return Type.ToString() + " '" + Text + "'";
            else
                return Type.ToString();
        }
    }

    #endregion
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class ParseError: Exception
    {
        private int _idx;
        public int Idx
        {
            get
            {
                return _idx;
            }
        }
        private String incorrStr;

        public String IncorrStr
        {
            get
            {
                return incorrStr;
            }
        }

        public ParseError(String msg, String rem, int index): base(msg)
        {
            incorrStr = rem; 
            _idx = index;
        }

    }

    class Parser
    {
        private string id;
        private int state;
        private CharChain chain;

        private List<ParseError> errors;

        public List<ParseError> GetErrors()
        {
            return errors;
        }

        public bool Parse (CharChain c)
        {
            chain = c;
            state = 1;
            id = "";

            errors = new List<ParseError>();

            chain.SkipSpaces();

            while (state != 13)
            {
                switch (state)
                {
                    case 1:
                        state1();
                        break;

                    case 2:
                        state2();
                        break;

                    case 3:
                        state3();
                        break;

                    case 4:
                        state4();
                        break;

                    case 5:
                        state5();
                        break;

                    case 6:
                        state6();
                        break;

                    case 7:
                        state7();
                        break;

                    case 8:
                        state8();
                        break;

                    case 9:
                        state9();
                        break;

                    case 10:
                        state10();
                        break;

                    case 11:
                        state11();
                        break;

                    case 12:
                        state12();
                        break;


                }
            }

            return true;
        }



        private void handleError(string eMess, string removed, Character  c)
        {
            errors.Add(new ParseError(eMess, removed, c.Idx));
        }

        private bool tryStop()
        {
            char next = chain.Next().Char;
            
            if (next == '\0' || next == ';')
            {
                chain.GetNext();
                state = 13;
                return true;
            }

            return false;
        }

        private void state1()
        {
            Character c = chain.GetNext();

            if(isLetter(c.Char))
            {
                state = 2;

                id += c.Char;
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while(!isLetter(chain.Next().Char))
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Ожидалось ключевое слово const.", remStr, firstIncorrect);

            }
        }

        private void state2()
        {
            Character c = chain.GetNext();
            if(isLetter(c.Char))
            {
                state = 2;

                id += c.Char;
            } 
            else if (c.Char == ' ') {
                state = 3;

                if(!id.Equals("const"))
                {
                    handleError("Ожидалось ключевое слово const.", null, c);
                }
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while (!isLetter(chain.Next().Char) && chain.Next().Char != ' ')
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Неожиданный символ: '" + firstIncorrect.Str + "'.", remStr, firstIncorrect);
            }
        }

        private void state3()
        {
            Character c = chain.GetNext();
            if(isLetter(c.Char))
            {
                state = 4;
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while (!isLetter(chain.Next().Char))
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Неожиданный символ: '" + firstIncorrect.Str + "'.", remStr, firstIncorrect);
            }
        }

        private void state4()
        {
            Character c = chain.GetNext();
            if(c.Char == '=')
            {
                state = 5;
            }
            else if (isLetter(c.Char))
            {
                state = 4;
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while (!isLetter(chain.Next().Char) && chain.Next().Char != '=')
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Неожиданный символ: '" + firstIncorrect.Str + "'.", remStr, firstIncorrect);
            }
        }

        private void state5()
        {
            Character c = chain.Next();
            if(c.Char == '+' || c.Char == '-')
            {
                state = 6;
                chain.GetNext();
            }
            else
            {
                state = 6;
            }
        }

        private void state6()
        {
            Character c = chain.GetNext();
            if(c.Char >= '0' && c.Char <= '9')
            {
                state = 7;
            }
            else if (c.Char == '.')
            {
                state = 8;
            }
            else if (c.Char == 'E')
            {
                state = 10;
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while (!isDigit(chain.Next().Char) && chain.Next().Char != 'E' && chain.Next().Char != '.')
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Неожиданный символ: '" + firstIncorrect.Str + "'.", remStr, firstIncorrect);
            }
        }

        private void state7()
        {
            Character c = chain.GetNext();
            if(c.Char >= '0' && c.Char <= '9')
            {
                state = 7;
            }
            else if(c.Char == '.')
            {
                state = 8;
            }
            else if (c.Char == 'E')
            {
                state = 10;
            }
            else if(c.Char == ';')
            {
                state = 13;
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while (!isDigit(chain.Next().Char) && chain.Next().Char != '.' && chain.Next().Char != ';')
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Неожиданный символ: '" + firstIncorrect.Str + "'.", remStr, firstIncorrect);
            }
        }

        private void state8()
        {
            Character c = chain.GetNext();
            if (c.Char >= '0' && c.Char <= '9')
            {
                state = 9;
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while (!isDigit(chain.Next().Char))
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Неожиданный символ: '" + firstIncorrect.Str + "'.", remStr, firstIncorrect);
            }
        }

        private void state9()
        {
            Character c = chain.GetNext();
            if (c.Char >= '0' && c.Char <= '9')
            {
                state = 9;
            }
            else if (c.Char == 'E')
            {
                state = 10;
            }
            else if (c.Char == ';')
            {
                state = 13;
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while (!isDigit(chain.Next().Char) && chain.Next().Char != 'E' && chain.Next().Char != ';')
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Неожиданный символ: '" + firstIncorrect.Str + "'.", remStr, firstIncorrect);
            }
        }

        private void state10()
        {
            Character c = chain.GetNext();
            if (c.Char >= '0' && c.Char <= '9')
            {
                state = 12;
            }
            else if (c.Char == '-' || c.Char == '+')
            {
                state = 11;
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while (!isDigit(chain.Next().Char) && chain.Next().Char != '-' && chain.Next().Char != '+')
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Неожиданный символ: '" + firstIncorrect.Str + "'.", remStr, firstIncorrect);
            }
        }

        private void state11()
        {
            Character c = chain.GetNext();
            if (c.Char >= '0' && c.Char <= '9')
            {
                state = 12;
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while (!isDigit(chain.Next().Char))
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Неожиданный символ: '" + firstIncorrect.Str + "'.", remStr, firstIncorrect);
            }
        }

        private void state12()
        {
            Character c = chain.GetNext();
            if (c.Char >= '0' && c.Char <= '9')
            {
                state = 12;
            }
            else if (c.Char == ';')
            {
                state = 13;
            }
            else
            {
                String remStr = "";
                Character firstIncorrect = c;

                while (!isDigit(chain.Next().Char) && chain.Next().Char != ';')
                {
                    if (tryStop()) break;
                    remStr += c.Char;
                    c = chain.GetNext();
                }
                remStr += c.Char;
                handleError("Неожиданный символ: '" + firstIncorrect.Str + "'.", remStr, firstIncorrect);
            }
        }

        private bool isLetter(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        private bool isDigit (char c)
        {
            return (c >= '0' && c <= '9');
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compiler
{
    class Character
    {
        private char _c;
        public char Char
        {
            get
            {
                return _c;
            }
        }

        public String Str
        {
            get
            {
                if (_c == '\0')
                {
                    return "<конец файла>";
                }
                else if (_c == '\n')
                {
                    return "<новая строка>";
                }
                else
                {
                    return "" + _c;
                }
            }
        }


        private int _idx;

        public int Idx
        {
            get
            {
                return _idx;
            }
        }

        public Character(char c, int idx)
        {
            _c = c;
            _idx = idx;
        }
    }


    class CharChain
    {
        private char[] chars;
        private int index;
         
        public CharChain(string text)
        {
            chars = text.ToCharArray();
            index = 0;
        }

        public Character GetNext()
        {
            if(index == chars.Length)
            {
                return new Character('\0', index);
            }

            Character result = new Character(chars[index], index);
            index++;

            if (isSpace(result.Char))
            {
                SkipSpaces();
            }

            return result;
        }

        private bool isSpace(char c)
        {
            return (c == ' ' || c == '\t' || c == '\r' || c == '\n');
        }

        public void SkipSpaces()
        {
            while (index < chars.Length && isSpace(chars[index]))
            {
                index++;
            }
        }

        public Character Next()
        {
            if (index == chars.Length)
            {
                return new Character('\0', index);
            }

            return new Character(chars[index], index);
        }
    }
}

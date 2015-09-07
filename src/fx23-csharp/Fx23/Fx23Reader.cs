using System;
using System.Collections.Generic;
using System.Text;

namespace Mgen.Fx23
{
    public abstract class Fx23Reader
    {
        public int Index { get; private set; }
        public int ColumnIndex { get; private set; }
        public int LineIndex { get; private set; }
        public int VisibleIndex { get; private set; }
        public abstract int Length { get; }

        char _prevChar;

        //Options
        public bool CollectLineInfo { get; set; }

        //ctor
        protected Fx23Reader()
        {
        }

        public virtual bool HasNext()
        {
            return Index < Length;
        }

        public virtual char Peek()
        {
            return PeekOverride();
        }

        public virtual char Next()
        {
            var next = NextOverride();
            if (CollectLineInfo)
            {
                switch (next)
                {
                    case '\r':
                        LineIndex++;
                        ColumnIndex = 0;
                        break;
                    case '\n':
                        if (_prevChar != '\r')
                        {
                            // checking for \r\n
                            LineIndex++;
                            ColumnIndex = 0;
                        }
                        break;
                    default:
                        {
                            ColumnIndex++;
                            VisibleIndex++;
                            break;
                        }
                }
            }//CollectLineInfo
            _prevChar = next;
            Index++;
            return next;
        }

        public abstract void Mark();

        public abstract string Collect();

        protected abstract char NextOverride();
        protected abstract char PeekOverride();
    }
}

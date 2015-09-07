using System;
using System.Collections.Generic;
using System.Text;

namespace Mgen.Fx23
{
    public class Fx23StringReader : Fx23Reader
    {
        int _markIndex;
        int _length; 
        string _innerString;

        public Fx23StringReader(string str)
        {
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }

            _innerString = str;
            _length = str.Length;
        }

        public override int Length
        {
            get { return _length; }
        }

        protected override char NextOverride()
        {
            return _innerString[Index];
        }

        protected override char PeekOverride()
        {
            return NextOverride();
        }

        public override void Mark()
        {
            _markIndex = Index;
        }

        public override string Collect()
        {
            if (_markIndex == Index)
                return null;
            return _innerString.Substring(_markIndex, Index - _markIndex);
        }
    }
}

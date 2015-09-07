using System;
using System.Collections.Generic;
using System.Text;

namespace Mgen.Fx23
{
    public static class Fx23ReaderExtension
    {
        public static string CollectWhile(this Fx23Reader stream, Predicate<char> predicate)
        {
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }

            stream.Mark();
            while (stream.HasNext() && predicate(stream.Peek()))
            {
                stream.Next();
            }
            return stream.Collect();
        }

        public static int SkipWhile(this Fx23Reader stream, Predicate<char> predicate)
        {
            int count = 0;
            if (predicate == null)
            {
                throw new ArgumentNullException("predicate");
            }
            while (stream.HasNext() && predicate(stream.Peek()))
            {
                stream.Next();
                count++;
            }
            return count;
        }

        public static int MoveToContent(this Fx23Reader stream)
        {
            return stream.SkipWhile(c => Char.IsWhiteSpace(c));
        }

        public static int SkipLine(this Fx23Reader stream)
        {
            var re = stream.SkipWhile(c => c != '\n' && c != '\r');
            stream.MoveOverNewline();
            return re;
        }

        public static string CollectLine(this Fx23Reader stream)
        {
            var re = stream.CollectWhile(c => c != '\n' && c != '\r');
            stream.MoveOverNewline();
            return re;
        }

        public static int MoveOverNewline(this Fx23Reader stream)
        {
            if (stream.HasNext())
            {
                var c = stream.Peek();
                if (c == '\r')
                {
                    stream.Next();

                    if (stream.HasNext() && stream.Peek() == '\n')
                    {
                        stream.Next();
                        return 2;
                    }
                    return 1;
                }
                else if (c == '\n')
                {
                    stream.Next();
                    return 1;
                }
            }
            return 0;
        }
    }
}

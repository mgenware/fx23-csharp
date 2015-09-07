using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mgen.Fx23;

namespace fx23_csharp
{
    class Program
    {

        static void Main(string[] args)
        {
            // the string we need to scan
            var str = "   abcdef\r\na b c\nMgen123abc";

            // initialize the reader
            var reader = new Fx23StringReader(str);
            // counts LineIndex and ColumnIndex during each read operation
            reader.CollectLineInfo = true;

            Console.WriteLine("Move to content");
            reader.MoveToContent();
            PrintInfo(reader);
            /*
             Reader position:
             "   abcdef\r\na b c\nMgen123abc"
              ---|------ - ------ ----------

             Output:
             Move to content
             Current char: 'a'
             Line: 1
             Column: 4
             Index(without newline): 3
             Index: 3

             */

            Console.WriteLine("Read until 'e'");
            Console.WriteLine("Result -> {0}", reader.CollectWhile(c =>
            {
                return c != 'e';
            }));
            PrintInfo(reader);
            /*
             Reader position:
             "   abcdef\r\na b c\nMgen123abc"
              ---====|-- - ------ ----------

             Output:
             Read until 'e'
             Result -> abcd
             Current char: "e"
             Line: 1
             Column: 8
             Index(without newline): 7
             Index: 7
             */

            Console.WriteLine("Get remaining characters in current line");
            Console.WriteLine("Result -> {0}", reader.CollectLine());
            PrintInfo(reader);
            /*
             Reader position:
             "   abcdef\r\na b c\nMgen123abc"
              -------==- - |----- ----------

             Output:
             Get remaining characters in current line
             Result -> ef
             Current char: 'a'
             Line: 2
             Column: 1
             Index(without newline): 9
             Index: 11

             */

            Console.WriteLine("Skip the whole line");
            reader.SkipLine();
            PrintInfo(reader);
            /*
             Reader position:
             "   abcdef\r\na b c\nMgen123abc"
              ---------- - ------ |---------

             Output:
             Skip the whole line
             Current char: 'M'
             Line: 3
             Column: 1
             Index(without newline): 14
             Index: 17

             */

            Console.WriteLine("Skip to first number");
            reader.SkipWhile(c =>
            {
                return !Char.IsNumber(c);
            });
            PrintInfo(reader);
            /*
             Reader position:
             "   abcdef\r\na b c\nMgen123abc"
              ---------- - ------ ----|-----

             Output:
             Skip to first number
             Current char: '1'
             Line: 3
             Column: 5
             Index(without newline): 18
             Index: 21

             */

            Console.WriteLine("Read all numbers");
            Console.WriteLine(reader.CollectWhile(c => {
                return Char.IsNumber(c);
            }));
            PrintInfo(reader);
            /*
             Reader position:
             "   abcdef\r\na b c\nMgen123abc"
              ---------------------------|--

             Output:
             Read all numbers
             123
             Current char: 'a'
             Line: 3
             Column: 8
             Index(without newline): 21
             Index: 24

             */

            Console.WriteLine("Read to end using mark and collect");
            reader.Mark();
            while (reader.HasNext())
            {
                reader.Next();
            }
            Console.WriteLine("Result -> {0}", reader.Collect());
            /*
             Reader position:
             "   abcdef\r\na b c\nMgen123abc"
              ---------------------------===

             Output:
             Result -> abc

             */

        }
        
        static void PrintInfo(Fx23Reader reader)
        {
            string curChar;
            if (reader.HasNext())
            {
                curChar = reader.Peek().ToString();
            }
            else
            {
                curChar = "End of string";
            }
            Console.WriteLine("Current char: '{0}'\nLine: {1}\nColumn: {2}\nIndex(without newline): {3}\nIndex: {4}\n",
                  curChar,
                  reader.LineIndex + 1,
                  reader.ColumnIndex + 1,
                  reader.VisibleIndex,
                  reader.Index);
        }
    }
}

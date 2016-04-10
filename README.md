# Fx23
Text scanner, available in [Node.js](https://github.com/mgenware/fx23-node), [C#](https://github.com/mgenware/fx23-csharp) and [Objective-C](https://github.com/mgenware/fx23-objc).

# API
## Fx23Reader Class
Base scanner class.
### Properties
* `CollectLineInfo` counts lineIndex and columnIndex during each read operation, default is false.
* `Index` the index position of current character.
* `ColumnIndex` zero-based column number of current character at current line.
* `LineIndex` zero-based line number of current character.
* `VisibleIndex` the index position of current character without newline characters.
* `Length` total length of the string.

### Methods
* `HasNext` returns false if no more character to read.
* `Peek` returns the next character without moving the internal index.
* `Next` returns the next character and move the internal index forward.
* `Mark` marks a flag at current position.
* `Collect` returns a sub-string from last marked position to current position.
* `NextOverride` implementated by subclass.
* `PeekOverride` implementated by subclass.

## Fx23StringReader Class
A concret class derived from Fx23Reader, use this to create a scanner from a string.

## Fx23ReaderExtension Class
This class adds some useful extension methods to Fx23Reader.
### Methods
* `CollectWhile` moves forward while condition is true, and returns the string scanned.
* `SkipWhile` moves forward while condition is true.
* `MoveToContent` moves to next non-whitespace character.
* `SkipLine` moves to next line.
* `CollectLine` moves to next line and returns current line.

# Example
```csharp
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

```

# License
[MIT](LICENSE)

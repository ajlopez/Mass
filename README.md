# Mass

Simple language, interpreter written in C#.

Work in Progress

## Design Notes

Mass takes few key ideas from different languages.

- Interpreted Language
- No typed variables
- Ruby syntax, but with explicit use of parentheses to call functions
- 'define' for function definition
- 'function' for anonymous functions
- End of line as command separation
- Explicit 'end' to close block statements
- Functions as first class citizens
- require('module') a la Node.js
- Dynamic objects, as in Javascript or Python. You can assign any property at any time
- No ; to separate commands nor { } to group statements
- No access to global variables inside a function
- Variable scope: the function
- Access to underlying classes and objects (.NET class libraries)

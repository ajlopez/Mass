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
- require('module') a la Node.js (WIP)
- Dynamic objects, as in Javascript or Python. You can assign any property at any time
- No ; to separate commands nor { } to group statements
- No access to global variables inside a function (WIP)
- Variable scope: the function (WIP)
- Access to underlying classes and objects (.NET class libraries)

## Expressions

Constants
```
"foo"
123
```

Simple arithmetic
```
a+b
12*a
a/b
a/(1+b)
```
The operators `+`, `-`, `*`, `/` are supported.

## Commands

if
```
if <expr>
	<command>
	...
end
```

while
```
while <expr>
	<command>
	...
end
```

define
```
define <name>([<name>, ... ])
	<command>
	...
end
```

for each
```
for <name> in <expr>
	<command>
	...
end
```

class and new
```
class Dog
	define initialize(name)
		self.name = name
	end
	
	define sayhello()
		println("warf!")
	end
end

nero = new Dog("Nero")
```

## Samples

Hello world
```
println("Hello, world")
```

For each
```
for k in [1,2,3,4,5,6]
	println(k)
end
```

Simple function
```
define add(a, b)
	a+b
end
```
A function returns the value of the last command

You can use a return command
```
define add(a, b)
	return a+b
end
```

Current directory files, using native object
```
dirinfo = new System.IO.DirectoryInfo(".")

for fileinfo in dirinfo.GetFiles()
	println(fileinfo.Name)
end
```

## Run

The executable mass.exe is the interpreter (compiled in Mass.Console project)
```
mass <file> [<file> ... ]
```

## To be done

- Exceptions with try/catch
- Class inheritance
- Object definition
- Real numbers
- Characters
- `require(<modulename>)`
- `continue` and `break` in loops
- `for <name> = <expr> to <expr> [step <expr>]`
- REPL

## Origin of Name

Mass language is dedicated to [@MArtinSaliaS](http://twitter.com/martinsalias)

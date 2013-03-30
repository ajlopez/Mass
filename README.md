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

Dynamic object
```
adam = { name = "Adam", age = 800 }
empty = {}
```

Array
```
numbers = [ 1, 2, 3 ]
empty = []
```

The expression can be continued in other line, after `[`, `{`, `,`
```
numbers = [ 
	1,
	2,
	3
	]
	
adam = {
	name = "Adam",
	age = 800
}
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

assignment
```
a = 123
b = "foo"
dog = {}
dog["name"] = "Nero"
numbers = [1, 2, 3]
numbers[0] = 0
```

if
```
if <expr>
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

for
```
for <name> = <expr> to <expr> [step <expr>]
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

continue and break for `for` and `while`
```
while <expr>
	...
	continue
	...
end

for ...
	...
	break
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

line comments with `#`
```
# this is a comment
a = 1 # a simple assignment
```

## Predefined functions

print and println
```
println("Hello, world")
println("Hello", "world") # output: two lines

print("Hello, ")
println("world")
```

### Require

To import a module
```
mymodule = require("mymodule")
```
Variants
```
mymodule = require("mymodule.ms")
mymodule = require("./mymodule")
mymodule = require("/libs/mymodule")
```

A module is a `.ms` file, executed in its own context. The value returned by `require` function is the value
returned by module code. If no explicit return, the value of the last command is returned

Example:
```
define add(a, b)
	a+b
end

{
	one = 1,
	two = 2,
	three = 3,
	foo = add
}
```

The return value is an object containing `one`, `two`, `three` and `foo`, the last one pointing to internal 
defined function `add`

The lookup module algorithm:

- If the name has an absolute path, search only in that path
- Search in current file directory
- If file exists with extensions `.ms`, that file is loaded
- If directory exists named as the module, the file `init.ms` in that directory is loaded
- If file does not exists, the same search is repeated in `modules` subdirectory in current file directory
- If file does not exists, the search is repeated at `modules` subdirectory in parent directory, and so on, up to top directory
- If file does not exists, the search is repeated in `node_modules` subdirectory in current file directory
- If file does not exists, the search is repeated at `node_modules` subdirectory in parent directory, and so on, up to top directory
- If the file is not found, search in current executing assembly directory instead of current file directory

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

For
```
for k = 1 to 6
	println(k)
end

for k = 1 to 6 step 2
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
- else in if
- REPL

## Origin of Name

Mass language is dedicated to [@MArtinSaliaS](http://twitter.com/martinsalias)

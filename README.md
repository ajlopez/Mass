# Mass

Simple language, interpreter written in C#.

Work in Progress

## Design Notes

Mass takes few key ideas from different languages.

- Interpreted Language
- No typed variables
- Local variable declaration
- Ruby syntax, but with explicit use of parentheses to call functions
- 'define' for function definition
- 'function' for anonymous functions
- End of line as command separation
- Explicit 'end' to close block statements
- Functions as first class citizens
- require('module') a la Node.js
- Dynamic objects, as in Javascript or Python. You can assign any property at any time
- No ; to separate commands nor { } to group statements
- Access to underlying classes and objects (.NET class libraries)

## Development

```
git clone git://github.com/ajlopez/Mass.git
cd Mass
git submodule init
git submodule update
```

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

function expression
```
foo = function(a,b)
	a+b
end
```

## Variable scope

A variable set in file, has file scope:
```
a = 1
```

A variable with file scope, can be accessed from functions defined in the same file
```
a = 1

define foo()
	a+1
end

foo() # returns 2
```

If a variable is declared  in a function, it will have function scope
```
a = 1

define foo()
	a = 2
end

foo() # returns 2
a # it is 2
```
Instead
```
a = 1

define foo()
    var a
	a = 2
end

foo() # returns 2
a # it is still 1
```

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
[else
	<command>
	...
]
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

subclass and call super initialize
```
class Rectangle
	define initialize(width, height)
		self.width = width
		self.height = height
	end
	
	define area()
		self.width * self.height
	end
end

class Square extends Rectangle
	define initialize(side)
		super(side, side)
	end
end
```


line comments with `#`
```
# this is a comment
a = 1 # a simple assignment
```

## Predefined variables

The `global` variable is special: it's the only variable that is not a local one. It points to root context.
```
# global variable
global.a = 1

# file local variable
a = 3

define foo()
	# set global variable
	global.a = 2
	
	# create and set local variable with function scope
	# you cannot change a file variable value from function scope
    var a
	a = 4
end

define bar()
	# get file local variable value
	a
end

foo() # == 4
bar() # == 3
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
- Object definition
- Real numbers
- Characters
- REPL

## References

- [Expression as a Compiler](http://www.infoq.com/articles/expression-compiler)
- [Dynamic LINQ (Part 1: Using the LINQ Dynamic Query Library)](http://weblogs.asp.net/scottgu/dynamic-linq-part-1-using-the-linq-dynamic-query-library)
- [The binary operator Multiply is not defined for the types 'System.Int32' and 'System.Double'.](http://stackoverflow.com/questions/6884141/the-binary-operator-multiply-is-not-defined-for-the-types-system-int32-and-sy)

## Origin of Name

Mass language is dedicated to [@MArtinSaliaS](http://twitter.com/martinsalias)

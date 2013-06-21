
# file variable
var a
a = 1

result = foo() # invoking the function before reach definition

define foo()
	a = 2 
	var a # local variable
end

result


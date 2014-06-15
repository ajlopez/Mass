
# file variable
a = 1

result = foo() # invoking the function before reach definition

define foo()
	a = 2 # local variable
end

result


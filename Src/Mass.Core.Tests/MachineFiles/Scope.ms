
# module variable
a = 1

define foo()
	a # access module variable value
	a = 2 # new local variable
	a # access local variable value
	module.a # access module variable value
	a = 3 # change local variable
	module.a = 4 # change module variable
end

foo()


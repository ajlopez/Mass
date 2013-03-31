
global.a = 1
a = 3 # file local variable

define foo()
	global.a = 2 # global variable
end

foo()


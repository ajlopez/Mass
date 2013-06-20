
# global variable
global.a = 1

# file local variable, explicitly declared
var a
a = 3

define foo()
	# set global variable
	global.a = 2
	
	# set local variable in function
	var a
	a = 4
end

foo()


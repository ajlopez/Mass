
# global variable
global.a = 1

# file local variable
a = 3

define foo()
	# set global variable
	global.a = 2
	
	# set local variable in function
	a = 4
end

foo()



define foo(a,b)
	for k = 1 to b
		if k > a
			return k
		end
	end
	a+b
end

foo(1,4)


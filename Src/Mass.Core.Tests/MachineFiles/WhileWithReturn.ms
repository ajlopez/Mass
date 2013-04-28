
define foo(a,b)
	k = 1
	
	while k <= b
		if k > a
			return k
		end
		k = k + 1
	end
	
	a+b
end

foo(1,4)


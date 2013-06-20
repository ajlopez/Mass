
define equal(expected, value)
	if expected == value
		return
	end
	
	throw "not equal"
end

{
	equal = equal
}

total = 0

for k = 1 to 3
	if k == 2
		break
	end
	total = total + k
end

total2 = 0

for k in [1,2,3]
	if k == 2
		break
	end
	total2 = total2 + k
end

total3 = 0

k = 0

while k < 3
	k = k + 1
	if k == 2
		break
	end
	total3 = total3 + k	
end


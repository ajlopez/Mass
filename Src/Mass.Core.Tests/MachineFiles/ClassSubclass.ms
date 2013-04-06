
class Rectangle
	define initialize(width, height)
		self.width = width
		self.height = height
	end
	
	define area()
		self.width * self.height
	end
end

class Square extends Rectangle
	define initialize(side)
		super(side, side)
	end
end

square = new Square(10)
area = square.area()


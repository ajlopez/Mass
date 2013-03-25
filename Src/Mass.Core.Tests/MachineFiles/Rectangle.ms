
class Rectangle
	define initialize(width, height)
		self.width = width
		self.height = height
	end
	
	define area()
		self.width * self.height
	end
	
	define scale(factor)
		self.width = self.width * factor
		self.height = self.height * factor
	end
end

rect = new Rectangle(2, 3)
area = rect.area()
rect.scale(2)
area2 = rect.area()





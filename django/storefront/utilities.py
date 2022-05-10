from typing import Counter


class Pet:
    def __init__(self,name,age):
        self.age=age
        self.name=name

    def speak():
        print("I do not know")
    def show(self):
        print(f"I am {self.name} and I am {self.age} years old.")
class Cat(Pet):
    def __init__(self,name,age,color):
        super().__init__(name,age)
        self.color=color
    def speak(self):
        print("Meow")
class Dog(Pet):
    def speak(self):
        print("Bark")

class Eagle(Pet):
    def __init__(self,name,age,beak):
        super().__init__(name,age)
        self.beak = beak
    def speak(self):
        print("caw")

    def fly():
        print("I can fly")
myEagle = Eagle("L", 15, "Curve") 
myEagle.show()
myEagle.speak()
from Core.Entities.TermUnit import TermUnit


class Token:
    def __init__(self, value, unit: TermUnit):
        self.value = value
        self.unit = unit
    
    def __str__(self):
        return  str(self.unit) +" = ("+ self.value + ")"
    
    def __repr__(self):
        return self.__str__()
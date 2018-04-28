from Core.Entities.Premise import Premise

class Production:
    def __init__(self, premise: Premise, right_index):
        self.premise = premise
        self.index = right_index
        self.term = premise.right[right_index]
        self.id = (premise.left, right_index)
    
    def __str__(self):        
        return str(self.premise.left) + " -> " + str(' ').join([str(x) for x in self.term])
    
    def __repr__(self):
        return '\n' + str(self)
from Core.Entities.Premise import Premise


class Reduction:
    def __init__(self, premise: Premise):
        self.id = premise.left
        self.premise = premise
    
    def __str__(self):
        return "R" + str(self.id)

    def __repr__(self):
        return str(self)
        
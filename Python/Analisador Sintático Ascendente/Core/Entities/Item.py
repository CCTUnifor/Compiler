from Core.Entities.Premise import Premise


class Item:
    def __init__(self, premise: Premise, cursor=0):
        self.cursor = cursor
        self.premise = premise

    def get_term_unit(self):
        return self.premise.right[self.cursor]
    
    def is_complete(self):
        return len(self.premise.right) <= self.cursor

    def get_next(self):
        return Item(self.premise, self.cursor + 1)
    
    def __str__(self):
        retorno = self.premise.left + " -> "

        for termUnit in self.premise.right:
            if(self.get_term_unit() is termUnit):
                retorno += "."

            retorno += termUnit.text

        return retorno
from Core.Entities.Premise import Premise


class Item:
    def __init__(self, premise: Premise, cursor=(0,0)):
        """
        cursor (x,y)
        x = premise.right index (some premises have "|" operator, that means two diferents premises in one)
        y = premise.right[x] index to know what unit the cursor is actually pointing
        """
        self.cursor = cursor
        self.premise = premise

    def get_term_unit(self):
        return self.premise.right[self.cursor[0]][self.cursor[1]]
    
    def is_complete(self):
        return len(self.premise.right[self.cursor[0]]) <= self.cursor[1]

    def get_next(self):
        return Item(self.premise, (self.cursor[0], self.cursor[1]+ 1))
    
    def __str__(self):
        retorno = self.premise.left + " -> "

        for termUnit in self.premise.right[self.cursor[0]]:
            if(not self.is_complete() and self.get_term_unit() is termUnit):
                retorno += "."

            retorno += str(termUnit)
        
        if(self.is_complete()):
            retorno += "."

        return retorno
    
    def __repr__(self):
       return str(self)
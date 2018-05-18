from Core.Entities.TermUnit import TermUnit


class Premise:
    RE = r'\s*(.+)\s*->\s*(.+)\s*'

    def __init__(self, left, text):
        self.left = left
        self.right = []        
        self.text = text

        self.lFix = 20

        self.first = set()
        self.follow = set()
    
    def __str__(self):
        return self.left #+ " -> " + str([[j.text for j in tunit] for tunit in self.right])
    
    def __repr__(self):
        return self.left
    
    def strFirst(self):
        x = ("first("+self.left + ")").ljust(self.lFix)
        return x + " => " + str(self.first)

    def strFollow(self):
        x = ("follow("+self.left + ")").ljust(self.lFix)
        return x + " => " + str(self.follow)



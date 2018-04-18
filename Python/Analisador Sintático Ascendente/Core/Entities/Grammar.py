from Core.Services.TextGrammar import TextToGrammar
from Core.Entities.TermUnit import TermUnit


class Grammar:
    STREAM_END_UNIT = TermUnit(TermUnit.STREAM_END, TermUnit.STREAM_END)
    
    def __init__(self, text):
        self.textGrammar = TextToGrammar(text)

        self.Premises = self.textGrammar.get_premises()

        self.StartSimbol = self.textGrammar.getStartSimbol()
        self.StartSimbolUnit = self.textGrammar.getStartSimbolUnit()
        
        self.Alphabet = [unit for unit in self.textGrammar.TermUnits if unit.type is TermUnit.TERMINAL]
        self.NonTerminals = [unit for unit in self.textGrammar.TermUnits if unit.type is TermUnit.NONTERMINAL]       

    def get_term(self, termString):
            for i in self.Premises:
                if(i.left == termString):
                    return i

    def __str__(self):
        return str(self.textGrammar.Premises)

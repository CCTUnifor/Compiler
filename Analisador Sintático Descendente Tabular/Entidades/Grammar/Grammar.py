from Entidades.Grammar.TextGrammar import TextGrammar
from Entidades.Term import TermUnit


class Grammar:

    def __init__(self, text):
        self.textGrammar = TextGrammar(text)

        self.Terms = self.textGrammar.get_terms()

        self.StartSimbol = self.textGrammar.getStartSimbol()
        
        self.Alphabet = [unit for unit in self.textGrammar.TermUnits if unit.type is TermUnit.TERMINAL]
        self.NonTerminals = [unit for unit in self.textGrammar.TermUnits if unit.type is TermUnit.NONTERMINAL]       

    def __str__(self):
        return str(self.textGrammar.Premises)
# ɛ
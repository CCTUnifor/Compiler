from Entidades.Grammar.TextGrammar import TextGrammar


class Grammar:

    def __init__(self, text):
        self.textGrammar = TextGrammar(text)
        
        self.Terms = self.textGrammar.get_terms()

        self.StartSimbol = self.textGrammar.getStartSimbol()

        self.AnalysisTable = {}

    def __str__(self):
        return str(self.textGrammar.Premises)
# ɛ
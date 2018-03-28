import re


class Term:
    RE = r'\s*(.+)\s*->\s*(.+)\s*'
    def __init__(self, left, term, text):
        self.left = left
        self.term = term

class Grammar:
    RE = r'N\s*=\s*{\n*(.+)\n*}\s*\n*Alf\s*=\s*{\n*(.+)\n*}\s*\n*S\s*=\s*{\n*(.+)\n*}\s*\n*P\s*=\s*{\n*((?:.*\n*)+)}\n*'

    def __init__(self, text):
        self.text = text
        self.matched = re.match(Grammar.RE, text)
        self.separator = ','

        self.NonTerminals = self.matched.group(1).split(self.separator)
        self.Alphabet = self.matched.group(2).split(self.separator)
        self.StartSimbol = self.matched.group(3).split(self.separator)
        self.Premises = self.matched.group(4)
        self.Terms = []

    def makeTerms(self):
        premises = self.Premises.split('\n')
        patternObj = re.compile(Term.RE)
        for premise in premises:
            print(premise)
            matched = patternObj.match(premise)
            self.Terms.append(Term(matched.group(1), matched.group(2), premise))

    def getTerms(self):
        if(len(self.Terms) is 0):
            self.makeTerms()
        
        return self.Terms
    
import re


class Term:
    RE = r'\s*(.+)\s*->\s*(.+)\s*'

    def __init__(self, left, term, text):
        self.left = left
        self.right = term
        self.text = text
    
    def __str__(self):
        return self.left + " -> " + str(self.right)


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

        self.NTInserteds = []

        self.Terms = []

    def validateLeft(self, leftHand):
        if(leftHand not in self.NonTerminals):
            raise Exception('Não terminal '+leftHand+' não foi definido em N')
    
    def addTerm(self, leftHand, rightHand, premise):
        termo = Term(leftHand, [], premise)
        if(rightHand.find('|') >= 0):
            ors = rightHand.split('|')
            for orOption in ors:
                termo.right.append(orOption.strip())
        else:
            termo.right.append(rightHand)

                
        self.Terms.append(termo)

    def makeTerms(self):
        premises = self.Premises.split('\n')
        patternObj = re.compile(Term.RE)
        for premise in premises:
            if(len(premise.strip()) is 0): continue

            termMatch = patternObj.match(premise)
            leftHand = termMatch.group(1).strip()
            rightHand = termMatch.group(2).strip()

            self.validateLeft(leftHand)

            self.addTerm(leftHand, rightHand, premise)

    def getTerms(self):
        if(len(self.Terms) is 0):
            self.makeTerms()
        
        return self.Terms
# ɛ
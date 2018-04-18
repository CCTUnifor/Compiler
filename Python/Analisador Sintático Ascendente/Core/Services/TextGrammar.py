import re
from Core.Entities.Premise import Premise
from Core.Entities.Premise import TermUnit


class TextToGrammar:
    EMPTY = 'ɛ'
    EMPTY_UNIT = TermUnit(TermUnit.EMPTY, EMPTY)
    RE = r'N\s*=\s*{\n*(.+)\n*}\s*\n*Sigma\s*=\s*{\n*(.+)\n*}\s*\n*S\s*=\s*{\n*(.+)\n*}\s*\n*P\s*=\s*{\n*((?:.*\n*)+)}\n*'
    
    def __init__(self, text):        
        self.text = text
        self.matched = re.match(TextToGrammar.RE, text)
        self.separator = ','

        self.NonTerminals = self.matched.group(1).split(self.separator)
        self.Alphabet = self.matched.group(2).split(self.separator)
        self.StartSimbol = self.matched.group(3).strip()
        self.Premises = self.matched.group(4)

        self.Premises = []
        self.TermUnits = []
    
    def getStartSimbol(self):
        for item in self.get_premises():
            if(item.left == self.StartSimbol):
                return item

    def getStartSimbolUnit(self):
        return self.TermUnits[0]            
    
    def get_premises(self):
        if(len(self.Premises) is 0):
            self.make_terms()
        
        return self.Premises
    
    def make_terms(self):
        premises = self.Premises.split('\n')
        patternObj = re.compile(Premise.RE)

        for premise in premises:
            if(len(premise.strip()) is 0): continue

            termMatch = patternObj.match(premise)
            leftHand = termMatch.group(1).strip()
            rightHand = termMatch.group(2).strip()

            self.validate_left(leftHand)

            self.rank_term(leftHand, rightHand, premise)
    
    def validate_left(self, leftHand):
        if(leftHand not in self.NonTerminals):
            raise Exception('Não terminal '+leftHand+' não foi definido em N')
    
    def rank_term(self, leftHand, rightHand, premise):
        term = Premise(leftHand, premise)
        
        self.rank_stream(leftHand)
        
        if(rightHand.find('|') >= 0):
            ors = rightHand.split('|')
            for orOption in ors:
                term.right.append(self.rank_stream(orOption.strip()))
        else:
            term.right.append(self.rank_stream(rightHand))
                
        self.Premises.append(term)
    
    def getUnit(self, item):
        return next((x for x in self.TermUnits if x.text == item), None)
    
    def getUnits(self, item):
        return [x for x in self.TermUnits if (x.text.find(item,0)+1)]
    
    def rank_stream(self, streamText):
        streamArray = []

        stream = streamText.split(' ')

        for item in stream:            
            if(item is ''):
                continue
            else:
                unit = self.getUnit(item)

                if(unit):
                    streamArray.append(unit)

                else:
                    if(item in self.Alphabet):
                        unit = TermUnit(TermUnit.TERMINAL, item)

                    elif(item in self.NonTerminals):
                        unit = TermUnit(TermUnit.NONTERMINAL, item)

                    elif(item == self.EMPTY):
                        unit = self.EMPTY_UNIT
                    
                    else:
                        raise Exception(item + " is not in N or Sigma")

                    streamArray.append(unit)
                    self.TermUnits.append(unit)                    

        return streamArray

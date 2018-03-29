import re


class Term:
    RE = r'\s*(.+)\s*->\s*(.+)\s*'
    TERMINAL = 'TERMINAL'
    NONTERMINAL = 'NON-TERMINAL'
    EMPTY = 'EMPTY'

    def __init__(self, left, text):
        self.left = left
        self.right = []        
        self.text = text

        self.first = []
        self.follow = []
    
    def __str__(self):
        return self.left + " -> " + str(self.right)


class Grammar:
    RE = r'N\s*=\s*{\n*(.+)\n*}\s*\n*Alf\s*=\s*{\n*(.+)\n*}\s*\n*S\s*=\s*{\n*(.+)\n*}\s*\n*P\s*=\s*{\n*((?:.*\n*)+)}\n*'
    EMPTY = 'epsilon'

    def __init__(self, text):
        self.text = text
        self.matched = re.match(Grammar.RE, text)
        self.separator = ','

        self.NonTerminals = self.matched.group(1).split(self.separator)
        self.Alphabet = self.matched.group(2).split(self.separator)
        self.StartSimbol = self.matched.group(3).split(self.separator)
        self.Premises = self.matched.group(4)

        self.Terms = []
        self.AnalysisTable = {}
    
    def build_grammar_matrix(self):
        self.get_terms()

        self.build_first()
        self.build_follow()

    def get_term(self, termString):
        for i in self.Terms:
            if(i.left == termString):
                return i

    def term_is_empty(self, term):
        for stream in term.right:
            for item in stream:
                if(item['type'] is Term.EMPTY):
                    return True

        return False

    @staticmethod
    def remove_empty(term_first):
        retorno = []
        for item in term_first:
            if(item['type'] is not Term.EMPTY):
                retorno.append(item)
        
        return retorno

    def first(self, stream):
        for item in stream:
            if(item['type'] is Term.TERMINAL or item['type'] is Term.EMPTY):
                return [item]

            elif(item['type'] is Term.NONTERMINAL):
                term = self.get_term(item['text'])
                if(self.term_is_empty(term)):
                    continue
                else:
                    return Grammar.remove_empty(self.first_of_term(term))

    def first_of_term(self, term):
        if(len(term.first) is not 0):
            return term.first

        first = term.first
        for stream in term.right:
            first += self.first(stream)
        
        return first        

    def build_first(self):
        for term in self.Terms:
            self.first_of_term(term)
    
    def build_follow(self):
        pass

    def validate_left(self, leftHand):
        if(leftHand not in self.NonTerminals):
            raise Exception('Não terminal '+leftHand+' não foi definido em N')
    
    def add_stream(self, termo, rightHand):
        streamArray = []
        accumulator = ''
        length = len(rightHand)

        stream = rightHand.split(' ')
        for item in stream:
            if(item is ''):
                continue
            elif(item in self.Alphabet):
                streamArray.append({'type':Term.TERMINAL, 'text':item})

            elif(item in self.NonTerminals):
                streamArray.append({'type':Term.NONTERMINAL, 'text':item})

            elif(item == Grammar.EMPTY):
                streamArray.append({'type':Term.EMPTY, 'text':item})
            
            else:
                raise Exception(item + " is not in N or Alf")
        # for i, charr in enumerate(rightHand):
        #     accumulator += charr

        #     if(charr is ' '):
        #         accumulator = ''

        #     if(accumulator in self.Alphabet):
        #         if(i+1 < length):
        #             if((accumulator + rightHand[i+1]) in self.Alphabet):
        #                 continue

        #         streamArray.append({'type':Term.TERMINAL, 'text':accumulator})
        #         accumulator = ''

        #     elif(accumulator in self.NonTerminals):
        #         if(i+1 < length):
        #             if((accumulator + rightHand[i+1]) in self.NonTerminals):
        #                 continue

        #         streamArray.append({'type':Term.NONTERMINAL, 'text':accumulator})
        #         accumulator = ''
            
        #     elif(accumulator == Grammar.EMPTY):
        #         streamArray.append({'type':Term.EMPTY, 'text':accumulator})
        #         accumulator = ''
                
        # if(accumulator is not ''):
        #     raise Exception(accumulator + " is not in N or Alf")

        termo.right.append(streamArray)

    def add_term(self, leftHand, rightHand, premise):
        term = Term(leftHand, premise)
        if(rightHand.find('|') >= 0):
            ors = rightHand.split('|')
            for orOption in ors:
                self.add_stream(term, orOption.strip())
                # term.right.append(orOption.strip())
        else:
            self.add_stream(term, rightHand)
            # term.right.append(rightHand)

                
        self.Terms.append(term)

    def make_terms(self):
        premises = self.Premises.split('\n')
        patternObj = re.compile(Term.RE)
        for premise in premises:
            if(len(premise.strip()) is 0): continue

            termMatch = patternObj.match(premise)
            leftHand = termMatch.group(1).strip()
            rightHand = termMatch.group(2).strip()

            self.validate_left(leftHand)

            self.add_term(leftHand, rightHand, premise)

    def get_terms(self):
        if(len(self.Terms) is 0):
            self.make_terms()
        
        return self.Terms

    
# ɛ
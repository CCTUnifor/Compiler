from collections import deque

from Entidades.Term import TermUnit
from Entidades.Term import Term
from Entidades.Grammar.TextGrammar import TextGrammar
from Entidades.Grammar.Grammar import Grammar
from Services.LexicAnalyzer import LexicAnalyzer


class TableService:    
    ErrorString = "Error"

    def __init__(self, grammar):
        self.grammar = grammar
        self.table = None

    def makeTable(self):
        self.table = {}
        
        for non_terminal in self.grammar.NonTerminals:
            self.table[non_terminal.text] = {Grammar.STREAM_END_UNIT.text: self.ErrorString}

            for terminal in self.grammar.Alphabet:
                self.table[non_terminal.text][terminal.text] = self.ErrorString            

    def build_table(self):
        self.makeTable()
         
        for term in self.grammar.Terms:
            for stream in term.right:
                termToStreamTuple = (term, stream)
                # print(stream)

                streamFirst = self.first(stream)

                for item in streamFirst:
                    if(item.type is TermUnit.TERMINAL):
                        self.table[term.left][item.text] = termToStreamTuple
                
                if(TextGrammar.EMPTY_UNIT in streamFirst):
                    if(Grammar.STREAM_END_UNIT in term.follow):
                        self.table[term.left][Grammar.STREAM_END_UNIT.text] = termToStreamTuple

                    for item in term.follow:
                        if(item.type is TermUnit.TERMINAL):
                            self.table[term.left][item.text] = termToStreamTuple
                    
                            
    def apply_follow_third_rule(self):
        """
        A -> Alfa B Beta, B.follow += A.follow
        Alfa is any stream
        Beta is a empty stream
        """
        calls = deque()

        for A in self.grammar.Terms:
            for stream in A.right:
                streamLen = len(stream)

                for index, unit in enumerate(stream):
                    if(unit.type is TermUnit.NONTERMINAL):
                        B = self.get_term(unit.text)

                        if(A is B):
                            continue

                        if(streamLen > index + 1):
                            beta = stream[index + 1::]

                            firstBeta = self.first(beta)

                            if(TextGrammar.EMPTY_UNIT not in firstBeta):
                                continue
                        
                        # B.follow |= A
                        calls.append((B, A))
        
        while(len(calls) is not 0):
            tupleBA = calls.popleft()
            B, A = tupleBA
        
            AinStack = next((x for x in calls if x[0] is A), None)

            if(AinStack):
                calls.append(tupleBA)
                continue

            # print(B.left,"___", A.left)

            B.follow |= A.follow

    def apply_follow_second_rule(self):
        """
        A -> Alfa B Beta, B.follow += first(Beta) - {ɛ}
        Alfa is any stream
        Beta is a not empty stream
        """
        for term in self.grammar.Terms:
            for stream in term.right:
                streamLen = len(stream)

                for index, unit in enumerate(stream):
                    if(unit.type is TermUnit.NONTERMINAL):
                        termB = self.get_term(unit.text)

                        if(streamLen > index + 1):
                            beta = stream[index + 1::]
                            firstBeta = self.first(beta)
                            
                            termB.follow |= set(firstBeta) - {self.grammar.textGrammar.EMPTY_UNIT}
    
    def apply_follow_first_rule(self):
        """
        S.follow = $
        """
        self.grammar.StartSimbol.follow.add(Grammar.STREAM_END_UNIT)

    def build_follow(self):
        """
        Routine that creates follow sets of the given grammar
        """
        self.apply_follow_first_rule()
        self.apply_follow_second_rule()
        self.apply_follow_third_rule()

    def first_of_non_terminal(self, term):
        if(len(term.first) is not 0):
            return term.first

        first = term.first
        for stream in term.right:
            if(len(stream) is 1 and stream[0] is self.grammar.textGrammar.EMPTY_UNIT):
                first |= {self.grammar.textGrammar.EMPTY_UNIT}
            else:    
                first |= self.first(stream) - {self.grammar.textGrammar.EMPTY_UNIT}
        
        term.first = first

        return first

    def term_has_empty(self, term):
        for stream in term.right:
            for item in stream:
                if(item is TextGrammar.EMPTY_UNIT):
                    return True

        return False

    def get_term(self, termString):
        for i in self.grammar.Terms:
            if(i.left == termString):
                return i

    def first_of_stream(self, stream):
        retorno = set()
        
        for index, item in enumerate(stream):
            if(item.type is TermUnit.NONTERMINAL):
                term = self.get_term(item.text)

                retorno |= self.first(term)
                
                if(self.term_has_empty(term)):
                    continue
                else:                
                    break

            elif(index is 0 and item.type is TermUnit.TERMINAL or item is TextGrammar.EMPTY_UNIT):
                retorno.add(item)
                break
        
        return retorno
    
    def first(self, p) :
        p_type = type(p)
        retorno = None

        if(p_type is list):
            retorno = self.first_of_stream(p)

        elif(p_type is TermUnit):
            term = self.get_term(p)
            retorno = self.first_of_non_terminal(term)
        
        elif(p_type is Term):
            retorno = self.first_of_non_terminal(p)

        return retorno

    def build_first(self):
        for term in self.grammar.Terms:
            self.first(term)
    
    def compileGrammar(self):
        if(self.table is None):
            self.build_first()
            self.build_follow()
            self.build_table()            
    
    def compile(self, text):
        self.compileGrammar()
        historic = ''
        lxa = LexicAnalyzer(text, self.grammar)

        stack = [Grammar.STREAM_END_UNIT, self.grammar.StartSimbolUnit]

        # while(lxa.isNotDone()):
        #     print(lxa.getToken())
        
        current = lxa.getToken()
        while( len(stack) ):#current.unit.type is not TermUnit.STREAM_END):
            hline = str(stack).ljust(50) + " " + str(current.value) + '\n'
            historic += hline

            top = stack.pop()
            print(hline)

            
            if(top.type is TermUnit.TERMINAL or top.type is TermUnit.STREAM_END):
                if(top.text == current.value):
                    current = lxa.getToken()
                
                elif(top.text == 'ide'):
                    current = lxa.getToken()

                elif(top.text == 'num'):
                    current = lxa.getToken()

                else:
                    raise Exception("Error on " + current.value + " at:")

            else:
                tupleCell = self.table[top.text][current.unit.text]

                if(tupleCell != self.ErrorString):
                    for term in tupleCell[1][::-1]:
                        if(term.type is not TermUnit.EMPTY):
                            stack.append(term)
                
                elif(current.unit.type is TermUnit.STREAM_END):
                    raise Exception("Error: invalid file end")

                else:
                    raise Exception("Error on " + current.value + " at:")
        
        # print("Compiling succes for entry:\n"+ text)
        return lxa.tokens, historic



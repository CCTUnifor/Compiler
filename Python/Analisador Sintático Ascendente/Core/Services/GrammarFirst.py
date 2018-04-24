from Core.Entities.TermUnit import TermUnit
from Core.Entities.Premise import Premise

from Core.Services.TextGrammar import TextToGrammar


class First:
    def __init__(self, grammar):
        self.grammar = grammar

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
                if(item is TextToGrammar.EMPTY_UNIT):
                    return True

        return False

    def first_of_stream(self, stream):
        retorno = set()
        
        for index, item in enumerate(stream):
            if(item.type is TermUnit.NONTERMINAL):
                term = self.grammar.get_premise(item.text)

                retorno |= self.first(term)
                
                if(self.term_has_empty(term)):
                    continue
                else:                
                    break

            elif(index is 0 and item.type is TermUnit.TERMINAL or item is TextToGrammar.EMPTY_UNIT):
                retorno.add(item)
                break
        
        return retorno
    
    def first(self, p) :
        p_type = type(p)
        retorno = None

        if(p_type is list):
            retorno = self.first_of_stream(p)

        elif(p_type is TermUnit):
            term = self.grammar.get_premise(p)
            retorno = self.first_of_non_terminal(term)
        
        elif(p_type is Premise):
            retorno = self.first_of_non_terminal(p)

        return retorno

    def build_first(self):
        for term in self.grammar.Terms:
            self.first(term)
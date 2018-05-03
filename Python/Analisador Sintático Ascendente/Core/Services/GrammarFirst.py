from Core.Entities.TermUnit import TermUnit
from Core.Entities.Premise import Premise

from Core.Services.TextGrammar import TextToGrammar


class First:
    def __init__(self, grammar):
        self.grammar = grammar

    def first_of_non_terminal(self, term: Premise):
        if(len(term.first) is not 0):
            return term.first

        first = term.first
        for stream in term.right:
            if(len(stream) is 1 and stream[0] is self.grammar.textGrammar.EMPTY_UNIT):
                first |= {self.grammar.textGrammar.EMPTY_UNIT}
            else:
                stream_first = self.first(stream, term)
                if stream_first is None:
                    continue
                first |= stream_first - {self.grammar.textGrammar.EMPTY_UNIT}
        
        term.first = first

        return first

    def term_has_empty(self, term):
        for stream in term.right:
            for item in stream:
                if(item is TextToGrammar.EMPTY_UNIT):
                    return True

        return False

    def first_of_stream(self, stream, who=None):
        retorno = set()
        
        for index, item in enumerate(stream):
            if(item.type is TermUnit.NONTERMINAL):
                term = self.grammar.get_premise(item.text)

                first = self.first(term, who)
                if first is None:
                    return None
                retorno |= first
                
                if(self.term_has_empty(term)):
                    continue
                else:                
                    break

            elif(index is 0 and item.type is TermUnit.TERMINAL or item is TextToGrammar.EMPTY_UNIT):
                retorno.add(item)
                break
        
        return retorno
    
    def first(self, p, who=None):
        p_type = type(p)
        retorno = None

        if(p_type is list):
            retorno = self.first_of_stream(p, who)

        elif(p_type is TermUnit):
            term = self.grammar.get_premise(p)

            if who is not term:
                retorno = self.first_of_non_terminal(term)
        
        elif(p_type is Premise):
            if who is not p:
                retorno = self.first_of_non_terminal(p)

        return retorno

    def build_first(self):
        for term in self.grammar.Premises:
            self.first(term)
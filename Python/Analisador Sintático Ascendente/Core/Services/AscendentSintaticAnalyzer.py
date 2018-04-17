from Core.Entities.TermUnit import TermUnit
from Core.Entities.Premise import Premise
from Core.Entities.Grammar import Grammar

from Core.Services.TextGrammar import TextToGrammar
from Core.Services.LexicAnalyzer import LexicAnalyzer

from Core.Services.GrammarFirst import First
from Core.Services.GrammarFollow import Follow


class TableService:    
    ErrorString = "Error"

    def __init__(self, grammar):
        self.grammar = grammar
        self.table = None
        self.first = First(grammar)
        self.follow = Follow(grammar, self.first)

    def build_table(self):
        pass
    
    def compileGrammar(self):
        if(self.table is None):
            self.first.build_first()
            self.follow.build_follow()
            self.build_table()
    
    def compile(self, text):
        self.compileGrammar()

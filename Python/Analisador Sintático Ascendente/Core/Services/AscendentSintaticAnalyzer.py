from Core.Entities.TermUnit import TermUnit
from Core.Entities.Premise import Premise
from Core.Entities.Grammar import Grammar
from Core.Entities.ItemGraph import ItemGraph

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

        self.item_graph = ItemGraph(grammar)

    def __build_table(self):
        pass
    
    def __compileGrammar(self):
        if(self.table is None):
            self.first.build_first()
            self.follow.build_follow()
            self.item_graph.build_graph()
            self.__build_table()
    
    def compile(self, text):
        self.__compileGrammar()

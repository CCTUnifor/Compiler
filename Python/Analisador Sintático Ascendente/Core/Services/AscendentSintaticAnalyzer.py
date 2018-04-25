from Core.Entities.TermUnit import TermUnit
from Core.Entities.Premise import Premise
from Core.Entities.Grammar import Grammar
from Core.Entities.ItemGraph import ItemGraph

from Core.Services.TextGrammar import TextToGrammar
from Core.Services.LexicAnalyzer import LexicAnalyzer
from Core.Services.GrammarFirst import First
from Core.Services.GrammarFollow import Follow
from Core.Services.SubsetsBuilder import Builder as SubsetBuilder



class TableService:    
    ErrorString = "Error"

    def __init__(self, grammar):
        self.grammar = grammar
        self.table = None

        self.first = First(grammar)
        self.follow = Follow(grammar, self.first)

        self.item_graph = ItemGraph(grammar)
        self.subset_builder = None

    def __build_table(self, matrix, subsets):
        pass
    
    def __compileGrammar(self):
        if(self.table is None):
            self.first.build_first()
            self.follow.build_follow()

            self.item_graph.build_graph()
            self.subset_builder = SubsetBuilder(self.item_graph)
            matrix, subsets = self.subset_builder.build()

            self.__build_table(matrix, subsets)
    
    def compile(self, text):
        self.__compileGrammar()

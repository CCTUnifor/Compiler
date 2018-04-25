from Core.Entities.TermUnit import TermUnit
from Core.Entities.Premise import Premise
from Core.Entities.Grammar import Grammar
from Core.Entities.ItemGraph import ItemGraph

from Core.Services.TextGrammar import TextToGrammar
from Core.Services.LexicAnalyzer import LexicAnalyzer
from Core.Services.GrammarFirst import First
from Core.Services.GrammarFollow import Follow
from Core.Services.SubsetsBuilder import Builder as SubsetBuilder
from Core.Services.SubsetsBuilder import State as State

class TableLine:
    Shift = "S"
    Go_to = "g"
    Reduce = "R"
    Accept = "ACC"
    StringCenterCount = 3

    def __init__(self, state: State):
        self.id = state.id
        self.columns = {}
        self.__build_columns(state)
    
    def __build_columns(self, state):
        for key in state.StatesByKey:
            self.columns[key] = ['', state.StatesByKey[key]]
        
        self.columns[TermUnit.STREAM_END] = ['', None]
    
    def __str__(self):
        txt = str(self.id) + "    | "
        for column in self.columns:
            cell = self.columns[column]
            value = (str(cell[1]) if cell[1] else '-')
            txt += (cell[0] + value).center(TableLine.StringCenterCount) + " | "
        return txt

class TableService:    
    ErrorString = "Error"

    def __init__(self, grammar):
        self.grammar = grammar
        self.table = None

        self.first = First(grammar)
        self.follow = Follow(grammar, self.first)

        self.item_graph = ItemGraph(grammar)
        self.subset_builder = None
        self.table = None
        self.header_term_units = None
    
    def __get_header_termunits(self):
        self.header_term_units = []

        for edge in self.item_graph.edge_list:
            if(edge.value is not None and edge.value not in self.header_term_units):
                self.header_term_units.append(edge.value)
        
    def __apply_shift_and_goto(self, line: TableLine):
        for c in line.columns:
            column = line.columns[c]

            if(column[1] is None):
                continue

            term_unit = next((x for x in self.header_term_units if x.text == c), None)

            if term_unit:
                if term_unit.type is TermUnit.TERMINAL:
                    column[0] = TableLine.Shift

                elif(term_unit.type is TermUnit.NONTERMINAL):
                    column[0] = TableLine.Go_to

    def __build_table(self, matrix, subsets):
        self.table = []

        self.__get_header_termunits()

        for state in matrix:
            line = TableLine(state)
            
            self.__apply_shift_and_goto(line)

            # line.columns[TermUnit.STREAM_END]
            self.table.append(line)
        
        self.grammar.Premises
    
    def __compileGrammar(self):
        if(self.table is None):
            self.first.build_first()
            self.follow.build_follow()

            self.item_graph.build_graph()
            self.subset_builder = SubsetBuilder(self.item_graph)
            matrix, subsets = self.subset_builder.build(reduce=False)

            self.__build_table(matrix, subsets)
    
    def compile(self, text):
        self.__compileGrammar()

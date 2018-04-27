import re

from Core.Entities.TermUnit import TermUnit
from Core.Entities.Premise import Premise
from Core.Entities.Grammar import Grammar
from Core.Entities.ItemGraph import ItemGraph
from Core.Entities.Reduction import Reduction

from Core.Services.TextGrammar import TextToGrammar
from Core.Services.LexicAnalyzer import LexicAnalyzer
from Core.Services.GrammarFirst import First
from Core.Services.GrammarFollow import Follow
from Core.Services.SubsetsBuilder import Builder as SubsetBuilder
from Core.Services.SubsetsBuilder import State as State


class TableCellValue:
    Shift = "S"
    Go_to = "g"
    Reduce = "R"
    Accept = "ACC"

    def __init__(self, line_id, column_id, value=None, cell_type=None):
        self.cell_type = cell_type
        self.value = value
        self.line_id = line_id
        self.column_id = column_id
    
    def __str__(self):
        cell_type = str(self.cell_type if self.cell_type else "")
        value = str(self.value if self.value else "")
        return cell_type + value
    
    def __repr__(self):
        return str(self)


class TableLine:
    StringCenterCount = 3

    def __init__(self, state: State):
        self.id = state.id
        self.columns = {}
        self.__build_columns(state)
    
    def __build_columns(self, state):
        for key in state.StatesByKey:
            if state.StatesByKey[key]:
                column_value = [TableCellValue(self.id, key, state.StatesByKey[key])]
            else:
                column_value = []

            self.columns[key] = column_value 
        
        self.columns[TermUnit.STREAM_END] = [TableCellValue(self.id, key)]
    
    def __str__(self):
        txt = str(self.id) + "    | "
        for column in self.columns:
            cell = self.columns[column]

            cell_text = re.sub(r'\[|\]|\s', '', str(cell))
            cell_text = cell_text if len(cell_text) else '-'

            txt += cell_text.center(TableLine.StringCenterCount) + " | "

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
        self.header_term_units = None
    
    def __get_header_termunits(self):
        self.header_term_units = []

        for edge in self.item_graph.edge_list:
            if(edge.value is not None and edge.value not in self.header_term_units):
                self.header_term_units.append(edge.value)
        
    def __apply_shift_and_goto(self, line: TableLine):
        for c in line.columns:
            cell = line.columns[c]

            if(len(cell) is 0):
                continue

            term_unit = next((x for x in self.header_term_units if x.text == c), None)

            if term_unit:
                cell_value_type = None
                if term_unit.type is TermUnit.TERMINAL:
                    cell_value_type = TableCellValue.Shift

                elif(term_unit.type is TermUnit.NONTERMINAL):
                    cell_value_type = TableCellValue.Go_to

                for cell_value in cell:
                    cell_value.cell_type = cell_value_type

    def __build_table(self, matrix, subsets):
        self.table = []

        self.__get_header_termunits()

        for state in matrix:
            line = TableLine(state)
            
            self.__apply_shift_and_goto(line)

            # line.columns[TermUnit.STREAM_END]
            self.table.append(line)
        
        """
        Encontrar o esto de cada premissa por meio do item completo,
        encontrar o follow dessa premissa
        atribuir na lista do estado e nas colunas dos follows aquela redução
        """

        complete_fechos = self.__load_complete_fechos(subsets)
        
        for i, premise in enumerate(self.grammar.Premises):
            follows = list(premise.follow)

            premise_subsets = complete_fechos[premise.left]

            for units in premise.right:
                ## aqui dentro preciso reconhecer
                ## qual o subset referente a minha
                ## cadeia de termUnit para poder
                ## pegar o id do subset e montar
                ## um obj reduce com essa premissa,
                ## esse subset e o index do vetor
                ## premise.right referente a esta
                ## cadeia de unidades
                pass

            
    def __load_complete_fechos(self, subsets):
        group_dict = {}
        for subset in subsets:
            for node in subset.fecho:
                if(node.value):
                    item = node.value
                    if(item.is_complete()):
                        if(item.premise.left not in group_dict):
                            group_dict[item.premise.left] = []

                        group_dict[item.premise.left].append(subset)
        
        return group_dict


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

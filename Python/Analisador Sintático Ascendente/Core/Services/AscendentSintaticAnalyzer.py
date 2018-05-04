import re

from Core.Entities.TermUnit import TermUnit
from Core.Entities.Premise import Premise
from Core.Entities.Production import Production
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
    Go_to = ""
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
    StringCenterCount = 5

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
        
        self.columns[TermUnit.STREAM_END] = []
    
    def __str__(self):
        txt = str(self.id).rjust(TableLine.StringCenterCount) + "   | "
        for column in self.columns:
            cell = self.columns[column]

            cell_text = re.sub(r'\[|\]|\s', '', str(cell))
            cell_text = cell_text if len(cell_text) else '-'

            txt += cell_text.center(TableLine.StringCenterCount) + " | "

        return txt
    
    def __repr__(self):
        return str(self.id)


class TableService:    
    ErrorString = "Error"

    def __init__(self, grammar):
        self.grammar = grammar
        self.table = None
        self.reductions = None

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
        self.reductions = []

        self.__get_header_termunits()

        for state in matrix:
            line = TableLine(state)
            
            self.__apply_shift_and_goto(line)

            # line.columns[TermUnit.STREAM_END]
            self.table.append(line)

        complete_fechos = self.__load_complete_fechos(subsets)

        self.__apply_reduce(complete_fechos)
        self.__apply_acc(complete_fechos)

    def __apply_acc(self, complete_fechos):
        indice = ItemGraph.FakeStartString + "_0"
        production_subsets = complete_fechos[indice]

        for production_subset in production_subsets:
            line = self.__get_state_line(production_subset.id)

            tablecellvalue = TableCellValue(line.id, ItemGraph.FakeStartString, cell_type=TableCellValue.Accept)
            line.columns[TermUnit.STREAM_END] = [tablecellvalue]        
    
    def __apply_reduce(self, complete_fechos):
        for i, production in enumerate(self.grammar.productions):
            follows = list(production.premise.follow)
    
            indice = str("_").join([str(x) for x in production.id])
            
            reduction = Reduction(i+1, production)
            self.reductions.append(reduction)

            if indice in complete_fechos:
                production_subsets = complete_fechos[indice]

                for production_subset in production_subsets:
                    for unit in follows:
                        line = self.__get_state_line(production_subset.id)

                        reduction.states.append(str(line.id) + "_" + str(unit.text))

                        tablecellvalue = TableCellValue(line.id, unit.text, reduction, TableCellValue.Reduce)
                        line.columns[unit.text].append(tablecellvalue)
            
    def __load_complete_fechos(self, subsets):
        group_dict = {}

        for subset in subsets:
            for node in subset.fecho:
                if(node.value):
                    item = node.value
                    
                    if(item.is_complete()):
                        indice = str(item.premise.left) + "_" + str(item.cursor[0])

                        if indice not in group_dict:
                            group_dict[indice] = []

                        group_dict[indice].append(subset)
        
        return group_dict

    def compileGrammar(self):
        if(self.table is None):
            self.first.build_first()
            self.follow.build_follow()

            self.item_graph.build_graph()
            self.subset_builder = SubsetBuilder(self.item_graph, is_ascii=False)
            matrix, subsets = self.subset_builder.build(reduce=False)

            self.__build_table(matrix, subsets)
    
    def __get_state_line(self, id):
        return next((line for line in self.table if line.id == id), None)
    
    def compile(self, text):
        self.compileGrammar()
        print('-------------------Compile-Process---------------------')

        history = ''

        lxa = LexicAnalyzer(text, self.grammar)

        stack = [self.table[0]]
        current = lxa.getToken()

        while(True):
            hline = str(current.value).ljust(20) + " " + str(stack) + '\n'
            history += hline
            
            print(hline)
            
            top = stack[len(stack) - 1]

            cells = top.columns[current.unit.text]

            if(not len(cells)):
                raise Exception('Erro no símbolo: ' + str(current.value))

            for cell in cells:
                if type(cell) is TableCellValue:
                    if cell.cell_type == TableCellValue.Shift:
                        stack.append(current.value)
                        stack.append(self.__get_state_line(cell.value.id))

                        current = lxa.getToken()
                    
                    elif cell.cell_type == TableCellValue.Reduce:
                        reduction = cell.value
                        production = reduction.production
                        desemp_qtd = 2 * len(production.term)                    
                        
                        for i in range(desemp_qtd):
                            stack.pop()
                        
                        last_state = stack[len(stack) - 1]
                        stack.append(production.premise.left)

                        go_tos = last_state.columns[production.premise.left]
                        go_to = go_tos[0]
                        
                        state_line = self.__get_state_line(go_to.value.id)
                        stack.append(state_line)
                        # for go_to in go_tos:
                        #     state_line = self.__get_state_line(go_to.value.id)

                        #     stack.append(state_line)
                        break
                    
                    elif cell.cell_type == TableCellValue.Accept:
                        return lxa.tokens, history
                    
                    else:
                        raise Exception('ACC não encontrado')
                
                else:
                    raise Exception('celula não encontrada')

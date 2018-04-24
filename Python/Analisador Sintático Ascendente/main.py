import sys
import io

from Core.Entities.Grammar import Grammar
from Core.Services.AscendentSintaticAnalyzer import TableService
from Core.Services.SubsetsBuilder import Builder as SBuilder

import printer as PRINTER


input_file_directory = "misc/inputs/input "
grammar_file_directory = "misc/grammars/Gramática "

grammar_name = "Tiny2"
grammar_name = "EABCD"
grammar_name = "SXYZ"
grammar_name = "SAB"
grammar_name = "ETF"
grammar_name = "Tiny"
grammar_name = "EB"
grammar_name = "A"
grammar_name = "SLR"

grammar_file_name = grammar_file_directory + grammar_name
input_file_name = input_file_directory + grammar_name

if(len(sys.argv) > 2):
    grammar_name = str(sys.argv[1])
    input_name = str(sys.argv[2])

with io.open(grammar_file_name, "r", encoding='utf8') as file_obj:
    fileTxt = file_obj.read()
    g = Grammar(fileTxt)
    tservice = TableService(g)

    tservice.compile('')    

    PRINTER.Grammar_Printer(tservice.grammar)

    # tservice.item_graph.build_graph()

    # PRINTER.printGraphLists(tservice.item_graph)

    # sb = SBuilder(tservice.item_graph)
    # matrix = sb.build()

    # PRINTER.printMinimunMatrix(matrix)
    # PRINTER.printSubsets(sb.subsets)

    # PRINTER.printMatplotlib(tservice.item_graph)

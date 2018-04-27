import sys
import io

from Core.Entities.Grammar import Grammar
from Core.Services.AscendentSintaticAnalyzer import TableService
from Core.Services.SubsetsBuilder import Builder as SubsetBuilder

import printer as PRINTER


input_file_directory = "misc/inputs/input "
grammar_file_directory = "misc/grammars/Gramática "

grammar_name = "Tiny2"
grammar_name = "EABCD"
grammar_name = "SXYZ"
grammar_name = "SAB"
grammar_name = "ETF_G"
grammar_name = "Tiny"
grammar_name = "SLR"
grammar_name = "EB"
grammar_name = "ETF"
grammar_name = "A"

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


    tservice.item_graph.build_graph()

    PRINTER.Grammar_Printer(tservice.grammar)

    PRINTER.printGraphLists(tservice.item_graph)

    PRINTER.printSubsets(tservice.subset_builder.subsets)
    PRINTER.printSubsetMatrix(tservice.subset_builder.matrix)
    PRINTER.printSintaticTable(tservice.table)

    # PRINTER.printMatplotlib(tservice.item_graph)

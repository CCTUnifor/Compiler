import re
import networkx as nx
from networkx.drawing.nx_agraph import write_dot, graphviz_layout
import matplotlib.pyplot as plt

# def recursiveMatPlotLib(graph, ploted, G, node, x, y):
#         """internal use"""
#         G.add_node(node.id, pos=(x, y))

#         ploted.append(node)
        
#         length = len(node.paths)
#         delta = int(length/2)
#         newX = x + 1
#         even = length%2 == 0

#         for i, path in enumerate(node.paths):
#             G.add_edge(node.id, path.dest.id, weight=(path.value.value if path.value else "ɛ"))
#             newY = y + (i -  delta)
#             if(even and newY is y):
#                 newY += 1
#             if(path.dest not in ploted):
#                 recursiveMatPlotLib(graph, ploted, G, path.dest, newX, newY)


# def printMatplotlib(graph):
#     """open a window with the informed graph"""
#     G=nx.Graph()

#     recursiveMatPlotLib(graph, [], G, graph.root, 1, 10)

#     pos = nx.get_node_attributes(G,'pos')
#     nx.draw(G,pos, with_labels=True)

#     labels = nx.get_edge_attributes(G,'weight')
#     nx.draw_networkx_edge_labels(G, pos, edge_labels=labels)
#     plt.show()
def printMatplotlib(graph):
    G = nx.DiGraph()
    
    first = graph.node_list[0]

    for x in graph.node_list:
        value = str(x.value) if x.value else x.id
        G.add_node(x.id, node_label=value)

    pos = nx.spring_layout(G)
    # nx.draw_networkx_nodes(G,pos,node_color='r',node_size=500,alpha=0.8)

    for x in graph.edge_list:
        value = str(x.value) if x.value else "ɛ" 
        G.add_edge(x.src.id, x.dest.id, edge_label=value)

    labels = nx.get_node_attributes(G,'node_label')
    nx.draw(G,pos, labels=labels, with_labels=True, font_size=10)#, node_size=1000)

    nx.draw_networkx_nodes(G,pos, nodelist=[first.id], node_color='b')

    labels = nx.get_edge_attributes(G,'edge_label')
    nx.draw_networkx_edge_labels(G, pos, edge_labels=labels)

    plt.axis('off')
    # plt.savefig("labels_and_colors.png") # save as png
    plt.show()

def printGraphLists(graph):
    print('-------------------------NODES-------------------------')
    print(graph.node_list)
    print('-------------------------EDGES-------------------------')
    print(graph.edge_list)

def Grammar_Printer(g):
    print('-----------------------GRAMÁTICA-----------------------')
    print(g)

    print('-----------------GRAMÁTICA-PRODUCTIONS-----------------')
    print(re.sub(r'\[|\]|,', '', str(g.productions))[1::])

    print('\n-------------------------FIRST-------------------------')
    for term in g.Premises:
        print(term.strFirst())

    print('\n------------------------FOLLOW-------------------------')
    for term in g.Premises:
        print(term.strFollow())
    
def Grammar_Table_Printer(s):
    ljust = 15
    # print('\n-------------------------TABLE-------------------------')
    # print(s.table)
    print()

    # header
    for non_terminal in s.table:
        line = "".ljust(ljust + len(non_terminal) - 1)

        for terminal in s.table[non_terminal]:
            line += terminal.ljust(ljust)
        
        centerWord = "TABLE"
        
        lineLen = len(line)
        halfLineLen = int(lineLen/2)
        centerWordLen = len(centerWord)
        halfCenterWordLen = int(centerWordLen/2)

        headerLines = "".ljust((halfLineLen - halfCenterWordLen), '-')
        print(headerLines + centerWord + headerLines)
        print("|" + line + "|")

        break

    # body
    for non_terminal in s.table:
        line = non_terminal.ljust(ljust)

        for terminal in s.table[non_terminal]:
            value = s.table[non_terminal][terminal]
            if(type(value) is tuple):
                line += str(value[1]).ljust(ljust)
            else:
                line += str(value).ljust(ljust)
        
        print("|" + line + "|")

def LexicPrint(tokens):
    print('\n-------------------------LEXIC-------------------------')
    
    for token in tokens:
        print(token)

def CompileHistoric(historic):
    print('\n-----------------------HISTORIC-----------------------')
    print(historic)

def printSubsetMatrix(matrix):
    print('\n-----------------------Matrix-----------------------')
    print('')
    header = r"k\Σ"
    body = ""
    for i, state in enumerate(matrix):
        body += str(state.id) + "  "

        for letter in state.StatesByKey:
            if(i is 0):
                header += "  |  " + letter
            if(state.StatesByKey[letter] is None):
                body += "  |  -"
            else:
                body += "  |  " + str(state.StatesByKey[letter].id)

        body += '\n'              

    print(header)
    print(body)

def printSubsets(subsets):
    print('\n-----------------------Subsets-----------------------')

    for s in subsets:
        print(s)

def printSintaticTable(tservice):
    table = tservice.table
    print('\n------------------Sintatic-Matrix-------------------')
    print('')
    first_line = table[0]
    header = (r"k\Σ").rjust(first_line.StringCenterCount)+"   | "
    
    for c in first_line.columns:
        header += c.center(first_line.StringCenterCount) + " | "

    print(header)

    for line in table:
        print(line)
    
    print('\n-----------------Reduction-Table--------------------')

    for reduction in tservice.reductions:
        print("R"+str(reduction.id) + " " + str(reduction.production) + " "+ str(reduction.states))
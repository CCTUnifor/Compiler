import networkx as nx
import matplotlib.pyplot as plt


def recursiveMatPlotLib(graph, ploted, G, node, x, y):
        """internal use"""
        G.add_node(node.id, pos=(x, y))
        ploted.append(node)
        
        length = len(node.paths)
        delta = int(length/2)
        newX = x + 1
        even = length%2 == 0

        for i, path in enumerate(node.paths):
            G.add_edge(node.id, path.dest.id, weight=(path.value.value if path.value else "ɛ"))
            newY = y + (i -  delta)
            if(even and newY is y):
                newY += 1
            if(path.dest not in ploted):
                recursiveMatPlotLib(graph, ploted, G, path.dest, newX, newY)


def printMatplotlib(graph):
    """open a window with the informed graph"""
    G=nx.Graph()

    recursiveMatPlotLib(graph, [], G, graph.root, 1, 10)

    pos = nx.get_node_attributes(G,'pos')
    nx.draw(G,pos, with_labels=True)

    labels = nx.get_edge_attributes(G,'weight')
    nx.draw_networkx_edge_labels(G, pos, edge_labels=labels)
    plt.show()
    

def printTable(graph):
    """write on console the matrix of the graph informed"""

    print('\n\n--------------------MATRIZ--------------------')
    gTable = [[0 for j in graph.node_list] for i in graph.node_list]

    for index, node in enumerate(graph.node_list):
        for path in node.paths:
            destI = graph.node_list.index(path.dest)
            gTable[destI][index] = path.value.value if path.value else "ɛ"

    firstLine = "|   " + "     "
    for node in graph.node_list:
        firstLine += str(node.id).ljust(5, ' ')

    print(firstLine + " |")

    for i in range(0, len(gTable)):
        line = "|    "
        line += str(graph.node_list[i].id).ljust(5, ' ')
        for index, j in enumerate(gTable[i]):
            txtValue = str(gTable[i][index]) if gTable[i][index] is not 0 else ' '
            line += txtValue.ljust(5, ' ')
        print(line + "|")

def printMinimunMatrix(matrix):
    print('')
    header = "k\Σ"
    body = ""
    for i, state in enumerate(matrix):
        body += state.id + "  "

        for letter in state.StatesByKey:
            if(i is 0):
                header += "  |  " + letter
            if(state.StatesByKey[letter] is None):
                body += "  |  -"
            else:
                body += "  |  " + state.StatesByKey[letter].id

        body += '\n'              

    print(header)
    print(body)

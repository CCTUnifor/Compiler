import networkx as nx
import matplotlib.pyplot as plt


class Path:
    def __init__(self, destination, value, source):
        self.src = source
        self.dest = destination
        self.value = value
    
    def __str__(self):
        return ("path: " 
                + str(self.value.value if self.value else "ɛ")
                + " from "
                + str(self.src.id)
                + " to " + str(self.dest))


class ThompsonNode:
    def __init__(self, identifier):
        self.id = identifier
        self.paths = []

    def addDestination(self, node, value):
        path = Path(node, value, self)
        self.paths.append(path)
        return path
    
    def getPath(self, node):
        for path in self.paths:
            if(path.dest == node):
                return path

    def __str__(self):
        returnStr = str(self.id)

        for item in self.paths:
            returnStr += '\n'+str(item)

        return returnStr

class ThompsonGraph:
    def __init__(self, id):
        self.id = id
        self.countId = 0
        self.node_list = []
        self.edge_list = []
        self.root = self.makeNode()
        self.cursor = self.root
    
    def makeNode(self):
        node = ThompsonNode(str(self.id)+ "-" + str(self.countId))
        self.countId+=1
        self.node_list.append(node)
        return node
    
    def addToken(self, token):
        newEnd = self.makeNode()
        path = self.cursor.addDestination(newEnd, token)
        self.edge_list.append(path)
        self.cursor = newEnd
        print(self.root)
        return newEnd
    
    def addSequence(self, thompsonGraph):
        oldRoot = thompsonGraph.root
        inRoot = []
        outRoot = []

        # self.cursor.paths += thompsonGraph.root.paths
        self.node_list += [x for x in thompsonGraph.node_list if x is not oldRoot]

        for edge in thompsonGraph.edge_list:
            if(edge.src is oldRoot):
                newPath = self.cursor.addDestination(edge.dest, edge.value)
                self.edge_list.append(newPath)
                
            elif(edge.dest is oldRoot):
                edge.dest = self.cursor
                self.edge_list.append(edge)
            else:
                self.edge_list.append(edge)

        self.cursor = thompsonGraph.cursor
    
    def addChoice(self, thompsonGraph, id):
        newGraph = ThompsonGraph(id)

        up = newGraph.addToken(None)
        newGraph.addSequence(self)
        up = newGraph.addToken(None)

        newGraph.cursor = newGraph.root
        down = newGraph.addToken(None)

        newGraph.addSequence(thompsonGraph)
        newGraph.cursor.addDestination(up, None)
        newGraph.cursor = up  

        return newGraph
        
    def repeatN(self, haveMinimun=False):
        pass
    
    def printMatplotlib(self):
        G=nx.Graph()

        for i, node in enumerate(self.node_list):
            G.add_node(node.id, pos=(i, i))
            print((node.id,(i,i)))

        for path in self.edge_list:
            G.add_edge(path.src.id, path.dest.id, weight=(path.value.value if path.value else "ɛ"))
            print((path.src.id, path.dest.id, (path.value.value if path.value else "ɛ")))

        pos = nx.get_node_attributes(G,'pos')
        nx.draw(G,pos, with_labels=True)

        labels = nx.get_edge_attributes(G,'weight')
        nx.draw_networkx_edge_labels(G, pos, edge_labels=labels)
        plt.show()
        
    
    def printTable(self):
        gTable = [[0 for j in self.node_list] for i in self.node_list]

        for index, node in enumerate(self.node_list):
            for path in node.paths:
                destI = self.node_list.index(path.dest)
                gTable[destI][index] = path.value.value if path.value else "ɛ"

        firstLine = "|   " + "     "
        for node in self.node_list:
            firstLine += str(node.id).ljust(5, ' ')

        print(firstLine + " |")

        for i in range(0, len(gTable)):
            line = "|    "
            line += str(self.node_list[i].id).ljust(5, ' ')
            for index, j in enumerate(gTable[i]):
                line += str(gTable[i][index]).ljust(5, ' ')
            print(line + "|")

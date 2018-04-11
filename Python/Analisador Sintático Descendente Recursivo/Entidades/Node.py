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


class Node:
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

class Graph:
    def __init__(self, id):
        self.id = id
        self.countId = 0
        self.node_list = []
        self.edge_list = []
        self.root = self.makeNode()
        self.cursor = self.root
    
    def makeNode(self):
        node = Node(str(self.id)+ "-" + str(self.countId))
        self.countId+=1
        self.node_list.append(node)
        return node
    
    def addToken(self, token):
        newEnd = self.makeNode()
        path = self.cursor.addDestination(newEnd, token)
        self.edge_list.append(path)
        self.cursor = newEnd
        # print(self.root)
        return newEnd
    
    def addSequence(self, graph):
        oldRoot = graph.root

        # self.cursor.paths += thompsonGraph.root.paths
        self.node_list += [x for x in graph.node_list if x is not oldRoot]

        for edge in graph.edge_list:
            if(edge.src is oldRoot):
                newPath = self.cursor.addDestination(edge.dest, edge.value)
                self.edge_list.append(newPath)
                
            elif(edge.dest is oldRoot):
                edge.dest = self.cursor
                self.edge_list.append(edge)
            else:
                self.edge_list.append(edge)

        self.cursor = graph.cursor
    
    def addChoice(self, thompsonGraph, id):
        newGraph = Graph(id)

        up = newGraph.addToken(None)
        newGraph.addSequence(self)
        up = newGraph.addToken(None)

        newGraph.cursor = newGraph.root
        newGraph.addToken(None)

        newGraph.addSequence(thompsonGraph)
        newPath = newGraph.cursor.addDestination(up, None)
        newGraph.cursor = up
        newGraph.edge_list.append(newPath)

        return newGraph
        
    def repeatN(self, haveMinimun=False):

        newPath = self.cursor.addDestination(self.root, None)
        self.edge_list.append(newPath)        

        newBegining = self.makeNode()
        newPath = newBegining.addDestination(self.root, None)
        self.edge_list.append(newPath)        
        self.root = newBegining

        self.addToken(None)

        if(not haveMinimun):
            newPath = self.root.addDestination(self.cursor, None)
            self.edge_list.append(newPath)        

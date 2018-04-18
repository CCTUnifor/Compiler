class Path:
    def __init__(self, destination, value, source):
        self.src = source
        self.dest = destination
        self.value = value
    
    def __str__(self):
        return ("path: " 
                + str(self.value if self.value else "ɛ")
                + " from "
                + str(self.src)
                + " to " + str(self.dest))


class Node:
    def __init__(self, identifier, value=None):
        self.id = identifier
        self.paths = []
        self.value = value

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
    def __init__(self, id=0, makeRoot=True):
        self.id = id
        self.countId = 0
        self.node_list = []
        self.edge_list = []
        self.root = None

        self.__make_root(makeRoot)

    def __make_root(self, makeRoot):
        if(makeRoot):
            self.root = self.makeNode()
    
    def makeNode(self, value=None):
        node = Node(str(self.id)+ "-" + str(self.countId), value)

        self.countId+=1
        self.node_list.append(node)

        return node
    
    def addPath(self, src, dest, value=None):
        path = src.addDestination(dest, value)

        self.edge_list.append(path)
        
        return path     

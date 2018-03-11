class Path:
    def __init__(self, destination, value, source):
        self.dest = destination
        self.value = value
        self.src = source
    
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
        self.paths.append(Path(node, value, self))
    
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
        self.root = self.makeNode()
        self.cursor = self.root
    
    def makeNode(self):
        return ThompsonNode(str(self.id) + str(self.countId))
    
    def addChoice(self, thompsonGraph):
        # newGraph = ThompsonGraph()
        # return newGraph
        pass
    
    def addSequence(self, thompsonGraph):
        pass
    
    def repeatN(self, haveMinimun=False):
        pass
    
    def addToken(self, token):
        newEnd = self.makeNode()
        self.cursor.addDestination(newEnd, token)
        self.cursor = newEnd
    

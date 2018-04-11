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

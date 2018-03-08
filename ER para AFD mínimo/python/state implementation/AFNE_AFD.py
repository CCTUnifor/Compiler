class SubSet:
    def __init__(self, closure, nodes, ide):
        self.closure = closure
        self.nodes = nodes
        self.id = ide

class AFNEtoAFD:
    def __init__(self):
        self.matrix
        self.sets = []
        self.alphabet = []
        self.charControl = 65 # 'A'

    def buildSubSet(self, closure, nodes):
        v = self.charControl
        self.charControl += 1
        return SubSet(closure, nodes) 
    
    def convert(self, root):
        stop = False
        closures = [root]

        while(not stop):
            closure = closures.pop()
            
            group = self.buildSubSet(closure, self.makeSet(closure))
            self.sets.append(group)

            partials = self.getPartialResults(group.nodes)

            # checar se o resultado das parciais já estão em algum grupo reconhecido
            for partial in partials:
                isSet = False
                for Set in self.sets:
                    if(type(Set.closure) is type([])):
                        # implementar pra caso de o closure ser uma lista
                       pass 
                    else if(Set.closure is partial):
                        # implementar pra caso de o partial ser uma lista quando o closure não é
                        isSet = True
                        break

                if(not isSet):
                    closures.append(partial)


            
            if(not len(closures)):
                stop = True

        # montar matriz
        # minimizar/otimizar a matriz     
            
    
    def makeSet(self, root):
        if(type(root) is type([])):
            Group = []
            for item in root:
                itemGroup = self.makeSet(item)
                Group += [node for node in itemGroup if node not in Group]

            return Group

        Group = [root]
        for path in root.paths:
            pv = path.value
            pdNode = path.dest
            if(pv is None):
                if(pdNode not in Group)
                    Group.append(pdNode)
                    Group + self.makeSets(pdNode)
            else:
                if(pv not in self.alphabet):
                    self.alphabet.append(pv)

        return Group
    
    def getPartialResults(nodes):
        partials = []
        for value in self.alphabet:
            partial = []
            for node in nodes:
                for path in node.paths
                    if(path.value is value):
                        partial.append(path.dest)
            if(len(partial) is 1):
                partials.append(partial[0])
            else:
                partials.append(partial)

        return partials

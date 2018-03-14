from Entidades.Node import ThompsonGraph


class Subset:
    def __init__(self, fecho, id, alphabet):
        self.fecho = fecho
        self.id = id
        self.states = []
        self.semiFunction = {}
        for c in alphabet:
            self.semiFunction[c] = []


class Builder:
    def __init__(self, Graph: ThompsonGraph):
        self.constructor(Graph)

    def constructor(self, Graph: ThompsonGraph):
        self.graph = Graph
        self.alphabet = self.get_alphabet()
        self.subsetId = 65
        self.cursor = Graph.root
    
    def get_alphabet(self):
        alphabet = []
        for edge in self.graph.edge_list:
            if(edge.value is not None):
                alphabet.append(edge.value.value)
        
        return alphabet
    
    def getNewSubset(fecho):
        # Ascii code
        charId = str(unichr(self.subsetId))
        self.subsetId += 1
        if(self.subsetId is 91):
            self.subsetId = 97
        elif(self.subsetId is 123):
            self.subsetId = 145
        
        return Subset(fecho, charId, self.alphabet)
    
    def build(self):
        fechos = [[self.graph.root]]
        doneFechos = []
        subsets = []
        
        while(len(fecho)):
            fecho = fechos.pop()
            doneFechos.append(fecho)

            subset = self.getNewSubset(fecho)
            subsets.append(subset)

            for node in fecho:
                self.recursiveFill(subset, node)
                
            self.alreadyRunned(doneFechos, fechos, subset)

        # pegar conjunto de subsets e retornar a matriz mínima

    def alreadyRunned(self, doneFechos, fechos, subset):
            semiFunction = subset.semiFunction
            for setKey in semiFunction:
                nextFecho = semiFunction[setKey]
                
                if(len(nextFecho)):
                    inFecho = True

                    for doneFecho in doneFechos:
                        inFecho = True

                        for node in nextFecho:
                            if(node not in doneFecho):
                                inFecho = False
                                break

                        if(inFecho):
                            break

                    if(not inFecho):
                        fechos.append(len(semiFunction[setKey]))

    @staticmethod
    def recursiveFill(subset, node):
        subset.states.append(node)

        for path in node.paths:
            if(path.value is None):
                if(path.dest not in subset.states):
                    recursiveFill(subset, path.dest)
            else:
                token = path.value

                alphabetSet = subset.alphabetSets[token.value]
                if(path.dest in alphabetSet):
                    alphabetSet.append(path.dest)

from Entidades.Node import ThompsonGraph


class Subset:
    def __init__(self, id, alphabet):
        self.id = id
        self.states = []
        self.alphabetSets = {}
        for c in alphabet:
            self.alphabetSets[c] = []


class Builder:
    def __init__(self, Graph: ThompsonGraph):
        self.graph = Graph
        self.alphabet = self.get_alphabet()
        self.subsets = []
        self.subsetId = 65
        self.cursor = Graph.root
    
    def get_alphabet(self):
        alphabet = []
        for edge in self.graph.edge_list:
            if(edge.value is not None):
                alphabet.append(edge.value.value)
        
        return alphabet
    
    def getNewSubset():
        # Ascii code
        charId = str(unichr(self.subsetId))
        self.subsetId += 1
        if(self.subsetId is 91):
            self.subsetId = 97
        elif(self.subsetId is 123):
            self.subsetId = 145
        
        return Subset(charId, self.alphabet)
    
    def build(self):
        subset = self.getNewSubset()
        
        while(True):
            self.recursiveFill(subset, self.cursor)


            if (True):
                break

    @staticmethod
    def recursiveFill(subset, node):
        subset.states.append(node)

        for path in node.paths:
            if(path.value is None):
                if(path.dest not in subset.states):
                    recursiveFill(subset, path.dest)
            else:
                token = path.value
                parei aqui: quando encontro um valor, devo adicionar o último estado de valor ou os dois estados?
                # recursiveFillSemiFunction(subset, token.value, path.dest)

                # alphabetSet = subset.alphabetSets[token.value]
                # alphabetSet.append(path.dest)
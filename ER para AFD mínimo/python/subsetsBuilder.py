from Entidades.Node import ThompsonGraph


class Subset:
    def __init__(self, fecho, id, alphabet):
        self.fecho = fecho
        self.id = id
        self.states = []
        self.semiFunction = {}
        for c in alphabet:
            self.semiFunction[c] = []

    def __str__(self):
        fecho = '('
        for node in self.fecho:
            fecho += node.id + ', '
        fecho = fecho[:len(fecho)-2:] + ")"        

        states = '('
        for node in self.fecho:
            states += node.id + ', '
        states = states[:len(states)-2:] + ")"

        semiFunctions = "\n    "
        for sf in self.semiFunction:
            sm = self.semiFunction[sf]
            # if(len(sm)):
            semiFunctions += "δ(" + self.id + ", " + sf + "): {"
            for s in sm:
                semiFunctions +=  s.id + ", "
            semiFunctions = (semiFunctions[:len(semiFunctions)-2:] if semiFunctions[len(semiFunctions)-1] is not "{" else semiFunctions)  + "}    "

        return ("Fecho-" + fecho 
                + " = " + states
                + " = "+self.id
                + semiFunctions)


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
    
    def getNewSubset(self, fecho):
        # Ascii code
        charId = str(chr(self.subsetId))
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
        
        while(len(fechos)):
            fecho = fechos.pop()
            doneFechos.append(fecho)

            subset = self.getNewSubset(fecho)
            subsets.append(subset)

            for node in fecho:
                Builder.recursiveFill(subset, node)
                
            self.alreadyRunned(doneFechos, fechos, subset)
        

        for s in subsets:
            print(s)
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
                        fechos.append(semiFunction[setKey])

    @staticmethod
    def recursiveFill(subset, node):
        subset.states.append(node)

        for path in node.paths:
            if(path.value is None):
                if(path.dest not in subset.states):
                    Builder.recursiveFill(subset, path.dest)
            else:
                token = path.value

                alphabetSet = subset.semiFunction[token.value]
                if(path.dest not in alphabetSet):
                    alphabetSet.append(path.dest)

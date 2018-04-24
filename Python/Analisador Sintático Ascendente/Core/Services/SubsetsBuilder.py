from Core.Entities.Graph import Graph

class State:
    def __init__(self, id, start=False, end=False):
        self.id = id
        self.start = start
        self.end = end
        self.StatesByKey = {}
    
    def __str__(self):
        return str(self.id)
      

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
            value = node.value if node.value else node.id
            fecho += str(value) + ', '
        fecho = fecho[:len(fecho)-2:] + ")"        

        states = '('
        for node in self.states:
            value = node.value if node.value else node.id
            states += str(value) + ', '
        states = states[:len(states)-2:] + ")"

        semiFunctions = "\n    "
        for sf in self.semiFunction:
            sm = self.semiFunction[sf]
            # if(len(sm)):
            semiFunctions += "δ(" + str(self.id) + ", " + sf + "): {"
            for s in sm:
                value = s.value if s.value else s.id
                semiFunctions +=  str(value) + ", "
            semiFunctions = (semiFunctions[:len(semiFunctions)-2:] if semiFunctions[len(semiFunctions)-1] is not "{" else semiFunctions)  + "}    "

        return ("Fecho-" + str(fecho )
                + " = " + str(states)
                + " = "+str(self.id)
                + str(semiFunctions))


class Builder:
    def __init__(self, Graph: Graph):
        self.__constructor(Graph)

    def __constructor(self, Graph: Graph):
        self.graph = Graph
        self.alphabet = self.__get_alphabet()
        # self.subsetId = 65 # ascii code id
        self.subsetId = 0 # counting id
        self.cursor = Graph.root
    
    def __get_alphabet(self):
        alphabet = []
        for edge in self.graph.edge_list:
            if(edge.value is not None):
                alphabet.append(edge.value.text)
        
        return alphabet
    
    def __getNewSubset(self, fecho):
        # Ascii code ID
        # charId = str(chr(self.subsetId))
        # self.subsetId += 1
        # if(self.subsetId is 91):
        #     self.subsetId = 97
        # elif(self.subsetId is 123):
        #     self.subsetId = 145

        # counting ID
        charId = self.subsetId    
        self.subsetId += 1

        return Subset(fecho, charId, self.alphabet)
    
    def build(self):
        self.subsets = self.__buildFechos()

        self.matrix = self.__buildMatriz(self.subsets)

        # self.__reduceMatrix(matrix)

        return self.matrix


    def __buildFechos(self):
        fechos = [[self.graph.root]]
        doneFechos = []
        subsets = []
        
        while(len(fechos)):
            fecho = fechos.pop()
            doneFechos.append(fecho)

            subset = self.__getNewSubset(fecho)
            subsets.append(subset)

            for node in fecho:
                Builder.recursiveFill(subset, node)
                
            self.__alreadyRunned(doneFechos, fechos, subset)

        # print('\n')
        # for s in subsets:
        #     print(s)
        #     print('')

        return subsets
    
    @staticmethod
    def equalListOfNodes(semiFunction, fecho):
        """SF equals fecho = True; not = False;"""
        if(len(semiFunction) is not len(fecho)):
            return False

        for nodeSF in semiFunction:
            if(nodeSF not in fecho):
                return False
        
        return True
            
    def __createState(self, matrix, subset):
        for ste in matrix:
            if(ste.id is subset.id):
                return ste

        start = self.graph.root in subset.states
        # end = self.graph.cursor in subset.states

        state = State(subset.id, start=start)#, end=end)
        matrix.append(state)

        return state
        
    def __buildMatriz(self, subsets):
        matrix = []

        for subset in subsets:
            state = self.__createState(matrix, subset) #State(subset.id, start=start, end=end)

            for letter in self.alphabet:
                semiFunction = subset.semiFunction[letter]
                state.StatesByKey[letter] = None

                if(len(semiFunction)):
                    for fecho in subsets:
                        # if(fecho is not subset):
                        if(Builder.equalListOfNodes(semiFunction, fecho.fecho)):
                            state.StatesByKey[letter] = self.__createState(matrix, fecho)
        
        matrix.sort(key=lambda state: state.id)
        
        return matrix

    @staticmethod
    def equalStates(st1, st2):
        for key in st1.StatesByKey:
            if(st1.StatesByKey[key] is not st2.StatesByKey[key]):
                return False
        
        return True

    def __reduceMatrix(self, matrix):
        similars = []
        similarsReposition = []

        for cursor in matrix:
            if(cursor not in similars ):#and cursor.end):
                for similar in matrix:
                    if(similar is not cursor ):#and similar.end):
                            if(Builder.equalStates(similar, cursor)):
                                similars.append(similar)
                                similarsReposition.append(cursor)

        for i, similar in enumerate(similars):
            matrix.remove(similar)

            for state in matrix:
                for key in state.StatesByKey:
                    if(state.StatesByKey[key] is similar):
                        state.StatesByKey[key] = similarsReposition[i]
        
        return matrix

    def __alreadyRunned(self, doneFechos, fechos, subset):
            semiFunction = subset.semiFunction
            for setKey in semiFunction:
                nextFecho = semiFunction[setKey]
                
                if(len(nextFecho)):
                    inFecho = True

                    for doneFecho in doneFechos + fechos:
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

                alphabetSet = subset.semiFunction[token.text]
                if(path.dest not in alphabetSet):
                    alphabetSet.append(path.dest)

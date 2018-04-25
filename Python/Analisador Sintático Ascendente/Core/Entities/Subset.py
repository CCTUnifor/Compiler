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
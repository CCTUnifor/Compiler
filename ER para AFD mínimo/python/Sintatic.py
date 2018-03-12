from Entidades.Token import Token
from Entidades.Node import ThompsonGraph as Graph


class SintaticAnalyzer:
    def __init__(self, inputTokens):
        self.input = inputTokens
    
    def raiseException(self, expected, gotit):
        raise Exception("Expected " + expected + " got: " + gotit + ' on cursor ' + str(self.cursor))
    
    def log(self, txt):
        msg = ''
        try:
            msg = ('<' + txt + '> on token: ' + str(self.peek()) + ' on cursor: ' +  str(self.cursor))
        except Exception :
            msg = ('<' + txt + '> on cursor: ' +  str(self.cursor))

        # for i in range(0, self.logCount):
        #     msg = ' ' +msg

        print(msg)
        self.logCount += 1

    def peek(self):
        # print(self.cursor)
        return self.input[self.cursor]

    def eat(self, token:str):
        print('EATING {' + token + '}')
        if(not self.empty() and self.peek().value is token):
            result = self.peek()
            self.cursor += 1
            return result
        else:
            self.raiseException(str(self.peek), token)

    def next(self):
        token = self.peek()
        self.eat(token.value)
        return token
    
    def empty(self):
        return len(self.input) <= self.cursor

    def analyze(self):
        self.cursor = 0
        self.logCount = 0
        self.thompsonCount = 0

        result = self.regex()
        if(not self.empty()):
            raise Exception('End not finded')
        
        return result
    
    def getNewGraph(self):
        self.thompsonCount += 1
        return Graph(self.thompsonCount - 1)
    
    def regex(self):
        self.log('regex')
        graph = self.term()

        if(not self.empty()):
            if(self.peek().ttype is Token.OR):
                self.eat('|')
                graph = graph.addChoice(self.regex(), self.thompsonCount)
                self.thompsonCount+=1

            else:
                self.raiseException('|', str(self.peek()))
        
        return graph

    def term(self):
        self.log('term')
        graph = None
        
        while(not self.empty() and self.peek().value is not ')' and self.peek().ttype is not Token.OR):
            if(graph is None):
                graph = self.factor()
            else:
                graph.addSequence(self.factor())
        
        if(graph is None):
            self.raiseException("<term>", "ɛ")
        
        return graph
            

    def factor(self):
        self.log('factor')
               
        graph = self.base()

        if(not self.empty()):
            if(self.peek().value is '*'):
                self.eat('*')
                graph.repeatN()

            elif(self.peek().value is '+'):
                self.eat('+')
                graph.repeatN(True)
        
        return graph
    
    def base(self):
        self.log('base')
        if(not self.empty()):
            if(self.peek().value is '('):
                self.eat('(') # consome '('
                graph = self.regex()
                self.eat(')') # consome ')'
                return graph
            elif(self.peek().ttype.terminal):
                graph = self.getNewGraph()                
                self.character(graph)
                return graph
            else:
                self.raiseException("<base>", "TERMINAL")                

        self.raiseException("<base>", "Ɛ")
            

    def character(self, graph):
        self.log('character')
        if(self.peek().ttype is Token.WORD or self.peek().ttype is Token.NUMBER):
            graph.addToken(self.next()) # consome caractéres
        else:
            self.raiseException("<character>", str(self.peek()))

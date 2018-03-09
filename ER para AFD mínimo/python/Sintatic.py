from Entidades.Token import Token


class SintaticAnalyzer:
    def __init__(self, inputTokens):
        self.input = inputTokens
        self.cursor = 0
    
    def raiseException(self, expected, gotit):
        raise Exception("Expected " + expected + " got: " + gotit + ' on cursor ' + str(self.cursor))
    
    def log(self, txt):
        print('<' + txt + '> on token: ' + str(self.peek()) + ' on cursor: ' +  str(self.cursor))

    def peek(self):
        return self.input[self.cursor]

    def eat(self, token):
        if(self.peek() is token):
            self.cursor += 1
        else:
            self.raiseException(str(self.peek), token)

    def next(self):
        token = self.peek()
        self.eat(token)
        return token
    
    def empty(self):
        return len(self.input) <= self.cursor

    def analyze(self):
        self.regex()
    
    def regex(self):
        self.log('regex')
        self.term()

        if(not self.empty()):
            if(self.peek().ttype is Token.OR):
                self.next() # consome o '|'
                self.regex()
            else:
                self.raiseException('|', str(self.peek()))

    def term(self):
        self.log('term')
        if(not self.empty()):
            self.factor()
            self.term()
            

    def factor(self):
        self.log('factor')
        if(not self.empty()):
            self.base()
            if(not self.empty()):
                if(self.peek().ttype is Token.OPERATOR):
                    self.next() # consome o '*'|'+'
        
        else:
            self.raiseException("<base>", "ɛ")
    
    def base(self):
        self.log('base')
        if(not self.empty):
            if(self.peek().ttype is Token.PARENTHESES):
                self.next() # consome '('
                self.regex()
                self.next() # consome ')'
            else:
                self.character()
            

    def character(self):
        self.log('character')
        if(self.peek().ttype is Token.WORD):
            self.next() # consome caractéres
        else:
            self.raiseException("<character>", str(self.peek()))
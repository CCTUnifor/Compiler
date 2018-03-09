from Entidades.Token import Token


class SintaticAnalyzer:
    def __init__(self, inputTokens):
        self.input = inputTokens
        self.cursor = 0
        self.logCount = 0
    
    def raiseException(self, expected, gotit):
        raise Exception("Expected " + expected + " got: " + gotit + ' on cursor ' + str(self.cursor))
    
    def log(self, txt):
        msg = ''
        try:
            msg = ('<' + txt + '> on token: ' + str(self.peek()) + ' on cursor: ' +  str(self.cursor))
        except Exception as identifier:
            msg = ('<' + txt + '> on cursor: ' +  str(self.cursor))

        # for i in range(0, self.logCount):
        #     msg = ' ' +msg

        print(msg)
        self.logCount += 1

    def peek(self):
        # print(self.cursor)
        return self.input[self.cursor]

    def eat(self, token):
        if(self.peek().value is token):
            self.cursor += 1
        else:
            self.raiseException(str(self.peek), token)

    def next(self):
        token = self.peek()
        self.eat(token.value)
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
                self.eat('|')
                self.regex()
            # else:
            #     self.raiseException('|', str(self.peek()))

    def term(self):
        self.log('term')
        if(not self.empty() and self.peek().value is not ')' and self.peek().ttype is not Token.OR):
            self.factor()
            self.term()
            

    def factor(self):
        self.log('factor')
        self.base()
        if(not self.empty()):
            if(self.peek().value is '*'):
                self.eat('*')

            elif(self.peek().value is '+'):
                self.eat('+')
    
    def base(self):
        self.log('base')
        if(not self.empty()):
            if(self.peek().value is '('):
                self.eat('(') # consome '('
                self.regex()
                self.eat(')') # consome ')'
            else:
                self.character()
            

    def character(self):
        self.log('character')
        if(self.peek().ttype is Token.WORD or self.peek().ttype is Token.NUMBER):
            self.next() # consome caractéres
        else:
            self.raiseException("<character>", str(self.peek()))

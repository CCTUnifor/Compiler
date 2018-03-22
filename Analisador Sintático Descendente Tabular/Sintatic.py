from Entidades.Token import Token
from Entidades.Token import TokenType


class TabularDescendentSintaticAnalyzer:
    def __init__(self, lexic):
        self.lexic = lexic
        self.current = lexic.getToken()

    def log(self, txt):
        print(txt.ljust(15, ' ') + " " + str(self.current))
    
    def raiseException(self, txt):
        raise Exception('Expected: '+ str(txt) +' got: ' + str(self.current))

    def hasNext(self):
        return self.lexic.notEnded()
    
    def eat(self, value):
        if(self.peek() is None):
            raise Exception('Expected: ' + str(value) + ' got: None')

        if (not((type(value) is str) and self.current.value.lower() == value.lower()) and not(type(value) is TokenType and self.peek().ttype is value)):
            raise Exception('Expected: ' + str(value) + ' got: ' + str(self.current))

        self.log('--EAT--')
        self.current = self.lexic.getToken()

        return self.current

    def peek(self):
        return self.current
    
    def next(self):
        token = self.current
        self.eat(token.value)
        return token

    def parse(self):
        self.log('parse')
        self.decl_sequencia()
    
    def decl_sequencia(self):
        self.log('decl_sequencia')
        self.declaracao()

        if(self.hasNext()):
            if(self.peek().ttype is Token.SEMICOLON):
                self.eat(';')
                self.decl_sequencia()
    
    def declaracao(self):
        self.log('declaracao')
        
        if(self.peek().ttype is Token.RESERVEDWORD):
            value = str(self.peek().value)
            
            if(value == 'if'):
                self.cond_decl()

            elif(value == 'repeat'):
                self.repet_decl()
            
            elif(value == 'read'):
                self.leit_decl()
            
            elif(value == 'write'):
                self.escr_decl()

            else:
                self.raiseException(Token.RESERVEDWORD)

        elif(self.peek().ttype is Token.IDENTIFIER):
            self.atrib_decl()
        
        else:
            self.raiseException('declaracao')

    def cond_decl(self):
        self.log('cond_decl')
        
        self.eat('if')
        exp = self.exp()
        self.eat('then')    
        self.decl_sequencia()
        
        if(self.hasNext() and self.peek().value == 'else'):
            self.eat('else')
            self.decl_sequencia()
        
        self.eat('end')
    
    def repet_decl(self):
        self.log('repet_decl')
        
        self.eat('repeat')
        self.decl_sequencia()
        self.eat('until')
        self.exp()
    
    def atrib_decl(self):
        self.log('atrib_decl')

        self.eat(Token.IDENTIFIER)
        self.eat(Token.ATRIB)        
        self.exp()
    
    def leit_decl(self):
        self.log('leit_decl')

        self.eat('read')
        self.eat(Token.IDENTIFIER)
    
    def escr_decl(self):
        self.log('escr_decl')
        
        self.eat('write')
        self.exp()
    
    def exp(self):
        self.log('exp')
        
        self.exp_simpels()
        if(self.hasNext() and self.peek().ttype is Token.OPERATOR):
            self.comp_op()
            self.exp_simpels()
    
    def comp_op(self):
        self.log('comp_op')
        
        if(self.peek().value == '<'):
            self.eat('<')
        else:
            self.eat('=')
    
    def exp_simpels(self):
        self.log('exp_simpels')
        
        self.termo()
        if(self.hasNext() and self.peek().ttype is Token.SUM):
            self.exp_simples_linha()
    
    def exp_simples_linha(self):
        self.log('exp_simples_linha')
        
        self.soma()
        self.termo()
        if(self.peek().ttype is Token.SUM):
            self.exp_simples_linha()        

    
    def soma(self):
        self.log('soma')
        
        if(self.peek().value == '+'):
            self.eat('+')
        else:
            self.eat('-')
    
    def termo(self):
        self.log('termo')
        
        self.fator()

        if(self.hasNext() and self.peek().ttype is Token.PROD):
            self.termo_linha()
    
    def termo_linha(self):
        self.log('termo_linha')
        
        self.mult()
        self.fator()

        if(self.peek().ttype is Token.PROD):
            self.termo_linha()
    
    def mult(self):
        self.log('mult')
        
        if(self.peek().value == '*'):
            self.eat('*')
        else:
            self.eat('/')
    
    def fator(self):
        self.log('fator')
        
        if(self.peek().ttype is Token.PARENTHESES):
            self.eat('(')
            self.exp()
            self.eat(')')

        elif(self.peek().ttype is Token.NUMBER):
            self.eat(Token.NUMBER)

        elif(self.peek().ttype is Token.IDENTIFIER):
            self.eat(Token.IDENTIFIER)
        
        else:
            self.raiseException('fator')
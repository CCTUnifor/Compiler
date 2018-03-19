from Entidades.Token import Token


class RecursiveDescendentSintaticAnalyzer:
    def __init__(self, lexic):
        self.lexic = lexic
        self.current = lexic.getToken()

    def log(self, txt):
        print(txt)

    def hasNext(self):
        return self.lexic.notEnded()
    
    def eat(self, value:str):
        if(self.current.value.lower() is value.lower()):
            self.current = self.lexic.getToken()
            return self.current
        else:
            raise Exception('Expected: ' + str(self.current) + ' got: ' + str(value))

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

        if(self.hasNext):
            if(self.peek().ttype is Token.SEMICOLON):
                self.eat(';')
                self.decl_sequencia()
    
    def declaracao(self):
        if(self.peek().ttype is Token.RESERVEDWORD):
            value = self.peek().value

            if(value is 'if' or value is 'IF'):
                self.cond_decl()

            elif(value is 'repeat' or value is 'REPEAT'):
                self.repet_decl()
            
            elif(value is 'read' or value is 'READ'):
                self.leit_decl()
            
            elif(value is 'write' or value is 'WRITE'):
                self.escr_decl()

        elif( self.peek.ttype is Token.ATRIB):
            self.atrib_decl()

    def cond_decl(self):
        self.eat('if')
        exp = self.exp()
        self.eat('then')
        
        if(exp):
            pass
        else:
            pass
    
    def repet_decl(self):
        pass
    
    def atrib_decl(self):
        pass
    
    def leit_decl(self):
        pass
    
    def escr_decl(self):
        pass
    
    def exp(self):
        pass
    
    def comp_op(self):
        pass
    
    def exp_simpels(self):
        pass
    
    def exp_simples_linha(self):
        pass
    
    def soma(self):
        pass
    
    def termo(self):
        pass
    
    def termo_linha(self):
        pass
    
    def mult(self):
        pass
    
    def fator(self):
        pass
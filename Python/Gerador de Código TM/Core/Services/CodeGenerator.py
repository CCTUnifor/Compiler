from Core.Entities.Token import Token
import re


class CodeGenerator:
    command_dict = {
    }
    # TINYP tem a máquina
    operators_dict = {
        "=": 'EQ',
        "IS": 'EQ',
        "!=": 'NE',
        "<>": 'NE',
        "NOT": 'NE',
        ">": 'GT',
        ">=": 'GE',
        "<": 'LT',
        "<=": 'LE',
        "*": 'MUL',
        "/": 'DIV',
        "-": 'SUB',
        "+": 'ADD'
    }

    operator_regex = re.compile(r'(<=|>=|<>|NOT|IS|>|<|=|!=)')
    algebric_operator_regex = re.compile(r'(\*|\/|\+|-)')

    def __init__(self, Tokens:Token=None):
        self.Tokens = Tokens
        self.intermediate_code = None
        self.variables = None
        self.state = None
    
    def compile(self):
        if self.Tokens is None:
            raise Exception('Invalid Compile process: the argument "Tokens" should not be None at this point')

        for token in self.Tokens:
            self.process_token(token)
    
    def process_token(self, token: Token):
        """
        Called by the compile process or the sintatic analyzer right after the token has been accepted
        """
        if self.state == None:
            if token.value == 'PROGRAM':
                self.variables = {}
                self.state = 'Start'
        
        elif self.state == 'Start':
            if token.unit.text == 'ide':
                if token.value in self.variables:
                    raise Exception('this variable has already been declared: ' + token.value)
                
                variable_location = None
                self.variables[token.value] = variable_location
                raise Exception("TODO - localização das variáveis (registradores)")
            
            elif token.unit.text == 'BEGIN':
                self.state = 'main program'
            
        elif self.state == 'main program':
            if(token.unit is None):
                return

            elif token.unit.text == 'ide':
                if token.value not in self.variables:
                    raise Exception('the variable "'+token.value+'" must be declared')

                raise Exception("TODO - Implementar ATRIB")
                self.state = 'ATRIB'

            elif token.unit.text == 'READ':
                raise Exception("TODO - Implementar READ")
                self.state = 'READ'
            
            elif token.unit.text == 'WRITE':
                raise Exception("TODO - Implementar WRITE")
                self.state = 'WRITE'

            elif token.unit.text == 'IF':
                raise Exception("TODO - Implementar IF")
                self.state = 'IF'
            
            elif token.unit.text == 'WHILE':
                raise Exception("TODO - Implementar WHILE")
                self.state = 'WHILE'
            
            elif token.unit.text == 'REPEAT':
                raise Exception("TODO - Implementar REPEAT")
                self.state = 'REPEAT'
                

            elif token.unit.text == 'END':
                raise Exception("TODO - Implementar END")

        elif self.state == 'READ':
            self.state = 'main program'
        
        elif self.state == 'WRITE':
            self.state = 'main program'
        
        elif self.state == 'IF':
            self.state = 'main program'

        elif self.state == 'WHILE':
            self.state = 'main program'
        
        elif self.state == 'ATRIB':
            self.state = 'main program'
            
        elif self.state == 'REPEAT':
            self.state = 'main program'
        
        elif self.state == 'UNTIL':
            self.state = 'main program'

    def boolean_exp(self, token:Token):
        """
        In case of an expression try to treat the Token and return True, if not return False
        """
        raise Exception("TODO - Implementar boolean_exp")
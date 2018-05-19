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

    operators_inverse{
        "=": "<>",
        "IS": '<>',
        "<>": "=",
        "!=": '=',
        "<>": '=',
        "NOT": '=',
        ">": '<=',
        ">=": '<',
        "<": '>=',
        "<=": '>',
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

                self.ide_cache = CodeGenerator.__int_to_byte(self.variables[token.value])
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
            var_adress = CodeGenerator.__int_to_byte(self.variables[token.value])
            self.__concat_to_bytecode(var_adress)
            self.state = 'main program'
        
        elif self.state == 'WRITE':
            var_adress = CodeGenerator.__int_to_byte(self.variables[token.value])
            self.__concat_to_bytecode(var_adress)
            self.__concat_to_bytecode(bytes([CodeGenerator.command_dict['OUT']]))            
            self.state = 'main program'
        
        elif self.state == 'IF':
            
            self.boolean_exp(token)

            if token.unit.text == 'THEN':
                self.__concat_to_bytecode(self.operator_cache)
                self.operator_cache = None

                self.backpatching_stack.append(('IF', self.bytecode_next_position()))
                self.__concat_to_bytecode(bytes([CodeGenerator.command_dict['JF'], 0, 0]))

                self.state = 'main program'
        
        elif self.state == 'WHILE':
            self.boolean_exp(token)

            if token.unit.text == 'DO':
                self.__concat_to_bytecode(self.operator_cache)
                self.operator_cache = None
                
                self.backpatching_stack.append(('WHILE', self.bytecode_next_position()))
                self.__concat_to_bytecode(bytes([CodeGenerator.command_dict['JF'], 0, 0]))

                self.state = 'main program'

        
        elif self.state == 'ATRIB':
            algebric_operator = CodeGenerator.algebric_operator_regex.match(token.unit.text)
            
            if token.unit.text == 'ide':
                var_adress = CodeGenerator.__int_to_byte(self.variables[token.value])
                self.__concat_to_bytecode(bytes([CodeGenerator.command_dict['LOD']]) + var_adress)
                
            elif algebric_operator:
                # regex (* | / | + | - | )
                algebric_operator = algebric_operator.group(1)
                operator_hex = CodeGenerator.command_dict[CodeGenerator.operators_dict[algebric_operator]]
                self.operator_cache = bytes([operator_hex])

            elif token.unit.text == 'num':
                num_hex = CodeGenerator.__int_to_byte(int(token.value))
                self.__concat_to_bytecode(bytes([CodeGenerator.command_dict['LDI']]) + num_hex)
            
            elif token.unit.text == ':=':
                pass

            else:
                if self.operator_cache:
                    self.__concat_to_bytecode(self.operator_cache)
                    self.operator_cache = None

                self.__concat_to_bytecode(bytes([CodeGenerator.command_dict['STO']]) + self.ide_cache)
                self.state = 'main program'
                
                self.process_token(token)
        
        elif self.state == 'REPEAT':
            self.backpatching_stack.append(('REPEAT', self.bytecode_next_position()))
            self.state = 'main program'
        
        elif self.state == 'UNTIL':
            if token.unit.text == 'UNTIL':
                pass

            elif not self.boolean_exp(token):
                self.state = 'main program'

                self.process_token(token)                

    def boolean_exp(self, token:Token):
        """
        In case of an expression try to treat the Token and return True, if not return False
        """
        raise Exception("TODO - Implementar boolean_exp")
        
        operator = CodeGenerator.operator_regex.match(token.unit.text)
        if token.unit.text == 'ide':
            var_adress = CodeGenerator.__int_to_byte(self.variables[token.value])
            self.__concat_to_bytecode(bytes([CodeGenerator.command_dict['LOD']]) + var_adress)
            
        elif operator:
            operator = operator.group(1)
            operator_hex = CodeGenerator.command_dict[CodeGenerator.operators_dict[operator]]
            self.operator_cache = bytes([operator_hex])

        elif token.unit.text == 'num':
            num_hex = CodeGenerator.__int_to_byte(int(token.value))
            self.__concat_to_bytecode(bytes([CodeGenerator.command_dict['LDI']]) + num_hex)
        
        else:        
            return False

        return True
    
# byte 22,23 0x43,0x00 but 00,00
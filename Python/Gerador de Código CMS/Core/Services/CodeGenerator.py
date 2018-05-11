from Core.Entities.Token import Token
import re


class CodeGenerator:
    command_dict = {
        'ADD':  0x01, # soma os inteiros do topo da pilha
        'SUB':  0x02, # subtrai os inteiros do topo da pilha
        'MUL':  0x03, # multiplica os inteiro do topo da pilha
        'LSP':  0x4F, # Stack Pointer<-endereço
        'JMP':  0x5A, # endereço | Instruction P<-endereço
        'LDI':  0x44, # move inteiro para o topo da pilha
        'ADI':  0x14, # adiciona inteiro ao topo da pilha
        'SUI':  0x15, # subtrai inteiro do topo da pilha
        'MUI':  0x16, # multiplica inteiro do topo da pilha
        'DVI':  0x17, # divide inteiro do topo da pilha
        'LOD':  0x40, # endereço | carrega o conteúdo de end p/ pilha
        'STO':  0x41, # endereço | carrega o topo da pilha para end
        'OUT':  0x58, # pega o topo da pilha e mostra
        'IN':   0x57, # ler do teclado e coloca na pilha
        'JF':   0x5C, # endereço | pula para o endereço se falso
        'EQ':   0x20, #  (=)  verdade empilha $FFFF falso $0000
        'NE':   0x21, #  (!=) verdade empilha $FFFF falso $0000
        'GT':   0x22, #  (>) verdade empilha $FFFF falso $0000
        'GE':   0x23, #  (>=) verdade empilha $FFFF falso $0000
        'LT':   0x24, #  (<) verdade empilha $FFFF falso $0000
        'LE':   0x25, #  (<=) verdade empilha $FFFF falso $0000
        'STOP': 0x61 # encerra a execuçäo do programa
    }
    # TINYP tem a máquina
    operators_dict = {
        "=": 'EQ',
        "!=": 'NE',
        ">": 'GT',
        ">=": 'GE',
        "<": 'LT',
        "<=": 'LE'
    }

    byteorder = 'little'
    int_byte_size = 2
    operator_regex = re.compile('(<=|>=|>|<|=|!=)')

    @staticmethod
    def __int_to_byte(integer:int):
        return integer.to_bytes(CodeGenerator.int_byte_size, byteorder=CodeGenerator.byteorder)

    # t = (4096).to_bytes(2, byteorder='little')
    # int.from_bytes(t, byteorder='little')

    def __init__(self, Tokens:Token=None):
        self.Tokens = Tokens
        self.prefix_size = 6
        self.bytecode = bytes([CodeGenerator.command_dict['LSP'], 0x00, 0x10, CodeGenerator.command_dict['JMP']])
        self.variables = None
        self.state = None
        self.backpatching_stack = []
        # print(self.bytecode)
    
    def __concat_to_bytecode(self, byte:bytes):
        self.bytecode +=  byte
    
    def add_variables(self):
        n_variables = len(self.variables)

        code_start = self.prefix_size + n_variables * 2
        code_index = CodeGenerator.__int_to_byte(code_start)

        self.__concat_to_bytecode(code_index)

        for i in range(n_variables):
            self.__concat_to_bytecode(bytes([0,0]))

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
                
                variable_location = len(self.variables) * 2 + self.prefix_size
                self.variables[token.value] = variable_location
            
            elif token.unit.text == 'BEGIN':
                self.add_variables()
                self.state = 'main program'
            
        elif self.state == 'main program':
            if(token.unit is None):
                return

            elif token.unit.text == 'READ':
                self.__concat_to_bytecode(bytes([CodeGenerator.command_dict['IN'],CodeGenerator.command_dict['STO']]))
                self.state = 'READ'

            elif token.unit.text == 'IF':
                self.state = 'IF'

        elif self.state == 'READ':
            var_adress = CodeGenerator.__int_to_byte(self.variables[token.value])
            self.__concat_to_bytecode(var_adress)
            self.state = 'main program'
        
        elif self.state == 'IF':
            operator = CodeGenerator.operator_regex.match(token.unit.text)
            
            if token.unit.text == 'ide':
                var_adress = CodeGenerator.__int_to_byte(self.variables[token.value])
                self.__concat_to_bytecode(bytes(CodeGenerator.command_dict['LOD']) + var_adress)
            
            elif operator:
                # carrega sinal - regex (< | = | != | >= | <=)
                operator = operator.group(1)
                operator_hex = CodeGenerator.command_dict[CodeGenerator.operators_dict[operator]]
                self.__concat_to_bytecode(bytes(operator_hex))

            elif token.unit.text == 'num':
                num_hex = CodeGenerator.__int_to_byte(int(token.value))
                self.__concat_to_bytecode(bytes(CodeGenerator.command_dict['LDI']) + num_hex)
                # ta errado bem aqui

            elif token.unit.text == 'THEN':
                # por JF no código e salvar o endereço
                self.backpatching_stack.append(('IF', 'endereço do byte após o JF'))
                self.state = 'main program'
            

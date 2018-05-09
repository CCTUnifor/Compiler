class CodeGenerator:
    command_dict = {
        'LSP' : 0x4F, # Stack Pointer<-endereço
        'JMP': 0x5A, # endereço | Instruction P<-endereço
        'LDI': 0x44, # move inteiro para o topo da pilha
        'ADI': 0x14, # adiciona inteiro ao topo da pilha
        'SUI': 0x15, # subtrai inteiro do topo da pilha
        'MUI': 0x16, # multiplica inteiro do topo da pilha
        'DVI': 0x17, # divide inteiro do topo da pilha
        'LOD': 0x40, # endereço | carrega o conteúdo de end p/ pilha
        'STO': 0x41, # endereço | carrega o topo da pilha para end
        'OUT': 0x58, # pega o topo da pilha e mostra
        'IN': 0x57, # ler do teclado e coloca na pilha
        'JF': 0x5C, # endereço | pula para o endereço se falso
        'EQ': 0x20, # verdade empilha $FFFF falso $0000
        'NE': 0x21, # verdade empilha $FFFF falso $0000
        'GT': 0x22, # verdade empilha $FFFF falso $0000
        'GE': 0x23, # verdade empilha $FFFF falso $0000
        'LT': 0x24, # verdade empilha $FFFF falso $0000
        'LE': 0x25, # verdade empilha $FFFF falso $0000
        'STOP': 0x61 # encerra a execuçäo do programa
    }

    def __init__(self, create_file, Tokens=None):
        self.Tokens = Tokens
        self.create_file = create_file

    def compile(self):
        if self.Tokens is None:
            raise Exception('Invalid Compile process: the argument "Tokens" should not be None at this point')
    
    def process_token(self, token):
        """
        Called by the compile process or the sintatic analyzer right after the token has been accepted
        """
        pass
    
    def get_byte_code(self):
        pass
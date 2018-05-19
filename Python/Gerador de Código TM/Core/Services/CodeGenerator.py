from Core.Entities.Token import Token
import re


class CodeGenerator:
    command_dict = {
        "HALT"  :  ("RO", 1)  , # (análoga ao STOP do CMS) os registradores s e t são ignorados
        "IN"    :  ("RO", 1)  , # (análoga ao IN do CMS) os registradores s e t são ignorados
        "OUT"   :  ("RO", 1)  , # (análoga ao OUT do CMS) os registradores s e t são ignorados
        "ADD"   :  ("RO", 3)  , # reg(r)=reg(s)+reg(t)
        "SUB"   :  ("RO", 3)  , # reg(r)=reg(s)-reg(t)
        "MUL"   :  ("RO", 3)  , # reg(r)=reg(s)*reg(t)
        "DIV"   :  ("RO", 3)  , # reg(r)=reg(s)/reg(t) pode gerar divisão por zero

        "LDA"   :  ("RM", 3)  , # (LOAD) reg[r]=dMem(a) (carrega r com valor de memória em a)
        "LD"    :  ("RM", 3)  , # (LOAD Adress) reg[r]=a
        "LDC"   :  ("RM", 3)  , # (LOAD Constant) reg[r]=d
        "ST"    :  ("RM", 3)  , # (STORE) dMem(a)=reg[r]
        "JLT"   :  ("RM", 3)  , # (<)  if (reg(r) <  0) reg(PC_REG)=a
        "JLE"   :  ("RM", 3)  , # (<=) if (reg(r) <= 0) reg(PC_REG)=a
        "JGE"   :  ("RM", 3)  , # (>=) if (reg(r) >= 0) reg(PC_REG)=a
        "JGT"   :  ("RM", 3)  , # (>)  if (reg(r) >  0) reg(PC_REG)=a
        "JEQ"   :  ("RM", 3)  , # (==) if (reg(r) == 0) reg(PC_REG)=a
        "JNE"   :  ("RM", 3)    # (!=) if (reg(r) != 0) reg(PC_REG)=a
    }

    PC_REG = 7

    operators_dict = {
        "<"     : 'JLT' ,
        "<="    : 'JLE' ,
        ">="    : 'JGE' ,
        ">"     : 'JGT' ,
        "="     : 'JEQ' ,
        "IS"    : 'JEQ' ,
        "!="    : 'JNE' ,
        "<>"    : 'JNE' ,
        "NOT"   : 'JNE' ,

        "+"     : 'ADD' ,
        "-"     : 'SUB' ,
        "*"     : 'MUL' ,
        "/"     : 'DIV' 
    }

    operators_inverse = {
        "="     : "<>" ,
        "IS"    : '<>' ,
        "<>"    : "="  ,
        "!="    : '='  ,
        "<>"    : '='  ,
        "NOT"   : '='  ,
        ">"     : '<=' ,
        ">="    : '<'  ,
        "<"     : '>=' ,
        "<="    : '>'
    }

    operator_regex = re.compile(r'(<=|>=|<>|NOT|IS|>|<|=|!=)')
    algebric_operator_regex = re.compile(r'(\*|\/|\+|-)')

    def __init__(self, Tokens:Token=None):
        self.Tokens = Tokens
        self.intermediate_code = None
        self.variables = None
        self.state = None
        self.ide_cache = None
        self.command_counter = 0
        self.end_stack = []
        self.command_cache = []
    
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
                
                variable_location = len(self.variables)
                self.variables[token.value] = variable_location

                if variable_location >= 7:
                    raise Exception('maximum of 6 variables was exceeded')
            
            elif token.unit.text == 'BEGIN':
                self.state = 'main program'
            
        elif self.state == 'main program':
            if(token.unit is None):
                return

            elif token.unit.text == 'READ':
                self.state = 'READ'
                

            elif token.unit.text == 'ide':
                if token.value not in self.variables:
                    raise Exception('the variable "'+token.value+'" must be declared')

                self.state = 'ATRIB'
                self.ide_cache = self.variables[token.value]
                raise Exception("TODO - Implementar ATRIB")
            
            elif token.unit.text == 'WRITE':
                raise Exception("TODO - Implementar WRITE")
                self.state = 'WRITE'

            elif token.unit.text == 'IF':
                self.state = 'IF'
            
            elif token.unit.text == 'WHILE':
                raise Exception("TODO - Implementar WHILE")
                self.state = 'WHILE'
            
            elif token.unit.text == 'REPEAT':
                raise Exception("TODO - Implementar REPEAT")
                self.state = 'REPEAT'
                

            elif token.unit.text == 'END':
                item = self.end_stack.pop()
                command = item[1]

                if command == "IF":
                    pass

                raise Exception("TODO - Implementar END")

        elif self.state == 'READ':
            if token.value not in self.variables:
                raise Exception("the variable \"" + token.value + "\" was not declared")
                
            reg = self.variables[token.value]
            
            self.__command_builder("IN", [reg])

            self.state = 'main program'
        
        elif self.state == 'WRITE':
            self.state = 'main program'
        
        elif self.state == 'IF':

            self.boolean_exp(token)

            if token.unit.text == 'THEN':
                operator = self.command_cache[1]
                first_register = self.command_cache[0]

                self.end_stack.append((self.command_counter, "IF", [operator, first_register]))

                self.command_counter += 1
                self.state = 'main program'

            raise Exception("TODO - Implementar IF")
            

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
        operator = CodeGenerator.operator_regex.match(token.unit.text)

        if operator:
            self.command_cache.append(CodeGenerator.operators_dict[token.value])

        elif token.unit.text == 'ide':
            self.command_cache.append(self.variables[token.value])

        elif token.unit.text == 'num':
            # self.command_cache.append(0) # boolean expression just compare with number 0 in TM VM
            pass
        
        else:
            return False
        
        return True

    
    def __write_on_code(self, code):
        self.intermediate_code += str(self.command_counter) + ": "
        self.intermediate_code += str(code) + "\n"

        self.command_counter += 1

    def __RO_commands_builder(self, command, params):
        command_size = CodeGenerator.command_dict[command][1]

        if not (len(params) is command_size):
            raise Exception('Invalid parameters ' + str(params) + ' for the command "' + command + '"')
        
        code = ""

        for i in range(4):
            if i is 0:
                code += command + " "
            
            elif i <= command_size:
                code += params[i-1]

            else:
                code += "0"
            
            if i is 1 or i is 2:
                code += ","
        
        self.__write_on_code(code)
    
    def __RM_commands_builder(self, command, params):
        raise Exception("TODO - Implementar __RM_commands_builder")

    
    def __command_builder(self, command, params):
        if command not in CodeGenerator.command_dict:
            raise Exception('Invalid command')

        command_category = CodeGenerator.command_dict[command][0]
        
        if command_category == "RO":
            self.__RO_commands_builder(command, params)

        elif command_category == "RM":
            self.__RM_commands_builder(command, params)

        else:
            raise Exception('Invalid command category: ' + command_category)

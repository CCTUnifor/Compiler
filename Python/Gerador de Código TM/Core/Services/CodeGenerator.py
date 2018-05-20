from Core.Entities.Token import Token
import re


class CodeGenerator:
    command_dict = {
        # (type, construction_type,size)
        "HALT"  :  ("RO","RO", 1)  , # (análoga ao STOP do CMS) os registradores s e t são ignorados
        "IN"    :  ("RO","RO", 1)  , # (análoga ao IN do CMS) os registradores s e t são ignorados
        "OUT"   :  ("RO","RO", 1)  , # (análoga ao OUT do CMS) os registradores s e t são ignorados
        "ADD"   :  ("RO","RO", 3)  , # reg(r)=reg(s)+reg(t)
        "SUB"   :  ("RO","RO", 3)  , # reg(r)=reg(s)-reg(t)
        "MUL"   :  ("RO","RO", 3)  , # reg(r)=reg(s)*reg(t)
        "DIV"   :  ("RO","RO", 3)  , # reg(r)=reg(s)/reg(t) pode gerar divisão por zero
        
        "LDA"   :  ("RM","RM", 3)  , # (LOAD) reg[r]=dMem(a) (carrega r com valor de memória em a)
        "LD"    :  ("RM","RM", 3)  , # (LOAD Adress) reg[r]=a
        "LDC"   :  ("RM","RO", 3)  , # (LOAD Constant) reg[r]=d
        "ST"    :  ("RM","RM", 3)  , # (STORE) dMem(a)=reg[r]
        "JLT"   :  ("RM","RM", 3)  , # (<)  if (reg(r) <  0) reg(PC_REG)=a
        "JLE"   :  ("RM","RM", 3)  , # (<=) if (reg(r) <= 0) reg(PC_REG)=a
        "JGE"   :  ("RM","RM", 3)  , # (>=) if (reg(r) >= 0) reg(PC_REG)=a
        "JGT"   :  ("RM","RM", 3)  , # (>)  if (reg(r) >  0) reg(PC_REG)=a
        "JEQ"   :  ("RM","RM", 3)  , # (==) if (reg(r) == 0) reg(PC_REG)=a
        "JNE"   :  ("RM","RM", 3)    # (!=) if (reg(r) != 0) reg(PC_REG)=a
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
        "NOT"   : '='  ,
        ">"     : '<=' ,
        ">="    : '<'  ,
        "<"     : '>=' ,
        "<="    : '>'
    }

    operators_reverse = {
        "="     : "=" ,
        "IS"    : '=' ,
        "<>"    : "<>"  ,
        "!="    : '!='  ,
        "NOT"   : 'NOT'  ,
        ">"     : '<' ,
        ">="    : '<='  ,
        "<"     : '>' ,
        "<="    : '>='
    }

    operator_regex = re.compile(r'(<=|>=|<>|NOT|IS|>|<|=|!=)')
    algebric_operator_regex = re.compile(r'(\*|\/|\+|-)')

    def __init__(self, Tokens:Token=None):
        self.Tokens = Tokens
        self.intermediate_code = []
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

                if variable_location > 6:
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
                self.command_cache = [-1, self.variables[token.value],-1,-1]
            
            elif token.unit.text == 'WRITE':
                self.state = 'WRITE'

            elif token.unit.text == 'IF':
                self.command_cache = [-1,-1]
                self.state = 'IF'
            
            elif token.unit.text == 'WHILE':
                raise Exception("TODO - Implementar WHILE")
                self.state = 'WHILE'
            
            elif token.unit.text == 'REPEAT':
                self.state = 'REPEAT'                

            elif token.unit.text == 'END':                
                if len(self.end_stack):
                    item = self.end_stack.pop()
                    command_counter = item[0]
                    command = item[1]

                    if command == "IF":
                        cmd = item[2][0] + " " + item[2][1] + "," + self.command_counter - command_counter - 1 + "(7)"
                        self.__write_on_code(cmd, False)
                    
                    elif command == "REPEAT":
                        self.state = 'UNTIL'
                        self.end_stack.append(item)

                else:
                    self.__command_builder("HALT", [0])

        elif self.state == 'READ':
            if token.value not in self.variables:
                raise Exception("the variable \"" + token.value + "\" was not declared")
                
            reg = self.variables[token.value]
            
            self.__command_builder("IN", [reg])

            self.state = 'main program'
        
        elif self.state == 'WRITE':
            if token.value not in self.variables:
                raise Exception("the variable \"" + token.value + "\" was not declared")
                
            reg = self.variables[token.value]
            
            self.__command_builder("OUT", [reg])

            self.state = 'main program'
        
        elif self.state == 'IF':
            self.boolean_exp(token)

            if token.unit.text == 'THEN':
                self.end_stack.append((self.command_counter, "IF", self.command_cache))
                self.command_cache = []

                self.command_counter += 1 
                self.state = 'main program'            

        elif self.state == 'WHILE':
            self.state = 'main program'
        
        elif self.state == 'ATRIB':
            algebric_operator = CodeGenerator.algebric_operator_regex.match(token.unit.text)
            
            if token.unit.text == 'num':
                self.command_cache[0] = 'LDC'
                self.command_cache[2] = str(token.value)
                self.command_cache[3] = '0'

                self.state = 'main program'
            
            elif token.unit.text == 'ide':
                reg = self.variables[token.value]

                if(self.command_cache[2] == -1):
                    self.command_cache[2] = reg

                else:
                    self.command_cache[3] = reg

                    self.state = 'main program'

            elif algebric_operator:
                algebric_operator = algebric_operator.group(1)
                operator_code = CodeGenerator.command_dict[CodeGenerator.operators_dict[algebric_operator]]
                self.command_cache[0] = operator_code
            
            if self.state == 'main program':
                self.__command_builder(self.command_cache[0], self.command_cache[1::])
            
        elif self.state == 'REPEAT':
            self.end_stack.append((self.command_counter, "REPEAT"))
            self.command_counter += 1
            
            self.state = 'main program'
        
        elif self.state == 'UNTIL':
            if token.unit.text == 'UNTIL':
                self.command_cache = [-1,-1]
            
            elif not self.boolean_exp(token):
                item = self.end_stack.pop()
                command_counter = item[0]
                command = item[1]
                
                op_code = CodeGenerator.operators_inverse[self.command_cache[0]]
                first_reg = self.command_cache[1]
                d_value = command_counter - self.command_counter

                cmd = op_code + " " + first_reg + "," + d_value + "(7)"
                
                self.__write_on_code(cmd, False)

                self.state = 'main program'
                self.process_token(token)

    def boolean_exp(self, token:Token):
        """
        In case of an expression try to treat the Tokens and return True, if not return False
        """
        # raise Exception("TODO - Implementar boolean_exp")
        operator = CodeGenerator.operator_regex.match(token.unit.text)

        if operator:
            operator_code = None

            if self.command_cache[1] is -1: # the variable was not found yet
                operator_code = CodeGenerator.operators_dict[CodeGenerator.operators_reverse[token.value]]
            else:
                operator_code = CodeGenerator.operators_dict[token.value]

            self.command_cache[0](operator_code)

        elif token.unit.text == 'ide':
            self.command_cache[1](self.variables[token.value])

        elif token.unit.text == 'num':            
            pass # boolean expression just compare with number 0 in TM VM
        
        else:
            return False
        
        return True

    
    def __write_on_code(self, code, next=True):
        self.intermediate_code.append(str(self.command_counter) + ": " + str(code))

        if next:
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

        command_category = CodeGenerator.command_dict[command][1]
        
        if command_category == "RO":
            self.__RO_commands_builder(command, params)

        elif command_category == "RM":
            self.__RM_commands_builder(command, params)

        else:
            raise Exception('Invalid command category: ' + command_category)

from Entidades.Token import Token


class LexicalAnalyzer:
    """Implementação de máquina de estados para reconher 
        Léxicamente uma Expressão Regular(ER)"""
    def __init__(self, stream):
        self.stream = stream
        self.tokens = []
        self.cursor = 0

    def raiseException(self, character):
        lines = self.stream.split('\n')
        print('--------------------------------------------------')
        print(lines)
        print('--------------------------------------------------')
        line = ''
        acum = 0
        counter = 0

        for item in lines:
            acum += len(item)
            if(acum >= self.cursor - counter):
                line = item
                break
            counter += 1
        

        raise Exception('Lexical error on character: ' + str(character) + ' on cursor: ' + str(self.cursor) + ' on line: "' + line + '"')

    def notEnded(self):
        return self.cursor < len(self.stream)
    
    def getAllTokens(self, includeComments=True):
        while(self.notEnded()):
            self.getToken()

        if(includeComments):
            return self.tokens
        else:
            retorno = []
            for item in self.tokens:
                if(item.ttype is not Token.COMMENT):
                    retorno.append(item)
            return retorno

    def getToken(self):
        return self.analyze()
    
    def addToken(self, token):
        self.tokens.append(token)
        self.cursor += 1
        return token

    def identifyWord(self, token):
        if(token.value in Token.RESERVEDWORD.values):
            token.ttype = Token.RESERVEDWORD
        else:
            token.ttype = Token.IDENTIFIER

    def analyze(self):
        state = 1
        accumulator = None
        token = None   

        while(self.notEnded()):
            character = self.stream[self.cursor] if self.cursor < len(self.stream) else ''
            # print('char: ' + charr + ' state ' + str(state))
            
            if(state == 1):
                if(character.isdigit()):
                    accumulator = Token(accumulator, Token.NUMBER)
                    accumulator.value = character
                    state = 2

                elif(character in Token.OPERATOR.values):
                    token = self.addToken(Token(character, Token.OPERATOR))
                    break

                elif(character in Token.PARENTHESES.values):
                    token = self.addToken(Token(character, Token.PARENTHESES))
                    break

                elif(character in Token.SPACE.values):
                    pass

                elif(character in Token.SEMICOLON.values):
                    token = self.addToken(Token(character, Token.SEMICOLON))
                    break

                elif(character is '{'):
                    accumulator = Token(accumulator, Token.COMMENT)
                    accumulator.value = character
                    state = 3

                elif(character.isalpha()):
                    accumulator = Token(accumulator, Token.WORD)
                    accumulator.value = character
                    state = 4

                elif(character is ':'):
                    accumulator = Token(accumulator, Token.ATRIB)
                    accumulator.value = character
                    state = 5

                elif(character in Token.SUM.values):
                    token = self.addToken(Token(character, Token.SUM))
                    break
                elif(character in Token.PROD.values):
                    token = self.addToken(Token(character, Token.PROD))
                    break

                else:
                    self.raiseException(character)

            elif(state == 2):
                if(character.isdigit()):
                    accumulator.value += character

                else:
                    token = self.addToken(accumulator)
                    self.cursor -= 1
                    accumulator = None
                    break
            
            elif(state == 3):
                if(character is '}'):
                    token = self.addToken(accumulator)
                    accumulator = None
                    break
                else:
                    accumulator.value += character
            
            elif(state == 4):
                if(character.isalpha() or character.isdigit()):
                    accumulator.value += character
                    
                else:
                    token = self.addToken(accumulator)
                    self.cursor -= 1
                    accumulator = None
                    self.identifyWord(token)
                    break
            
            elif(state == 5):
                if(character is '='):
                    accumulator.value += character
                    token = self.addToken(accumulator)
                    accumulator = None
                    break
                    
                else:
                    self.raiseException(accumulator)

            self.cursor += 1

        if(accumulator is not None):
            token = self.addToken(accumulator)

        # do not return a comment
        if(token is not None and token.ttype is Token.COMMENT):
            return self.analyze()

        return token

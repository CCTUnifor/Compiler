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
        line = ''
        acum = 0
        counter = 0

        for item in lines:
            acum += len(item)
            if(acum >= self.cursor - counter):
                line = item
                break
            counter += 1
        

        raise Exception('Lexical error on character: ' + str(character) + ' on cursor: ' + str(self.cursor)
                        + ' on line: ' + line)

    def notEnded(self):
        return self.cursor <= len(self.stream)
    
    def getToken(self):
        return self.analyze()
    
    def addToken(self, token):
        self.tokens.append(token)
        self.cursor += 1
        return token

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

                elif(character.isalpha()):
                    token = self.addToken(Token(character, Token.WORD))
                    break

                elif(character is '/'):
                    accumulator = Token(accumulator, Token.COMMENT)
                    accumulator.value = character
                    state = 3

                else:
                    self.raiseException(character)

            elif(state == 2):
                if(character.isdigit()):
                    accumulator.value += character
                    
                else:
                    token = self.addToken(accumulator)
                    break
            
            elif(state == 3):
                if(character is '*'):
                    accumulator.value += character
                    state = 4
                else:
                    self.raiseException(character)
            
            elif(state == 4):
                if(character is '*'):
                    accumulator.value += character
                    state = 5

                else:
                    accumulator.value += character

            elif(state == 5):
                accumulator.value += character
                if(character is '/'):
                    token = self.addToken(accumulator)
                    break
                else:
                    state = 4

            self.cursor += 1

        if(accumulator is not None):
            token = self.addToken(accumulator)

        if(token is not None and token.ttype is Token.COMMENT):
            return self.analyze()

        return token

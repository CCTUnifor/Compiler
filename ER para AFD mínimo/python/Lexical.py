from Entidades.Token import Token


class LexicalAnalyzer:
    def __init__(self):
        pass
    
    @staticmethod
    def analyze(stream):
        tokens = []
        state = 1
        cursor = 0
        accumulator = ""

        while(cursor <= len(stream)):
            character = stream[cursor] if cursor < len(stream) else ''
            # print('char: ' + charr + ' state ' + str(state))
            
            if(state == 1):
                if(character.isdigit()):
                    accumulator = Token(accumulator, Token.NUMBER)
                    accumulator.value = character
                    state = 2

                elif(character in Token.OPERATOR.values):
                    tokens.append(Token(character, Token.OPERATOR))
                    state = 3

                elif(character in Token.PARENTHESES.values):
                    tokens.append(Token(character, Token.PARENTHESES))
                    state = 4

                elif(character is '' or character is ' '):
                    pass

                elif(character.isalpha()):
                    tokens.append(Token(character, Token.WORD))
                    state = 5

                elif(character in Token.COMMENT.values):
                    accumulator = Token(accumulator, Token.COMMENT)
                    accumulator.value = character
                    tokens.append(accumulator)
                    state = 6

                else:
                    raise Exception('Lexical error on character: ' + str(character))

            elif(state == 2):
                if(character.isdigit()):
                    accumulator.value += character
                    
                else:
                    tokens.append(accumulator)
                    accumulator = ""
                    state = 1
                    continue    

            elif(state == 3):
                state = 1
                continue

            elif(state == 4):
                state = 1
                continue

            elif(state == 5):
                state = 1
                continue

            elif(state == 6):
                if(character in Token.COMMENT.values):
                    accumulator.value += character
                    state = 7
                else:
                    raise Exception('Lexical error on character: ' + str(character))

            elif(state == 7):
                accumulator.value += character
                if(character in Token.COMMENT.values):
                    state = 8

            elif(state == 8):
                if(character in Token.COMMENT.values):
                    accumulator.value += character
                    accumulator = ''
                    state = 1
                    
                else:
                    raise Exception("Expected '-' on closing comment at: " + str(cursor))

            cursor += 1
        
        print('______________')
        print('Found ' + str(len(tokens)) + ' tokens')

        for item in tokens:
            print(item)

        print('______________')
        return tokens

from Entidades.Node import ThompsonNode as Node
from Entidades.Token import Token


class ERtoAFNE:
    def __init__(self):
        self.nodeCount = 1

    def get_node_count(self):
        self.nodeCount += 1
        return self.nodeCount - 1
    
    def make_path(self, cursor, token, used = None):
        new_node = used if used else self.get_new_node()
        cursor.addDestination(new_node, token)
        return new_node

    def get_new_node(self):
        return Node(self.get_node_count())

    def to_thompson(self, tokens):
        root = self.get_new_node()
        cursor = root
        structStart = root
        structEnd = root
        index = 0

        tlen = len(tokens)
        state = 1

        while(index < tlen):
            token = tokens[index]
            if(state == 1):
                if(token.ttype.id == Token.WORD.id or token.ttype.id == Token.NUMBER.id):
                    state = 'WORD'
                    continue

                elif(token.ttype.id == Token.OPERATOR.id):
                    if(token.value == "|"):
                        state = 'OR'
                        continue
                    elif(token.value == "+"):
                        state = 'REPEATE'
                        continue
                    elif(token.value == "*"):
                        state = 'REPEATE 0'
                        continue
                elif(token.ttype.id == Token.COMMENT.id):
                    pass
                elif(token.ttype.id == Token.COMMENT.id):
                    pass
            elif(state == 'WORD'):
                structStart = cursor
                cursor = self.make_path(cursor, token)
                state = 1
            
            elif(state == 'OR'):
                pathToCursor = structStart.getPath(cursor)
                oldValue = pathToCursor.value
                pathToCursor.value = None

                cursor = self.make_path(cursor, oldValue) # structUp
                
                state = 'OR STRUCTURE'

            elif(state == 'OR STRUCTURE'):
                structDown = self.make_path(structStart, None)
                structDown = self.make_path(structDown, token)
                structDown = self.make_path(structDown, None)
                cursor = self.make_path(cursor, None, structDown)
                # structStart = cursor
                state = 1

            elif(state.startswith('REPEATE')):
                pathToCursor = structStart.getPath(cursor)
                oldValue = pathToCursor.value
                pathToCursor.value = None

                repeatNode = cursor
                cursor = self.make_path(repeatNode, oldValue)
                self.make_path(cursor, None, repeatNode)
                cursor = self.make_path(cursor, None)

                if(state == 'REPEATE 0'):
                    cursor = self.make_path(structStart, None, cursor)
                

            index+=1

        return root

    def convert(self, tokens):
        root = self.to_thompson(tokens)

        return root

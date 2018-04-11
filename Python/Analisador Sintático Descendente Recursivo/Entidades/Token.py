import os


class TokenType:
    def __init__(self, ttype, values, name):
        self.id = ttype
        self.values = values
        self.name = name
    
    def __str__(self):
        return self.name


class Token:
    NUMBER = TokenType(1, None, 'Number')
    OPERATOR = TokenType(2, ['<', '='], 'Operator')
    PARENTHESES = TokenType(3, ['(', ')'], 'Parentheses')
    WORD = TokenType(4, None, 'Word')
    COMMENT = TokenType(5, ['{', '}'], 'Comment')
    SEMICOLON = TokenType(6, [';'], 'Decl-sep')
    IDENTIFIER = TokenType(7, None, 'Identifier')
    RESERVEDWORD = TokenType(8, [
        'if'    ,
        'then'  ,
        'else'  ,
        'end'   ,
        'repeat',
        'until' ,
        'write' ,
        'read'  ,
    ], 'ReservedWord')
    SUM = TokenType(9, ['+', '-'], 'Sum')
    PROD = TokenType(10, ['*', '/'], 'Prod')
    ATRIB = TokenType(11, [':', '='], 'Atrib-decl')
    SPACE = TokenType(12, ['', ' ', '\n', os.linesep], 'Space')

    def __init__(self, value, ttype):
        self.value = str(value).strip()
        self.ttype = ttype
    
    def __str__(self):
        return str(self.value).ljust(10, ' ') + str(self.ttype)

import os


class TokenType:
    def __init__(self, ttype, values, name):
        self.id = ttype
        self.values = values
        self.name = name


class Token:
    NUMBER = TokenType(1, None, 'Number')
    OPERATOR = TokenType(2, ['<', '='], 'Operator')
    PARENTHESES = TokenType(3, ['(', ')'], 'Parentheses')
    WORD = TokenType(4, None, 'Word')
    COMMENT = TokenType(5, ['{', '}'], 'Comment')
    SEMICOLON = TokenType(6, [';'], 'Decl-sep')
    IDENTIFICATOR = TokenType(7, None, 'Ide')
    RESERVEDWORD = TokenType(8, [
        'if'    , 'IF',
        'then'  , 'THEN',
        'else'  , 'ELSE'
        'end'   , 'END',
        'repeat', 'REPEAT',
        'until' , 'UNTIL',
        'write' , 'WRITE',
        'read'  , 'READ',
    ], 'RESERVEDWORD')
    SUM = TokenType(9, ['+', '-'], 'Sum')
    PROD = TokenType(10, ['*', '/'], 'Prod')
    ATRIB = TokenType(11, [':', '='], 'Atrib-decl')
    SPACE = TokenType(12, ['', ' ', '\n', os.linesep], 'Space')

    def __init__(self, value, ttype):
        self.value = value
        self.ttype = ttype
    
    def __str__(self):
        return str(self.value).ljust(10, ' ') + self.ttype.name

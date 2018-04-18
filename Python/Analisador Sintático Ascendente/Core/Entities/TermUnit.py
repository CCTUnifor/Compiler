class TermUnit:
    TERMINAL = 'TERMINAL'
    NONTERMINAL = 'NON-TERMINAL'
    EMPTY = 'EMPTY'
    STREAM_END = '$'

    def __init__(self, Type, Text):
        self.type = Type
        self.text = Text

    def __str__(self):
        return self.text
    
    def __repr__(self):
        return self.__str__()
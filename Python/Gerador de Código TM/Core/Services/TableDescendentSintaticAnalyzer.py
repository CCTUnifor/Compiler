from Core.Entities.TermUnit import TermUnit
from Core.Entities.Premise import Premise
from Core.Entities.Grammar import Grammar

from Core.Services.TextGrammar import TextToGrammar
from Core.Services.LexicAnalyzer import LexicAnalyzer

from Core.Services.GrammarFirst import First
from Core.Services.GrammarFollow import Follow


class TableService:    
    ErrorString = "Error"

    def __init__(self, grammar):
        self.grammar = grammar
        self.table = None
        self.first = First(grammar)
        self.follow = Follow(grammar, self.first)

    def makeTable(self):
        self.table = {}
        
        for non_terminal in self.grammar.NonTerminals:
            self.table[non_terminal.text] = {Grammar.STREAM_END_UNIT.text: self.ErrorString}

            for terminal in self.grammar.Alphabet:
                self.table[non_terminal.text][terminal.text] = self.ErrorString            

    def passToTable(self, i, j, value):
        actualValue = self.table[i][j]

        if(actualValue == self.ErrorString):
            self.table[i][j] = value
        else:
            raise Exception('Ambiguos grammar on '+ i + ' -> ' + j)

    def build_table(self):
        self.makeTable()
         
        for term in self.grammar.Terms:
            for stream in term.right:
                termToStreamTuple = (term, stream)

                streamFirst = self.first.first(stream)

                for item in streamFirst:
                    if(item.type is TermUnit.TERMINAL):
                        self.passToTable(term.left, item.text, termToStreamTuple)
                
                if(TextToGrammar.EMPTY_UNIT in streamFirst):
                    if(Grammar.STREAM_END_UNIT in term.follow):
                        self.passToTable(term.left, Grammar.STREAM_END_UNIT.text, termToStreamTuple)

                    for item in term.follow:
                        if(item.type is TermUnit.TERMINAL):
                            self.passToTable(term.left, item.text, termToStreamTuple)
    
    def compileGrammar(self):
        if(self.table is None):
            self.first.build_first()
            self.follow.build_follow()
            self.build_table()            
    
    def compile(self, text):
        self.compileGrammar()
        historic = ''
        lxa = LexicAnalyzer(text, self.grammar)

        stack = [Grammar.STREAM_END_UNIT, self.grammar.StartSimbolUnit]

        # while(lxa.isNotDone()):
        #     print(lxa.getToken())
        
        current = lxa.getToken()
        while( len(stack) ):#current.unit.type is not TermUnit.STREAM_END):
            hline = str(current.value).ljust(20) + " " + str(stack) + '\n'
            historic += hline

            top = stack.pop()
            # print(hline)
            
            if(top.type is TermUnit.TERMINAL or top.type is TermUnit.STREAM_END):
                if(top.text == current.value):
                    current = lxa.getToken()
                
                elif(top.text == 'ide'):
                    current = lxa.getToken()

                elif(top.text == 'num'):
                    current = lxa.getToken()

                else:
                    raise Exception("Error on " + current.value + " at:")

            else:
                tupleCell = self.table[top.text][current.unit.text]

                if(tupleCell != self.ErrorString):
                    for term in tupleCell[1][::-1]:
                        if(term.type is not TermUnit.EMPTY):
                            stack.append(term)
                
                elif(current.unit.type is TermUnit.STREAM_END):
                    raise Exception("Error: invalid file end")

                else:
                    raise Exception("Error on " + current.value + " at:")
        
        # print("Compiling succes for entry:\n"+ text)
        return lxa.tokens, historic

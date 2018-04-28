from collections import deque

from Core.Services.TextGrammar import TextToGrammar
from Core.Entities.TermUnit import TermUnit
from Core.Entities.Grammar import Grammar


class Follow:

    def __init__(self, grammar, first):
        self.grammar = grammar
        self.first = first
                            
    def __apply_follow_third_rule(self):
        """
        A -> Alfa B Beta, B.follow += A.follow
        Alfa is any stream
        Beta is a empty stream
        """
        calls = deque()

        for A in self.grammar.Premises:
            for stream in A.right:
                streamLen = len(stream)
        
                for index, unit in enumerate(stream):
                    if(unit.type is TermUnit.NONTERMINAL):
                        B = self.grammar.get_premise(unit.text)

                        if(A is B):
                            continue

                        if(streamLen > index + 1):
                            beta = stream[index + 1::]

                            firstBeta = self.first.first(beta)

                            if(TextToGrammar.EMPTY_UNIT not in firstBeta):
                                continue
                        
                        # B.follow |= A
                        calls.append((B, A))
        
        while(len(calls) is not 0):
            tupleBA = calls.popleft()
            B, A = tupleBA
        
            AinQueue = next((x for x in calls if x[0] is A), None)

            if(AinQueue):
                calls.append(tupleBA)
                continue

            # print(B.left,"___", A.left)

            B.follow |= A.follow


    def __apply_follow_second_rule(self):
        """
        A -> Alfa B Beta, B.follow += first(Beta) - {ɛ}
        Alfa is any stream
        Beta is a not empty stream
        """
        for term in self.grammar.Premises:
            for stream in term.right:
                streamLen = len(stream)

                for index, unit in enumerate(stream):
                    if(unit.type is TermUnit.NONTERMINAL):
                        termB = self.grammar.get_premise(unit.text)

                        if(streamLen > index + 1):
                            beta = stream[index + 1::]
                            firstBeta = self.first.first(beta)
                            
                            termB.follow |= set(firstBeta) - {self.grammar.textGrammar.EMPTY_UNIT}
    

    def __apply_follow_first_rule(self):
        """
        S.follow = $
        """
        self.grammar.StartSimbol.follow.add(Grammar.STREAM_END_UNIT)


    def build_follow(self):
        """
        Routine that creates follow sets of the given grammar
        """
        self.__apply_follow_first_rule()
        self.__apply_follow_second_rule()
        self.__apply_follow_third_rule()

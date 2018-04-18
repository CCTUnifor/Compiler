from Core.Entities.Graph import Graph
from Core.Entities.Item import Item
from Core.Entities.Premise import Premise
from Core.Entities.Grammar import Grammar
from Core.Entities.TermUnit import TermUnit


class ItemGraph(Graph):
    FakeStartString = '*'

    def __init__(self, grammar: Grammar, id=0):
        super().__init__(id, makeRoot=True)
        self.grammar = grammar
        self.augmented_grammar = ItemGraph.augment_grammar(grammar)

        self.__startGraph()

    @staticmethod
    def augment_grammar(grammar):
        augmentedgrammar = Grammar(grammar.textGrammar)

        premise = Premise(ItemGraph.FakeStartString, ItemGraph.FakeStartString + ' -> ' + grammar.StartSimbol.left)
        premise.right = [[grammar.StartSimbolUnit]]

        augmentedgrammar.Premises = [premise] + augmentedgrammar.Premises
        
        start = TermUnit(TermUnit.TERMINAL, ItemGraph.FakeStartString)

        augmentedgrammar.StartSimbol = ItemGraph.FakeStartString
        augmentedgrammar.StartSimbolUnit = start
        
        augmentedgrammar.NonTerminals = [start] + grammar.NonTerminals

        augmentedgrammar.augmented = True

        return augmentedgrammar

    def __startGraph(self):
        premise = self.augmented_grammar.get_term(self.augmented_grammar.StartSimbol)
        firstItem = Item(premise)

        newNode = self.makeNode(firstItem)

        self.addPath(self.root, newNode)

    def build_graph(self):
        pass
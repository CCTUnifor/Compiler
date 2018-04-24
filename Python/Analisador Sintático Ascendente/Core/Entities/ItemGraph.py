from collections import deque as Deque

from Core.Entities.Graph import Graph
from Core.Entities.Item import Item
from Core.Entities.Premise import Premise
from Core.Entities.Grammar import Grammar
from Core.Entities.TermUnit import TermUnit


class ItemGraph(Graph):
    FakeStartString = '@'

    def __init__(self, grammar: Grammar, id=0):
        super().__init__(id, makeRoot=True)
        self.grammar = grammar
        self.augmented_grammar = ItemGraph.augment_grammar(grammar)
        self.itemnode_queue = Deque()
        self.explored_term_units = {}

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
        premise = self.augmented_grammar.get_premise(self.augmented_grammar.StartSimbol)
        first_item = Item(premise)        

        first_node = self.makeNode(first_item)
        self.itemnode_queue.append(first_node)

        self.addPath(self.root, first_node)

    def build_graph(self):
        while(len(self.itemnode_queue)):
            item_node = self.itemnode_queue.popleft()
            item = item_node.value

            if(item.is_complete()):
                continue
            
            self.__apply_second_rule(item_node)
            self.__apply_third_rule(item_node)
     
    def __apply_third_rule(self, item_node):
        item = item_node.value
        
        term_unit = item.get_term_unit()

        next_item = item.get_next()
        next_node = self.makeNode(next_item)

        self.itemnode_queue.append(next_node)

        self.addPath(item_node, next_node, term_unit)
    
    def __apply_second_rule(self, item_node):
        item = item_node.value
        term_unit = item.get_term_unit()
        if(term_unit.type is TermUnit.NONTERMINAL):
            # adicionar premisas repetidas
            next_premise = self.grammar.get_premise(term_unit.text)

            if term_unit.text in self.explored_term_units:
                descendents = self.explored_term_units[term_unit.text]

                for node in descendents:
                    self.addPath(item_node, node)
            
            else:
                descendents = []
                self.explored_term_units[term_unit.text] = descendents

                for i, term in enumerate(next_premise.right):
                    next_item = Item(next_premise, (i, 0))
                    next_node = self.makeNode(next_item)
                    descendents.append(next_node)

                    self.itemnode_queue.append(next_node)

                    self.addPath(item_node, next_node)

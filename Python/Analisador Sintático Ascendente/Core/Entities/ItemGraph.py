from Core.Entities.Graph import Graph
from Core.Entities.Item import Item
from Core.Entities.Premise import Premise


class ItemGraph(Graph):
    def __init__(self, grammar):
        self.grammar = grammar

        self.startGraph()

    def startGraph(self):
        premise = Premise('*', "*->")
        Item(premise)
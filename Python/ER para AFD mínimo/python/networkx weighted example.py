import networkx as nx
import matplotlib.pyplot as plt

G=nx.Graph()
i=1
G.add_node(i,pos=(i,i))
G.add_node(2,pos=(2,2))
G.add_node(3,pos=(1,3))
G.add_edge(1,2,weight=0.5)
G.add_edge(1,3,weight=9.8)

pos=nx.get_node_attributes(G,'pos')
nx.draw(G,pos, with_labels=True)

labels = nx.get_edge_attributes(G,'weight')
nx.draw_networkx_edge_labels(G,pos,edge_labels=labels)
plt.show() # display

# pos (dictionary) – A dictionary with nodes as keys and positions as values. Positions should be sequences of length 2.
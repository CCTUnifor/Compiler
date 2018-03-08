from Lexical import LexicalAnalyzer as LxA
from ER_AFNE import ERtoAFNE
from AFNE_AFD import AFNEtoAFD

# entry= 'e(e|d)*' falta implementar os ()
# entry = 'e|d'
# entry = 'e*'
entry = '78d2'


tokens = LxA.analyzer(entry)

root = ERtoAFNE().convert(tokens)
matrix = AFNEtoAFD().convert(root)

print(root)
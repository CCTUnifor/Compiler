def Grammar_Printer(g):
    print('-----------------------GRAMÁTICA-----------------------')
    print(g)

    print('-------------------------FIRST-------------------------')
    for term in g.Terms:
        print(term.strFirst())

    print('\n------------------------FOLLOW-------------------------')
    for term in g.Terms:
        print(term.strFollow())
    
def Grammar_Table_Printer(s):
    ljust = 15
    # print('\n-------------------------TABLE-------------------------')
    # print(s.table)
    print()

    # header
    for non_terminal in s.table:
        line = "".ljust(ljust + len(non_terminal) - 1)

        for terminal in s.table[non_terminal]:
            line += terminal.ljust(ljust)
        
        centerWord = "TABLE"
        
        lineLen = len(line)
        halfLineLen = int(lineLen/2)
        centerWordLen = len(centerWord)
        halfCenterWordLen = int(centerWordLen/2)

        headerLines = "".ljust((halfLineLen - halfCenterWordLen), '-')
        print(headerLines + centerWord + headerLines)
        print("|" + line + "|")

        break

    # body
    for non_terminal in s.table:
        line = non_terminal.ljust(ljust)

        for terminal in s.table[non_terminal]:
            value = s.table[non_terminal][terminal]
            if(type(value) is tuple):
                line += str(value[1]).ljust(ljust)
            else:
                line += str(value).ljust(ljust)
        
        print("|" + line + "|")
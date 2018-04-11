import sys

tokens = []
# cadeia = '233*)(+724'
# cadeia = '456'
# cadeia = '1290()(12)-17+14/5*13+1/2'
if(len(sys.argv) < 2):
    # print()
    cadeia = input('Digite a cadeia: ')
    # cadeia = '((20+50)*(130/25))/(15-31)'
else:
    cadeia = sys.argv[1]# .pop()

estado = 1
i = 0
acumulador = ""

while(i <= len(cadeia)):
    charr =  cadeia[i] if i < len(cadeia) else ''
    print('char: ' + charr + ' estado ' + str(estado))
    
    if(estado == 1):
        if(charr.isdigit()):
            acumulador += charr
            estado = 2

        elif(charr in ['+', '-', '*', '/']):
            tokens.append({'key':'operador', 'value': charr})
            estado = 3

        elif(charr in ['(', ')']):
            tokens.append({'key':'parenteses', 'value': charr})
            estado = 4

        elif(charr is '' or charr is ' '):
            pass

        else:
            raise Exception('Não reconhecido')

    elif(estado == 2):
        if(charr.isdigit()):
            acumulador += charr
        else:
            tokens.append({'key':'número', 'value': acumulador})
            acumulador = ''
            estado = 1
            continue    

    elif(estado == 3):
        estado = 1
        continue

    elif(estado == 4):
        estado = 1
        continue

    i+=1

print('______________')
print('Cadeia: ' + cadeia)
# print(tokens)
print('Reconheceu ' + str(len(tokens)) + ' tokens')

for item in tokens:
    print(str(item['value']).ljust(10, ' ') + ' ' + item['key'])
                
                
                
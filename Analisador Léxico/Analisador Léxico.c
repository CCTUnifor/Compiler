#include<stdio.h>
#include<string.h>

int main(int argc, char *argv[])
{
    char *cadeia;
    int estado = 1,
        index = 0,
        lenCadeia = 0;
    char *acumulador;
    char cursor;

    if(argc < 2) {
        return 1;
    }
    cadeia = argv[1];
    // else{
    //     printf("Informe a cadeia: ");
    //     // scanf("%s", cadeia);
    //     // fgets(cadeia, 256, stdin);
    // }

    printf("Cadeia informada: %s\n", cadeia);

    lenCadeia = strlen(cadeia);
    while(index <= lenCadeia){

        if(index == lenCadeia){
            cursor = (char) 0;
        }else{
            cursor = cadeia[index];
        }

        printf("char: %c estado: %d\n", cursor, estado);

        switch(estado){
            case 1:
                if(isdigit(cursor)){

                    estado = 2;
                }else if(isOperador(cursor)){

                }else if(isParenteses(cursor)){

                }else if(cursor == ' ' || cursor = ' '){

                }else{
                    return 1;
                }
                break;
            case 2:
                break;
            case 3:
                estado = 1;
                break;
            case 4:
                estado = 1;
                break;
        }

        index++;
    }



    return 0;
}
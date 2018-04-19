
import java.io.*;

public class tiny {

     /*****************************************************/
     /*********** ANALISADOR LEXICO ***********************/
     /*****************************************************/

     static String[] analex (String[] linha) {

     String fimarq;
     String pal;
     String palaux;
     String lint;
     String car;
     String nlin;
     String palcomp;
     String claspal;

     int lin;
     int col;
     int coli;
     int colf;
     int i;
     int j;
     int k;
     int pontres=400;
     int pontloc=0;
     boolean sempre;
     boolean inclui;
     char carc;


     String[] vetor = new String[512];

     String[] palres = new String[21];

     String[] simbum = {";","[","]","(",")","+","-","=","*"};

     palres[1]="program";
     palres[2]="var";
     palres[3]="begin";
     palres[4]="end";
     palres[5]="integer";
     palres[6]="array";
     palres[7]="of";
     palres[8]="if";
     palres[9]="then";
     palres[10]="repeat";
     palres[11]="until";
     palres[12]="read";
     palres[13]="write";
     palres[14]="while";
     palres[15]="do";
     palres[16]="not";
     palres[17]="or";
     palres[18]="div";
     palres[19]="mod";
     palres[20]="and";

     fimarq="$";
     palcomp="";
     nlin="";
     pal="";
     palaux="";
     lin=1;
     col=0;
     i=1;
     lint=linha[lin];
     claspal="";
     sempre=true;

     j=0;

     /**************************************************/
     /***** a partir da posi��o 400 do vetor[j] ********/
     /******h� a tabela de identificadores ************/
     /*************************************************/

     while (j<512) {
           vetor[j]="";
           j=j+1;
           }


     while  (!fimarq.equalsIgnoreCase(lint)) {

        k=0;
        coli=col;
        colf=col;
        car=lint.substring(col);
        if (car.equalsIgnoreCase("&")) {
           lin=lin+1;
           col=0;
           lint=linha[lin];   /* pega linha corrente */
        }
        car=lint.substring(col);

        carc=car.charAt(0); /* converte para char */

        while (Character.isSpace ((char) carc)){
              col=col+1;
              car=lint.substring(col);
              carc=car.charAt(0);
            }
        colf=col;

        if (Character.isLetter ((char) carc)) {
            coli=col;
            while ((Character.isLetter((char) carc)) | (Character.isDigit ((char) carc))) {
                  car=lint.substring(col);
                  carc=car.charAt(0);
                  col=col+1;
                 }
            colf=col;
            k=colf-1;
            if (k==0) k=1;

            pal=lint.substring(coli,k);
            col=colf-1;
            nlin=Integer.toString(lin);
            /***********************************/
            /* verifica se e palavra reservada */
            /***********************************/
            j=1;
            claspal="";
            while (j<=20) {
                  palcomp=palres[j];
                  if (palcomp.equalsIgnoreCase(pal)) claspal = "RES";
                  j=j+1;
                  }
            if (!claspal.equalsIgnoreCase("RES")) {
               claspal = "IDE";
               j=400;
               inclui=true;
               while (j<512) {
                     palcomp=vetor[j];
                     if (palcomp.equalsIgnoreCase(pal)) {
                        pontloc=j;    /* encontrou o identificador */
                        inclui=false;
                     }

                     j=j+1;
                     }
               j=1;
               if (inclui) {
                  vetor[pontres]=pal;
                  pontloc=pontres;
                  pontres=pontres+1;
               }
            }


            vetor[i]=claspal+" "+pal+" "+nlin;
            if (claspal.equalsIgnoreCase("IDE")) {
               pal=pal.valueOf(pontloc);
               vetor[i]="";
               vetor[i]=claspal+" "+pal+" "+nlin;
            }

            System.out.println(vetor[i]);
            i=i+1;
            pal="";
          }
        else if (Character.isDigit ((char) carc)) {
                 coli=col;
                 while (Character.isDigit ((char) carc)){
                    car=lint.substring(col);
                    carc=car.charAt(0);
                    col=col+1;
                   }
                 colf=col;
                 k=colf-1;
                 if (k==0) k=1;
                 pal=lint.substring(coli,k);
                 col=colf-1;
                 nlin=Integer.toString(lin);
                 vetor[i]="NUM "+pal+" "+nlin;
                 System.out.println(vetor[i]);
                 i=i+1;
                 pal="";
                       }

           else
              {
              /***********************************/
              /* verifica se e simbolo */
              /**********************************/
               pal=lint.substring(col,col+1);
               if (pal.equalsIgnoreCase(":")) {
                   palaux=lint.substring(col+1,col+2);
                   if (palaux.equalsIgnoreCase("=")){
                       nlin=Integer.toString(lin);
                       claspal="SDU";
                       vetor[i]=claspal+" "+pal+palaux+" "+nlin;
                       System.out.println(vetor[i]);
                       i=i+1;
                       col=col+2;
                       colf=col;
                    }
                    else
                       {
                        nlin=Integer.toString(lin);
                        claspal="SUN";
                        vetor[i]=claspal+" "+pal+" "+nlin;
                        System.out.println(vetor[i]);
                        i=i+1;
                        col=col+1;
                        colf=col;
                        }
                   }
               else if (pal.equalsIgnoreCase(">")) {
                        palaux=lint.substring(col+1,col+2);
                        if (palaux.equalsIgnoreCase("=")){
                           nlin=Integer.toString(lin);
                           claspal="SDU";
                           vetor[i]=claspal+" "+pal+palaux+" "+nlin;
                           System.out.println(vetor[i]);
                           i=i+1;
                           col=col+2;
                           colf=col;
                        }
                        else {
                           nlin=Integer.toString(lin);
                           claspal="SUN";
                           vetor[i]=claspal+" "+pal+" "+nlin;
                           System.out.println(vetor[i]);
                           i=i+1;
                           col=col+1;
                           colf=col;
                        }
                   }
                   else if (pal.equalsIgnoreCase("<")) {
                           palaux=lint.substring(col+1,col+2);
                           if (palaux.equalsIgnoreCase("=")){
                              nlin=Integer.toString(lin);
                              claspal="SDU";
                              vetor[i]=claspal+" "+pal+palaux+" "+nlin;
                              System.out.println(vetor[i]);
                              i=i+1;
                              col=col+2;
                              colf=col;
                           }
                           else if (palaux.equalsIgnoreCase(">")){
                                nlin=Integer.toString(lin);
                                claspal="SDU";
                                vetor[i]=claspal+" "+pal+palaux+" "+nlin;
                                System.out.println(vetor[i]);
                                i=i+1;
                                col=col+2;
                                colf=col;
                                }
                            else {
                                 nlin=Integer.toString(lin);
                                 claspal="SUN";
                                 vetor[i]=claspal+" "+pal+" "+nlin;
                                 System.out.println(vetor[i]);
                                 i=i+1;
                                 col=col+1;
                                 colf=col;
                            }
                          }
                     else  {
                            nlin=Integer.toString(lin);
                            j=0;
                            claspal="";

                            while (j<=8) {
                                  palcomp=simbum[j];
                                  if (palcomp.equalsIgnoreCase(pal)) claspal = "SUN";
                                  j=j+1;
                            }

                             vetor[i]=claspal+" "+pal+" "+nlin;
                             System.out.println(vetor[i]);
                             i=i+1;
                           pal="";
                           coli=col;
                           col=col+1;
                           colf=col;

                           }
                        }
        pal="";
        if (lint.length()<=colf){
           lin=lin+1;
           col=0;
           lint=linha[lin];   /* pega linha corrente */
            }
     }
     return vetor;
     }


/*******************************************************************/
/*******************************************************************/
/*********************** comando ***********************************/
/*******************************************************************/

     static int[] cmd(String[] result, int[] memoria){

      String tokaux;
      tokaux="";
      String pal="";
      String linaux="";
      String palaux;
      String car="";
      int y=0;
      int x=0;
      int ptfix=0;
      int tempint=0;
      int tempint2=0;
      int j=0;
      int pontmem=memoria[2];
      boolean fixa=false;
      /****************************************************/
      /***********************ATRIBUI��O*******************/
      /****************************************************/
      x=memoria[1];
      linaux=result[x];

      pal=linaux.substring(y,y+3);
      if (pal.equalsIgnoreCase("IDE")) {
         System.out.println("AS cmd atribui��o **");
         y=4;
         tempint2=0;
         palaux=linaux.substring(y,y+3);
         tempint2=Integer.parseInt(palaux); /*end do lado esquerdo*/

         x=x+1;
         linaux=result[x];
         y=4;
         pal=linaux.substring(y,y+2);
         if (pal.equalsIgnoreCase(":=")){
             x=x+1;
             linaux=result[x];
             y=0;
             pal=linaux.substring(y,y+3);
             if (pal.equalsIgnoreCase("NUM")) {
                memoria[pontmem]=68;  /*************LDI ******/
                pontmem=pontmem+1;
                memoria[2]=pontmem;
                y=4;
                tempint=0;
                palaux=linaux.substring(y,y+2);
                tempint=Integer.parseInt(palaux);
                memoria[pontmem]=tempint;
                pontmem=pontmem+1;
                memoria[2]=pontmem;
                memoria[pontmem]=0;
                pontmem=pontmem+1;
                memoria[2]=pontmem;

                memoria[pontmem]=65;  /*************STO ******/
                pontmem=pontmem+1;
                memoria[2]=pontmem;
                memoria[pontmem]=(tempint2-400)*2+6;

                pontmem=pontmem+1;
                memoria[2]=pontmem;
                memoria[pontmem]=0;
                pontmem=pontmem+1;
                memoria[2]=pontmem;
                memoria[1]=x;
                return(memoria);
             }
             if (pal.equalsIgnoreCase("IDE")) {
                y=4;
                tempint=0;
                palaux=linaux.substring(y,y+3);
                tempint=Integer.parseInt(palaux);
                memoria[pontmem]=64;  /*************LOD ******/
                pontmem=pontmem+1;
                memoria[2]=pontmem;

                memoria[pontmem]=(tempint-400)*2+6;
                pontmem=pontmem+1;
                memoria[2]=pontmem;
                memoria[pontmem]=0;
                pontmem=pontmem+1;
                memoria[2]=pontmem;

                memoria[pontmem]=65;  /*************STO ******/
                pontmem=pontmem+1;
                memoria[2]=pontmem;
                memoria[pontmem]=(tempint2-400)*2+6;

                pontmem=pontmem+1;
                memoria[2]=pontmem;
                memoria[pontmem]=0;
                pontmem=pontmem+1;
                memoria[2]=pontmem;

                memoria[1]=x;
                return(memoria);

             }
             else {
                  System.out.println("NUM ou IDE esperados !");
                  memoria[1]=x;
                  return(memoria);
                  }
             }
          else System.out.println(":= esperado");
       }
       /*******************************************************/
       /********************** IF *****************************/
       /*******************************************************/
       y=4;
       if (linaux.length()>=6) pal=linaux.substring(y,y+2);

       if (pal.equalsIgnoreCase("if")) {
          System.out.println("AS cmd if *****");

           x=x+1;

           linaux=result[x];
           y=0;
           pal=linaux.substring(y,y+3);
           if (pal.equalsIgnoreCase("IDE")) {
               memoria[pontmem]=64; /********** LOD *******/
               pontmem=pontmem+1;
               memoria[2]=pontmem;
               y=4;
               tempint=0;
               palaux=linaux.substring(y,y+3);
               tempint=Integer.parseInt(palaux);
               memoria[pontmem]=(tempint-400)*2+6;
               pontmem=pontmem+1;
               memoria[2]=pontmem;
               memoria[pontmem]=0;
               pontmem=pontmem+1;
               memoria[2]=pontmem;
               x=x+1;
               linaux=result[x];
               y=0;
               pal=linaux.substring(y,y+3);
               y=4;
               if (pal.equalsIgnoreCase("SUN")) {
                   pal=linaux.substring(y,y+1);
                   if (pal.equalsIgnoreCase(">")) memoria[pontmem+3]=34;
                   if (pal.equalsIgnoreCase("=")) memoria[pontmem+3]=32;
                   if (pal.equalsIgnoreCase("<")) memoria[pontmem+3]=36;
                   }
               else {
                    if (pal.equalsIgnoreCase("SDU")) {
                       pal=linaux.substring(y,y+2);
                       if (pal.equalsIgnoreCase("<>")) memoria[pontmem+3]=33;
                       if (pal.equalsIgnoreCase(">=")) memoria[pontmem+3]=35;
                       if (pal.equalsIgnoreCase("<=")) memoria[pontmem+3]=37;
                    }
                    else System.out.println("S�mbolo esperado!");
                }
                x=x+1;
                linaux=result[x];
                y=0;
                pal=linaux.substring(y,y+3);
                if (pal.equalsIgnoreCase("IDE")){
                    memoria[pontmem]=64; /********** LOD *******/
                    pontmem=pontmem+1;
                    memoria[2]=pontmem;
                    y=4;
                    tempint=0;
                    palaux=linaux.substring(y,y+3);
                    tempint=Integer.parseInt(palaux);
                    memoria[pontmem]=(tempint-400)*2+6;
                    pontmem=pontmem+1;
                    memoria[2]=pontmem;
                    memoria[pontmem]=0;
                    pontmem=pontmem+2;
                    memoria[2]=pontmem;
                    memoria[pontmem]=92; /*************JF *******/
                    pontmem=pontmem+1;
                    ptfix=pontmem;
                    memoria[pontmem]=0;
                    pontmem=pontmem+1;
                    memoria[pontmem]=0;
                    pontmem=pontmem+1;
                    memoria[2]=pontmem;
                    memoria[5]=ptfix;
                    fixa=true;
                   }
                else {
                   if (pal.equalsIgnoreCase("NUM")){

                       memoria[pontmem]=68; /********** LDI *******/
                       pontmem=pontmem+1;
                       memoria[2]=pontmem;
                       y=4;
                       tempint=0;
                       palaux=linaux.substring(y,y+2);
                       tempint=Integer.parseInt(palaux);
                       memoria[pontmem]=tempint;
                       pontmem=pontmem+1;
                       memoria[2]=pontmem;
                       memoria[pontmem]=0;
                       pontmem=pontmem+2;
                       memoria[2]=pontmem;
                       memoria[pontmem]=92; /*************JF *******/
                       pontmem=pontmem+1;
                       ptfix=pontmem;
                       memoria[pontmem]=0;
                       pontmem=pontmem+1;
                       memoria[pontmem]=0;
                       pontmem=pontmem+1;
                       memoria[2]=pontmem;
                       memoria[5]=ptfix;
                       fixa=true;
                    }
                    else System.out.println("IDE ou NUM esperados!");
                 }
                 x=x+1;
                 linaux=result[x];
                 y=4;
                 pal=linaux.substring(y,y+4);
                 if (pal.equalsIgnoreCase("then")){
                    x=x+1;
                    memoria[1]=x;
                    memoria=cmd(result,memoria);
                    pontmem=memoria[2];
                    ptfix=memoria[5];
                    if (ptfix>0) {
                       memoria[ptfix]=pontmem-10;
                       memoria[5]=0;
                    }
                    x=memoria[1];
                    return(memoria);
                 }
                 else System.out.println("Then esperado!");
           }
           else System.out.println("Identificador Esperado!");
       }

       /*******************************************************/
       /********************** WHILE *****************************/
       /*******************************************************/
       y=4;
       if (linaux.length() >=9) pal=linaux.substring(y,y+5);
       if (pal.equalsIgnoreCase("while")) {

          System.out.println("AS cmd while ***");
           x=x+1;
           linaux=result[x];
           y=0;
           pal=linaux.substring(y,y+3);
           if (pal.equalsIgnoreCase("IDE")) {
               memoria[pontmem]=64; /********** LOD *******/
               memoria[4]=pontmem-10;
               pontmem=pontmem+1;
               memoria[2]=pontmem;
               y=4;
               tempint=0;
               palaux=linaux.substring(y,y+3);
               tempint=Integer.parseInt(palaux);
               memoria[pontmem]=(tempint-400)*2+6;
               pontmem=pontmem+1;
               memoria[2]=pontmem;
               memoria[pontmem]=0;
               pontmem=pontmem+1;
               memoria[2]=pontmem;
               x=x+1;
               linaux=result[x];
               y=0;
               pal=linaux.substring(y,y+3);
               y=4;


               if (pal.equalsIgnoreCase("SUN")) {
                   pal=linaux.substring(y,y+1);
                   if (pal.equalsIgnoreCase(">")) memoria[pontmem+3]=34;
                   if (pal.equalsIgnoreCase("=")) memoria[pontmem+3]=32;
                   if (pal.equalsIgnoreCase("<")) memoria[pontmem+3]=36;
                   }
               else {
                    if (pal.equalsIgnoreCase("SDU")) {
                       pal=linaux.substring(y,y+2);
                       if (pal.equalsIgnoreCase("<>")) memoria[pontmem+3]=33;
                       if (pal.equalsIgnoreCase(">=")) memoria[pontmem+3]=35;
                       if (pal.equalsIgnoreCase("<=")) memoria[pontmem+3]=37;
                    }
                    else System.out.println("S�mbolo esperado!");
                }
                x=x+1;
                linaux=result[x];
                y=0;
                pal=linaux.substring(y,y+3);
                if (pal.equalsIgnoreCase("IDE")){
                    memoria[pontmem]=64; /********** LOD *******/
                    pontmem=pontmem+1;
                    memoria[2]=pontmem;
                    y=4;
                    tempint=0;
                    palaux=linaux.substring(y,y+3);
                    tempint=Integer.parseInt(palaux);
                    memoria[pontmem]=(tempint-400)*2+6;
                    pontmem=pontmem+1;
                    memoria[2]=pontmem;
                    memoria[pontmem]=0;
                    pontmem=pontmem+2;
                    memoria[2]=pontmem;

                    memoria[pontmem]=92; /*************JF *******/

                    pontmem=pontmem+1;
                    ptfix=pontmem;
                    memoria[pontmem]=0;
                    pontmem=pontmem+1;
                    memoria[pontmem]=0;
                    pontmem=pontmem+1;
                    memoria[2]=pontmem;
                    memoria[3]=ptfix;
                    fixa=true;
                   }
                else {
                   if (pal.equalsIgnoreCase("NUM")){

                       memoria[pontmem]=68; /********** LDI *******/
                       pontmem=pontmem+1;
                       memoria[2]=pontmem;
                       y=4;
                       tempint=0;
                       palaux=linaux.substring(y,y+2);
                       tempint=Integer.parseInt(palaux);

                       memoria[pontmem]=tempint;

                       pontmem=pontmem+1;
                       memoria[2]=pontmem;
                       memoria[pontmem]=0;
                       pontmem=pontmem+2;
                       memoria[2]=pontmem;

                       memoria[pontmem]=92; /*************JF *******/
                       pontmem=pontmem+1;
                       memoria[2]=pontmem;

                       ptfix=pontmem;
                       memoria[pontmem]=0;
                       pontmem=pontmem+1;
                       memoria[2]=pontmem;
                       memoria[pontmem]=0;
                       pontmem=pontmem+1;
                       memoria[2]=pontmem;
                       memoria[3]=ptfix;
                       fixa=true;
                    }
                    else System.out.println("IDE ou NUM esperados!");
                 }
                 x=x+1;
                 linaux=result[x];
                 y=4;
                 pal=linaux.substring(y,y+2);
                 if (pal.equalsIgnoreCase("do")){
                    x=x+1;
                    memoria[1]=x;

                    memoria=cmd(result,memoria);

                    pontmem=memoria[2];
                    memoria[pontmem]=90; /*************JMP *******/
                    pontmem=pontmem+1;
                    memoria[pontmem]=memoria[4];
                    memoria[4]=0;
                    pontmem=pontmem+1;
                    memoria[2]=pontmem;
                    memoria[pontmem]=0;
                    pontmem=pontmem+1;
                    memoria[2]=pontmem;
                    ptfix=memoria[3];
                    memoria[ptfix]=pontmem-10;
                    memoria[3]=0;
                    return(memoria);
                 }
                 else System.out.println("do esperado!");
           }
           else System.out.println("Identificador Esperado!");
       }

           /*****************************************************/
           /****************************************************/
           /******************** READ **************************/
           y=4;
           if (linaux.length() >= 8) pal=linaux.substring(y,y+4);
           if (pal.equalsIgnoreCase("read")){
              System.out.println("AS cmd READ ***");
              memoria[pontmem]=87;  /********  IN ***************/
              pontmem=pontmem+1;
              memoria[2]=pontmem;
              x=x+1;
              linaux=result[x];
              y=4;
              pal=linaux.substring(y,y+1);
              if (pal.equalsIgnoreCase("(")) {
                 x=x+1;
                 linaux=result[x];
                 y=0;
                 pal=linaux.substring(y,y+3);
                 if (pal.equalsIgnoreCase("IDE")) {
                     memoria[pontmem]=65; /********** STO *******/
                     pontmem=pontmem+1;
                     memoria[2]=pontmem;
                     y=4;
                     tempint=0;
                     palaux=linaux.substring(y,y+3);
                     tempint=Integer.parseInt(palaux);
                     memoria[pontmem]=(tempint-400)*2+6;
                     pontmem=pontmem+1;
                     memoria[2]=pontmem;
                     memoria[pontmem]=0;
                     pontmem=pontmem+1;
                     memoria[2]=pontmem;
                     x=x+1;
                     linaux=result[x];
                     y=4;
                     pal=linaux.substring(y,y+1);
                     if (pal.equalsIgnoreCase(")")) {
                        memoria[1]=x;
                        return(memoria);
                     }
                     else System.out.println(") esperados!");
                   }
                   else System.out.println("IDE esperado!");
                 }
                 else System.out.println("( esperado!");
              }
              /********************************************/
              /*****************WRITE *********************/
              /********************************************/
              y=4;
              if (linaux.length() >=9) pal=linaux.substring(y,y+5);
              if (pal.equalsIgnoreCase("write")){
                 System.out.println("AS cmd write **");
                 x=x+1;
                 linaux=result[x];
                 y=4;
                 pal=linaux.substring(y,y+1);
                 if (pal.equalsIgnoreCase("(")) {
                    x=x+1;
                    linaux=result[x];
                    y=0;
                    pal=linaux.substring(y,y+3);
                    if (pal.equalsIgnoreCase("IDE")) {
                       pontmem=memoria[2];
                       memoria[pontmem]=64; /********** LOD *******/
                       pontmem=pontmem+1;
                       y=4;
                       tempint=0;
                       palaux=linaux.substring(y,y+3);
                       tempint=Integer.parseInt(palaux);
                       memoria[pontmem]=(tempint-400)*2+6;
                       pontmem=pontmem+1;
                       memoria[pontmem]=0;
                       pontmem=pontmem+1;
                       memoria[pontmem]=88; /**********OUT**********/
                       pontmem=pontmem+1;
                       memoria[2]=pontmem;

                       x=x+1;
                       linaux=result[x];
                       y=4;
                       pal=linaux.substring(y,y+1);
                       if (pal.equalsIgnoreCase(")")) {
                       memoria[1]=x;
                       return(memoria);
                       }
                       else System.out.println(") esperados!");
                    }
                    else System.out.println("IDE esperado !");
                }
                else System.out.println("( esperado !");
               }
            /********************************************/
           /***************** COMANDO COMPOSTO **********/

              y=4;
              if (linaux.length() >=9) pal=linaux.substring(y,y+5);
              if (pal.equalsIgnoreCase("begin")){

                  System.out.println("AS cmd composto");
                  x=x+1;
                  memoria[1]=x;

                  while (!pal.equalsIgnoreCase("end")) {

                        memoria=cmd(result,memoria);

                        x=memoria[1];

                        x=x+1;
                        linaux=result[x];

                        y=4;
                        pal=linaux.substring(y,y+3);

                        String palt;
                        
                        palt=linaux.substring(y,y+1);

                        if (palt.equalsIgnoreCase(";")){
                           x=x+1;
                           linaux=result[x];
                           y=4;
                           pal=linaux.substring(y,y+3);
                           }
                        memoria[1]=x;
                  }

                  if (pal.equalsIgnoreCase("end")){
                     memoria[1]=x;
                     return(memoria);
                     }
                  else System.out.println("End esperado !");
              }

   return(memoria);
}

/*****************************************************************/
/*****************************************************************/
/************************DECVAR **********************************/

     static int[] decvar(String[] result,int[] memoria){

        System.out.println("AS Decvar ***");
        String tokaux;
        tokaux="IDE";
        String pal="";
        String linaux="";
        String car="";
        int y=0;
        int x=0;
        x=memoria[1];
        linaux=result[x];
        pal=linaux.substring(y,y+3);

        while (!pal.equalsIgnoreCase("begin")) {

        if (pal.equalsIgnoreCase(tokaux)) {
           x=x+1;
           linaux=result[x];
           tokaux=":";
           y=4;
           pal=linaux.substring(y,y+1);
           if (pal.equalsIgnoreCase(tokaux)) {
              }
           else System.out.println(": esperado");

           x=x+1;
           linaux=result[x];
           tokaux="integer";
           y=4;
           pal=linaux.substring(y,y+7);
           if (pal.equalsIgnoreCase(tokaux)) {
              memoria[2]=memoria[2]+2;  /**** incrementa ponteiro ****/
              tokaux="IDE";
             }
           else System.out.println("Integer esperado");
           x=x+1;
           linaux=result[x];
           y=4;
           tokaux=";";
           pal=linaux.substring(y,y+1);
           if (!pal.equalsIgnoreCase(tokaux)) System.out.println("; esperado");
           tokaux="IDE";
           }
        else {
           /***************procura por begin ************/
           y=4;
           tokaux="begin";
           pal=linaux.substring(y,y+5);
           if (pal.equalsIgnoreCase(tokaux)) {
              x=x+1;
              memoria[1]=x;
              return (memoria);
              }
           else System.out.println("Identif. ou Begin esperados");
           }

           x=x+1;
           linaux=result[x];
           y=0;
           pal=linaux.substring(y,y+3);
           memoria[1]=x;
          }
     return(memoria);
     }
/*****************************************************************/

/*****************************************************************/
     static void variavel(String[] result){

        System.out.println("AS Var ***");
        String tokaux;
        tokaux="var";
        String pal="";
        String linaux="";
        String car="";
        int x=2;
        int y=4;

        linaux=result[x];
        pal=linaux.substring(y,y+3);

        if (!tokaux.equalsIgnoreCase(pal)) System.out.println("Erro : var esperado !");
        }
/*****************************************************************/
/***************** PROGRAMA **************************************/
/*****************************************************************/
static void programa(String[] result){

        System.out.println("AS Program ***");
        String tokaux;
        tokaux="program";
        String pal="";
        String linaux="";
        String car="";
        int x=1;
        int y=4;

        linaux=result[x];
        pal=linaux.substring(y,y+7);
        if (!tokaux.equalsIgnoreCase(pal)) System.out.println("Erro : program esperado !");
     }
/*****************************************************************/
/*****************************************************************/
/********************** Programa Principal ***********************/
//************** Linguagem Tiny Pascal ***************************/
/*****************************************************************/
/*****************************************************************/
    public static void main(String[] args) throws IOException {

        File inputFile = new File(args[0]+".tpa"); 
        File outputFile = new File(args[0]+".obj");

        FileReader in = new FileReader(inputFile);
        FileWriter out = new FileWriter(outputFile);
        String[] linha = new String[64];
		// Na matriz result vou guardar os tokens da an�lise l�xica
        String[] result = new String[512];
        //		
        // Na matriz memoria vou guardar os bytecodes gerados
        int[] memoria = new int[1000];
		//

        int pontmem;
        String linp;
        String fl;
        String pal;
        int ptfix;
        int ptfix2;
        int y;
        int r;
        int c;
        int i;
        int clinha;
        clinha=0;
        i=0;
        clinha=clinha+1;
        c=in.read();
        linp="";
        i=1;
        y=0;
        r=0;
        while (i<64) {
              linha[i]="";
              i=i+1;
              }
        while (c!=-1) {
             linp="";
             while (c!='\n') {
                 linp=linp+(char)c;
                 c=in.read();
                 }
            c=in.read();
            if (c=='\r') c=in.read();
            linha[clinha]=linp.trim()+"&";
            clinha=clinha+1;
            }
       linha[clinha]="$";
       in.close();
       i=1;
       while (i<1000) {
            memoria[i]=0;
            i=i+1;
       }

		// ********** Chama analisador l�xico ***************//
        result=analex(linha);
		
		// ********** Chama s�mbolo inicial *************//
        programa(result);
        // ********** Continua an�lise sint�tica *********//
		variavel(result);
		
		// ********* Reservei os dez primeiros elementos do vetor de resultados 
		// ********* para guardar alguns ponteiros
		// ********* como o ponteiro do l�xico e da mem�ria
		// ********* Mas quando vou gerar o c�digo, come�o da posi��o i=10, l�gico

        memoria[10]=79; /*************LSP*************/
        memoria[11]=0;
        memoria[12]=16; /******** Vetor de 4096 bytes ***/
        memoria[13]=90; /*************JMP*************/
        memoria[14]=0;
        memoria[15]=0;
        memoria[2]=15;    /****** ponteiro da mem�ria do CMS *****/
        memoria[1]=3;     /****** ponteiro do vetor l�xico ****/

        memoria=decvar(result,memoria);
        memoria[14]=memoria[2]-10+1; /*** fixa salto do JMP *****/
        memoria[2]=memoria[2]+1;
        memoria=cmd(result,memoria);

        r=memoria[1];
        r=r+1;
        linp=result[r];
        y=4;
        pal=linp.substring(y,y+1);

        while (pal.equalsIgnoreCase(";")) {
              r=r+1;
              memoria[1]=r;
              memoria=cmd(result,memoria);
              pontmem=memoria[2];
              ptfix=memoria[5];
              ptfix2=memoria[6];
              if (ptfix>0) {
                  memoria[ptfix]=pontmem-10;
                  memoria[5]=0;
                 }
              if (ptfix2>0) {
                  memoria[ptfix2]=pontmem-10;
                  memoria[6]=0;
                  }
              r=memoria[1]+1;
              linp=result[r];
              y=4;
              pal=linp.substring(y,y+1);
              }
       r=memoria[1]+1;
       linp=result[r];
       y=4;
       pal=linp.substring(y,y+3);
       pontmem=memoria[2];
       if (pal.equalsIgnoreCase("end")) {
          memoria[pontmem]=97; /****************STOP*****/
       }
       else System.out.println("end esperado !");
       i=10;
       while (i<=pontmem){
             out.write(memoria[i]);
             i=i+1;
             }
      out.write(0xFF); // fim de arquivo
      out.close();
   }

}

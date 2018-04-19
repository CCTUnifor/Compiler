package comp;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.io.InputStreamReader;

public class cms {

	int A,B,SP,PC=0;
    byte F,IR;
    int RO,IP,X,Y,FTP;
    byte[] memoria = new byte[5000];
    byte[] memoriatemp = new byte[500];
    int i;
    static boolean FIM_PROG;
    String STR12, programa;
    
    public void PUSH(int valor){
    	memoria[SP-1] = (byte) valor;
    	memoria[SP-2] = (byte) (valor >>> 8);
    	SP = SP-2;
    	
    }
    
    public int VALOR(int ENDR)
    {
     int val = (memoria[ENDR + 1] * 256) + memoria[ENDR];
     return val;
    }
    
    public void SALVA(int endr,int valor)
    {
    	     memoria[endr] = (byte) valor;
    	     memoria[endr+1] = (byte) (valor >>> 8);
    }
    
    
    public int POP(){
    	int POP = (memoria[SP + 1]) + memoria[SP];
    	SP = SP + 2;
    	return POP;
    }
    
    public int W(){
    	int W = (memoria[PC + 1]*256) + memoria[PC];
    	PC = PC + 2;
    	return W;
    }
    
    public void carga_programa(String file){
    	try {
    		File f = new File(file+".OBJ");  
			FileInputStream arquivo = new FileInputStream(f);
			
			byte[] b = new byte[(int)f.length()];  
	        int i=-1;
	        while ( (i=arquivo.read(b)) !=-1){  
	            arquivo.read(b,0,i);
	        }
	        memoriatemp = b;
			PC=0;
			SP=4096;
			for (int j=0;j<memoriatemp.length;j++){
				memoria[j]=memoriatemp[j];
			}
			int tamanho = memoria.length + 1;
			while (tamanho <= 4096){
				memoria[tamanho]=0x0000;
				tamanho=tamanho+1;
			}
			
		} catch (FileNotFoundException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
 
    	
    }
    
	
	public void hardware(){
		
//		System.out.println("PROCEDURE HARDWARE "+PC+"   "+memoria[PC]);
//		System.out.println("    ");
		IR = memoria[PC];
		PC = PC + 1;
		
		switch (IR){
		case 0x00: {
				  PUSH(POP() ^ 0xFFFF);
				  break;
				}
		case 0x01: {
				PUSH(POP()+POP());
				break;
			}
		case 0x02 :{
			    X = POP();
			    Y = POP();
			    X = Y-X;
			    PUSH(X);
			    break;
			}
		case 0x03 : {
			    PUSH(POP() * POP());
			    break;
			}
		case 0x04 :{
			    X = POP();
			    Y = POP();
			    X = Y/X;
			    PUSH(X);
			    break;
			}
		case 0x05 :{
			    X = POP();
			    Y = POP();
			    X = Y%X;
			    PUSH(X);
			    break;
			}
		case 0x06 : {
			    PUSH(POP() | POP());
			    break;
			}
		case 0x07 : {
			    PUSH(POP() & POP());
			    break;
			}
		case 0x08 :{
			    PUSH(POP() + 1);
			    break;
			}
		case 0x09 :{
			    PUSH(POP() - 1);
			    break;
			}
		case 0x0A :{
			    A = A ^ 0xFFFF;
			    break;
			}
		case 0x0B :{
			    A = A + B;
			    break;
			}
		case 0x0C :{
			    A = A - B;
			    break;
			}
		case 0x0D :{
			    A = A * B;
			    break;
			}
		case 0x0E : {
				A = A / B;
				break;
			}
		case 0x0F : {
				A = A % B;
				break;
			}
		case 0x10 : {
				A = A | B;
				break;
			}
		case 0x11 : {
	            A = A & B;
	            break;
			}
		case 0x12 : {
			    A = A + 1;
			    break;
			}
		case 0x13 : {
			    A = A - 1;
			    break;
			}
		// ADI - ADD Integer
		case 0x14 : {
				//System.out.println("Instru��o ADI : ");
				PUSH(POP()+W());
				break;
			}
		// SUI - SUBTRACT Integer
		case 0x15 : {
				PUSH(POP()-W());
				break;
			}
		// MUI - MULTIPLY Integer
		case 0x16 : {
				PUSH(POP()*W());
				break;
			}
		// DVI - DIVIDE Integer
		case 0x17 : {
				PUSH(POP()/W());
				break;
			}
		// 
		case 0x18 : {
				PUSH(POP()%W());
				break;
			}
		case 0x19 : {
				PUSH(POP()|W());
				break;
			}
		case 0x1A : {
				PUSH(POP()&W());
				break;
			}
		case 0x1B : {
				A = A + W();
				break;
			}
		case 0x1C : {
				A = A - W();
				break;
			}
		case 0x1D : {
				A = A * W();
				break;
			}
		case 0x1E : {
				A = A / W();
				break;
			}
		case 0x1F : {
				A = A & W();
				break;
			}
		// EQ - EQUAL (=)
		case 0x20 : {
				X = POP();
				Y = POP();
				if (X==Y) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		// NE - NOT EQUAL (<>)
		case 0x21 : {
				X = POP();
				Y = POP();
				if (X!=Y) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		// GT - GREAT THEN (>)
		case 0x22 : {
				Y = POP();
				X = POP();
				if (X>Y) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		// GE- GREAT THAN OR EQUAL (>=)
		case 0x23 : {
				X = POP();
				Y = POP();
				if (X>=Y) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		// LT - LESS THAN (<)
		case 0x24 : {
				Y = POP();
				X = POP();
				if (X<Y) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		// LE - LESS THAN OR EQUAL (<=)
		case 0x25 : {
				X = POP();
				Y = POP();
				if (X<=Y) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x26 : {
				X = POP();
				if (X==W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x27 : {
				X = POP();
				if (X!=W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x28 : {
				X = POP();
				if (X>W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x29 : {
				X = POP();
				if (X>=W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x2A : {
				X = POP();
				if (X<W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x2B : {
				X = POP();
				if (X<=W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x2C : {
				if (A==B) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x2D : {
				if (A!=B) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x2E : {
				if (A>B) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x2F : {
				if (A>=B) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x30 : {
				if (A<B) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x31 : {
				if (A<=B) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x32 : {
				if (A<=W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x33 : {
				if (A!=W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x34 : {
				if (A>W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x35 : {
				if (A>=W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x36 : {
				if (A<W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x37 : {
				if (A<=W()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x38 : {
				if (A==POP()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x39 : {
				if (A!=POP()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x3A : {
				if (A>POP()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x3B : {
				if (A>=POP()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x3C : {
				if (A<POP()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x3D : {
				if (A<=POP()) PUSH(0xFF);
				else PUSH(0x0000);
				break;
			}
		case 0x3E : {
			    X = A;
			    A = B;
			    B = X;
			    break;
			}
		case 0x3F :{
				X = POP();
				PUSH(A);
				A = X;
				break;
			}
		// LOD - LOAD
		case 0x40 :{
			PUSH(VALOR(W()));
			break;
		}
		// STO - STORE
		case 0x41 :{
			SALVA(W(),POP());
			break;
		}
		case 0x42 :{
			PUSH(VALOR(W()+POP()));
			break;
		}
		case 0x43 : {
			X = W();
			Y = POP();
			SALVA(X+POP(),Y);
			break;
		}
		// LDI - Load Integer (0x44)
		case 0x44 :{
			PUSH(W());
			break;
		}
		case 0x45 :{
			A = VALOR(W());
			break;
		}
		case 0x46 :{
			SALVA(A,W());
			break;
		}
		case 0x47 :{
			A = VALOR(W()+POP());
			break;
		}
		case 0x48 :{
			SALVA(A,W()+POP());
			break;
		}
		case 0x49 :{
			A = W();
			break;
		}
		case 0x4A :{
			B = VALOR(W());
			break;
		}
		case 0x4B :{
			SALVA(B,W());
			break;
		}
		case 0x4C :{
			B=VALOR(POP()+W());
			break;
		}
		case 0x4D :{
			SALVA(B,W()+POP());
			break;
		}
		case 0x4E :{
			B=W();
			break;
		}
		// LSP - Load Stack Pointer (0x4F)
		case 0x4F : {
			SP = W();
			break;
		}
		case 0x50 :{
			SP=SP+2;
			break;
		}
		case 0x51 :{
			SP=SP-2;
			break;
		}
		case 0x52 :{
			X = POP();
			Y = POP();
			PUSH(X);
			PUSH(Y);
			break;
		}
		case 0x53 :{
			A = POP();
			break;
		}
		case 0x54 :{
			PUSH(A);
			break;
		}
		case 0x55 :{
			B = POP();
			break;
		}
		case 0x56 :{
			PUSH(B);
			break;
		}
		// IN
		case 0x57 :{
			BufferedReader br = new BufferedReader(new InputStreamReader(System.in));
			try {
				X=(int) Integer.parseInt(br.readLine());
			} catch (NumberFormatException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
			PUSH(X);
			break;
		}
		// OUT
		case 0x58 :{
			System.out.println("Sa�da do CMS *****>> "+POP());
			
			break;
		}
		case 0x59 :{
			break;
		}
		// JMP - JUMP (0x5A)
		case 0x5A :{
			PC = W();
			break;
		}
		case 0x5B :{
			X = W();
			if (POP() == 0xFFFF) PC=X;
			break;
		}
		// JF - JUMP IF FALSE
		case 0x5C :{
			X = W();
			if (POP() == 0x0000) PC=X;
			break;
		}
		case 0x5D :{
			X = W();
			PUSH(PC);
			PC=X;
			break;
		}
		case 0x5E :{
			X = W();
			if (POP()==0xFFFF) {
				PUSH(PC);
				PC=X;
			}
			PC=X;
			break;
		}
		case 0x5F :{
			X = W();
			if (POP()==0x0000) {
				PUSH(PC);
				PC=X;
			}
			PC=X;
			break;
		}
		case 0x60 :{
			PC = POP();
			break;
		}
		// STOP
		case 0x61 :{
			FIM_PROG=true;
			break;
		}
		
		
	}
		
	}
	
	public static void main(String[] args){
		FIM_PROG = false;
	    cms cms = new cms();
	    cms.carga_programa(args[0]);
	    while (!FIM_PROG)  cms.hardware();
	}
}

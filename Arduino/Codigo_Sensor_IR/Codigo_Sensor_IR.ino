int IRSense0 = A0;
int IRSense1 = A1;
int IRSense2 = A2;
int IRSense3 = A3;
int IRSense4 = A4;
int IRSense5 = A5;
int analogPorts[] = {IRSense0,IRSense1,IRSense2,IRSense3,IRSense4,IRSense5};
int numeroControlador =-1;
void setup() 
{
  //Programe as portas de 2 até 9 na forma de um vetor de 8 bits para corresponder ao numero do controlador. Programa até 256:11111111
  //Cada porta analogica do arduino corresponde à uma vaga. No máximo 6 vagas podem ser geranciadas sem multiplexação;
  pinMode(2, INPUT);//+1
  pinMode(3, INPUT);//+2
  pinMode(4, INPUT);//+4
  pinMode(5, INPUT);//+8
  pinMode(6, INPUT);//+16
  pinMode(7, INPUT);//+32
  pinMode(8, INPUT);//+64
  pinMode(9, INPUT);//+128
  numeroControlador = readPins();
  pinMode(IRSense0, INPUT);
  pinMode(IRSense1, INPUT);
  pinMode(IRSense2, INPUT);
  pinMode(IRSense3, INPUT);
  pinMode(IRSense4, INPUT);
  pinMode(IRSense5, INPUT);
  Serial.begin(9600);
}



void loop() 
{
  Serial.print("<controlador id=\"");
  Serial.print(numeroControlador);
  Serial.print("\">");   
  for(int i=0; i<6;i++){
      int value = analogRead(analogPorts[i]);
      float dist = 0.0089*value -4.0853;
      Serial.print("<vaga><numero>");   
      Serial.print(i+1);    
      Serial.print("</numero><estado>");
      Serial.print(dist<4.5?1:0);
      Serial.print("</estado></vaga>");
      
  }    
Serial.println("</controlador>");
delay(1000);
}

int readPins(){//le a programação do numero da vaga
  int numeroControlador=0;
  if((digitalRead(9) == HIGH)){
          numeroControlador+=128;
        }
      if((digitalRead(8) == HIGH)){
          numeroControlador+=64;
        }
      if((digitalRead(7) == HIGH)){
          numeroControlador+=32;
        }
      if((digitalRead(6) == HIGH)){
          numeroControlador+=16;
        }
      if((digitalRead(5) == HIGH)){
          numeroControlador+=8;
        }
      if((digitalRead(4) == HIGH)){
          numeroControlador+=4;
        }
      if((digitalRead(3) == HIGH)){
          numeroControlador+=2;
        }
      if((digitalRead(2) == HIGH)){
          numeroControlador+=1;
        }
    return numeroControlador;
  
  }

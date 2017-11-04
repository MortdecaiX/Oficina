int IRSense = A0;

void setup() 
{
  //Programe as portas de 2 até 9 na forma de um vetor de 8 bits para corresponder ao numero da vaga. Programa até 256:11111111
  pinMode(2, INPUT);//+1
  pinMode(3, INPUT);//+2
  pinMode(4, INPUT);//+4
  pinMode(5, INPUT);//+8
  pinMode(6, INPUT);//+16
  pinMode(7, INPUT);//+32
  pinMode(8, INPUT);//+64
  pinMode(9, INPUT);//+128
  
  pinMode(IRSense, INPUT);
  Serial.begin(9600);
}



void loop() 
{

  int numeroVaga = readPins();
      



  int value = analogRead(IRSense);
  float dist = 0.0089*value -4.0853;
  if(dist<4.5){
      Serial.print("<vaga><numero>");
       Serial.print(numeroVaga);   
      Serial.println("</numero><estado>1</estado></vaga>");
      for(int i=0; i<30; i++){//Para de enviar feed por 30 segundos
        delay(1000);
      }
  }else{
    Serial.print("<vaga><numero>");
      Serial.print(numeroVaga);
/*
      Serial.print((digitalRead(9) == HIGH)?1:0);
      Serial.print((digitalRead(8) == HIGH)?1:0);
      Serial.print((digitalRead(7) == HIGH)?1:0); 
      Serial.print((digitalRead(6) == HIGH)?1:0); 
      Serial.print((digitalRead(5) == HIGH)?1:0); 
      Serial.print((digitalRead(4) == HIGH)?1:0); 
      Serial.print((digitalRead(3) == HIGH)?1:0); 
      Serial.print((digitalRead(2) == HIGH)?1:0); 
 */     
      Serial.println("</numero><estado>0</estado></vaga>");
      delay(5000);//feed a cada 5 segundos
    }
  
}

int readPins(){//le a programação do numero da vaga
  int numeroVaga=0;
  if((digitalRead(9) == HIGH)){
          numeroVaga+=128;
        }
      if((digitalRead(8) == HIGH)){
          numeroVaga+=64;
        }
      if((digitalRead(7) == HIGH)){
          numeroVaga+=32;
        }
      if((digitalRead(6) == HIGH)){
          numeroVaga+=16;
        }
      if((digitalRead(5) == HIGH)){
          numeroVaga+=8;
        }
      if((digitalRead(4) == HIGH)){
          numeroVaga+=4;
        }
      if((digitalRead(3) == HIGH)){
          numeroVaga+=2;
        }
      if((digitalRead(2) == HIGH)){
          numeroVaga+=1;
        }
    return numeroVaga;
  
  }

int IRSense0 = A0;
int IRSense1 = A1;
int IRSense2 = A2;
int IRSense3 = A3;
int IRSense4 = A4;
int IRSense5 = A5;
int analogPorts[] = {IRSense0,IRSense1,IRSense2,IRSense3,IRSense4,IRSense5};
int sensibilidade =-1;

int maxAnalogSensors = 0;

void setup() 
{
  //Programe as portas de 2 até 6 na forma de um vetor de 5 bits para corresponder a sensibilidade do controlador. Programe até 32:11111
  //Programe as portas de 7 até 9 na forma de um vetor de 3 bits para corresponder a sensibilidade do controlador. Programe até 6
  //Cada porta analogica do arduino corresponde à uma vaga. No máximo 6 vagas podem ser geranciadas sem multiplexação;
  pinMode(2, INPUT);//+1
  pinMode(3, INPUT);//+2
  pinMode(4, INPUT);//+4
  pinMode(5, INPUT);//+8
  pinMode(6, INPUT);//+16
  pinMode(7, INPUT);//+32
  pinMode(8, INPUT);//+64
  pinMode(9, INPUT);//+128
  sensibilidade = readPins();
  maxAnalogSensors = readMaxAnalogPins();
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
  Serial.print("<controlador sensibilidade=\"");
  Serial.print(sensibilidade);
  Serial.print("\" sensores=\">");   
  Serial.print(maxAnalogSensors);
  Serial.print("\">");   
  for(int i=0; (i<maxAnalogSensors) && (i<6);i++){
      int value = analogRead(analogPorts[i]);
      float dist = 0.03125*value;
      Serial.print("<vaga><numero>");   
      Serial.print(i+1);    
      Serial.print("</numero><estado>");
      Serial.print((dist<sensibilidade)?1:0);
      Serial.print("</estado>");

      Serial.print("<distancia>");
      Serial.print(dist);
      Serial.print("</distancia></vaga>");
      
      
  }    
Serial.println("</controlador>");
delay(1000);
}

int readPins(){//le a programação do numero da vaga
  int sensibilidade=0;
      if((digitalRead(6) == HIGH)){
          sensibilidade+=16;
        }
      if((digitalRead(5) == HIGH)){
          sensibilidade+=8;
        }
      if((digitalRead(4) == HIGH)){
          sensibilidade+=4;
        }
      if((digitalRead(3) == HIGH)){
          sensibilidade+=2;
        }
      if((digitalRead(2) == HIGH)){
          sensibilidade+=1;
        }
    return sensibilidade;
  
  }
  int readMaxAnalogPins(){//le a programação do numero da vaga
  int qnt=0;
  if((digitalRead(9) == HIGH)){
          qnt+=4;
        }
      if((digitalRead(8) == HIGH)){
          qnt+=2;
        }
      if((digitalRead(7) == HIGH)){
          qnt+=1;
        }
    return qnt;
  
  }

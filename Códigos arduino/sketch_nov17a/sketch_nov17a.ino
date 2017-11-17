
int pinoledverm = 5; //Pino ligado ao led vermelho
int pinoledazul = 6; //Pino ligado ao led azul
int pinopir = 3;  //Pino ligado ao sensor PIR
int acionamento;  //Variavel para guardar valor do sensor
int estado = 0;

void setup() 
{
  pinMode(pinoledverm, OUTPUT); //Define pino como saida
  pinMode(pinoledazul, OUTPUT); //Define pino como saida
  pinMode(pinopir, INPUT);   //Define pino sensor como entrada
  Serial.begin(9600);
}



void loop() 
{
  acionamento = digitalRead(pinopir); //Le o valor do sensor PIR
  if (acionamento == HIGH)
  {
    if(estado == 0){
      estado = 1;
      digitalWrite(pinoledverm, HIGH);
      digitalWrite(pinoledazul, LOW);
    }
    else{
      estado = 0;
      digitalWrite(pinoledverm, LOW);
      digitalWrite(pinoledazul, HIGH);
    }
  }
      Serial.print("<controlador>");   
      Serial.print("<vaga><numero>");   
      Serial.print(1);    
      Serial.print("</numero><estado>");
      Serial.print(acionamento);
      Serial.print("</estado>");
      Serial.print("</vaga>");  
Serial.println("</controlador>");
delay(1000);
}

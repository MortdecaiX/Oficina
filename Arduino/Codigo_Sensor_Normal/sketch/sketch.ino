//Arduino - Sensor de Presença

int pinoledverm = 5; //Pino ligado ao led vermelho
int pinoledazul = 6; //Pino ligado ao led azul
int pinopir = 3;  //Pino ligado ao sensor PIR
int acionamento = 0;  //Variavel para guardar valor do sensor

int ID_VAGA = 1;

void setup()
{
  pinMode(pinoledverm, OUTPUT); //Define pino como saida
  pinMode(pinoledazul, OUTPUT); //Define pino como saida
  pinMode(pinopir, INPUT);   //Define pino sensor como entrada
  Serial.begin(9600); // Define comunicacao serial a 9600 bps
}

void loop()
{
 acionamento = digitalRead(pinopir); //Le o valor do sensor PIR
 Serial.println("<vaga><id>"+String(ID_VAGA)+"</id><estado>"+String(acionamento)+"</estado></vaga>");
 if (acionamento == LOW)  //Sem movimento, mantem led azul ligado
 {
    digitalWrite(pinoledverm, LOW);
    digitalWrite(pinoledazul, HIGH);
 }
 else  //Caso seja detectado um movimento, aciona o led vermelho
 {
    digitalWrite(pinoledverm, HIGH);
    digitalWrite(pinoledazul, LOW);
	//Serial.println("<vaga><id>"+String(ID_VAGA)+"</id><estado>"+String(acionamento)+"</estado></vaga>");
 }
}

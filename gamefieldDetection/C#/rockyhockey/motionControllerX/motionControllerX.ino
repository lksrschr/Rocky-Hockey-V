#include <AccelStepper.h>

//pin setup
//tourque hold pin
#define ENABLE_PIN 11
//end-switch pin, to be configured as pullup-input!!
#define END_PIN 8

AccelStepper stepper(AccelStepper::DRIVER, 5, 6);

#define EN_DISABLE digitalWrite(ENABLE_PIN, HIGH);
#define EN_ENABLE digitalWrite(ENABLE_PIN, LOW);

//constants
long maxPosition = 3900;

//used variables
long movement = 0;
int speedSetting = 0;
int enabled = 0;
float assumedPosition = 0;
char movement_string = 0;

void setup()
{  

  TCCR1B = (TCCR1B & 0b11111000) | 0x02;
  //TCCR1B = 0; TCCR1B |= (1 « CS10);
  
  pinMode(ENABLE_PIN, OUTPUT);
  pinMode(END_PIN, INPUT_PULLUP);
  stepper.setMinPulseWidth(60);
  stepper.setMaxSpeed(1500);
  stepper.setAcceleration(9999);
  stepper.setEnablePin(ENABLE_PIN);
  Serial.begin(115200);

  calibrate();
}

//calibrates position by moving to assumed null position +x till end switch toggled
void calibrate(){
    EN_ENABLE;
    //assumedPosition = stepper.currentPosition();
    //inverting assumed position and adding estimated deviation
    //stepper.move(assumedPosition * -1.0f + 100.0);
    stepper.move(-maxPosition);
    stepper.setSpeed(1400);
    while(stepper.distanceToGo() != 0 && digitalRead(END_PIN) == 1){
      stepper.runSpeedToPosition();
    }
    stepper.setCurrentPosition(0);
    EN_DISABLE;
}

//check if movement will cause serious harm or death
bool moveAllowed(){
  if(digitalRead(END_PIN) == 0){
    if(stepper.distanceToGo() > 0){
       return true;
    }
    else{
      stepper.stop();
      stepper.setCurrentPosition(0);
      return false;
    }
  }
  else if(stepper.targetPosition() > maxPosition){
    stepper.stop();
    return false;
  }
  else{
    return true;
  }
}

void loop()
{  
    //just testing the switches
    //Serial.print(digitalRead(END_PIN));
    //delay(999);

if(Serial.available() > 0) {
  if (Serial.available() > 0){
    movement_string  = Serial.read();
  }
    
  if((movement_string >= '0') && (movement_string <= '9')){
    long x = 200;
    stepper.runToNewPosition(x);
    Serial.print(stepper.currentPosition());
  }
}
}

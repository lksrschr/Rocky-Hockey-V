#include <Arduino.h>

#include <AccelStepper.h>

#define ENABLE_PIN 8
#define MOTOR_A_STEP_PIN 2
#define MOTOR_A_DIR_PIN 5
#define END_PIN_X 9
#define END_PIN_Y 10

#define MOTOR_B_STEP_PIN 4
#define MOTOR_B_DIR_PIN 7

AccelStepper stepperx(1, MOTOR_A_STEP_PIN, MOTOR_A_DIR_PIN);
AccelStepper steppery(1, MOTOR_B_STEP_PIN, MOTOR_B_DIR_PIN);

//constants
long max_x_position = 3900;
long max_y_position = 3900;
bool st_enabled = false;
//used variables
long movement_x = 0;
long movement_y = 0;

//calibrates position by moving to assumed null position +x till end switch toggled
void calibrate_x()
{
    stepperx.enableOutputs();
    Serial.println("Calibrate X");
    stepperx.setSpeed(5000);

    stepperx.move(-max_x_position);
    while (stepperx.distanceToGo() != 0 && digitalRead(END_PIN_X) == 1)
    {
        stepperx.runSpeedToPosition();
    }
    stepperx.setCurrentPosition(0);
    stepperx.disableOutputs();
}

void calibrate_y()
{
    steppery.enableOutputs();
    Serial.println("Calibrate Y");
    stepperx.setSpeed(5000);

    steppery.move(-max_y_position);
    while (steppery.distanceToGo() != 0 && digitalRead(END_PIN_X) == 1)
    {
        steppery.runSpeedToPosition();
    }
    steppery.setCurrentPosition(0);
    steppery.disableOutputs();
}

void setup()
{
    Serial.println("Setup");

    // TCCR1B = (TCCR1B & 0b11111000) | 0x02;
    //TCCR1B = 0; TCCR1B |= (1 « CS10);

    pinMode(ENABLE_PIN, OUTPUT);
    pinMode(END_PIN_X, INPUT_PULLUP);
    pinMode(END_PIN_Y, INPUT_PULLUP);
    stepperx.setPinsInverted(false, false, true);
    steppery.setPinsInverted(false, false, true);

    // stepperx.setMinPulseWidth(60);
    stepperx.setMaxSpeed(5000);
    stepperx.setAcceleration(2500000);
    stepperx.setSpeed(1500);

    stepperx.setEnablePin(ENABLE_PIN);

    // steppery.setMinPulseWidth(60);
    steppery.setMaxSpeed(5000);
    steppery.setAcceleration(2500000);
    steppery.setSpeed(1500);

    steppery.setEnablePin(ENABLE_PIN);

    Serial.begin(9600);

    // stepperx.enableOutputs();
    // steppery.enableOutputs();
    // calibrate_x();
    // calibrate_y();
}

bool moveAllowedx()
{
    if (digitalRead(END_PIN_X) == 0)
    {
        if (stepperx.distanceToGo() > 0)
        {
            return true;
        }
        else
        {
            stepperx.stop();
            stepperx.setCurrentPosition(0);
            return false;
        }
    }
    else if (stepperx.targetPosition() > max_x_position)
    {
        stepperx.stop();
        return false;
    }
    else
    {
        return true;
    }
}

bool moveAllowedy()
{
    if (digitalRead(END_PIN_Y) == 0)
    {
        if (steppery.distanceToGo() > 0)
        {
            return true;
        }
        else
        {
            steppery.stop();
            steppery.setCurrentPosition(0);
            return false;
        }
    }
    else if (steppery.targetPosition() > max_y_position)
    {
        steppery.stop();
        return false;
    }
    else
    {
        return true;
    }
}

void loop()
{
    if ((stepperx.distanceToGo() != 0) || (steppery.distanceToGo() != 0) && !st_enabled)
    {
        Serial.println("Enable steppers");
        st_enabled = true;
        stepperx.enableOutputs();
        steppery.enableOutputs();
    }

    while ((stepperx.distanceToGo() != 0 || steppery.distanceToGo() != 0))
    {
        if (!st_enabled)
        {
            st_enabled = true;
            stepperx.enableOutputs();
            steppery.enableOutputs();
        }
        if (stepperx.distanceToGo() != 0 && moveAllowedx())
        {
            stepperx.runSpeedToPosition();
        }
        if (steppery.distanceToGo() != 0 && moveAllowedy())
        {
            steppery.runSpeedToPosition();
        }
    }

    if (stepperx.distanceToGo() == 0 && steppery.distanceToGo() == 0 && st_enabled)
    {
        Serial.println("Disable steppers");
        st_enabled = false;

        stepperx.disableOutputs();
        steppery.disableOutputs();
    }

    if (Serial.available() > 0)
    {
        String movement_string = Serial.readStringUntil('\n');
        if ((movement_string == "position\n") || (movement_string == "position"))
        {
            Serial.print("{\"x_position\": \"" + String(stepperx.currentPosition()) + "\",");
            Serial.println("\"y_position\": \"" + String(steppery.currentPosition()) + "\"}");
        }
        else if ((movement_string == "calibrate\n") || (movement_string == "calibrate"))
        {
            Serial.println("Calibrate triggered");
            calibrate_x();
            calibrate_y();
        }
        else
        {
            movement_x = movement_string.toInt();
            movement_y = movement_string.toInt();
            stepperx.move(movement_x);
            steppery.move(movement_y);
            // 1000;1234 -1000;2000
            // movement_x = movement_string.substring(0,4).toInt();
            // movement_y = movement_string.substring(5,9).toInt();
            // Serial.println("Got x: " + String(movement_x));
            // Serial.println("Got y: " + String(movement_y));
        }
    }
}

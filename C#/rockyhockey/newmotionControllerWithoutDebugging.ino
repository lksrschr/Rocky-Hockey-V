#include <Arduino.h>
#include <AccelStepper.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>

/*
 * 11: Limit Z-Axis
 * 10: Limit Y-Axis
 * 9 : Limit X-Axis
 * 8 : Stepper Enable/Disable
 * 7 : Direction Z
 * 6 : Direction Y
 * 5 : Direction X
 * 4 : StepPulse Z
 * 3 : StepPulse Y
 * 2 : StepPulse X
 *
 * 1 Motor  : X
 * 2 Motoren: Y
 *
 * Y Positive = Bildschirme
 * Y Negative= Tafel
 *
 * X Postive = Tür
 * X Negative = Fenster
 */

#define ENABLE_PIN 8
#define MOTOR_X_STEP_PIN 2
#define MOTOR_X_DIR_PIN 5
#define MOTOR_Y_STEP_PIN 3
#define MOTOR_Y_DIR_PIN 6
#define END_PIN_X 9
#define END_PIN_Y 10

AccelStepper stepperx(1, MOTOR_X_STEP_PIN, MOTOR_X_DIR_PIN);
AccelStepper steppery(1, MOTOR_Y_STEP_PIN, MOTOR_Y_DIR_PIN);

//constants
long max_x_position = 22800;
long max_y_position = 15600;
bool st_enabled = false;
//used variables
long movement_x = 0;
long movement_y = 0;

void random_movement() {
  int lowerx = -20000;
  int lowery = -15000;
  int rand_x;
  int rand_y;
  do {
      rand_x = random(lowerx, max_x_position);
      rand_x = (rand_x+500)/1000;
      rand_x = rand_x*1000;
  } while (((rand_x + stepperx.currentPosition()) > max_x_position) ||
  ((rand_x + stepperx.currentPosition()) < 1000) ||
  (rand_x == 0));

  do {
      rand_y = random(lowery, max_y_position);
      rand_y = (rand_y+500)/1000;
      rand_y = rand_y*1000;
  } while (((rand_y + steppery.currentPosition()) > max_y_position) ||
  ((rand_y + steppery.currentPosition()) < 1000) ||
  (rand_y == 0));

  stepperx.move(rand_x);
  steppery.move(rand_y);
}

//calibrates position by moving till end switch toggled
void calibrate_x()
{
    long homing=-1;
    stepperx.enableOutputs();
    while (digitalRead(END_PIN_X)) {
        stepperx.moveTo(homing);
        homing--;
        stepperx.run();
    }

    stepperx.setCurrentPosition(0);
    stepperx.moveTo(11400);
    stepperx.run();
    stepperx.disableOutputs();
}

void calibrate_y()
{
    long homing=-1;
    steppery.enableOutputs();
    while (digitalRead(END_PIN_Y)) {
        steppery.moveTo(homing);
        homing--;
        steppery.run();
    }

    steppery.setCurrentPosition(0);
    steppery.moveTo(7800);
    steppery.run();
    steppery.disableOutputs();
}

void setup()
{
    pinMode(ENABLE_PIN, OUTPUT);
    pinMode(END_PIN_X, INPUT_PULLUP);
    pinMode(END_PIN_Y, INPUT_PULLUP);

    stepperx.setPinsInverted(false, false, true);
    steppery.setPinsInverted(false, false, true);

    stepperx.setMaxSpeed(9000);
    stepperx.setAcceleration(2500000000);
    stepperx.setSpeed(9000);

    steppery.setMaxSpeed(9000);
    steppery.setAcceleration(2500000000);
    steppery.setSpeed(9000);

    stepperx.setEnablePin(ENABLE_PIN);
    steppery.setEnablePin(ENABLE_PIN);

    Serial.begin(9600);
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
        st_enabled = false;

        stepperx.disableOutputs();
        steppery.disableOutputs();
    }

    if (Serial.available() > 0)
    {
        String movement_string = Serial.readStringUntil('\n');
        if ((movement_string == "ready\n") || (movement_string == "ready"))
        {
          if (st_enabled == 0)
            Serial.print("True");
          else
            Serial.print("False");
        }
        else if ((movement_string == "position\n") || (movement_string == "position"))
        {
            Serial.println(String(stepperx.currentPosition()) + "," + String(steppery.currentPosition()));
        }
        else if ((movement_string == "calibrate\n") || (movement_string == "calibrate"))
        {
            calibrate_x();
            calibrate_y();
        }
        else if ((movement_string == "xcalibrate\n") || (movement_string == "xcalibrate"))
        {
            calibrate_x();
        }
        else if ((movement_string == "ycalibrate\n") || (movement_string == "ycalibrate"))
        {
            calibrate_y();
        }
        else if ((movement_string == "random\n") || (movement_string == "random"))
        {
            random_movement();
        }
        else
        {
            int delimiterIndex = movement_string.indexOf(',');
            String XValue = movement_string.substring(0, delimiterIndex);
            String YValue = movement_string.substring(delimiterIndex + 1);
            Serial.println(YValue);
            movement_x = XValue.toInt();
            movement_y = YValue.toInt();
            stepperx.moveTo(movement_x);
            steppery.moveTo(movement_y);
        }
    }
}

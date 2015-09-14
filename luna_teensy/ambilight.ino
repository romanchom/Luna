#include <Adafruit_NeoPixel.h>

#include "TimerOne.h"
#include "TimerThree.h"

#define Bluetooth Serial

#define pixelCount 120
#define PIN_PIXELS_LEFT 5
#define PIN_PIXELS_RIGHT 7
#define PIN_POWER_ENABLE 27
#define PIN_LED 6
#define PIN_WHITE_LEFT 25
#define PIN_WHITE_RIGHT 26

enum PacketType{
	pacektTurnOn = 0,
	pacektTurnOff,
	packetSetLights
};
	
byte data[1024];

Adafruit_NeoPixel pixelsLeft = Adafruit_NeoPixel(pixelCount, PIN_PIXELS_LEFT, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixelsRight = Adafruit_NeoPixel(pixelCount, PIN_PIXELS_RIGHT, NEO_GRB + NEO_KHZ800);

volatile unsigned char waitForData = 0;
unsigned char ledState = 0;

void setup() {
	Serial.begin(9600);
	Bluetooth.begin(115200);
	
	pinMode(PIN_LED, OUTPUT);
	digitalWrite(PIN_LED, HIGH);
	
	pinMode(PIN_POWER_ENABLE, OUTPUT);
	digitalWrite(PIN_POWER_ENABLE, HIGH);
	
	
	Timer1.initialize(200);
	pinMode(PIN_WHITE_LEFT, OUTPUT);
	Timer1.pwm(PIN_WHITE_LEFT, 0);
	pinMode(PIN_WHITE_RIGHT, OUTPUT);
	Timer1.pwm(PIN_WHITE_RIGHT, 0);
	
	Timer3.initialize(160000);
	Timer3.attachInterrupt(dataTimeoutInterrupt);
}

void loop()
{
	if(Bluetooth.available()){
		ledState ^= 1;
		digitalWrite(PIN_LED, ledState);
		waitForData = 2;
		Timer3.restart();
		
		unsigned short packetLength;
		((byte*)&packetLength)[0] = Bluetooth.read();
		
		while((!Bluetooth.available()) && waitForData != 0);	
		if(!waitForData) return;
		
		((byte*)&packetLength)[1] = Bluetooth.read();
		//Serial.print(packetLength);
		//Serial.print("\n\r");
		if(packetLength > 1024) return;
		
		for(unsigned short i = 0; i < packetLength; ++i){
			while(!Bluetooth.available() && waitForData != 0);
			if(!waitForData) return;
			data[i] = Bluetooth.read();
		}
		
		Timer3.stop();
		
		switch(data[0]){
		case pacektTurnOn:
			digitalWrite(PIN_POWER_ENABLE, LOW);
			digitalWrite(PIN_LED, LOW);
			break;
		case pacektTurnOff:
			digitalWrite(PIN_POWER_ENABLE, HIGH);
			digitalWrite(PIN_LED, HIGH);
			break;
		case packetSetLights:
			if(packetLength == (5 + pixelCount * 6)) setLights(data + 1);
			break;
		default:
			Serial.print('?');
		}
	}
}

void setLights(byte * data){
	unsigned short whiteLeft, whiteRight;
	whiteLeft = ((unsigned short *) data)[0];
	whiteRight = ((unsigned short *) (data))[1];
	
	Timer1.setPwmDuty(PIN_WHITE_LEFT, whiteLeft >> 6);
	Timer1.setPwmDuty(PIN_WHITE_RIGHT, whiteRight >> 6);
	
	/*
	for(int i = 0; i < pixelCount; ++i){
		pixelsLeft.setPixelColor(i, data[i * 3 + 4]);
		pixelsLeft.setPixelColor(i, data[i * 3 + 5]);
		pixelsLeft.setPixelColor(i, data[i * 3 + 6]);
	}
	
	for(int i = 0; i < pixelCount; ++i){
		pixelsRight.setPixelColor(i, data[i * 3 + 4 + pixelCount * 3]);
		pixelsRight.setPixelColor(i, data[i * 3 + 5 + pixelCount * 3]);
		pixelsRight.setPixelColor(i, data[i * 3 + 6 + pixelCount * 3]);
	}
	
	pixelsLeft.show();
	pixelsRight.show();*/
}

void dataTimeoutInterrupt(){
	waitForData--;
	if(waitForData == 0){
		Timer3.stop();
		Serial.print("T");
	}
}
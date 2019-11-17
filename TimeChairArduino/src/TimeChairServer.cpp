#include <Arduino.h>

/*
 * TimeChairServer.cpp
 *
 *  Created on: 17.11.2019
 *  Beat Gerber
 *
 */

#include "timeChair.h"
#include <SPI.h>
#include "MAX31855.h"

const int doPin = 19;
const int csPin = 5;
const int clPin = 18;



int SolarPanelPin = A4; 
int RelaisHeatPin = 27;
int RelaisCoolPin = 14;
int SlavePin1 = 4; //Thermocouple #1

int solarPanelVoltage;
int solarPanelVoltageAverage=500;
const int threshold =20;
const int solarPanelValueSmoothing=4;
bool assis = false;
bool isConnected = false;

WiFiServer server(SERVER_PORT);
WebSocketsClient webSocketClient;
MAX31855 tc(clPin, csPin, doPin);

WiFiMulti wfMulti;


#define USE_SERIAL Serial

void hexdump(const void *mem, uint32_t len, uint8_t cols = 16) {
	const uint8_t* src = (const uint8_t*) mem;
	USE_SERIAL.printf("\n[HEXDUMP] Address: 0x%08X len: 0x%X (%d)", (ptrdiff_t)src, len, len);
	for(uint32_t i = 0; i < len; i++) {
		if(i % cols == 0) {
			USE_SERIAL.printf("\n[0x%08X] 0x%08X: ", (ptrdiff_t)src, i);
		}
		USE_SERIAL.printf("%02X ", *src);
		src++;
	}
	USE_SERIAL.printf("\n");
}

void pharseText( uint8_t * payload, size_t length)
{
	char buff_p[length];
	for (int i = 0; i < length; i++)
	{
		buff_p[i] = (char)payload[i];
	}
	buff_p[length] = '\0';
	String msg_p = String(buff_p);
	int val;
	val = msg_p.toInt(); // to Int
	Serial.print("Setpoint: ");
	Serial.print(val);
	Serial.print("°C -  Actual Temperature : ");
	Serial.print(wsd.sensor.temperature);  
	Serial.println("°C");
    if(val-2 > wsd.sensor.temperature)   digitalWrite(RelaisCoolPin,HIGH);	else digitalWrite(RelaisCoolPin,LOW);
	if(val+2 < wsd.sensor.temperature )  digitalWrite(RelaisHeatPin,HIGH);	else digitalWrite(RelaisHeatPin,LOW);
}
void webSocketClientEvent(WStype_t type, uint8_t * payload, size_t length) {

	switch(type) {
		case WStype_DISCONNECTED:
			USE_SERIAL.printf("[WSc] Disconnected!\n");
			break;
		case WStype_CONNECTED:
			USE_SERIAL.printf("[WSc] Connected to url: %s\n", payload);

			// send message to server when Connected
			webSocketClient.sendTXT("Connected");
			break;
		case WStype_TEXT:
			pharseText(payload,length);
			break;
		case WStype_BIN:
			USE_SERIAL.printf("[WSc] get binary length: %u\n", length);
			hexdump(payload, length);

			// send data to server
			// webSocket.sendBIN(payload, length);
			break;
		case WStype_ERROR:			
		case WStype_FRAGMENT_TEXT_START:
		case WStype_FRAGMENT_BIN_START:
		case WStype_FRAGMENT:
		case WStype_FRAGMENT_FIN:
			break;
	}

}

void setup() {
    Serial.begin(9600);
    delay(1000);
    pinMode(RelaisHeatPin, OUTPUT);
    pinMode(RelaisCoolPin, OUTPUT);
	tc.begin();

    WiFi.mode(WIFI_AP);
    WiFi.softAP(SSID, PASSWORD);
    Serial.print("Access point running. IP address: ");
    Serial.println(WiFi.softAPIP());
    server.begin();
 
    //Serial.setDebugOutput(true);
    Serial.setDebugOutput(true);

    Serial.println();
    Serial.println();
    Serial.println();

    for(uint8_t t = 4; t > 0; t--) {
        Serial.printf("[SETUP] BOOT WAIT %d...\n", t);
        Serial.flush();
        delay(1000);
    }

	webSocketClient.begin("192.168.4.3", 8989, "/chair");

	// event handler
	webSocketClient.onEvent(webSocketClientEvent);

	// use HTTP Basic Authorization this is optional remove if not needed
	//webSocket.setAuthorization("user", "Password");

	// try every 5000 ms again if connection has failed
	webSocketClient.setReconnectInterval(5000);
	isConnected = true;



 }

void loop() {
/*	WiFiClient newClient = server.available();
	if (newClient) {
		// TODO: Check whether we have an existing connection from this IP
		// TODO: Get the first 'free' array index
		IPAddress ip = newClient.remoteIP();
		Serial.println(ip.toString());
		if(ip.toString() == "198.168.4.2" and isConnected == false)
		{
			
			//clients[firstFree] = newClient;
			webSocketClient.begin(ip.toString(), 8989, "/");

			// event handler
			webSocketClient.onEvent(webSocketClientEvent);

			// use HTTP Basic Authorization this is optional remove if not needed
			//webSocket.setAuthorization("user", "Password");

			// try ever 5000 again if connection has failed
			webSocketClient.setReconnectInterval(5000);
			isConnected = true;
		}
	}
*/
    if(isConnected) webSocketClient.loop();
    // put your main code here, to run repeatedly:
    float temp;
    solarPanelVoltage = analogRead(SolarPanelPin);
    temp=((solarPanelValueSmoothing-1)*solarPanelVoltageAverage+solarPanelVoltage)/solarPanelValueSmoothing;
    solarPanelVoltageAverage =temp;
    if(temp < threshold )
    {
        assis=true;
        Serial.println("assis");
    }
    else 
    {
        if(assis) Serial.println("No more assis");
        assis=false; 
    }

    wsd.sensor.solarPanelVoltage = solarPanelVoltage;
    wsd.sensor.assis = assis;

	int status = tc.read();
 	wsd.sensor.temperature = (int)tc.getTemperature();

	if(isConnected) webSocketClient.sendBIN(wsd.bytePacket, sizeof(sensorData_t));
/*
int status = tc.read();
  Serial.print("stat:\t\t");
  Serial.println(status);

  float internal = tc.getInternal();
  Serial.print("internal:\t");
  Serial.println(internal);

  float temper1 = tc.getTemperature();
  Serial.print("temperature:\t");
  Serial.println(temper1);
*/
    delay(100);
}


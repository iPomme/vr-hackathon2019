#include <WiFi.h>
#include <WiFiClientSecure.h>
#include <WebSocketsClient.h>
#include <WiFi.h>
#include <WiFiMulti.h>
#include <driver/dac.h>
//#include <ArduinoJson.h>
#include <WebSocketsServer.h>

#define SSID "TimeChairServer"
#define PASSWORD "87654321"
#define SERVER_PORT 8989

typedef struct sensorData_t{
  uint16_t solarPanelVoltage;
  uint16_t assis;
  int16_t temperature;
};

typedef union Websock_Packet_t{
    sensorData_t sensor;
    byte bytePacket[sizeof(sensorData_t)];
};

Websock_Packet_t wsd;  

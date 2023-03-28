# EspressIf ESP32-Azure IoT Kit

## Overview

The [ESP32-Azure IoT Kit](https://www.espressif.com/en/products/devkits/esp32-azure-kit/overview) is built on the ESP32 chip.  It includes motion, magnetometer, barometer, temperature and humidity, and light sensors.  There are several demos to explore.

## Simple Ardunio example for getting the board setup and exploring the sensors

1. Install the Arduino IDE.
1. Add the [ESP32 board manager](https://docs.espressif.com/projects/arduino-esp32/en/latest/installing.html#installing-using-arduino-ide).
1. Open the [sample](/ESP32IoTDevKit/DevKitArduinoSample/DevKitArduinoSample.ino).  This is a clone from [Ewerton Scaboro da Silva](https://github.com/ewertons/esp32-azureiotkit-sensors)
1. Install ESPRESSIF ESP32 Sensors library for Arduino

    - On the Arduino IDE, go to menu `Sketch`, `Include Library`, `Manage Libraries...`
    - Search for and install `Espressif ESP32 Azure IoT Kit Sensors` by Ewerton Scaboro da Silva.

1. Select the board to be `ESP32 Wrover Module`

## Azure SDK for C Ardruino library to connect to Azure

Follow the [tutorial](https://github.com/Azure/azure-sdk-for-c-arduino/tree/main/examples/Azure_IoT_Central_ESP32_AzureIoTKit) provided by Azure.

## Azure IoT Middleware for FreeRTOS

Follow the [tutorial](https://github.com/Azure-Samples/iot-middleware-freertos-samples/tree/main/demos/projects/ESPRESSIF/aziotkit) provided in Azure Samples.

## Learnings

- This dev board provides a lot of flexibility for testing.
- This is a major upgrade for developers using FreeRTOS and want to use Azure.  It greatly simplifies things.
- I like how the Espressif IoT Dev Framework (IDF) utilities included help ensure you're not hardcoding connection strings.
- The IDF tools included are easy to work with and customize.  A few of the menus need to be looked at carefully, but not a show stopper.

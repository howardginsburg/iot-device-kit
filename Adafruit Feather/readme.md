# Adafruit Feather ESP32 Running Nanoframework

## Overview

This demo shows off how to use a simple ESP32 board to explore the .Net Nanoframework.

- [Adafruit ESP32 Feather](https://www.adafruit.com/product/3591)
  - [Pinout Diagram](https://learn.adafruit.com/adafruit-huzzah32-esp32-feather/pinouts)
- [Elegoo 37 Piece Sensor Kit](https://www.elegoo.com/products/elegoo-37-in-1-sensor-kit)
- Learn more about the [.Net Nanoframework](https://www.nanoframework.net/).
  - To get this sample running, I used Visual Studio 2022 and followed the [Getting Started](https://www.nanoframework.net/) instructions for getting the Nanoframework setup and the device flashed with the firmware.  After that, it was pretty straight forward.

## Deployment

### Construction

![Fritzing Diagram](../images/Feather%20ESP32_bb.png)

1. Feather ESP32
2. Tilt sensor
3. RGB LED
4. Piezo speaker

### Deploying to IoT Central

1. Create a new device template in IoT Central.
2. Select to Import Model, and select the Feather.json file.
3. Create a new device in IoT Central using the new Feather template.  Make note of the connection information.

### Feather

1. Wire up the board and sensors according the the fritzing diagram.
2. Follow the instructions for getting the Feather running the nanoframework and recognized in Visual Studio.
3. Open the AdaFruitFeather.sln file in Visual Studio 2022.
4. Select your board in the Device Explorer view.  You may have to enable the view in Visual Studio to see it.
5. Edit Configuration.cs and replace DPS_REGISTRATION_ID, DPS_ADDRESS, DPS_SCOPE, and DPS_SAS_KEY with the connection information from IoT Central.
6. Change the configuration from Debug to Release, and compile/deploy the code to the board.

## Learnings

- One of the main reasons I explored this framework is that I'm just not very good with writing C code.  C# and other more modern languages is where I'm comfortable.  I found the samples provided on the nanoframework website are very robust, as well as the API documentation around using the sensors.  Getting the code working with my sensors was very straightforward.  The Azure IoT SDK nuget packages met my needs for not only connecting, but receiving direct method callbacks.  I did not use device twins for this project, but they would also be easy to setup.

- My favorite feature of the tool integration is the ability to use the Visual Studio debugger connected to the device.  It made it really easy to troubleshoot stacktraces, etc as I was learning!

- A future learning would be to explore the threading option rather than using timestamps and compares to determine when to send telemetry data.  It was the easy way out as I had other devices to explore.

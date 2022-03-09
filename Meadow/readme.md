# Wilderness Labs Meadow F7v1

## Overview

This demo shows off how to use a Meadow F7 with .Net Standard and a micro-RTOS.

- [Meadow F7](https://store.wildernesslabs.co/collections/frontpage/products/meadow-f7)
  - [Pinout Diagram](http://developer.wildernesslabs.co/Meadow/Meadow_Basics/Hardware/F7v1/)
- [Elegoo 37 Piece Sensor Kit](https://www.elegoo.com/products/elegoo-37-in-1-sensor-kit)
- Learn more about .Net Standard on the Meadow board at <http://developer.wildernesslabs.co/Meadow/>.
  - To get this sample running, I used Visual Studio 2022 and followed the [Getting Started](http://developer.wildernesslabs.co/Meadow/Getting_Started/) instructions for getting the board setup and the device flashed with the latest firmware.  After that, it was pretty straight forward.

## Deployment

### Construction

TODO: Insert Fritzing Diagram

### Deploying to IoT Central

1. Create a new device template in IoT Central.
2. Select to Import Model, and select the Feather.json file.
3. Create a new device in IoT Central using the new Feather template.  Make note of the connection information.

### Feather

1. Wire up the board and sensors according the the fritzing diagram.
2. Follow the instructions for getting the Feather running the nanoframework and recognized in Visual Studio.
3. Open the RubeGoldbergNano.sln file in Visual Studio 2022.
4. Select your board in the Device Explorer view.  You may have to enable the view in Visual Studio to see it.
5. Edit Configuration.cs and replace DPS_REGISTRATION_ID, DPS_ADDRESS, DPS_SCOPE, and DPS_SAS_KEY with the connection information from IoT Central.
6. Change the configuration from Debug to Release, and compile/deploy the code to the board.

## Learnings

- One of the main reasons I explored this framework is that I'm just not very good with writing C code.  C# and other more modern languages is where I'm comfortable.  I found the samples provided on the nanoframework website are very robust, as well as the API documentation around using the sensors.  Getting the code working with my sensors was very straightforward.  The Azure IoT SDK nuget packages met my needs for not only connecting, but receiving direct method callbacks.  I did not use device twins for this project, but they would also be easy to setup.

- My favorite feature of the tool integration is the ability to use the Visual Studio debugger connected to the device.  It made it really easy to troubleshoot stacktraces, etc as I was learning!

- A future learning would be to explore the threading option rather than using timestamps and compares to determine when to send telemetry data.  It was the easy way out as I had other devices to explore.

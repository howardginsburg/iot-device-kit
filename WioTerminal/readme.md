# Wio Terminal

## Overview

The [Wio Terminal](https://www.seeedstudio.com/Wio-Terminal-p-4509.html) has an ATSAMD51 core mcu and is capable of running Arduino and MicroPython code.  It has a built in LCD, a few sensors, and buttons which make it a fantastic prototyping device.  In addition, the Grove connectors make it easy to add new sensors without worrying about breadboards and wiring.  I tried out the sample of which showcases the Azure IoT embedded-c sdk.

## Deployment

Follow the [tutorial](https://wiki.seeedstudio.com/Connect-Wio-Terminal-to-Microsoft-Azure-IoT-Central/) WIO puts out.  For this, I did not add any additional sensors.

## Learnings

- This is a fantastic prototyping board for anyone that does not want to deal with breadboards and wires.
- The sample makes use of a great architecture pattern for putting the device into a setup mode to prevent hard coding connection strings.

# Pic-IOT WA

## Overview

The [Pic-IOT WA](https://www.microchip.com/development-tool/EV54Y39A) has an PIC24FJ128GA705 MCU, an ATECC608A CryptoAuthentication™ secure element IC and the fully-certified ATWINC1510 Wi-Fi® network controller and runs embedded c.  It includes temperature, light, buttons, and LEDs.  This demo showcases how Pic can manage device certificate generation and connectivity to IoT Central.

## Deployment

Follow the [tutorial](https://github.com/Azure-Samples/Microchip-PIC-IoT-Wx) Microchip puts out.

## Learnings

- The content around provisioning is interesting as you use their console to provision the cert, and IoT Central to provision the device.
- Their provisioning software and sample allow you to onboard the device without hard coding connection strings.

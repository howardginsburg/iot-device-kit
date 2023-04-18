# MxChip

## Overview

The [MxChip](https://microsoft.github.io/azure-iot-developer-kit/) contains an STM32F412 ARM Cortex M4F MCU and there is an Azure RTOS configuration available that will run on it.

## Deployment

There are a few options to explore with the MXChip.

### Azure RTOS

Follow the [tutorial](https://docs.microsoft.com/azure/iot-develop/quickstart-devkit-mxchip-az3166) to get things going quickly.

### Arduino

When the MxChip was first released, working with it was a combination of a Visual Studio Code plugin plus the Arduino IDE.  That has since been retired.  However, you can still use the Arduino IDE to work with the board.  The legacy documentation can still be found [online](https://microsoft.github.io/azure-iot-developer-kit/docs/get-started/).

The following steps are needed to get started.

1. ### Install ST-Link drivers

[ST-Link/V2](http://www.st.com/en/development-tools/st-link-v2.html) is the USB interface that IoT DevKit uses to communicate with your development machine. Follow the OS-specific steps to allow the machine access to your device.

* **Windows**: Download and install USB driver from [STMicroelectronics website](http://www.st.com/en/development-tools/stsw-link009.html).
* **macOS**: No driver is required for macOS.
* **Ubuntu**: Run the following in terminal and log out and log in for the group change to take effect:

    ```bash
    # Copy the default rules. This grants permission to the group 'plugdev'
    sudo cp ~/.arduino15/packages/AZ3166/tools/openocd/0.10.0/linux/contrib/60-openocd.rules /etc/udev/rules.d/
    sudo udevadm control --reload-rules

    # Add yourself to the group 'plugdev'
    # Logout and log back in for the group to take effect
    sudo usermod -a -G plugdev $(whoami)
    ```

1. Plug in the MxChip into your PC.
1. Download the latest [firmware](https://github.com/microsoft/devkit-sdk/releases/download/2.0.0/devkit-firmware-2.0.0.bin).
1. Drag & drop the `.bin` file you downloaded to `AZ3166` device.
1. Download and install the latest Arduino IDE.
1. Under File -> Preferences -> Settings -> Additional boards manager URLs add the following item:

    * `https://raw.githubusercontent.com/VSChina/azureiotdevkit_tools/master/package_azureboard_index.json`
  
    Note, at the time of this writing, the file is missing checksum data for the supporting 'tools' section of the json and you will get a failure trying to install everything when you add the AZ3166 board.  If that happens, I created a duplicate json that has the data.

    * `https://raw.githubusercontent.com/howardginsburg/iot-device-kit/main/mxchip/package_azureboard_index.json`

1. Under Tools -> Board -> Board Manager

    * Search for `mxchip` and install 2.0.0

1. Open the MXChipTest.ino file in this repo and upload it to the board.

Note: Do not use the IoT Hub SDK that's provided with the board packaging.  Instead, install the latest IoT Hub SDK for Arduino from Microsoft.

## Learnings

- If you're looking for RTOS solutions, Azure RTOS is a fantastic choice.  As you explore the code, you'll very quickly see that the different components of the framework integrate together very nicely.
- The threading model is shown off by being able to work the buttons and not have to interrupt telemetry being sent.
- You unfortunately have to hardcode the connection information into the code.  It would be great to see this evolve into something similar to have the Wio Terminal sample does this.

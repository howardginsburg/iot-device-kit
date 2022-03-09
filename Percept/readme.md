# Azure Percept Development Kit

## Overview

The [Percept DK](https://docs.microsoft.com/azure/azure-percept/overview-azure-percept-dk) provides a development friendly experience for performing AI at the edge.  It can do both video and audio inferencing.  The vision module contains a Intel Movidius Myriad X (MA2085) vision processing unit (VPU).  It's highly flexible since the carrier board runs IoT Edge.

## Deployment

I used the people counting [sample](https://docs.microsoft.comazure/azure-percept/create-people-counting-solution-with-azure-percept-devkit-vision) as the foundation for building this out.  Currently, the Percept DK has no direct integration with IoT Central, only IoT Hub.  My workaround for this is to use an Azure Function to serve as a bridge from IoT Hub to IoT Central.  For real world implementations, see this [reference implementation](https://docs.microsoft.com/azure/iot-central/core/howto-build-iotc-device-bridge).

### Percept DK

1. Complete the [unboxing tutorial](https://docs.microsoft.com/azure/azure-percept/quickstart-percept-dk-unboxing).
2. Complete the [setup tutorial](https://docs.microsoft.com/azure/azure-percept/quickstart-percept-dk-set-up).
3. Complete the [update tutorial](https://docs.microsoft.com/azure/azure-percept/how-to-update-over-the-air) to upgrade your Percept DK to the latest OS and firmware.
4. Complete Steps 0-3 of the [sample](https://docs.microsoft.com/azure/azure-percept/create-people-counting-solution-with-azure-percept-devkit-vision) instructions to deploy the IoT Edge configuration to your Percept DK.

### IoT Central

1. Create a new Device Template named 'Percept'.
2. Import the model 'Percept.json'.
3. Create a new device and assign it to the Percept template.
4. Make note of the Device ID, Device Key, and Scope ID.

### IoT Hub to IoT Central Function

1. Create a new Azure Function instance using a Consumption Plan.
2. Add the following values to the Configuration of the Function.

    - DEVICE_ID = "Your device value"
    - DEVICE_KEY = "Your device key"
    - DEVICE_SCOPE = "Your device scope"

3. Open the function code in VS Code and deploy it.
4. Open the Integration blade for your function in Azure.
5. Click on the trigger and create a new Event Grid subscription for the function pointing it to the IoT Hub your Percept DK is connected to and selecting just the Telemetry data points.

## Learnings

- Going through the over-the-air update showcases how the Azure Device Update service works.
- For a production workload, any kind of "Bridge" capability should leverage a cache to get the connection information once retrieved.  This demo will get throttled from time to time since it triggers every second.  Consumption plans for functions do not do any kind of caching.
- Connecting the Percept DK to IoT Central was a cool experiment, but really not feasible at the moment.  The lack of support for the Device Provisioning Service, Azure Device Update, and an easy way to manipulate IoT Edge deployments is limiting.  Working through those obstacles was definitely a good learning experience.
- Outside of this experiment, the Percept DK was fun to play with and work with Custom Vision and Speech integrations within Azure.

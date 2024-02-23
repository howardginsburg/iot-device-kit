# Azure Percept Development Kit

## Overview

<b>Update:</b> The Percept DK has been retired, so I've updated the instructions for how to apply the latest firmware and IoT Edge deployment to it.  After applying these changes, the Percept Studio experience in the Azure Portal will no longer recognize the device.  The device will still be able to run IoT Edge modules and connect to IoT Hub.

The [Percept DK](https://docs.microsoft.com/azure/azure-percept/overview-azure-percept-dk) provides a development friendly experience for performing AI at the edge.  It can do both video and audio inferencing.  The vision module contains a Intel Movidius Myriad X (MA2085) vision processing unit (VPU).  It's highly flexible since the carrier board runs IoT Edge.  There is no direct integration with IoT Central, so I used an Azure Function to bridge the gap.

## Percept DK Setup

These instructions will get your Percept DK setup, update to the final firmware that unlocks the SOM, and deploys the appropriate IoT Edge configuration.

1. Complete the [unboxing tutorial](https://docs.microsoft.com/azure/azure-percept/quickstart-percept-dk-unboxing).
2. Complete the [setup tutorial](https://docs.microsoft.com/azure/azure-percept/quickstart-percept-dk-set-up).
3. Complete the [update tutorial](https://aka.ms/audio_vision_som_update) to upgrade your Percept DK to the latest (and final) OS and firmware.
4. [Deploy](https://learn.microsoft.com/azure/iot-edge/how-to-deploy-modules-vscode?view=iotedge-1.4) the updated [IoT Edge configuration](/PerceptDeployment/PerceptSolution/deployment.amd64.json) to your Percept DK.  This is the default deployment that also contains updates to use IoT Edge 1.4 and also the proper Eye and Speech tags.

## Deployment

Now that the Percept Studio experience has been deprecated, there are several key repos that are important to reference for working with the device.

[Percept Advanced Development](https://github.com/microsoft/azure-percept-advanced-development) - This repo contains some interesting ML notebooks, and also the source code for the Azure Eye Module.
[Percept Reference Solutions](https://github.com/microsoft/Azure-Percept-Reference-Solutions) - Several reference solutions to explore, including the people counting example we'll use here.

I used the people counting [sample](https://docs.microsoft.com/azure/azure-percept/create-people-counting-solution-with-azure-percept-devkit-vision) as the foundation for building this out.  The Percept DK has no direct integration with IoT Central, only IoT Hub.  My workaround for this is to use an Azure Function to serve as a bridge from IoT Hub to IoT Central.  For real world implementations, see this [reference implementation](https://docs.microsoft.com/azure/iot-central/core/howto-build-iotc-device-bridge).

### Percept DK

1. Complete Steps 0-3 of the [sample](https://docs.microsoft.com/azure/azure-percept/create-people-counting-solution-with-azure-percept-devkit-vision) instructions to deploy the IoT Edge configuration to your Percept DK.  Make sure to use the [deployment.template.json](/PerceptDeployment/PerceptSolution/deployment.template.json) file as the basis for the deployment so you retain the latest Percept updates.

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

## Helpers

### Webstream Viewer

http://'PerceptIP Address':3000

### RTSP Stream

Raw: rtsp://'PerceptIP Address':8554/rawTCP
Result: rtsp://'PerceptIP Address':8554/resultTCP

## Learnings

- Going through the over-the-air update showcases how the Azure Device Update service works.
- For a production workload, any kind of "Bridge" capability should leverage a cache to get the connection information once retrieved.  This demo will get throttled from time to time since it triggers every second.  Consumption plans for functions do not do any kind of caching.
- Connecting the Percept DK to IoT Central was a cool experiment, but really not feasible at the moment.  The lack of support for the Device Provisioning Service, Azure Device Update, and an easy way to manipulate IoT Edge deployments is limiting.  Working through those obstacles was definitely a good learning experience.
- Outside of this experiment, the Percept DK was fun to play with and work with Custom Vision and Speech integrations within Azure.

# Overview

This Iot Edge Solution leverages a Sense Hat on a Raspberry Pi to send data to IoT Hub/Central.  There are a few things to explore with this sample:

1. DotNet 6
   - The project and subsequent docker container have been upgraded from netcoreapp3.1 to net6.0 to take advantage of all the latest features of .Net.
2. Docker
   - The docker file builds against the AMD64 so I can build on my laptop.  Then it switches over to the AMR32 runtime container.
   - The "runuser" section of the docker file is commented out so it can run as root and access the sensehat.
3. Edge
   - The create options of the SenseHatModule set privileged to true so that the container can access the sensehat.
4. SenseHatModule
   - The dotnet api is very robust and works perfectly with all the features of the SenseHat.

## Deployment

### IoT Edge install on Raspberry Pi Bullseye

Follow the [documentation/tutorials](https://docs.microsoft.com/azure/iot-edge/how-to-provision-single-device-linux-symmetric?view=iotedge-2020-11&tabs=azure-portal) on how to deploy to a Raspberry Pi.  

At the time of this writing, there is an outstanding [issue](https://github.com/Azure/iotedge/issues/5812) with running IoT Edge 1.2 on the Raspberry Pi Bullseye OS.  To fix it, you must take the following steps:

- sudo apt install cgroupfs-mount -y
- sudo iotedge system stop
- sudo docker rm -f edgeAgent
- sudo systemctl stop docker
- sudo cgroupfs-mount
- sudo systemctl restart docker
- sudo rm -rf /var/run/iotedge/mgmt.sock
- sudo rm -rf /var/run/iotedge/workload.sock
- sudo iotedge system restart
- Edit /boot/cmdline.txt and add **cgroup_enable=1 cgroup_enable=memory cgroup_memory=1** to the end of the file.

### Deploying to IoT Central

1. Generate a deployment manifest in VS Code.
2. In IoT Central, create a new device template and select Azure IoT Edge.
3. Give your template a name, and upload the deployment manifest file you just created.
4. In the template editor, click on the "Module SenseHatModule" label, then select "Add inerited interface".
5. Select "Import interface" and select the [telemetry](TelemetryInterface.json) file and save it.
6. Create any views you are interested in.
7. Publish the template.

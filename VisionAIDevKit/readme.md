# Vision AI Dev Kit

## Overview

The [Vision AI DevKit](https://azure.github.io/Vision-AI-DevKit-Pages/) is meant to be an all in one IoT Edge device with a built in camera.  At this time, it does not appear to supported anymore, and firmware updates haven't been published since 2020 which also locks the device at IoTEdge 1.0 and 1.1.  You could look to do something outside of this that still published a model onto the device as it does not require IoTEdge to do inferencing.  However, in this project, we'll leverage the device just to turn it into a simple RTSP camera.  The challenge is that whenever the device resets, it will turn off the RTSP feed.  So we'll need to create a docker container that runs on the device to turn it back on.

## Factory Reset

### Option 1

1. If you've gone through the setup process in the past, you can initiate a full reset by ssh into the camera and running the following command:
`sudo su`
`mv /etc/iotedge/config.yaml /etc/iotedge/config.yaml.bak`
1. Then hold the power button in for 12 seconds until the lights turn off.  When it reboots, the lights will blink red, all docker images will be erased, and the device will be ready for setup.

### Option 2

1. Follow the instructions to do a [firmware update](https://azure.github.io/Vision-AI-DevKit-Pages/docs/Firmware/).

## Setup

1. Install the latest [firmware](https://azure.github.io/Vision-AI-DevKit-Pages/docs/Firmware/) and then follow the [tutorial](https://azure.github.io/Vision-AI-DevKit-Pages/docs/Get_Started/) to get the device connected to your network.  STOP after this step.  We do not need to connect to IotHub.  At this point, the lights on the front of the camera should be blinking green to show we're connected to wifi.
1. ssh into the device using the IP address and the credentials you created during setup.
1. Run the following commands to copy Dockerfile and rtspenabler.py to the device and build the docker container locally.  Alternatively, you could publish them to a docker repo and pull them down.
    - `sudo su`
    - `mkdir /etc/rtspenabler`
    - `cd /etc/rtspenabler`
    - `vi <filename>`
    - `press 'i' to enter insert mode`
    - `press Ctrl+v`
    - `press Esc`
    - `press ':'`
    - `press 'wq'`
    - `docker build -t rtspenabler .`
    - `docker run -d --net=host --restart unless-stopped --log-opt max-size=1m --log-opt max-file=3 --log-driver=json-file --name rtspenabler rtspenabler`
    - `docker logs rtspenabler`
1. Open an RTSP client and connect to `rtsp://<ip address>:8900`

## Camera Settings
1. You can log into the camera at `http://<ip address>:1080`as admin/admin.  Here you view/change resolution settings, enable onvif, etc.  If this is something you want to keep, consider updating the python file to make these changes for you.
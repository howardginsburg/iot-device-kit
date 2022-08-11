# UP Squared AI Vision X Developer Kit

## Overview

The [UP Squared Vision AI Vision X Developer Kit](https://up-board.org/upkits/up-squared-ai-vision-kit/) runs an Intel Atom X7 processor and a Intel Movidius Myriad X VPU for scoring machine learning models at the edge.  It can run various flavors of Linux and Windows plus Azure IoT Edge.

## Setup

The instructions provided by UP are dated, and getting everything you need to get OpenVino running is made tricky.  After doing a trials of installing, I created these instructions, and copied a few of the setup scripts to make things as streamlined as possible.

- The Wiki for the Up community can be found at https://github.com/up-board/up-community/wiki
- Full instructions for hardware setup can be found at https://github.com/up-board/up-community/wiki/Ubuntu_20.04.  Note, there is currently no kernel for Ubuntu 22.04.
- Full instructions for OpenVino Development Tools setup can be found at https://docs.openvino.ai/latest/openvino_docs_install_guides_install_dev_tools.html

### Ubuntu Installation

1. Download the [Ubuntu 20.04 Desktop](https://releases.ubuntu.com/20.04.4/ubuntu-20.04.4-desktop-amd64.iso) ISO, burn it to a thumbdrive, and perform a minimal installation.
2. Run latest updates
    - `sudo apt update`
    - `sudo apt upgrade`
3. Install the UP kernel
    - `sudo add-apt-repository ppa:up-division/5.4-upboard`
    - `sudo apt update`
    - `sudo apt-get autoremove --purge 'linux-.*generic'` (Select No when prompted to Abort kernel removal)
    - `sudo apt-get install linux-generic-hwe-18.04-5.4-upboard`
    - `sudo apt dist-upgrade -y`
    - `sudo update-grub`
    - `sudo reboot`
4. Install optional packages
    - SSH
        - `sudo apt install openssh-server`
    - Edge Broswer
        - Download and install [Microsoft Edge Browser](https://www.microsoft.com/edge) and make it your default browser.

## Exploration of OpenVINO

Running the latest version of OpenVINO with a VPU created challenges, and since the end state is to have everything running as docker containers running within IoT Edge, we'll start there.  I did explore [local installation](./openvino%20install.md).

### Docker Setup

Source for the docker containers and instructions on how to build your own are on [GitHub](https://github.com/openvinotoolkit/docker_ci).  Pre-built images can be found on [DockerHub](https://hub.docker.com/r/openvino/ubuntu20_dev)

1. Install the Microsoft [Moby](https://docs.microsoft.com/en-us/azure/iot-edge/how-to-provision-single-device-linux-symmetric?view=iotedge-2020-11&tabs=azure-portal%2Cubuntu#install-iot-edge) container engine.
    - `wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb`
    - `sudo dpkg -i packages-microsoft-prod.deb`
    - `rm packages-microsoft-prod.deb`
    - `sudo apt-get update; sudo apt-get install moby-engine`

### Run the Ubuntu 20 container

1. Launch the docker container.
    - `sudo docker run -it --device /dev/dri:/dev/dri --user 0 --device-cgroup-rule='c 189:* rmw' -v /dev/bus/usb:/dev/bus/usb -v /openvino_env:/openvino_env --rm openvino/ubuntu20_dev:latest`
    - Notes:
        - Workspace for persisting models and other data.
            - -v /openvino_env:/openvino_env
        - GPU - allow access to the GPU and run as root.
            - --device /dev/dri:/dev/dri --user 0
        - VPU - pass in the rules and usb so that the MYRIAD is found.
            - --device-cgroup-rule='c 189:* rmw' -v /dev/bus/usb:/dev/bus/usb

### Testing the [Benchmark Python Tool](https://docs.openvino.ai/latest/openvino_inference_engine_tools_benchmark_tool_README.html)

1. While running the docker container, create initial folders for holding models and images
    - `mkdir /openvino_env/models`
    - `mkdir /openvino_env/ir`
    - `mkdir /openvino_env/images`
2. Download the googlenet-v1 model
    - `omz_downloader --name googlenet-v1 -o /openvino_env/models`
3. Run the model optimizer
    - `mo --input_model /openvino_env/models/public/googlenet-v1/googlenet-v1.caffemodel --data_type FP32 --output_dir /openvino_env/ir`
4. Download a sample image to test with.
    - `curl https://storage.openvinotoolkit.org/data/test_data/images/224x224/dog.bmp --output /openvino_env/images/dog.bmp`
5. Test against CPU
    - `benchmark_app -m /openvino_env/ir/googlenet-v1.xml -d CPU -api async -i /openvino_env/images/dog.bmp -progress -b 1`
6. Test against GPU
    - `benchmark_app -m /openvino_env/ir/googlenet-v1.xml -d GPU -api async -i /openvino_env/images/dog.bmp -progress -b 1`
7. Test against VPU/Myriad
    - `benchmark_app -m /openvino_env/ir/googlenet-v1.xml -d MYRIAD -api async -i /openvino_env/images/dog.bmp -progress -b 1`

## IoT Edge Configuration

TBD - note there is a sample that leverages an ONNX conversation in the [Azure Samples](https://github.com/Azure-Samples/onnxruntime-iot-edge/blob/master/README-ONNXRUNTIME-OpenVINO.md).

## Deploying to IoT Central

TBD

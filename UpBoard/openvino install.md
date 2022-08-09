# OpenVINO Local Installation

Running the latest version of OpenVINO with a VPU is tricky as you have to install the runtime first, and then the dev tools second.  Previous versions have an offline installer that takes care of everything for you.  I was unable to get the VPU to run without error and opened a [support ticket](https://github.com/openvinotoolkit/openvino/issues/12429).  Some of the feedback was helpful, but I never did get it working.  I recommend the Docker container, especially if you intend to use IoT Edge for your models.

1. Install the OpenVINO Runtime (this will give us what we need to get the GPU and VPU working)
    - [Download](https://www.intel.com/content/www/us/en/developer/tools/openvino-toolkit/download.html) the latest OpenVINO Runtime Offline Installer.
    - `chmod +x l_openvino_toolkit_p_<version>.sh`
    - `sudo ./l_openvino_toolkit_p_<version>.sh`
    - `sudo -E /opt/intel/openvino_2022/install_dependencies/install_openvino_dependencies.sh`
    - `source /opt/intel/openvino_2022/setupvars.sh`
    - `sudo -E /opt/intel/openvino_2022/extras/scripts/download_opencv.sh`
    - `sudo -E /opt/intel/openvino_2022/install_dependencies/install_NEO_OCL_driver.sh`
    - `sudo -E /opt/intel/openvino_2022/install_dependencies/install_NCS_udev_rules.sh`
2. Install OpenVINO Development Tools (Note, this all installs the runtime)
    - `python3 -m venv openvino_env`
    - `source openvino_env/bin/activate`
    - `python -m pip install --upgrade pip`
    - `pip install openvino-dev[caffe,kaldi,mxnet,onnx,pytorch,tensorflow,tensorflow2]`
    - `sudo reboot`
3. Test the install by running the Model Optimizer
    - `source /opt/intel/openvino_2022/setupvars.sh`
    - `source openvino_env/bin/activate`
    - `mo -h`
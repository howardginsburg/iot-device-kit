# MxChip

## Overview

The [MxChip](https://microsoft.github.io/azure-iot-developer-kit/) contains an STM32F412 ARM Cortex M4F MCU and there is an Azure RTOS configuration available that will run on it.

## Deployment

Follow the [tutorial](https://docs.microsoft.com/azure/iot-develop/quickstart-devkit-mxchip-az3166) to get things going quickly.

## Learnings

- If you're looking for RTOS solutions, Azure RTOS is a fantastic choice.  As you explore the code, you'll very quickly see that the different components of the framework integrate together very nicely.
- The threading model is shown off by being able to work the buttons and not have to interrupt telemetry being sent.
- You unfortunately have to hardcode the connection information into the code.  It would be great to see this evolve into something similar to have the Wio Terminal sample does this.

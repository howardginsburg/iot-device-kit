# Avnet MT3620 Azure Sphere (Rev 1)

## Overview

The Avnet board allows us to test out [Azure Sphere](https://docs.microsoft.com/azure-sphere/).  In this demo, I tried out deploying a high level application.  An alternative would be to deploy a realtime application.  The Avnet board has a bunch of sensors built into it and some good samples to deploy to take advantage of.

## Deployment

Getting everything setup was a 3 step process.
1. [Sphere tenant creation and claiming the dev kit](https://community.element14.com/products/devtools/avnetboardscommunity/azure-sphere-starter-kits/b/blog/posts/avnet-s-azure-sphere-starter-kit-out-of-box-demo-part-1-of-3)
2. [Deployment of IoT Central and Dev Kit configuration](https://community.element14.com/products/devtools/avnetboardscommunity/azure-sphere-starter-kits/b/blog/posts/avnet-s-azure-sphere-starter-kit-out-of-box-demo-part-3-of-3)
3. [Creating a Sphere Deployment](https://docs.microsoft.com/azure-sphere/install/qs-first-deployment?tabs=cliv2beta)
    - By creating a deployment, the sphere board can run standalone outside of Visual Studio.
    - Create a product
        - azsphere product create --name Avnet --description "Avnet IoTCentral Demo"
    - Enable the board to cloud test the product.  This automatically places the device into the Field Test Group.
        - azsphere device enable-cloud-test --product Avnet 
    - Navigate to the Samples\AvnetAzureSphereHacksterTTC\HighLevelExampleApp\out\ARM-Debug directory and upload the image.  Make sure to retain the image id that is part of the output.
        - azsphere image add --image AvnetReferenceDesign.imagepackage --temporary
    - Get the device group id for Field Test
        - azsphere device-group list
    - Deploy the image to the device.
        - azsphere device-group deployment create --device-group {Field Test Group ID} --images {Image ID}

## Learnings

- The Azure Sphere chip provides a tremendous amount of security for real world deployments.  With the foundation of implementing the [7 Properties of Highly Secured Devices](https://www.microsoft.com/research/uploads/prod/2020/11/Seven-Properties-of-Highly-Secured-Devices-2nd-Edition-R1.pdf), we find that we can control device connectivity to specific endpoints, and also what the device can connect to regarding peripherals such as gpio, uart, etc (crack open the app_manifest.json file in the sample to see this!).
- Having deployments as packages, it's very easy to review and monitor what an instance of a device has, and manage rollouts.
- Device certificate management and automatic renewal ensures that the device is always secure.
    - One downside is that right now you have to manage the Root CA within IotHub/Central so this is something to watch out for as the Root CA reaches expiration.

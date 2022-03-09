// Default URL for triggering event grid function in the local environment.
// http://localhost:7071/runtime/webhooks/EventGrid?functionName={functionname}
using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.EventGrid.Models;
using Microsoft.Azure.WebJobs.Extensions.EventGrid;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Provisioning.Client.Transport;
using Microsoft.Azure.Devices.Shared;
using Microsoft.Azure.Devices.Provisioning.Client;
using System.Text;
using System.Threading.Tasks;

namespace Percept.Demo
{
    public class HubToCentral
    {
        [FunctionName("HubToCentral")]
        public async Task Run([EventGridTrigger]EventGridEvent eventGridEvent, ILogger log)
        {
            log.LogInformation(eventGridEvent.Data.ToString());
            // Reading deviceId and temperature for IoT Hub JSON
            JObject deviceMessage = (JObject)JsonConvert.DeserializeObject(eventGridEvent.Data.ToString());
            string deviceId = (string)deviceMessage["systemProperties"]["iothub-connection-device-id"];
            int peopleCount = (int)deviceMessage["body"]["People_Count"];

            TelemetryMessage telemetryMessage = new TelemetryMessage();
            telemetryMessage.PeopleCount = peopleCount;

            var security = new SecurityProviderSymmetricKey(
                Environment.GetEnvironmentVariable("DEVICE_ID"),
                Environment.GetEnvironmentVariable("DEVICE_KEY"),
                null);

            var transportHandler = new ProvisioningTransportHandlerMqtt();

            ProvisioningDeviceClient provClient = ProvisioningDeviceClient.Create(
                "global.azure-devices-provisioning.net",
                Environment.GetEnvironmentVariable("DEVICE_SCOPE"),
                security,
                transportHandler);

            log.LogInformation($"Initialized for registration Id {security.GetRegistrationID()}.");

            log.LogInformation("Registering with the device provisioning service...");
            DeviceRegistrationResult result = await provClient.RegisterAsync();

            log.LogInformation($"Registration status: {result.Status}.");
            if (result.Status != ProvisioningRegistrationStatusType.Assigned)
            {
                log.LogInformation($"Registration status did not assign a hub");
                return;
            }

            log.LogInformation($"Device {result.DeviceId} registered to {result.AssignedHub}.");

            log.LogInformation("Creating symmetric key authentication for IoT Hub...");
            IAuthenticationMethod auth = new DeviceAuthenticationWithRegistrySymmetricKey(
                result.DeviceId,
                security.GetPrimaryKey());

            log.LogInformation($"Connecting to IoT Hub");
            using DeviceClient iotClient = DeviceClient.Create(result.AssignedHub, auth, TransportType.Mqtt);

            log.LogInformation("Sending a telemetry message...");
            string jsonString = JsonConvert.SerializeObject(telemetryMessage);
            var message = new Message(Encoding.UTF8.GetBytes(jsonString));
            await iotClient.SendEventAsync(message);

            await iotClient.CloseAsync();

            
        }
    }

    public class TelemetryMessage
    {
        public int PeopleCount { get; set; }
    }
}

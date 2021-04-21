using Microsoft.Azure.EventHubs;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartCafe.EventHub.Receivers.Direct
{
    class Program
    {
        const string eventHubConnectionString = "Endpoint=sb://smartcafeeh-ns.servicebus.windows.net/;SharedAccessKeyName=SendAndListenPolicy;SharedAccessKey=EMZmvA3HQzK7vSltYr5MWG2DQ5ZNbIYibW5mFduanpY=;EntityPath=smartcafeeh";
        static async Task Main(string[] args)
        {
            Console.WriteLine("Connecting to the Event Hub...");
            var eventHubClient =
                EventHubClient.CreateFromConnectionString(eventHubConnectionString);

            //var partitionReceiver = eventHubClient.CreateReceiver("$Default", "0", DateTime.Now);

            var runtimeInformation = await eventHubClient.GetRuntimeInformationAsync();
            var partitionReceivers = runtimeInformation.PartitionIds.Select(
                    partitionId => eventHubClient.CreateReceiver("smartcafe_console_direct", partitionId, DateTime.Now)
                ).ToList();

            Console.WriteLine("Waiting for incoming events...");

            foreach(var partitionReceiver in partitionReceivers)
            {
                partitionReceiver.SetReceiveHandler(
                    new SmartCafePartitionReceiveHandler(partitionReceiver.PartitionId));
            }

            Console.WriteLine("Press any key to shutdown");
            Console.ReadLine();
            await eventHubClient.CloseAsync();

        }
    }
}

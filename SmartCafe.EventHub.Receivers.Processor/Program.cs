using Microsoft.Azure.EventHubs.Processor;
using System;
using System.Threading.Tasks;

namespace SmartCafe.EventHub.Receivers.Processor
{
    class Program
    {

        const string eventHubPath = "smartcafeeh_2partitions";// "smartcafeeh";
        const string consumerGroupName = "smartcafe_console_console_processor";
        const string eventHubConnectionString = "Endpoint=sb://smartcafeeh-ns.servicebus.windows.net/;SharedAccessKeyName=SendAndListenPolicy;SharedAccessKey=2/N/0NRpEKNDGJQRE4W+Jmf6e6LoHxyn90zjxaSMKFo=;EntityPath=smartcafeeh_2partitions";//"Endpoint=sb://smartcafeeh-ns.servicebus.windows.net/;SharedAccessKeyName=SendAndListenPolicy;SharedAccessKey=EMZmvA3HQzK7vSltYr5MWG2DQ5ZNbIYibW5mFduanpY=;EntityPath=smartcafeeh";
        const string storageConnectionString = "DefaultEndpointsProtocol=https;AccountName=smartcafestoragemc;AccountKey=l13ugGKLY3Vnp4v3LKQO0z1NSO8LvuhB8MJjDYt2W/5lOUhwrd3/V0blscEBev7pTcLQG+dhu8XWmVNvukqj3Q==;EndpointSuffix=core.windows.net";
        const string leaseContainerName = "processcheckpointmc";
        
        static void Main(string[] args)
        {
            MainAsync().Wait();
        }

        private static async Task MainAsync()
        {
            Console.WriteLine($"Register the {nameof(SmartCafeEventProcessor)}");

            var eventProcessorHost = new EventProcessorHost(
                eventHubPath,
                consumerGroupName,
                eventHubConnectionString,
                storageConnectionString,
                leaseContainerName);

            await eventProcessorHost.RegisterEventProcessorAsync < SmartCafeEventProcessor>();

            Console.WriteLine("Waiting for incoming events...");
            Console.WriteLine("Press any key to shutdown");
            Console.ReadLine();

            await eventProcessorHost.UnregisterEventProcessorAsync();
        }
    }
}

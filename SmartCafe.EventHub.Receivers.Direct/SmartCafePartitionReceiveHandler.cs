using Microsoft.Azure.EventHubs;
using Newtonsoft.Json;
using SmartCafe.EventHub.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartCafe.EventHub.Receivers.Direct
{
    public class SmartCafePartitionReceiveHandler : IPartitionReceiveHandler
    {

        public SmartCafePartitionReceiveHandler(string partitionId)
        {
            PartitionId = partitionId;
        }
        public int MaxBatchSize => 10;

        public string PartitionId { get; }

        public Task ProcessErrorAsync(Exception error)
        {
            Console.WriteLine($"Exception: {error.Message}");
            return Task.CompletedTask;
        }

        public Task ProcessEventsAsync(IEnumerable<EventData> eventDatas)
        {
            if (eventDatas != null)
            {
                foreach (var eventData in eventDatas)
                {
                    var dataAsJson = Encoding.UTF8.GetString(eventData.Body.Array);
                    var coffeeMachineData =
                        JsonConvert.DeserializeObject<CoffeeMachineData>(dataAsJson);
                    Console.WriteLine($"{coffeeMachineData} | Partition Id: {PartitionId}" +
                        $" | Offset: {eventData.SystemProperties.Offset}");
                }
            }
            return Task.CompletedTask;
        }
    }
}

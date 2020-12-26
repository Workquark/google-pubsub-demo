using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jaafan.Proxy
{
    public class PubSub : IPubSub
    {
        
        public async Task PublishMessageWithCustomAttributesAsync(string projectId, string topicId, string messageText)
        {
            TopicName topicName = TopicName.FromProjectTopic(projectId, topicId);
            PublisherClient publisher = await PublisherClient.CreateAsync(topicName);

            var pubsubMessage = new PubsubMessage
            {
                // The data is any arbitrary ByteString. Here, we're using text.
                Data = ByteString.CopyFromUtf8(messageText),
                // The attributes provide metadata in a string-to-string dictionary.
                Attributes =
                {
                    { "year", "2020" },
                    { "author", "unknown" }
                }
            };
            string message = await publisher.PublishAsync(pubsubMessage);
            Console.WriteLine($"Published message {message}");
        }
    }
}

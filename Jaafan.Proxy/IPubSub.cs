using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jaafan.Proxy
{
    public interface IPubSub
    {
        Task PublishMessageWithCustomAttributesAsync(string projectId, string topicId, string message);
    }
}

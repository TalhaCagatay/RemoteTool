using System;
using Samples.Core;

namespace Samples.Remote.Interface
{
    public interface IRemoteController : IController
    {
        event Action<string> RemoteUpdated;
    }
}
using System;

namespace Samples.Core
{
    public interface IController
    {
        event Action Initialized;
        bool IsInitialized { get; }
        void Init();
    }
}
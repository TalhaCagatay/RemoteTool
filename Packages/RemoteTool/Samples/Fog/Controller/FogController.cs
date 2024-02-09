using System;
using System.Linq;
using Madbox;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Samples.Bridge;
using Samples.Fog.Interface;
using Samples.Remote.DTO;
using Samples.Remote.Interface;
using UnityEngine;
using RemoteController = Samples.Remote.Controller.RemoteController;

namespace Samples.Fog.Controller
{
    public class FogController : IFogController
    {
        public event Action Initialized;
        public bool IsInitialized { get; private set; }
        public void Init()
        {
            var remoteController = ControllerHandler.GetController<IRemoteController>();
            remoteController.RemoteUpdated += remote =>
            {
                UpdateFogSettings();
            };
            
            remoteController.Initialized += () =>
            {
                UpdateFogSettings();
                Debug.Log($"<color=green>{GetType().Name} Initialized</color>");
                IsInitialized = true;
                Initialized?.Invoke();
            };
        }

        private static void UpdateFogSettings()
        {
            var fogEnabledConfig = RemoteController.MadboxSheet.FogConfig.FirstOrDefault(c => c.key.Equals("fogEnabled", StringComparison.InvariantCultureIgnoreCase));
            var fogDensityConfig = RemoteController.MadboxSheet.FogConfig.FirstOrDefault(c => c.key.Equals("fogDensity", StringComparison.InvariantCultureIgnoreCase));

            RenderSettings.fog = fogEnabledConfig?.BoolValue() ?? false;
            RenderSettings.fogDensity = fogDensityConfig?.FloatValue() ?? 0f;
        }
    }
}
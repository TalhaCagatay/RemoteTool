using System;
using System.Linq;
using Samples.Bridge;
using Samples.Remote.Controller;
using Samples.Remote.Interface;
using UnityEngine;

namespace Samples.Player.Config
{
    [CreateAssetMenu(fileName = "RunningMovementConfig", menuName = "Madbox/Movement/Running", order = 0)]
    public class RunningMovementConfig : MovementConfig
    {
        
    }

    public class RunningMovementConfigLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void FetchRemoteValuesToConfig()
        {
            ControllerHandler.GetController<IRemoteController>().RemoteUpdated += s =>
            {
                ConfigHandler.GetMovementConfig<RunningMovementConfig>().Speed = RemoteController.MadboxSheet
                    .MovementConfig.First(c => c.type.Equals("run", StringComparison.InvariantCultureIgnoreCase)).speed;
            };
        }
    }
}
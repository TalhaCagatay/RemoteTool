using System;
using System.Linq;
using Samples.Bridge;
using Samples.Remote.Controller;
using Samples.Remote.Interface;
using UnityEngine;

namespace Samples.Player.Config
{
    [CreateAssetMenu(fileName = "FlyMovementConfig", menuName = "Madbox/Movement/Fly", order = 0)]
    public class FlyMovementConfig : MovementConfig
    {
        
    }
    
    public class FlyMovementConfigLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void FetchRemoteValuesToConfig()
        {
            ControllerHandler.GetController<IRemoteController>().RemoteUpdated += s =>
            {
                ConfigHandler.GetMovementConfig<FlyMovementConfig>().Speed = RemoteController.MadboxSheet
                    .MovementConfig.First(c => c.type.Equals("fly", StringComparison.InvariantCultureIgnoreCase)).speed;
            };
        }
    }
}
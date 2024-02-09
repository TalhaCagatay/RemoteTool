using System;
using System.Linq;
using Samples.Bridge;
using Samples.Remote.Controller;
using Samples.Remote.Interface;
using UnityEngine;

namespace Samples.Player.Config
{
    [CreateAssetMenu(fileName = "ClimbMovementConfig", menuName = "Madbox/Movement/Climb", order = 0)]
    public class ClimbMovementConfig : MovementConfig
    {
        
    }
    
    public class ClimbMovementConfigLoader
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void FetchRemoteValuesToConfig()
        {
            ControllerHandler.GetController<IRemoteController>().RemoteUpdated += s =>
            {
                var climbMovementConfig = RemoteController.MadboxSheet.MovementConfig
                    .FirstOrDefault(c => c.type.Equals("Climb", StringComparison.InvariantCultureIgnoreCase));

                ConfigHandler.GetMovementConfig<ClimbMovementConfig>().Speed =
                    climbMovementConfig?.speed ?? 3;
            };
        }
    }
}
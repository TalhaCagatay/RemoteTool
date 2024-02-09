using System;
using System.Collections.Generic;
using System.Linq;
using Samples.Core;
using Samples.Helpers;
using Samples.Player.Config;
using UnityEngine;

namespace Samples.Bridge
{
    [DefaultExecutionOrder(-1)]
    public class MonoBridge : MonoBehaviour
    {
        private void Awake() => Application.targetFrameRate = 60;
    }
    
    public class ControllerHandler
    {
        private static List<IController> _controllers = new List<IController>();
        private static Dictionary<IController, Type> _controllersDictionary = new Dictionary<IController, Type>();

        static ControllerHandler()
        {
            var monoControllers = Creator.GetMonoInstances<IController>();
            var nonMonoControllers = Creator.CreateInstancesOfType<IController>(typeof(MonoBehaviour));
            _controllers.AddRange(nonMonoControllers);
            _controllers.AddRange(monoControllers);
            _controllers.ForEach(c => c.Init());
            CreateControllerDictionary();
            foreach (var (key, value) in _controllersDictionary)
                Debug.Log($"{key}:{value}");
        }
        
        private static void CreateControllerDictionary() => _controllersDictionary = _controllers.ToDictionary(controller => controller, controller => controller.GetType());
        
        public static T GetController<T>() where T : IController
        {
            foreach (var controller in _controllers)
            {
                if (controller is T typedController)
                {
                    return typedController;
                }
            }

            return default;
        }
    }

    public static class ConfigHandler
    {
        private static MovementConfig[] _movementConfigs;
        
        static ConfigHandler() => _movementConfigs = Resources.LoadAll<MovementConfig>("Movement");

        public static T GetMovementConfig<T>() where T : MovementConfig
        {
            foreach (var movementConfig in _movementConfigs)
            {
                if (movementConfig is T config)
                {
                    return config;
                }
            }

            return default;
        }
    }
}
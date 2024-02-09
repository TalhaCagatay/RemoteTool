using System;
using Samples.Bridge;
using Samples.Player.Interface;
using Samples.Remote.Interface;
using UnityEngine;

namespace Samples.Player.Controller
{
    public class PlayerController : MonoBehaviour, IPlayerController
    {
        public event Action Initialized;
        public bool IsInitialized { get; private set; }

        private Vector3 _initialPosition;
        
        public void Init()
        {
            _initialPosition = transform.position;
            var remoteController = ControllerHandler.GetController<IRemoteController>();
            remoteController.Initialized += () =>
            {
                Debug.Log($"<color=green>{GetType().Name} Initialized</color>");
                IsInitialized = true;
                Initialized?.Invoke();
            };
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Finish")) return;

            transform.position = _initialPosition;
        }
    }
}
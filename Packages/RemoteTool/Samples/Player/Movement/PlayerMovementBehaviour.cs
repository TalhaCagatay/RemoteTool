using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Samples.Bridge;
using Samples.Helpers;
using Samples.Player.Interface;
using Samples.Player.Movement.Interface;
using UnityEngine;

namespace Samples.Player
{
    public class PlayerMovementBehaviour : MonoBehaviour
    {
        public IMovement[] Movements { get; private set; }
        private IMovement _currentMovement;

        private bool _readyForMovement;
        
        private IEnumerator Start()
        {
            var playerController = ControllerHandler.GetController<IPlayerController>();
            Movements = Creator.CreateInstancesOfType<IMovement>().ToArray();
            foreach (var movement in Movements)
                movement.Init(this);
            
            yield return new WaitUntil(() => playerController.IsInitialized);

            _readyForMovement = true;
        }

        private void Update()
        {
            if (!_readyForMovement) return;
            _currentMovement?.Move();
        }

        public void SwitchMovement(IMovement movementToSwitch) => _currentMovement = movementToSwitch;

        private void OnCollisionEnter(Collision other)
        {
            foreach (var movement in Movements)
                movement.OnCollisionEnter(other);
        }
        
        private void OnCollisionExit(Collision other)
        {
            foreach (var movement in Movements)
                movement.OnCollisionExit(other);
        }
    }
}
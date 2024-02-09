using System;
using System.Linq;
using Samples.Bridge;
using Samples.Player.Config;
using Samples.Player.Movement.Interface;
using UnityEngine;

namespace Samples.Player
{
    public class ClimbMovement : IMovement
    {
        private PlayerMovementBehaviour _playerMovementBehaviour;
        private MovementConfig _movementConfig;

        public void Init(PlayerMovementBehaviour playerMovementBehaviour)
        {
            _playerMovementBehaviour = playerMovementBehaviour;
            _movementConfig = ConfigHandler.GetMovementConfig<ClimbMovementConfig>();
        }
        
        public void Move() => _playerMovementBehaviour.transform.Translate((Vector3.up) * (Time.deltaTime * _movementConfig.Speed));
        public void OnCollisionEnter(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Wall")) return;

            _playerMovementBehaviour.SwitchMovement(this);
            Debug.Log($"Switching to {GetType().Name}");
        }
        
        public void OnCollisionExit(Collision collision)
        {
            if (!collision.gameObject.CompareTag("Wall")) return;

            Debug.Log($"Switching to RunningMovement");
            _playerMovementBehaviour.SwitchMovement(_playerMovementBehaviour.Movements.First(m=> m.GetType() == typeof(RunningMovement)));
        }
    }
}